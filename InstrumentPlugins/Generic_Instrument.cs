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
        //private List<System.Timers.Timer> animateTimers = new List<System.Timers.Timer>();
        private System.Timers.Timer animateTimer;
        private List<double> animationSteps = new List<double>();
        private List<Bitmap> animationImages = new List<Bitmap>();
        private delegate void SafeUpdateDelegate(object sender, PaintEventArgs e);
        private SafeUpdateDelegate delgte;
        private int animationTimeInMs = 3000;
        private int animationStepCount = 10;
        private int animationUpdateInMs = 300;
        private DateTime lastExecution = DateTime.MinValue;
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
            UpdateStepCount(animationTimeInMs);
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
            catch (Exception ex)
            {

            }
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.Height = controlHeight;
            Control.Width = controlWidth;
            previousResults = new List<ClientRequestResult>();
            currentResults = new List<ClientRequestResult>();
            animateTimer = null; // new List<System.Timers.Timer>();
            animationSteps = new List<double>();
            if(config?.AnimationUpdateInMs > 0)
            {
                animationUpdateInMs = config.AnimationUpdateInMs; 
            }
            if (config?.Animations != null)
            {
                animationImages = new List<Bitmap>();
                foreach(var animation in config.Animations)
                {
                    // If this is an Image Animation - pre-fetch the image into the cache
                    if (animation is AnimationImage)
                    {
                        animationImages.Add((Bitmap)LoadImage(((AnimationImage)animation).ImagePath));
                    }
                    else
                        animationImages.Add(DrawPoints((AnimationDrawing)animation));
                }
                foreach (var clientRequest in config?.ClientRequests)
                {
                    if (!currentResults.Any(x => x.Request.Name == clientRequest.Name && x.Request.Unit == clientRequest.Unit))
                    {
                        currentResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                        previousResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)-1 });
                        //animateTimers.Add(null);
                        animationSteps.Add(1);
                    }
                }
                Control.Paint += PaintControl;
                Control.Invalidate();
                Control.Update();
            }
        }

        private void PaintControl(object sender, PaintEventArgs e)
        {
            //if (((Control)sender).InvokeRequired)
            //{
            //    try
            //    {
            //        var ctrl = (Control)sender;
            //        var d = new SafeUpdateDelegate(PaintControl);
            //        ctrl.Invoke(d, new object[] { ctrl, e });
            //    }
            //    catch { }
            //    return;
            //}
            if (config?.Animations != null)
            {
                // We have a foreground to update
                Bitmap animationImage = new Bitmap(Control.Width, Control.Height, PixelFormat.Format32bppArgb);
                animationImage.MakeTransparent();
                for (var animationId = 0; animationId < config.Animations.Length; animationId++)
                {
                    try
                    {
                        var overlapImage = GenerateAnimationImage(animationId);
                        animationImage = (Bitmap)Overlap((Image)animationImage, overlapImage);
                    }
                    catch { }
                }
                if (Control.Controls["Animation"] == null)
                {
                    Control.Controls.Add(new PictureBox { Name = "Animation", Image = animationImage, Height = Control.Height, Width = Control.Width });
                }
                else
                {
                    ((PictureBox)Control.Controls["Animation"]).Image = animationImage;
                }
            }
        }

        private Image GenerateAnimationImage(int animationId)
        {
            Bitmap initialImage = animationImages[animationId];
            lock (config.Animations)
            {
                var triggers = (AnimationTriggerClientRequest[])config.Animations[animationId].Triggers.Where(x => x.Type == AnimationTriggerTypeEnum.ClientRequest).Select(x => (AnimationTriggerClientRequest)x).ToArray();
                foreach (var trigger in triggers)
                {
                    var nextValue = config.Animations[animationId].LastAppliedValue ?? 0;
                    if (trigger is AnimationTriggerClientRequest)
                    {
                        nextValue = previousResults.First(x => x.Request.Name == trigger.Request.Name && x.Request.Unit == trigger.Request.Unit).Result;
                    }
                    config.Animations[animationId].LastAppliedValue = nextValue;
                    foreach (var action in trigger.Actions)
                    {
                        if (action is AnimationActionRotate)
                        {
                            // Rotate our control background, either clockwise or counter-clockwise, depending on the value of te Request Result
                            var rotateAction = (AnimationActionRotate)action;
                            var displayVal = (double)nextValue % rotateAction.MaximumValueExpected;
                            var rotateAngle = (float)((360 * displayVal) / rotateAction.MaximumValueExpected);
                            initialImage = RotateImage(initialImage, rotateAngle);
                        }
                        if (action is AnimationActionClip)
                        {
                            // Clip a circle or square using the 2 points to mark the outer edge or top-left/bottom-right
                            initialImage = ClipImage(initialImage, ((AnimationActionClip)action).Style, ((AnimationActionClip)action).StartPoint, ((AnimationActionClip)action).EndPoint);
                        }
                    }
                }
            }
            return initialImage;
        }

        public Image Overlap(Image source1, Image source2)
        {
            using (var target = new Bitmap(Control.Width, Control.Height, PixelFormat.Format32bppArgb))
            {
                target.MakeTransparent();
                using (var graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                    graphics.DrawImage(source1, 0, 0);
                    graphics.DrawImage(source2, 0, 0);
                }
                return target;
            }
        }

        private Bitmap ClipImage(Bitmap image, AnimateActionClipEnum style, AnimationPoint start, AnimationPoint end)
        {
            Bitmap dstImage = new Bitmap(image.Width, image.Height, image.PixelFormat);
            var onePercentX = image.Width / 100.0f;
            var onePercentY = image.Height / 100.0f;
            var topLeft = new PointF(start.X * onePercentX/2, start.Y * onePercentY/2);
            var btmRight = new PointF(end.X * onePercentX, end.Y * onePercentY);
            using (Graphics g = Graphics.FromImage(dstImage))
            {
                // enables smoothing of the edge of the circle (less pixelated)
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // fills background color
                using (Brush br = new SolidBrush(Color.Transparent))
                {
                    g.FillRectangle(br, 0, 0, dstImage.Width, dstImage.Height);
                }

                // adds the new ellipse & draws the image again 
                using (GraphicsPath path = new GraphicsPath())
                {
                    // Clipping a circular path
                    if (style == AnimateActionClipEnum.Circle)
                        path.AddEllipse(topLeft.X, topLeft.Y, btmRight.X, btmRight.Y);
                    // Clipping a square
                    if (style == AnimateActionClipEnum.Square)
                        path.AddRectangle(new RectangleF(topLeft.X, topLeft.Y, btmRight.X, btmRight.Y));
                    g.SetClip(path);
                    g.DrawImage(image, 0, 0);
                }
                return dstImage;
            }
        }

        private Bitmap RotateImage(Bitmap bmp, float angle)
        {
            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                // Draw the image on the bitmap
                g.DrawImage(bmp, new Point(0, 0));
            }

            return rotatedImage;
        }


        private void RemoveTimer()
        {
            StopTimer();
            if (animateTimer != null)
            {
                animateTimer.Dispose();
                animateTimer = null;
            }
            //if(timerIdx > -1-1 && animateTimers[timerIdx] != null)
            //{
            //    if (animateTimers[timerIdx].Enabled)
            //        animateTimers[timerIdx].Stop();
            //    animateTimers[timerIdx]?.Dispose();
            //    animateTimers[timerIdx] = null;
            //}
        }

        private void StopTimer()
        {
            if (animateTimer != null)
            {
                if (animateTimer.Enabled)
                    animateTimer.Stop();
            }
        }

        private void StartTimer()
        {
            StopTimer();
            animateTimer = new System.Timers.Timer(animationUpdateInMs);
            animateTimer.Elapsed += ExecuteAnimation;
            animateTimer.Start();
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
                //RemoveTimer();
                // Step our animation closer to the target value
                lock (currentResults)
                {
                    foreach (var currentResult in currentResults)
                    {
                        var triggerIdx = currentResults.IndexOf(currentResult);
                        var previousResult = previousResults[triggerIdx];
                        if (previousResult.Result != currentResult.Result)
                        {
                            var stepValue = Math.Abs(animationSteps[triggerIdx]);
                            if ((double)previousResult.Result < (double)currentResult.Result)
                            {
                                previousResult.Result = (double)previousResult.Result + stepValue;
                                if ((double)previousResult.Result > (double)currentResult.Result)
                                {
                                    previousResult.Result = currentResult.Result;
                                }
                            }
                            if ((double)previousResult.Result > (double)currentResult.Result)
                            {
                                previousResult.Result = (double)previousResult.Result - stepValue;
                                if ((double)previousResult.Result < (double)currentResult.Result)
                                {
                                    previousResult.Result = currentResult.Result;
                                }
                            }
                        }
                    }
                }
                // Trigger paint event of control
                Control.Invalidate();
                Control.Update();
                //PaintControl(Control, new PaintEventArgs(Control.CreateGraphics(), Control.DisplayRectangle));
                //UpdateInstrument(Control); // Force this control to repaint itself and any children
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

        private Bitmap DrawPoints(AnimationDrawing animation)
        {
            if (animation.PointMap?.Count() > 0)
            {
                float absoluteX = 0;
                float absoluteY = 0;
                PointF[] points = null;
                if (animation.ScaleMethod == AnimationScaleMethodEnum.Percent)
                {
                    // Resize image using the current scale 
                    var scaleX = animation.RelativeX / 100.0f;
                    var scaleY = animation.RelativeY / 100.0f;
                    var pixelsPerPercentX = Control.Width / 100.0f;
                    var pixelsPerPercentY = Control.Height / 100.0f;
                    absoluteX = (float)(Control.Width * scaleX);
                    absoluteY = (float)(Control.Height * scaleY);
                    points = animation.PointMap.Select(x => new PointF(x.Point.X * pixelsPerPercentX + absoluteX, x.Point.Y * pixelsPerPercentY + absoluteY)).ToArray();
                }
                Bitmap bitmap = new Bitmap(Control.Width, Control.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmap.MakeTransparent();
                using (Graphics graph = Graphics.FromImage(bitmap))
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    using (SolidBrush fill = new SolidBrush(animation.FillColor))
                    {
                        graph.FillPolygon(fill, points);
                    }
                }
                return bitmap;
            }
            return null;
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

        public int UpdateFrequency
        {
            get
            {
                return animationTimeInMs / 1000;
            }
            set
            {
                animationTimeInMs = value * 1000;
            }
        }
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
                if (value.Request.Name == "UPDATE FREQUENCY" && value.Request.Unit == "second")
                {
                    // We now know how often we expect to receive updates - revise the timings accordingly
                    UpdateStepCount((int)value.Result);
                }
                // Check if any animations use this variable as a trigger
                if (config.Animations.Any(x => x.Triggers.Any(y=> y is AnimationTriggerClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == value.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == value.Request.Unit)))
                {
                    // We want to update the animation every 0.3 seconds - determine how much we should move it
                    // Modify the step size for our control
                    lock (animationSteps)
                        animationSteps[resultIdx] = ((double)(currentResults[resultIdx].Result ?? 0.0) - (double)(lastResult.Result ?? 0.0)) / animationStepCount;
                    // Start timer (or Restart timer if already running), to modify the animation speed correctly and start animating
                    StartTimer();
                }
            }
        }

        private void UpdateStepCount(int serverUpdateTimeInMs)
        {
            animationTimeInMs = serverUpdateTimeInMs < 100 ? 100 : serverUpdateTimeInMs;
            animationStepCount = animationTimeInMs / animationUpdateInMs > 50 ? 50 : animationTimeInMs / animationUpdateInMs;
            animationStepCount = animationStepCount < 10 ? 10 : animationStepCount;
        }

        //private Image ClipToCircle(Image srcImage, PointF center, float radius, Color backGround)
        //{
        //    Image dstImage = new Bitmap(srcImage.Width, srcImage.Height, srcImage.PixelFormat);

        //    using (Graphics g = Graphics.FromImage(dstImage))
        //    {
        //        RectangleF r = new RectangleF(center.X - radius, center.Y - radius,
        //                                                 radius * 2, radius * 2);

        //        // enables smoothing of the edge of the circle (less pixelated)
        //        g.SmoothingMode = SmoothingMode.AntiAlias;

        //        // fills background color
        //        using (Brush br = new SolidBrush(backGround))
        //        {
        //            g.FillRectangle(br, 0, 0, dstImage.Width, dstImage.Height);
        //        }

        //        // adds the new ellipse & draws the image again 
        //        GraphicsPath path = new GraphicsPath();
        //        path.AddEllipse(r);
        //        g.SetClip(path);
        //        g.DrawImage(srcImage, 0, 0);

        //        return dstImage;
        //    }
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    config = null;
                    RemoveTimer();
                    // Stop any callback to update Control
                    if (delgte != null)
                    {
                        try
                        {
                            Delegate.RemoveAll(delgte, delgte);
                        }
                        catch { }
                        delgte = null;
                    }
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
