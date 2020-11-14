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
            if(config?.AnimationUpdateInMs > 0)
            {
                animationUpdateInMs = config.AnimationUpdateInMs; 
            }
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
                var ctrl = (Control)sender;
                var animation = config.Animations.First(x => x.Name == ctrl.Name);
                var triggers = (AnimationTriggerClientRequest[])animation.Triggers.Where(x => x.Type == AnimationTriggerTypeEnum.ClientRequest).Select(x => (AnimationTriggerClientRequest)x).ToArray();
            foreach (var trigger in triggers)
            {
                var nextResult = previousResults.First(x => x.Request.Name == trigger.Request.Name && x.Request.Unit == trigger.Request.Unit);
                var valuesIdx = previousResults.IndexOf(nextResult);
                var nextValue = nextResult.Result;
#if DEBUG
                var targetValue = currentResults[valuesIdx].Result;
#endif
                if (animation.LastAppliedValue != nextValue)
                {
                    animation.LastAppliedValue = nextValue;
                    foreach (var action in trigger.Actions)
                    {
                        if (action is AnimationActionRotate)
                        {
                            // Rotate our control background, either clockwise or counter-clockwise, depending on the value of te Request Result

                            var rotateAction = (AnimationActionRotate)action;
                            var rotateAngle = (float)((360 * (double)nextValue) / rotateAction.MaximumValueExpected);
                            Bitmap initialImage = null;
                            initialImage = DrawPoints((AnimationDrawing)animation);
                            initialImage = RotateImage(initialImage, rotateAngle);

                            if (initialImage != null)
                            {
                                ctrl.BackColor = Color.Transparent;
                                ((PictureBox)ctrl).Image = initialImage;
                                ctrl.BringToFront();
                                ctrl.Invalidate();
                            }
                        }
                    }
                }
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
            //timer.AutoReset = true;
            //timer.Enabled = true;
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
                lock (animateTimers)
                    StopTimer(animateTimers[timerIdx]);
                if (timerIdx > -1)
                {
                    var timeDiff = DateTime.Now.Subtract(lastExecution).TotalMilliseconds;
                    var lastValue = previousResults[timerIdx];
                    var currentValue = currentResults[timerIdx];
                    var stepSize = animationSteps[timerIdx];
                    lastValue.Result = (double)lastValue.Result + stepSize;
                    lastExecution = DateTime.Now;
                    if (stepSize < 0)
                    {
                        if ((double)lastValue.Result < (double)currentValue.Result)
                        {
                            // Reached our limit
                            lastValue.Result = currentValue.Result;
                            RemoveTimer(timerIdx);
                        }
                    }
                    else
                    {
                        if ((double)lastValue.Result > (double)currentValue.Result)
                        {
                            // Reached our limit
                            lastValue.Result = currentValue.Result;
                            RemoveTimer(timerIdx);
                        }
                    }
                    // Trigger paint event of any controls that rely on this value
                    var animations = config.Animations.Where(x => x.Triggers.Any(y => y.Type == AnimationTriggerTypeEnum.ClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == lastValue.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == lastValue.Request.Unit)).Select(x => x).ToArray();
                    foreach (var name in animations.Select(x => x.Name))
                    {
                        if (Control.Controls.ContainsKey(name))
                            UpdateInstrument(Control.Controls[name]); // Force this control to repaint itself and any children
                    }
                    if (timerIdx > -1)
                        lock (animateTimers)
                        {
                            animateTimers[timerIdx] = new System.Timers.Timer(animationTimeInMs);
                            StartTimer(animateTimers[timerIdx]);
                        }
                }
            }
            catch { }
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
                //PaintAnimation(obj, new PaintEventArgs(obj.CreateGraphics(), obj.DisplayRectangle));
            }
            catch(Exception ex)
            {
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
                // Check if any animations use this variable as a trigger
                if (config.Animations.Any(x => x.Triggers.Any(y=> y is AnimationTriggerClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == value.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == value.Request.Unit)))
                {
                    // We want to update the animation every 0.3 seconds - determine how much we should move it
                    animationStepCount = (animationTimeInMs / animationUpdateInMs);
                    // Modify the step size for our control
                    lock (animationSteps)
                        animationSteps[resultIdx] = ((double)(currentResults[resultIdx].Result ?? 0.0) - (double)(lastResult.Result ?? 0.0)) / animationStepCount;
                    // Start timer (or Restart timer if already running), to modify the animation speed correctly and start animating
                    var animationTime = animationTimeInMs / animationStepCount; // Expected Update Time divided by number of animation steps we want within that time
                    // This will cause ExecuteAnimation to run, which should update the Last Value by Step Size & trigger Paint Event for any controls relying on this variable to update
                    lock (animateTimers)
                    {
                        lastExecution = DateTime.Now;
                        animateTimers[resultIdx] = new System.Timers.Timer(animationTime);
                        StartTimer(animateTimers[resultIdx]);
                    }
                }
            }
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
