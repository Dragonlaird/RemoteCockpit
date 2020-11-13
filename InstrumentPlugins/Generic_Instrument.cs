using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using RemoteCockpitClasses;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using RemoteCockpitClasses.Animations;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;
using System.Numerics;

namespace InstrumentPlugins
{
    public class Generic_Instrument : ICockpitInstrument
    {
        private Configuration config;
        private string configPath;
        private double aspectRatio;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private double scaleFactor = 1;
        private List<ClientRequestResult> previousResults = new List<ClientRequestResult>();
        private List<ClientRequestResult> currentResults = new List<ClientRequestResult>();
        private List<System.Timers.Timer> animateTimers = new List<System.Timers.Timer>();
        private List<double> animationSteps = new List<double>();
        private delegate void SafeUpdateDelegate(Control ctrl);
        private SafeUpdateDelegate delgte;
        private int animationTimeInMs = 3000;
        private int animationStepCount = 10;

        public Generic_Instrument()
        {
            config = new Configuration();
            configPath = null;
        }

        public Generic_Instrument(string filePath)
        {
            configPath = filePath;
            config = null;
            Initialize();
        }

        public Generic_Instrument(Configuration configuration)
        {
            config = configuration;
            configPath = null;
            Initialize();
        }

        private void Initialize()
        {
            Control = new Panel();
            try
            {
                if (!string.IsNullOrEmpty(configPath))
                    config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));
                scaleFactor = 1.0;
                var image = LoadImage(config.BackgroundImagePath);
                aspectRatio = (double)image.Height / image.Width;
                scaleFactor = controlHeight < controlWidth ? (double)controlWidth / image.Width : (double)controlHeight / image.Height;
                var backgroundImage = LoadImage(config.BackgroundImagePath); //new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
                controlHeight = backgroundImage.Height;
                controlWidth = backgroundImage.Width;
                Control.BackColor = Color.Transparent;
                Control.BackgroundImage = backgroundImage;
            }
            catch(Exception ex) { }
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.Height = controlHeight;
            Control.Width = controlWidth;
            previousResults = new List<ClientRequestResult>();
            currentResults = new List<ClientRequestResult>();
            animateTimers = new List<System.Timers.Timer>();
            animationSteps = new List<double>();
            if (config?.Animations != null)
            {
                foreach (var clientRequest in config?.ClientRequests)
                {
                    currentResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                    previousResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)-1 });
                    animateTimers.Add(null);
                    animationSteps.Add(1);
                }
                Control.Paint += PaintControl;
                Control.Invalidate(true);
                //PaintControl(Control, new PaintEventArgs(Control.CreateGraphics(), Control.DisplayRectangle));
            }
        }

        private void PaintControl(object sender, PaintEventArgs e)
        {
            if (config?.Animations != null)
            {
                // We have a foreground to update
                foreach (var animation in config.Animations)
                {
                    if (!Control.Controls.ContainsKey(animation.Name))
                    {
                        foreach (var trigger in animation.Triggers)
                        {
                            var imagePanel = new PictureBox();
                            imagePanel.BackColor = Color.Transparent;
                            imagePanel.Width = Control.Width;
                            imagePanel.Height = Control.Height;
                            imagePanel.Name = animation.Name;
                            imagePanel.Paint += PaintAnimation;
                            Control.Controls.Add(imagePanel);
                            imagePanel.Invalidate();
                            imagePanel.BringToFront();
                        }
                    }
                    else
                    {
                        try
                        {
                            Control.Controls[animation.Name].Invalidate(true);
                            PaintAnimation(Control.Controls[animation.Name], new PaintEventArgs(Control.Controls[animation.Name].CreateGraphics(), Control.Controls[animation.Name].DisplayRectangle));
                        }
                        catch { }
                    }
                }
            }
        }

        private void PaintAnimation(object sender, PaintEventArgs e)
        {
            try
            {
                var ctrl = (Control)sender;
                var animation = config.Animations.First(x => x.Name == ctrl.Name);
                var triggers = (AnimationTriggerClientRequest[])animation.Triggers.Where(x => x.Type == AnimationTriggerTypeEnum.ClientRequest).Select(x => (AnimationTriggerClientRequest)x).ToArray();
                foreach (var trigger in triggers)
                {
                    var currentResult = currentResults.First(x => x.Request.Name == trigger.Request.Name && x.Request.Unit == trigger.Request.Unit);
                    var resultIdx = currentResults.IndexOf(currentResult);
                    var animationTimer = animateTimers[resultIdx];

                    var previousResult = previousResults[resultIdx];
                    var nextValue = (double)(previousResult.Result ?? 0.0);
                    var currentValue = (double)currentResult.Result;

                    if (nextValue != currentValue && nextValue != (double)(animation?.LastAppliedValue ?? (object)0.0))
                    {
                        animation.LastAppliedValue = nextValue;
                        foreach (var action in trigger.Actions)
                        {
                            if (action is AnimationActionRotate)
                            {
                                // Rotate our control background, either clockwise or counter-clockwise, depending on the value of te Request Result

                                var rotateAction = (AnimationActionRotate)action;
                                var rotateAngle = (float)((Math.PI * 2 * nextValue) / rotateAction.MaximumValueExpected);
                                Image initialImage = null;
                                initialImage = DrawPoints((AnimationDrawing)animation, rotateAngle);
                                if (initialImage != null)
                                {
                                    ctrl.BackColor = Color.Transparent;
                                    ctrl.BackgroundImage = initialImage;
                                    ctrl.BringToFront();
                                    //ctrl.Invalidate();
                                }
                            }
                        }
                    }
                }
            }
            catch { } // Animation failed - will be refreshed again soon if this isn't the last animation
        }

        private void RemoveTimer(int timerIdx)
        {
            if(timerIdx > -1-1 && animateTimers[timerIdx] != null)
            {
                if (animateTimers[timerIdx].Enabled)
                    animateTimers[timerIdx].Stop();
                animateTimers[timerIdx]?.Dispose();
                animateTimers[timerIdx] = null;
            }
        }

        private void StopTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                var timerIdx = animateTimers.IndexOf(timer);
                if (timerIdx > -1 && animateTimers[timerIdx] != null)
                {
                    RemoveTimer(timerIdx);
                }
                else
                {
                    if (timer.Enabled)
                        timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
        }

        private System.Timers.Timer StartTimer(System.Timers.Timer timer)
        {
            var timerIdx = animateTimers.IndexOf(timer);
            if (timer != null && timer.Enabled)
                StopTimer(timer);
            timer = new System.Timers.Timer(animationTimeInMs / animationStepCount);
            timer.Elapsed += ExecuteAnimation;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            if (timerIdx > -1)
                animateTimers[timerIdx] = timer;
            return timer;
        }

        /// <summary>
        /// Update Last Value for associated variable, check if Current Value has been reached, if so, set Last Value to Current Value and stop timer
        /// Trigger Paint eveent for each animation control that relies on this variable
        /// </summary>
        /// <param name="sender">Timer initiating the method</param>
        /// <param name="e">Arguments associated with this timer event</param>
        private void ExecuteAnimation(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var timerIdx = animateTimers.IndexOf((System.Timers.Timer)sender);
                var animateTimer = (System.Timers.Timer)sender;
                StopTimer(animateTimer);
                if (timerIdx > -1)
                {
                    var lastValue = previousResults[timerIdx];
                    var currentValue = currentResults[timerIdx];
                    var stepSize = animationSteps[timerIdx];
                    lastValue.Result = (double)lastValue.Result + stepSize;
                    if (stepSize < 0)
                        if ((double)lastValue.Result < (double)currentValue.Result)
                        {
                            // Reached our limit
                            lastValue.Result = currentValue.Result;
                            RemoveTimer(timerIdx);
                            animateTimer = null;
                        }
                        else
                        {
                            if ((double)lastValue.Result > (double)currentValue.Result)
                            {
                                // Reached our limit
                                lastValue.Result = currentValue.Result;
                                RemoveTimer(timerIdx);
                                animateTimer = null;
                            }
                        }
                    // Trigger paint event of any controls that rely on this value
                    var animations = config.Animations.Where(x => x.Triggers.Any(y => y.Type == AnimationTriggerTypeEnum.ClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == lastValue.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == lastValue.Request.Unit)).Select(x => x).ToArray();
                    foreach (var name in animations.Select(x => x.Name))
                    {
                        if (Control.Controls.ContainsKey(name))
                            UpdateInstrument(Control); // Force this control to repaint itself and any children
                    }
                }
                if (animateTimer != null)
                    StartTimer(animateTimer);
            }
            catch { }
        }

        private void UpdateInstrument(Control obj)
        {
            try
            {
                if (obj.InvokeRequired)
                {
                    delgte = new SafeUpdateDelegate(UpdateInstrument);
                    obj.Invoke(delgte, new object[] { obj });
                    return;
                }
                else
                {
                    obj.Invalidate(true);
                }
            }
            catch { }
        }


        private Image LoadImage(string imagePath)
        {
            var diretory = Directory.GetCurrentDirectory();
            var imageFile = File.OpenRead(new Uri(Path.Combine(diretory,imagePath)).AbsolutePath);
            var image = Image.FromStream(imageFile);
            aspectRatio = (double)image.Height / image.Width;
            var resizedImage = new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
            return resizedImage;
        }

        private Image DrawPoints(AnimationDrawing item, float angleInRadians)
        {
            if (item.PointMap?.Count() > 0)
            {
                /*
                var imageHolder = new PictureBox();
                //imageHolder.Top = 0;
                //imageHolder.Left = 0;
                imageHolder.Height = Control.Height;
                imageHolder.Width = Control.Width;
                imageHolder.Enabled = false;
                */
                var pen = new Pen(item.FillColor, 1);
                //var g = e.Graphics;
                Bitmap bitmap = new Bitmap(Control.Width, Control.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmap.MakeTransparent();
                Graphics graph = Graphics.FromImage(bitmap);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                GraphicsPath gp = new GraphicsPath();
                double absoluteX = item.RelativeX;
                double absoluteY = item.RelativeY;
                PointF[] points = null;
                if (item.ScaleMethod == AnimationScaleMethodEnum.Percent)
                {
                    // Resize image using the current scale 
                    absoluteX = Control.Width * item.RelativeX / (float)100;
                    absoluteY = Control.Height * item.RelativeY / (float)100;
                    points = RemapPoints(item.PointMap, (float)absoluteX, (float)absoluteY, (float)Control.Width < Control.Height ? Control.Width : Control.Height, angleInRadians);
                }
                if(item.ScaleMethod == AnimationScaleMethodEnum.None)
                {
                    // Use unmodified dimensions (no scaling)
                    points = RemapPoints(item.PointMap, (float)absoluteX, (float)absoluteY, (float)Control.Width < Control.Height ? Control.Width : Control.Height, angleInRadians);
                }
                gp.AddLines(points);
                graph.FillPath(pen.Brush, gp);
                return bitmap;
            }
            return null;
        }

        public PointF[] RemapPoints(AnimationPoint[] points, float absoluteX, float absoluteY, float imageSize, float angleInRadians)
        {
            List<PointF> remappedPoints = new List<PointF>();
            for (var i = 0; i < points.Length; i++)
            {
                var nextPoint = points[i];
                var pixelsPerPercent = imageSize / 100;
                // Need to calculate the angle from origin for this point, then increment the angle by angleInRadians
                //var lastPoint = i == 0 ? new AnimationPoint(0, 0) : points[i - 1];
                //var deltaY = nextPoint.Y - lastPoint.Y;
                //var deltaX = nextPoint.X - lastPoint.X;
                //var pointAngle = (float)Math.Atan2(deltaY, deltaX) + angleInRadians;
                nextPoint = new AnimationPoint(nextPoint.X * pixelsPerPercent, nextPoint.Y * pixelsPerPercent);
                //remappedPoints.Add(GetPoint(nextPoint, absoluteX, absoluteY, pointAngle));
                var newPoint = CartesianWithAngularOffset(nextPoint.Point, angleInRadians);
                newPoint.X += absoluteX;
                newPoint.Y += absoluteY;
                remappedPoints.Add(newPoint);
            }
            return remappedPoints.ToArray();
        }

        private float GetPointAngle(PointF startPoint, PointF destPoint)
        {
            var deltaY = startPoint.Y - destPoint.Y;
            var deltaX = startPoint.X - destPoint.X;
            return (float)Math.Atan2(deltaY, deltaX);
        }

        private float GetPointRadius(PointF point)
        {
            var radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
            return (float)radius;
        }

        private PointF CartesianWithAngularOffset(PointF point, float angleInRadians)
        {
            var pointAngle = GetPointAngle(new PointF { X = 0, Y = 0 }, point);
            var radius = GetPointRadius(point);
            var newAngle = pointAngle + angleInRadians;
            return PolarToCartesian(newAngle, radius);
        }

        private PointF PolarToCartesian(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);
            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);
            return new PointF((float)x, (float)y);
        }

        private PointF GetPoint(AnimationPoint point, float absoluteX, float absoluteY, float angleInRadians)
        {
            /*
            var x1 = point.X * Math.Sin(angleInRadians) + absoluteX;
            var y1 = point.Y * Math.Cos(angleInRadians) + absoluteY;
            return new PointF((float)x1, (float)y1);
            */
            var x2 = point.Point.X * point.Point.X;
            var y2 = point.Point.Y * point.Point.Y;
            var length = Math.Sqrt(x2 + y2);
            var x1 = length * Math.Sin(angleInRadians);
            var y1 = length * Math.Cos(angleInRadians);
            return new PointF((float)(absoluteX + x1), (float)(absoluteY - y1));
        }

        public void LoadConfigFile(string filePath)
        {
            configPath = filePath;
            Initialize();
        }

        private bool disposedValue;

        public IEnumerable<ClientRequest> RequiredValues => config?.ClientRequests;

        public string[] Aircraft => config?.Aircraft;

        public DateTime PluginDate=> config?.CreateDate ?? DateTime.MinValue;

        public InstrumentType Type => config?.Type ?? InstrumentType.Other;

        public Control Control { get; private set; }

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name => config?.Name;

        public string Author => config?.Author;

        public event EventHandler Disposed;

        public void SetLayout(int top, int left, int height, int width)
        {
            controlTop = top;
            controlLeft = left;
            controlHeight = height;
            controlWidth = width;
            Initialize();
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            var clientRequestResult = currentResults.FirstOrDefault(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit);
            if (clientRequestResult != null)
            {
                var resultIdx = currentResults.IndexOf(clientRequestResult);
                var lastResult = previousResults[resultIdx];
                lock (currentResults)
                    currentResults[resultIdx] = value;
                // Check if any animations use this variable as a trigger
                if (config.Animations.Any(x => x.Triggers.Any(y=> y is AnimationTriggerClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == value.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == value.Request.Unit)))
                {
                    // Modify the step size for our control
                    lock (animationSteps)
                        animationSteps[resultIdx] = Math.Abs(((double)(currentResults[resultIdx].Result ?? 0.0) - (double)(lastResult.Result ?? 0.0)) / animationStepCount);
                    // Start timer (or Restart timer if already running), to modify the animation speed correctly and start animating
                    var animationTime = animationTimeInMs / animationStepCount; // Expected Update Time divided by number of animation steps we want within that time
                    // This will cause ExecuteAnimation to run, which should update the Last Value by Step Size & trigger Paint Event for any controls relying on this variable to update
                    lock (animateTimers)
                    {
                        animateTimers[resultIdx] = new System.Timers.Timer(animationTime);
                        animateTimers[resultIdx] = StartTimer(animateTimers[resultIdx]);
                    }
                }
            }
        }

        /// <summary>
        /// Method to rotate an Image object. The result can be one of three cases:
        /// - upsizeOk = true: output image will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false & clipOk = true: output same size as input, clipping occurs
        /// - upsizeOk = false & clipOk = false: output same size as input, image reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated image. If color = transparent the output image will be 32-bit, 
        /// otherwise the output image will be 24-bit.
        /// 
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputImage">input Image object, is not modified</param>
        /// <param name="angleRadians">angle of rotation, in radians</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input image</returns>
        private Bitmap RotateImage(Image inputImage, float angleRadians, bool upsizeOk,
                                         bool clipOk, Color backgroundColor)
        {
            // Test for zero rotation and return a clone of the input image
            if (angleRadians == 0f)
                return (Bitmap)inputImage.Clone();

            // Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
            float oldWidth = inputImage.Width;
            float oldHeight = inputImage.Height;
            int newWidth = (int)oldWidth;
            int newHeight = (int)oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
            if (upsizeOk || !clipOk)
            {
                //double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            // If upsizing not wanted and clipping not OK need a scaling factor
            if (!upsizeOk && !clipOk)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = (int)oldWidth;
                newHeight = (int)oldHeight;
            }

            // Create the new bitmap object. If background color is transparent it must be 32-bit, 
            //  otherwise 24-bit is good enough.
            Bitmap newBitmap = new Bitmap((int)newWidth, (int)newHeight, backgroundColor == Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

            // Create the Graphics object that does the work
            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {

                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

                // Fill in the specified background color if necessary
                if (backgroundColor != Color.Transparent)
                    graphicsObject.Clear(backgroundColor);

                // Set up the built-in transformation matrix to do the rotation and maybe scaling
                Matrix matrix = new Matrix();

                // Rotate about image center point
                matrix.RotateAt(20, new PointF(newWidth / 2f, newHeight / 2f));
                var transformX = oldWidth / 2;
                var transformY = oldHeight / 2;
                matrix.Translate(transformX, transformY);
                // Move the transform point back to the top left
                //graphicsObject.TranslateTransform(-1*(newWidth - oldWidth), -1*(newHeight - oldHeight));

                graphicsObject.Transform = matrix;
                graphicsObject.DrawImage(newBitmap, new Point());
                //graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);
                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                //graphicsObject.RotateTransform(angleRadians);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                // Draw the result 
                graphicsObject.DrawImage(inputImage, 0, 0);
            }

            return newBitmap;
        }

        private Image ClipToCircle(Image srcImage, PointF center, float radius, Color backGround)
        {
            Image dstImage = new Bitmap(srcImage.Width, srcImage.Height, srcImage.PixelFormat);

            using (Graphics g = Graphics.FromImage(dstImage))
            {
                RectangleF r = new RectangleF(center.X - radius, center.Y - radius,
                                                         radius * 2, radius * 2);

                // enables smoothing of the edge of the circle (less pixelated)
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // fills background color
                using (Brush br = new SolidBrush(backGround))
                {
                    g.FillRectangle(br, 0, 0, dstImage.Width, dstImage.Height);
                }

                // adds the new ellipse & draws the image again 
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(r);
                g.SetClip(path);
                g.DrawImage(srcImage, 0, 0);

                return dstImage;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    config = null;
                    if (animateTimers != null) // Stop and dispose of any running timers
                        for (var i = 0; i < animateTimers.Count; i++)
                        {
                            RemoveTimer(i);
                        }
                    animateTimers?.Clear();
                    animateTimers = null;
                    // Stop any callback to update Control
                    if (delgte != null)
                        delgte = null;
                    // Clear last result cache
                    previousResults?.Clear();
                    previousResults = null;
                    // Clear current result cache
                    currentResults?.Clear();
                    currentResults = null;
                    // Clear animation speed cache
                    animationSteps?.Clear();
                    animationSteps = null;
                    // Clear cuurent config
                    config = null;
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Generic_Instrument()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
