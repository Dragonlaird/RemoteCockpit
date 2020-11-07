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

namespace InstrumentPlugins
{
    public class Generic_Instrument : ICockpitInstrument
    {
        private Configuration config;
        private string configPath;
        private System.Timers.Timer animateTimer;
        private double aspectRatio;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private double scaleFactor = 1;
        private List<ClientRequestResult> previousResults = new List<ClientRequestResult>();
        private List<ClientRequestResult> currentResults = new List<ClientRequestResult>();

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
                Control.BackgroundImage = backgroundImage;
            }
            catch(Exception ex) { }
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.Height = controlHeight;
            Control.Width = controlWidth;
            previousResults = new List<ClientRequestResult>();
            if (config.Animations != null)
            {
                foreach (var clientRequest in config?.ClientRequests)
                {
                    previousResults.Add(new ClientRequestResult { Request = clientRequest, Result = (object)-1 });
                    currentResults.Add(new ClientRequestResult { Request = clientRequest, Result = (object)0 });
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
                        imagePanel.BackColor = Color.Transparent;
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
                    image = LoadImage(((AnimationImage)animation).ImagePath);
                }
                ((Control)sender).BackgroundImage = image;
                // Record the new value so we don't redraw again
            }
        }

        private Image LoadImage(string imagePath)
        {
            var imageFile = File.OpenRead(imagePath);
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

        public string[] Layouts => config?.Aircraft;

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
            var currenResult = currentResults.FirstOrDefault(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit);
            if (lastResult != null)
            {
                if (currentResults != null)
                    lastResult.Result = currenResult.Result;
                lastResult.Result = value.Result;
                // Check if any animations use this ClientRequest, if so - set timer to perform update
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
