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
        private List<ClientRequestResult> lastResults = new List<ClientRequestResult>();

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
                var imageFile = File.OpenRead(config.BackgroundImagePath);
                var image = Image.FromStream(imageFile);
                aspectRatio = (double)image.Height / image.Width;
                var imageScaleFactor = controlHeight < controlWidth ? (double)controlWidth / image.Width : (double)controlHeight / image.Height;
                var backgroundImage = new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor)));
                controlHeight = backgroundImage.Height;
                controlWidth = backgroundImage.Width;
                Control.BackgroundImage = backgroundImage;
            }
            catch(Exception ex) { }
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.Height = controlHeight;
            Control.Width = controlWidth;
            lastResults = new List<ClientRequestResult>();
            if (config.Animations != null)
            {
                foreach (var clientRequest in config.Animations
                    .Where(x => x?.Trigger?.Type == AnimationTriggerTypeEnum.ClientRequest)
                    .Select(x => ((AnimationTriggerClientRequest)x?.Trigger)?.Request).Distinct())
                {
                    lastResults.Add(new ClientRequestResult { Request = clientRequest, Result = (object)null });
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
                Image image = null;
                // We have a foreground to update
                foreach(var animation in config.Animations)
                {
                    if(animation.Item.Type == AnimationItemTypeEnum.Drawing)
                    {
                        image = DrawPoints((AnimationDrawing)animation.Item);
                    }
                    if(animation.Item.Type == AnimationItemTypeEnum.Image)
                    {
                        image = LoadImage((AnimationImage)animation.Item);
                    }
                    var picture = new PictureBox();
                    picture.Name = animation.Item.Name;
                    picture.Image = image;
                    if (Control.Controls[animation.Item.Name] != null)
                        Control.Controls.RemoveByKey(animation.Item.Name);
                    Control.Controls.Add(picture);
                }
            }
        }

        private Image LoadImage(AnimationImage item)
        {
            var imageFile = File.OpenRead(item.ImagePath);
            var image = Image.FromStream(imageFile);
            var imageScaleFactor = (double)Control.Width / image.Width;
            aspectRatio = (double)image.Height / image.Width;
            if (image.Height * imageScaleFactor > Control.Height)
                imageScaleFactor = (double)Control.Height / image.Height;
            var resizedImage = new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor)));
            return resizedImage;
        }

        private Image DrawPoints(AnimationDrawing item)
        {
            var points = item.PointMap;
            if (points?.Count() > 0)
            {
                var imageHolder = new PictureBox();
                imageHolder.Top = 0;
                imageHolder.Left = 0;
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
                var absoluteX = imageHolder.Width * (float)100 / item.RelativeX;
                var absoluteY = imageHolder.Height * (float)100 / item.RelativeY;
                var scaleX = item.ScaleSize * imageHolder.Width / absoluteX;
                var scaleY = item.ScaleSize * imageHolder.Height / absoluteY;

                gp.AddLines(RemapPoints(points, (float)absoluteX, (float)absoluteY, (float)scaleX, (float)scaleY));
                graph.FillPath(pen.Brush, gp);
                return bitmap;
            }
            return null;
        }

        public PointF[] RemapPoints(PointF[] points, float absoluteX, float absoluteY, float scaleX, float scaleY)
        {
            List<PointF> remappedPoints = new List<PointF>();
            foreach(var point in points)
            {
                remappedPoints.Add(new PointF
                {
                    X = (point.X * scaleX) + absoluteX,
                    Y = (point.Y * scaleY) + absoluteY
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
            var lastResult = lastResults.FirstOrDefault(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit);
            if (lastResult != null)
            {
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
                    lastResults?.Clear();
                    lastResults = null;
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
