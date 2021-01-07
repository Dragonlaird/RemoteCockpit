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
using System.Net.Http;

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
        private bool disposedValue;
        private List<ClientRequestResult> previousResults = new List<ClientRequestResult>();
        private List<ClientRequestResult> currentResults = new List<ClientRequestResult>();
        //private List<System.Timers.Timer> animateTimers = new List<System.Timers.Timer>();
        private System.Timers.Timer animateTimer;
        private List<double> animationSteps = new List<double>();
        private List<Bitmap> animationImages = new List<Bitmap>();
        private delegate void SafeUpdateDelegate(object sender, PaintEventArgs e);
        private int animationTimeInMs = 3000;
        private int animationStepCount = 10;
        private int animationUpdateInMs = 300;
        private DateTime lastExecution = DateTime.MinValue;
        public event EventHandler Disposed;
        public event EventHandler<string> LogMessage;
        private bool isStarting = true;
        public Generic_Instrument()
        {
            config = new Configuration();
            configPath = null;
            SetLayout(0, 0, 100, 100);
        }

        public Generic_Instrument(int maxHeight, int maxWidth)
        {
            config = new Configuration();
            configPath = null;
            SetLayout(0, 0, maxHeight, maxWidth);
        }

        public Generic_Instrument(int maxHeight, int maxWidth, string filePath)
        {
            configPath = filePath;
            config = null;
            SetLayout(0, 0, maxHeight, maxWidth);
        }

        public Generic_Instrument(int maxHeight, int maxWidth, Configuration configuration)
        {
            config = configuration;
            configPath = null;
            SetLayout(0, 0, maxHeight, maxWidth);
        }


        private void Initialize()
        {
            isStarting = true;
            UpdateStepCount(animationTimeInMs);
            if (Control == null)
                Control = new Panel();
            try
            {
                if (!string.IsNullOrEmpty(configPath))
                    config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));
                scaleFactor = 1.0;
                var image = LoadImage(config.BackgroundImagePath);
                if (image != null)
                {
                    aspectRatio = (double)image.Height / image.Width;
                    scaleFactor = controlHeight < controlWidth ? (double)controlWidth / image.Width : (double)controlHeight / image.Height;
                }
                //var backgroundImage = LoadImage(config.BackgroundImagePath); //new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
                //controlHeight = image.Height;
                //controlWidth = image.Width;
                Control.Height = controlHeight;
                Control.Width = controlWidth;
                Control.BackColor = Color.Transparent;
                Control.BackgroundImage = image;
                Control.BackgroundImageLayout = ImageLayout.Stretch;
                if (Control?.Controls != null && Control.Controls["Animation"] != null)
                    Control.Controls["Animation"].Dispose();
                Control.Controls.Add(new PictureBox { Name = "Animation", Height = controlHeight, Width = controlWidth });
            }
            catch (Exception ex)
            {
                WriteLog("Initialize: Failed to populate Config.", ex);
            }
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.Height = controlHeight;
            Control.Width = controlWidth;
            Control.Name = config?.Name?.Replace(' ', '_');
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
                foreach (var clientRequest in config?.ClientRequests)
                {
                    if (!currentResults.Any(x => x.Request.Name == clientRequest.Name && x.Request.Unit == clientRequest.Unit))
                    {
                        currentResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                        previousResults.Add(new ClientRequestResult { Request = clientRequest, Result = (double)0 });
                        //animateTimers.Add(null);
                        animationSteps.Add(1);
                    }
                }
                animationImages = new List<Bitmap>();
                foreach (var animation in config.Animations)
                {
                    switch (animation.Type)
                    {
                        case AnimationItemTypeEnum.Image:
                            // If this is an Image Animation - pre-fetch the image into the cache
                            animationImages.Add((Bitmap)LoadImage(((AnimationImage)animation).ImagePath));
                            break;
                        case AnimationItemTypeEnum.Drawing:
                            // If this is a Drawing Animation - generate the image and add to the cache
                            animationImages.Add(DrawPoints((AnimationDrawing)animation));
                            break;
                        case AnimationItemTypeEnum.External:
                            // If this is an External Animation - fetch, update the image and add to the cache
                            animationImages.Add(LoadImageFromRemote((AnimationExternal)animation));
                            break;
                    }
                }

                Control.Paint += PaintControl;
                Control.Invalidate(true);
                Control.Update();
            }
        }

        private Bitmap LoadImageFromRemote(AnimationExternal animation)
        {
            try
            {
                // Check if image is already in cache, with surrounding images - If not, fetch any that are missing

                //https://api.mapbox.com/styles/v1/mapbox/outdoors-v11/static/-3.7375,56.035,11,0/300x200?access_token=YOUR_MAPBOX_ACCESS_TOKEN
                //https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/\[-77.043686,38.892035,-77.028923,38.904192\]/400x400?access_token=YOUR_MAPBOX_ACCESS_TOKEN
                var remoteUrl = animation.RemoteURL;
                var requestFormat = animation.RequestFormat;
                // The following values are static and should only be applied once
                foreach (var property in this.GetType().GetProperties())
                {
                    var placeholder = "{" + property.Name + "}";
                    if (remoteUrl.IndexOf(placeholder) > -1 || requestFormat.IndexOf(placeholder) > -1)
                    {
                        if (property.CanRead)
                        {
                            var val = property.GetValue(this)?.ToString();
                            remoteUrl = remoteUrl.Replace(placeholder, val);
                            requestFormat = requestFormat.Replace(placeholder, Uri.EscapeUriString(val));
                        }
                    }
                }
                animation.RemoteURL = remoteUrl;
                animation.RequestFormat = requestFormat;
                foreach (var trigger in animation.Triggers)
                {
                    var placeholder = "{" + trigger.Name + "}";
                    if (remoteUrl.IndexOf(placeholder) > -1 || requestFormat.IndexOf(placeholder) > -1)
                    {
                        string val = null;
                        switch (trigger.Type)
                        {
                            case AnimationTriggerTypeEnum.ClientRequest:
                                val = currentResults.FirstOrDefault(x => x?.Request?.Name == ((AnimationTriggerClientRequest)trigger).Request.Name)?.Result?.ToString();
                                break;
                            case AnimationTriggerTypeEnum.MouseClick:
                                val = currentResults.FirstOrDefault(x => x?.Request?.Name == trigger.Name)?.Result?.ToString() ?? "2";
                                break;
                        }
                        if (val == null)
                        {
                            val = "";
                        }
                        remoteUrl = remoteUrl.Replace(placeholder, val);
                        requestFormat = requestFormat.Replace(placeholder, Uri.EscapeUriString(val));
                    }
                }
                foreach (var property in animation.GetType().GetProperties())
                {
                    var placeholder = "{" + property.Name + "}";
                    if (remoteUrl.IndexOf(placeholder) > -1 || requestFormat.IndexOf(placeholder) > -1)
                    {
                        if (property.CanRead)
                        {
                            var val = property.GetValue(animation)?.ToString() ?? "1";
                            remoteUrl = remoteUrl.Replace(placeholder, val);
                            requestFormat = requestFormat.Replace(placeholder, Uri.EscapeUriString(val));
                        }
                    }
                }
                foreach (var property in Control.GetType().GetProperties())
                {
                    var placeholder = "{" + property.Name + "}";
                    if (remoteUrl.IndexOf(placeholder) > -1 || requestFormat.IndexOf(placeholder) > -1)
                    {
                        if (property.CanRead)
                        {
                            var val = property.GetValue(Control)?.ToString() ?? "1";
                            remoteUrl = remoteUrl.Replace(placeholder, val);
                            requestFormat = requestFormat.Replace(placeholder, Uri.EscapeUriString(val));
                        }
                    }
                }
                // We should have the URL for our remote service - fetch the image
                Image image = null;
                HttpResponseMessage httpResponse;
                if(remoteUrl.IndexOf("Calculate{") > -1 && remoteUrl.IndexOf("}") > remoteUrl.IndexOf("Calculate{"))
                {
                    // Looks like our URL still contains some non-static values - perform a calculation to retrieve the value(s) required
                    remoteUrl = Calculate(remoteUrl);
                }
                if (requestFormat.IndexOf("Calculate{") > -1 && requestFormat.IndexOf("}") > remoteUrl.IndexOf("Calculate{"))
                {
                    // Looks like our Request still contains some non-static values - perform a calculation to retrieve the value(s) required
                    requestFormat = Calculate(requestFormat);
                }
                WriteLog(string.Format("Remote Image Request: From: {0}{1} ({2})", remoteUrl, requestFormat, animation.RequestMethod));
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(remoteUrl);

                    switch (animation.RequestMethod.ToString())
                    {
                        case "POST":
                            httpResponse = client.PostAsync(remoteUrl, new StringContent(requestFormat)).Result;
                            break;
                        default:
                            httpResponse = client.GetAsync(string.Format("{0}{1}", remoteUrl, requestFormat)).Result;
                            break;

                    }
                }
                if (httpResponse.IsSuccessStatusCode && (httpResponse.Content.Headers.ContentType.MediaType?.ToLower().StartsWith("image") ?? false))
                {
                    image = Image.FromStream(httpResponse.Content.ReadAsStreamAsync().Result);
                }
                else
                {
                    WriteLog(string.Format("Remote Image Failed: Response: {0} ({1})", httpResponse.StatusCode, httpResponse.ReasonPhrase));
                }
                if (image != null)
                {
                    return (Bitmap)image;
                }
            }
            catch(Exception ex)
            {
                WriteLog("Remote Image Request: Failed", ex);
            }
            return null;
        }

        /// <summary>
        /// If a URL or Parameters need to perform a calculation, extract the values and operators to replace the content with the result
        /// </summary>
        /// <param name="initialString">URL or Parameters containing the calculation(s) to perform</param>
        /// <returns>initialString with all calculations replaced by results</returns>
        private string Calculate(string initialString)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            while(initialString.IndexOf("Calculate{")>-1 && initialString.IndexOf("}")> initialString.IndexOf("Calculate{"))
            {
                double result = 0;
                var rawCalculation = initialString.Substring(initialString.IndexOf("Calculate{"), 1 + initialString.IndexOf("}") - initialString.IndexOf("Calculate{"));
                var actualCalculation = rawCalculation.Substring(rawCalculation.IndexOf("{") + 1, rawCalculation.IndexOf("}") - rawCalculation.IndexOf("{") - 1);
                try
                {
                    result = double.Parse(dt.Compute(actualCalculation, "").ToString());
                }
                catch(Exception ex)
                {
                    // Cannot perform calculation
                    WriteLog(string.Format("Unable to perform calculation on: {0}", actualCalculation), ex);
                }
                initialString = initialString.Replace(rawCalculation, result.ToString());
            }
            return initialString;
        }

        private void PaintControl(object sender, PaintEventArgs e)
        {
            if (!disposedValue && ShouldUpdate())
            {
                if (config?.Animations != null)
                {
                    // We have a foreground to update
                    Bitmap animationImage = new Bitmap(Control.Width, Control.Height, PixelFormat.Format32bppArgb);
                    animationImage.MakeTransparent();
                    lock (config.Animations)
                    {
                        foreach (var animation in config.Animations)
                        {
                            try
                            {
                                var overlapImage = GenerateAnimationImage(animation);
                                try
                                {
                                    if (animationImage != null && overlapImage != null)
                                        animationImage = (Bitmap)Overlap((Image)animationImage, overlapImage);
                                }
                                catch (Exception ex)
                                {
                                    WriteLog("Paint Control: Failed to Overlay Animation Images.", ex);
                                }
                                //overlapImage.Dispose();
                            }
                            catch (Exception ex)
                            {
                                WriteLog("PaintControl: Failed to Generate Animation Image.", ex);
                            }
                        }
                    }
                    UpdateAnimationImage(animationImage);
                }
            }
        }

        private bool ShouldUpdate()
        {
            var result = isStarting || currentResults.Any(x => previousResults.First(y => y.Request.Name == x.Request.Name && y.Request.Unit == x.Request.Unit).Result?.ToString() != x.Result?.ToString());
            if (isStarting)
                isStarting = false;
            return result;
        }

        private void UpdateAnimationImage(Image animationImage)
        {
            if (Control.InvokeRequired)
            {
                Control.Invoke(new MethodInvoker(delegate
                {
                    ((PictureBox)Control.Controls["Animation"]).Image = (Image)animationImage;
                }));
                return;
            }
            ((PictureBox)Control.Controls["Animation"]).Image = (Image)animationImage;
        }

        private Image GenerateAnimationImage(IAnimationItem animation)
        {
            var animationId = config.Animations.ToList().IndexOf(animation);
            var triggers = animation.Triggers.Where(x => x.Type == AnimationTriggerTypeEnum.ClientRequest).Select(x => (AnimationTriggerClientRequest)x).ToArray(); Bitmap initialImage;
            lock (animationImages)
            {
                // Fetch the image we want to animate
                initialImage = animationImages[animationId];
                foreach (var trigger in triggers)
                {
                    var nextValue = animation.LastAppliedValue ?? 0;
                    if (trigger is AnimationTriggerClientRequest)
                    {
                        nextValue = previousResults.First(x => x.Request.Name == trigger.Request.Name && x.Request.Unit == trigger.Request.Unit).Result;
                    }
                    animation.LastAppliedValue = nextValue;
                    lock (config.Animations)
                    {
                        config.Animations[animationId] = animation;
                    }
                    foreach (var action in trigger.Actions)
                    {
                        if (action is AnimationActionRotate)
                        {
                            // Rotate our control background, either clockwise or counter-clockwise, depending on the value of te Request Result
                            var rotateAction = (AnimationActionRotate)action;
                            var displayVal = (double)nextValue % rotateAction.MaximumValueExpected;
                            var rotateAngle = (float)((360 * displayVal) / rotateAction.MaximumValueExpected);
                            var centrePoint = new PointF { X = initialImage.Width * rotateAction.CentrePoint.X / 100, Y = initialImage.Height * rotateAction.CentrePoint.Y / 100 };
                            initialImage = RotateImage(initialImage, centrePoint, rotateAngle);
                        }
                        if (action is AnimationActionClip)
                        {
                            // Clip a circle or square using the 2 points to mark the outer edge or top-left/bottom-right
                            initialImage = ClipImage(initialImage, ((AnimationActionClip)action).Style, ((AnimationActionClip)action).StartPoint, ((AnimationActionClip)action).EndPoint);
                        }
                        if(action is AnimationActionMove)
                        {
                            var moveAction = (AnimationActionMove)action;
                            bool moveVertically = moveAction.Type == AnimationActionTypeEnum.MoveY;
                            var moveAmount = moveAction.Invert ? -1 : 1 * (double)nextValue % moveAction.MaxValue / moveAction.MaxValue;
                            //var moveAmount = moveAction.Invert ? -1 : 1 * ((double)nextValue % moveAction.MaxValue);
                            initialImage = MoveImage(initialImage, moveVertically, moveAmount);
                        }
                    }
                }
            }
            return initialImage;
        }

        public Bitmap MoveImage(Bitmap initialImage, bool moveVertically, double movePercent)
        {
            // movePercent has a range of -1 to 1 (equiv. to -100% to +100%)
            var moveAbsolute = movePercent * (moveVertically ? initialImage.Height : initialImage.Width);
            // Create a blank image for us to populate with the result, same dimensions as the original image
            Bitmap target = new Bitmap(initialImage.Width, initialImage.Height);
            target.MakeTransparent();
            // Create an oversized image as a canvas, to allow us to position the original image anywhere
            using (Bitmap canvasImage = new Bitmap(
                initialImage.Width * (int)(moveVertically ? 1 : 3),
                initialImage.Height * (int)(moveVertically ? 3 : 1),
                PixelFormat.Format32bppArgb))
            {
                canvasImage.MakeTransparent();
                // Place original image at correct position on our oversized canvas image
                using (var graphics = Graphics.FromImage(canvasImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(initialImage,
                        new Point(
                            moveVertically ? 0 : initialImage.Width + (int)moveAbsolute,
                            moveVertically ? initialImage.Height + (int)moveAbsolute : 0
                            )
                        );
                }
                // Now crop the image back to the original size from the centre of our canvas image
                Rectangle srcRect = new Rectangle(
                            moveVertically ? 0 : initialImage.Width,
                            moveVertically ? initialImage.Height : 0,
                            initialImage.Width,
                            initialImage.Height);
                target = (Bitmap)canvasImage.Clone(srcRect, canvasImage.PixelFormat);
            }
            return target;
        }

        public Bitmap Overlap(Image source1, Image source2)
        {
            var target = new Bitmap(Control.Width, Control.Height, PixelFormat.Format32bppArgb);
            target.MakeTransparent();
            using (var graphics = Graphics.FromImage(target))
            {
                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                graphics.DrawImage(source1, 0, 0);
                graphics.DrawImage(source2, 0, 0);
            }
            return target;
        }

        private Bitmap ClipImage(Bitmap image, AnimateActionClipEnum style, AnimationPoint start, AnimationPoint end)
        {
            if (image != null)
            {
                Bitmap dstImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
                var onePercentX = image.Width / 100.0f;
                var onePercentY = image.Height / 100.0f;
                var topLeft = new PointF(start.X * onePercentX / 2, start.Y * onePercentY / 2);
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
            return null;
        }

        private Bitmap RotateImage(Bitmap bmp, PointF centrePoint, float angle)
        {
            if (bmp != null)
            {
                Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
                rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

                using (Graphics g = Graphics.FromImage(rotatedImage))
                {
                    // Set the rotation point to the center in the matrix
                    g.TranslateTransform(centrePoint.X, centrePoint.Y);
                    // Rotate
                    g.RotateTransform(angle);
                    // Restore rotation point in the matrix
                    g.TranslateTransform(-centrePoint.X, -centrePoint.Y);
                    // Draw the image on the bitmap
                    g.DrawImage(bmp, new Point(0, 0));
                }

                return rotatedImage;
            }
            return null;
        }


        private void RemoveTimer()
        {
            StopTimer();
            if (animateTimer != null)
            {
                animateTimer.Dispose();
                animateTimer = null;
            }
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
                                    // Reached our target - no need for the timer anymore
                                    RemoveTimer();
                                    previousResult.Result = currentResult.Result;
                                }
                            }
                            if ((double)previousResult.Result > (double)currentResult.Result)
                            {
                                previousResult.Result = (double)previousResult.Result - stepValue;
                                if ((double)previousResult.Result < (double)currentResult.Result)
                                {
                                    // Reached our target - no need for the timer anymore
                                    RemoveTimer();
                                    previousResult.Result = currentResult.Result;
                                }
                            }
                        }
                    }
                }
                UpdateControl();
            }
            catch (Exception ex)
            {
                WriteLog("ExecuteAnimation: Failed to update latest values.", ex);
            }
        }

        private void UpdateControl() {
            // Trigger Paint event of control
            try
            {
                if (Control.InvokeRequired)
                {
                    Control.Invoke(new MethodInvoker(delegate
                    {
                        Control.Invalidate(true);
                        Control.Update();
                        PaintControl(Control, new PaintEventArgs(Control.CreateGraphics(), Control.DisplayRectangle));
                    }));
                }
                else
                {
                    Control.Invalidate(true);
                    Control.Update();
                }
            }
            catch (InvalidOperationException ex)
            {
                // Force Paint event - it will resubmit if invoke is required
                PaintControl(Control, new PaintEventArgs(Control.CreateGraphics(), Control.DisplayRectangle));
            }
            catch (Exception ex)
            {
                // Something else cause a problem - cannot update this control
                WriteLog("UpdateControl: Failed to invoke Control Update.", ex);
            }
        }

        private Image LoadImage(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    var diretory = Directory.GetCurrentDirectory();
                    var imageFile = File.OpenRead(Path.Combine(diretory, imagePath));
                    var image = Image.FromStream(imageFile);
                    aspectRatio = (double)image.Height / image.Width;
                    var resizedImage = new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
                    return resizedImage;
                }
            }
            catch(Exception ex)
            {
                WriteLog("LoadImage: Failed to read/resize Image file.", ex);
            }
            return null;
        }

        private Bitmap DrawPoints(AnimationDrawing animation)
        {
            try
            {
                if (animation.PointMap?.Count() > 0)
                {
                    float absoluteX = 0;
                    float absoluteY = 0;
                    // Resize animation image to fit the current control size
                    var scaleX = animation.OffsetX / 100.0f;
                    var scaleY = animation.OffsetY / 100.0f;
                    // Points can be between -100% to +100% (a range of 200%)- Therefore we assume 1 percent of control is actually 0.5 percent
                    var pixelsPerPercentX = Control.Width / 200.0f;
                    var pixelsPerPercentY = Control.Height / 200.0f;
                    absoluteX = (float)(Control.Width * scaleX);
                    absoluteY = (float)(Control.Height * scaleY);
                    var points = animation.PointMap.Select(x => new PointF(x.Point.X * pixelsPerPercentX + absoluteX, x.Point.Y * pixelsPerPercentY + absoluteY)).ToArray();
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
            }
            catch(Exception ex)
            {
                WriteLog("DrawPoints: Failed to generate image from PointMap.", ex);
            }
            return null;
        }

        public void LoadConfigFile(string filePath)
        {
            configPath = filePath;
            Initialize();
        }

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

        public void SetLayout(int top, int left, int height, int width)
        {
            controlTop = top;
            controlLeft = left;
            controlHeight = height == 0 ? 50 : height;
            controlWidth = width == 0 ? 50 : width;
            Initialize();
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            try
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
                    foreach (var animation in config.Animations)
                    {
                        if (animation is AnimationExternal && animation.Triggers.Any(x => ((AnimationTriggerClientRequest)x).Request.Name == value.Request.Name && ((AnimationTriggerClientRequest)x).Request.Unit == value.Request.Unit))
                        {
                            // Need to update an external image
                            animationImages[config.Animations.ToList().IndexOf(animation)] = LoadImageFromRemote((AnimationExternal)animation);
                        }
                    }
                    // Check if any animations use this variable as a trigger
                    if (config.Animations.Any(x => x.Triggers.Any(y => y is AnimationTriggerClientRequest && ((AnimationTriggerClientRequest)y).Request.Name == value.Request.Name && ((AnimationTriggerClientRequest)y).Request.Unit == value.Request.Unit)))
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
            catch(Exception ex)
            {
                WriteLog("ValueUpdate: Invalid ClientRequestValue supplied.", ex);
            }
        }

        private void UpdateStepCount(int serverUpdateTimeInMs)
        {
            try
            {
                animationTimeInMs = serverUpdateTimeInMs < 100 ? 100 : serverUpdateTimeInMs;
                animationStepCount = animationTimeInMs / animationUpdateInMs > 50 ? 50 : animationTimeInMs / animationUpdateInMs;
                animationStepCount = animationStepCount < 10 ? 10 : animationStepCount;
            }
            catch(Exception ex)
            {
                WriteLog(string.Format("UpdateStepCount: Invalid serverUpdateTime value supplied ({0}).", serverUpdateTimeInMs), ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Prevent any animation updates
                    RemoveTimer();
                    // Clear cuurent config
                    config = null;
                    // Clear last result cache
                    previousResults?.Clear();
                    previousResults = null;
                    // Clear current result cache
                    currentResults?.Clear();
                    currentResults = null;
                    // Clear animation speed cache
                    animationSteps?.Clear();
                    animationSteps = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void WriteLog(string message)
        {
            if(LogMessage != null)
            {
                try
                {
                    LogMessage.DynamicInvoke(new object[] { this, message });
                }
                catch(Exception ex) { }
            }
        }

        private void WriteLog(string message, Exception ex)
        {
            while(ex != null && !string.IsNullOrEmpty(ex.Message))
            {
                message += string.Format("\r\nError: {0}", ex.Message);
                ex = ex.InnerException;
            }
            WriteLog(message);
        }
    }
}
