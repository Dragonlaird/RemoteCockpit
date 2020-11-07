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
        private List<double> animationSpeed = new List<double>();
        private delegate void SafeUpdateDelegate(Control ctrl);

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
            animationSpeed = new List<double>();
            if (config?.Animations != null)
            {
                foreach (var clientRequest in config?.ClientRequests)
                {
                    currentResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                    previousResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                    animateTimers.Add(null);
                    animationSpeed.Add(0);
                }
                Control.Paint += PaintControl;
                Control.Invalidate();
                PaintControl(Control, new PaintEventArgs(Control.CreateGraphics(), Control.DisplayRectangle));
            }
        }

        private void PaintControl(object sender, PaintEventArgs e)
        {
            //throw new NotImplementedException();
            if(config?.Animations != null)
            {
                //Image image = null;
                // We have a foreground to update
                foreach(var animation in config.Animations)
                {
                    foreach (var trigger in animation.Triggers) {
                        //if (animation.Type == AnimationItemTypeEnum.Drawing)
                        //{
                        //    image = DrawPoints((AnimationDrawing)animation);
                        //}
                        //if (animation.Type == AnimationItemTypeEnum.Image)
                        //{
                        //    image = LoadImage(((AnimationImage)animation).ImagePath);
                        //}
                        var imagePanel = new Panel();
                        
                        imagePanel.Width = Control.Width;
                        imagePanel.Height = Control.Height;
                        imagePanel.Name = animation.Name;
                        //imagePanel.BackColor = Color.Transparent;
                        //imagePanel.Opacity = 0;
                        //imagePanel.BackgroundImage = image;
                        imagePanel.Paint += PaintAnimation;
                        if (!Control.Controls.ContainsKey(animation.Name))
                        {
                            //Control.Controls.RemoveByKey(animation.Name);
                            Control.Controls.Add(imagePanel);
                            PaintAnimation(imagePanel, new PaintEventArgs(imagePanel.CreateGraphics(), imagePanel.DisplayRectangle));
                        }
                        imagePanel.Invalidate();
                        imagePanel.BringToFront();
                    }
                }
                /*
                var g = Control.CreateGraphics();
                foreach (Control c in Control.Controls)
                {
                    if (c.Bounds.IntersectsWith(Control.Bounds) && c.Visible)
                    {
                        // Load appearance of underlying control and redraw it on this background
                        Bitmap bmp = new Bitmap(c.Width, c.Height, g);
                        c.DrawToBitmap(bmp, c.ClientRectangle);
                        g.TranslateTransform(c.Left - Control.Left, c.Top - Control.Top);
                        g.DrawImageUnscaled(bmp, Point.Empty);
                        g.TranslateTransform(Control.Left - c.Left, Control.Top - c.Top);
                        bmp.Dispose();
                    }
                }
                */
            }
        }

        private void PaintAnimation(object sender, PaintEventArgs e)
        {
            var animationName = ((Control)sender).Name;
            // Repaint Animation Image here - if not already upto date
            var animation = config.Animations.First(x => x.Name == animationName);
            bool redrawImage = false;
            foreach (var trigger in animation.Triggers.Where(x => x.Type == AnimationTriggerTypeEnum.ClientRequest))
            {
                var clientRequest = ((AnimationTriggerClientRequest)trigger).Request;
                var previousResult = previousResults.First(x => x.Request.Name == clientRequest.Name && x.Request.Unit == clientRequest.Unit);
                var currentResult = currentResults.First(x => x.Request.Name == clientRequest.Name && x.Request.Unit == clientRequest.Unit);
                if (previousResult.Result?.ToString() != currentResult.Result?.ToString())
                {
                    redrawImage = true;
                    break;
                }
            }
            if (redrawImage)
            {
                // Attribute least 1 ClientRequest has been updated - redraw the image using new value
                var panel = Control.Controls[animationName];
                Image image = null;
                if (animation.Type == AnimationItemTypeEnum.Drawing)
                {
                    image = DrawPoints((AnimationDrawing)animation);
                }
                if (animation.Type == AnimationItemTypeEnum.Image)
                {
                    image = LoadImage(((AnimationImage)(IAnimationItem)animation).ImagePath);
                }
                ((Control)sender).BackgroundImage = image;
                // Record the new value so we don't redraw again
            }
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

        private Image DrawPoints(AnimationDrawing item)
        {
            if (item.PointMap?.Count() > 0)
            {
                var imageHolder = new PictureBox();
                //imageHolder.Top = 0;
                //imageHolder.Left = 0;
                imageHolder.Height = Control.Height;
                imageHolder.Width = Control.Width;
                imageHolder.Enabled = false;
                var pen = new Pen(item.FillColor, 1);
                //var g = e.Graphics;
                Bitmap bitmap = new Bitmap(imageHolder.Width, imageHolder.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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
                    absoluteX = imageHolder.Width * item.RelativeX / (float)100;
                    absoluteY = imageHolder.Height * item.RelativeY / (float)100;
                    points = RemapPoints(item.PointMap, (float)absoluteX, (float)absoluteY, (float)imageHolder.Width < imageHolder.Height ? imageHolder.Width : imageHolder.Height);
                }
                if(item.ScaleMethod == AnimationScaleMethodEnum.None)
                {
                    // Use unmodified dimensions (no scaling)
                    points = RemapPoints(item.PointMap, (float)absoluteX, (float)absoluteY, (float)imageHolder.Width < imageHolder.Height ? imageHolder.Width : imageHolder.Height);
                }
                gp.AddLines(points);
                graph.FillPath(pen.Brush, gp);
                return bitmap;
            }
            return null;
        }

        public PointF[] RemapPoints(AnimationPoint[] points, float absoluteX, float absoluteY, float imageSize)
        {
            List<PointF> remappedPoints = new List<PointF>();
            var onePercentInPixelsX = imageSize / 100;
            var onePercentInPixelsY = imageSize / 100;
            foreach (var point in points)
            {
                remappedPoints.Add(new PointF
                {
                    X = point.Point.X * onePercentInPixelsX + absoluteX, //(point.X * scaleX) + absoluteX,
                    Y = point.Point.Y * onePercentInPixelsY + absoluteY //(point.Y * scaleY) + absoluteY
                });
            }
            return remappedPoints.ToArray();
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
            var lastResult = previousResults.FirstOrDefault(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit);
            if (lastResult != null)
            {
                var resultIdx = previousResults.IndexOf(lastResult);
                var currentResult = currentResults[resultIdx];
                var animateTimer = animateTimers[resultIdx];
                currentResult.Result = value.Result;
                // Set timer to perform update, if not already running
                if (animateTimer == null || !animateTimer.Enabled)
                {
                    var currentValue = (double)(currentResult.Result == null ? (double)0 : (double)currentResult.Result);
                    var previousValue = (double)(lastResult.Result == null ? (double)0 : (double)lastResult.Result);
                    var animSpeed = Math.Abs(currentValue - previousValue);
                    if (animSpeed != 0)
                    {
                        animationSpeed[resultIdx] = animSpeed;
                        animateTimer = new System.Timers.Timer(1000 / animSpeed);
                        animateTimer.Elapsed += ExecuteAnimation;
                        animateTimer.AutoReset = true;
                        animateTimer.Enabled = true;
                        animateTimer.Start();
                        animateTimers[resultIdx] = animateTimer;
                    }
                }
            }
        }

        private void ExecuteAnimation(object sender, System.Timers.ElapsedEventArgs e)
        {
            var animateTimer = (System.Timers.Timer)sender;
            var animateTimerIdx = animateTimers.IndexOf(animateTimer);
            if (animateTimerIdx > -1)
            {
                var currentResult = currentResults[animateTimerIdx];
                var previousResult = previousResults[animateTimerIdx];
                var increaseValue = animationSpeed[animateTimerIdx];
                if(increaseValue > 0 && (double)previousResult.Result + increaseValue > (double)currentResult.Result)
                {
                    increaseValue = (double)currentResult.Result - (double)previousResult.Result;
                    animateTimer.Stop();
                    animateTimers[animateTimerIdx].Stop();
                    animateTimers[animateTimerIdx] = null;
                }
                if (increaseValue < 0 && (double)previousResult.Result + increaseValue < (double)currentResult.Result)
                {
                    increaseValue = (double)currentResult.Result - (double)previousResult.Result;
                    animateTimer.Stop();
                    animateTimers[animateTimerIdx].Stop();
                    animateTimers[animateTimerIdx] = null;
                }
                var newValue = new ClientRequestResult
                {
                    Request = currentResult.Request,
                    Result = (double)currentResult.Result + increaseValue
                };
                var animations = config.Animations.Where(x => x.Triggers?.Any(y => y is AnimationTriggerClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == currentResult.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == currentResult.Request.Unit) == true);
                foreach (var animation in animations)
                {
                    if (Control.Controls.ContainsKey(animation.Name))
                    {
                        var ctrl = Control.Controls[animation.Name];
                        var triggers = animation.Triggers.Where(x => ((AnimationTriggerClientRequest)x).Request.Name == currentResult.Request.Name && ((AnimationTriggerClientRequest)x).Request.Unit == currentResult.Request.Unit);
                        foreach (var trigger in triggers)
                        {
                            foreach (var action in trigger.Actions)
                            {
                                if(action is AnimationActionRotate)
                                {
                                    // Rotate our control background, either clockwise or counter-clockwise, depending on the value of te Request Result
                                    var rotateAction = (AnimationActionRotate)action;
                                    var rotateAngle = (float)((Math.PI * 2 * (double)newValue.Result) / rotateAction.MaximumValueExpected);
                                    Image initialImage = null;
                                    if(animation.Type == AnimationItemTypeEnum.Drawing)
                                        initialImage = DrawPoints((AnimationDrawing)animation);
                                    if (initialImage != null)
                                    {
                                        ctrl.BackgroundImage = RotateImage(initialImage, rotateAngle, true, false, Color.Transparent);
                                        UpdateInstrument(ctrl);
                                    }
                                }
                            }
                        }
                        previousResults[animateTimerIdx] = newValue;
                    }
                }
            }
        }

        private void UpdateInstrument(Control obj)
        {
            if (obj.InvokeRequired)
            {
                try
                {
                    var d = new SafeUpdateDelegate(UpdateInstrument);
                    obj.Invoke(d, new object[] { obj });
                }
                catch { }
                return;
            }
            try
            {
                obj.Update();
                PaintAnimation(obj, new PaintEventArgs(obj.CreateGraphics(), obj.DisplayRectangle));
            }
            catch { }
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
                    config = null;
                    previousResults?.Clear();
                    previousResults = null;
                    currentResults?.Clear();
                    currentResults = null;
                    animateTimers?.Clear();
                    animateTimers = null;
                    animationSpeed?.Clear();
                    animationSpeed = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
