using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteCockpitClasses;

namespace InstrumentPlugins
{
    public class Generic_Attitude_Indicator : ICockpitInstrument
    {
        private Control control = null;
        private bool disposedValue = false;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private List<ClientRequestResult> values = new List<ClientRequestResult>();
        private Point centre;
        private delegate void SafeControlUpdateDelegate(Control ctrl);
        private double CurrentBank = 0;
        private double LastBank = -1;
        private double CurrentPitch = 0;
        private double LastPitch = -1;
        private double needleMoveSpeed = 0;
        System.Timers.Timer animateTimer;

        /// <summary>
        /// Constructor to initialise any private values, set any defaults 
        /// and generally provide a basic (even empty) control for use on the dashboard
        /// </summary>
        public Generic_Attitude_Indicator()
        {
            RedrawControl();
        }

        private void RedrawControl()
        {
            if (control == null)
            {
                control = new PictureBox();
                control.Name = "Generic_Attitude_Indicator";
                control.Top = controlTop;
                control.Left = controlLeft;
                control.BackColor = Color.Transparent;
                control.ForeColor = Color.Transparent;
                control.Paint += Paint;
            }
            DrawControlBackground();
        }

        private void DrawControlBackground()
        {
            var img = ImageLibrary.Attitude_Background;
            var aspectRatio = (double)img.Height / img.Width;
            // Need to rescale control to match the aspect ratio of the image
            var x = (int)(aspectRatio > 1 ? controlHeight : Math.Floor((double)controlWidth / aspectRatio));
            var y = (int)(aspectRatio > 1 ? Math.Floor((double)controlHeight / aspectRatio) : controlWidth);
            var resizedImage = new Bitmap(img, new Size(y, x));

            control.Height = resizedImage.Height;
            control.Width = resizedImage.Width;

            control.BackgroundImage = resizedImage;
            control.BackgroundImageLayout = ImageLayout.Center;

            centre = new Point(control.Width / 2, control.Height / 2);
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            // If we haven't already created the needle, do it now
            if (!control.Controls.ContainsKey("Needle"))
            {
                var needle = new Panel();
                needle.Name = "Needle";
                needle.Height = control.Height;
                needle.Width = control.Width;
                needle.BackColor = Color.Transparent;
                needle.Paint += PaintNeedle;
                control.Controls.Add(needle);
            }
        }

        private void PaintNeedle(object sender, PaintEventArgs e)
        {
            var needle = control.Controls["Needle"];
            var gimbal = ImageLibrary.Attitude_Indicator_Gimbal;
            var resizedImage = new Bitmap(gimbal, new Size(needle.Width, needle.Height));

            // Now move the resized image, based on updated attitude values
            //resizedImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            CurrentBank = 0.35;
            var rotatedImage = RotateImage(resizedImage, (float)CurrentBank, true, false, Color.Transparent);

            // Add the clipped image to our instrument
            var dstImage = ClipToCircle(rotatedImage, centre, 0.7f * (float)resizedImage.Width / 2.0f, Color.Transparent);

            needle.BackgroundImage = dstImage;

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
        public Bitmap RotateImage(Image inputImage, float angleRadians, bool upsizeOk,
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

        /// <summary>
        /// An array or list of SimVar (FS SDK Variables) this instrument needs.
        /// These values will be requsted and returned every time FS provides a new value for each.
        /// </summary>
        public IEnumerable<ClientRequest> RequiredValues
        {
            get
            {
                return new ClientRequest[]
                {
                    new ClientRequest
                    {
                        Name = "ATTITUDE INDICATOR BANK DEGREES",
                        Unit = "radians"
                    },
                    new ClientRequest
                    {
                        Name = "ATTITUDE INDICATOR PITCH DEGREES",
                        Unit = "radians"
                    }

                };
            }
        }

        /// <summary>
        /// A simple array of FS Aircraft names that this instrument can be used with.
        /// </summary>
        public string[] Aircraft
        {
            get
            {
                return new string[] { "" }; // Blank can be used on all layouts
            }
        }

        /// <summary>
        /// The creation date of this plugin (instrument).
        /// This is used to determine the most recent (and therefore most accurate) instrument for each layout.
        /// This instrument may be used for multiple aircraft but others may have produced a newer instrument for a specific aircraft.
        /// </summary>
        public DateTime PluginDate
        {
            get
            {
                return DateTime.MinValue; // Should always use Min Date for a Generic Plugin so it can be overridden by newer plugins
            }
        }

        /// <summary>
        /// The Type of instrument, such as Altimeter, Airspeed Indicator and so on
        /// </summary>
        public InstrumentType Type
        {
            get
            {
                return InstrumentType.Attitude_Indicator;
            }
        }

        /// <summary>
        /// This is the Windows Form Control that will be added to the coskpit dashboard.
        /// It should be resized to fit within the limits that will be supplied via SetLayout.
        /// </summary>
        public Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// Inherited from IComponent - not really used
        /// </summary>
        public ISite Site { get; set; }

        public string Name => "Generic Attitude Indicator";

        public string Author => "Dragonlaird";

        /// <summary>
        /// Notify the cockpit form if the instrument is disposed.
        /// Not currently used by the cockpit form but retained for future use.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Dispose of Cockpit Instrument.
        /// Removes any objects added during the use of this instance before finally disposing itself.
        /// </summary>
        /// <param name="disposing">Indicates if this Cockpit Instrument is already being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Any items that should be disposed of, remove them here
                }
                disposedValue = true;
                if (this.Disposed != null)
                    try
                    {
                        Disposed.DynamicInvoke();
                    }
                    catch { } // Don't care if it failed, it's being removed anyway
            }
        }

        /// <summary>
        /// Dispose of this Cocpkit Instrument instance.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            if(this.Disposed != null)
                try
                {
                    this.Disposed.DynamicInvoke();
                }
                catch { }
        }

        /// <summary>
        /// Set the maximum bounds the Control can fill.
        /// Control must be resized to fit within the supplied limits.
        /// Control should maintain it's own aspect ratio, the aspect ratio of these limits
        /// will be based on the size and position of the dashboard.
        /// Recommend using a Timer to animate the control changes
        /// </summary>
        /// <param name="top">Exact Top position of the Control</param>
        /// <param name="left">Exact Left position of the Control</param>
        /// <param name="height">Maximum height of the Control</param>
        /// <param name="width">Maximum width of the Control</param>
        public void SetLayout(int top, int left, int height, int width)
        {
            controlTop = top;
            controlLeft = left;
            controlHeight = height;
            controlWidth = width;
            if (control != null)
            {
                control.Top = top;
                control.Left = left;
            }
            RedrawControl();
        }

        /// <summary>
        /// Whenever a value associated with one of our RequiredValues is updated by FS,
        /// it will be passed to this control via this method.
        /// This method should provide a low overhead, such as simply adding or updating a local variable with the new value.
        /// It should NOT be used to redraw the control itself.
        /// </summary>
        /// <param name="value">Required Variable Value</param>
        public void ValueUpdate(ClientRequestResult value)
        {
            try
            {
                if (!values.Any(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit))
                    lock (values)
                        values.Add(value);
                else
                    lock (values)
                        values.First(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit).Result = value.Result;
            }
            catch
            {
                // Likely something is being disposed or updated - we'll need to add/update the value next time
            }
        }
    }
}
