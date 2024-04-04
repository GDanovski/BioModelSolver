using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radial_BandUnB_noPX
{
    class ModelCellTool
    {
        Form myForm;
        Dictionary<string, double[][]> data = new Dictionary<string, double[][]>();
        private BitMapEditor currentImage;
        private int width = 0;
        private int height = 0;

        public ModelCellTool(int width, int height)
        {
            this.width = width;
            this.height = height;
            currentImage = new BitMapEditor(new Bitmap(width, height, PixelFormat.Format32bppArgb));
        }

        public Dictionary<string, double[][]> SetData
        {
            set
            {
                this.data = value;

                if (this.data != null)
                    foreach (var kvp in data)
                    {
                        double scale = ((double)byte.MaxValue - 1d) / (GetMax(kvp));

                        NormalizeToMax(kvp, scale);
                    }
            }
        }
        private double GetMax(KeyValuePair<string, double[][]> kvp)
        {
            double MaxValue = double.MinValue;
            //Get the maximum in all
            foreach (var image in kvp.Value)
                if (image != null)
                    foreach (double val in image)
                        if (val > MaxValue) MaxValue = val;

            return MaxValue;
        }
        private void NormalizeToMax(KeyValuePair<string, double[][]> kvp, double scale)
        {
            //Get the maximum in all
            foreach (var column in kvp.Value)
                if (column != null)
                    for (int i = 0; i < column.Length; i++)
                        column[i] *= scale;
        }
        public void LoadGUI()
        {
            if (data == null || data.Count == 0) return;

            if (myForm != null)
            {
                myForm.Hide();
                myForm.Dispose();
            }
            myForm = new Form()
            {
                Text = "Model cell",
                Width = 400,
                Height = 400
            };

            Panel topPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 70
            };
            myForm.Controls.Add(topPanel);

            Button saveBtn = new Button()
            {
                Text = "Export",
                Location = new Point(5, 5)
            };
            topPanel.Controls.Add(saveBtn);

            Label frameLab = new Label()
            {
                Location = new Point(150, 7),
                Text = "Frame: 0"
            };
            topPanel.Controls.Add(frameLab);

            ComboBox imageNames = new ComboBox()
            {
                Location = new Point(5, 30)
            };

            if (data != null)
            {
                foreach (var kvp in data)
                    imageNames.Items.Add(kvp.Key);

                imageNames.SelectedIndex = 0;
            }
            topPanel.Controls.Add(imageNames);

            TrackBar tb = new TrackBar()
            {
                Minimum = 0,
                Maximum = data.ElementAt(0).Value.Length - 1,
                Value = 0,
                Dock = DockStyle.Bottom,
                TickStyle = TickStyle.None
            };
            myForm.Controls.Add(tb);

            PictureBox picBox = new PictureBox()
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            myForm.Controls.Add(picBox);

            //events
            tb.ValueChanged += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (data == null) return;

                picBox.Image = ResizeImage(picBox.Size, GetBitmap(tb.Value, imageNames.SelectedIndex));

                frameLab.Text = "Frame: " + tb.Value;
                myForm.Update();
                myForm.Invalidate();
                myForm.Refresh();
                Application.DoEvents();
            });
            imageNames.SelectedIndexChanged += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (data == null) return;
                int max = data.ElementAt(imageNames.SelectedIndex).Value.Length - 1;
                if (tb.Value > max) tb.Value = max;
                tb.Maximum = max;

                picBox.Image = ResizeImage(picBox.Size, GetBitmap(tb.Value, imageNames.SelectedIndex));

                frameLab.Text = "Frame: " + tb.Value;
                myForm.Update();
                myForm.Invalidate();
                myForm.Refresh();
                Application.DoEvents();
            });
            saveBtn.Click += new EventHandler(delegate (object sender, EventArgs e)
            {
                if (data == null) return;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save image - " + imageNames.Text;
                saveFileDialog1.Filter = "Tagged image file format(*.tif)|*.tif";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.CheckFileExists = false;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SaveMultiFrameTiff(saveFileDialog1.FileName, imageNames.SelectedIndex);
                }

            });

            picBox.Image = ResizeImage(picBox.Size, GetBitmap(tb.Value, imageNames.SelectedIndex));
            myForm.Show();
            myForm.BringToFront();
        }
        private Bitmap ResizeImage(Size sz, Bitmap bmp)
        {
            int size = Math.Min(sz.Width, sz.Height);
            var zoomed = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(zoomed))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
            }
            return zoomed;
        }
        private Bitmap GetBitmap(int frame, int setIndex)
        {
            try
            {
                if (data == null) return new Bitmap(width, height);

                double[] image = data.ElementAt(setIndex).Value[frame];

                if (image == null) return new Bitmap(width, height);
                currentImage.LockBits();

                PointF center = new Point(image.Length / 2, image.Length / 2);

                Color col = Color.Empty;
                int val = 0;
                for (int x = 0, y = 0; x < width; x++)
                    for (y = 0; y < height; y++)
                    {
                        val = (int)Math.Round(center.X - GetDistance(x, y, center.X, center.Y));
                        if (val < 0) val = 0;

                        val = (int)image[val];

                        if (val < 0) val = 0;
                        else if (val > 255) val = 255;

                        col = Color.FromArgb(255, val, val, val);
                        currentImage.SetPixel(x, y, col);
                    }

                currentImage.UnlockBits();
                return currentImage.GetImage;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return new Bitmap(width, height);
            }
        }
        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public void SaveMultiFrameTiff(string outputFileName, int setIndex)
        {
            try
            {
                Bitmap MasterBitmap = GetBitmap(0, setIndex); //Start page of document(master)
                Image imageAdd = GetBitmap(0, setIndex);  //Frame Image that will be added to the master          
                Guid guid = imageAdd.FrameDimensionsList[0]; //GUID
                FrameDimension dimension = new FrameDimension(guid);

                EncoderParameters ep = new EncoderParameters(1);

                //Get Image Codec Information
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo codecInfo = codecs[3]; //image/tiff

                //MultiFrame Encoding format
                EncoderParameter epMultiFrame = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                ep.Param[0] = epMultiFrame;
                MasterBitmap.SelectActiveFrame(dimension, 0);
                MasterBitmap.Save(outputFileName, codecInfo, ep);//create master document

                //FrameDimensionPage Encoding format
                EncoderParameter epFrameDimensionPage = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                ep.Param[0] = epFrameDimensionPage;

                for (int i = 0; i < data.ElementAt(setIndex).Value.Length; i++)
                {
                    imageAdd.SelectActiveFrame(dimension, i);//select next frame
                    MasterBitmap.SaveAdd(new Bitmap(GetBitmap(i, setIndex)), ep);//add it to the master
                }

                //Flush Encoding format
                EncoderParameter epFlush = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                ep.Param[0] = epFlush;
                MasterBitmap.SaveAdd(ep); //flush the file                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
