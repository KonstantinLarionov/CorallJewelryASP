using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string[] filename = openFileDialog1.FileNames;
            Resizer(filename);
            // читаем файл в строку
        }
        private static void Resizer(string[] inputPath)
        {
            const int size = 600;
            const int quality = 75;
            for (int i = 0; i < inputPath.Length; i++)
            {
                using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath[i])))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        if (image.Width <= size)
                        {
                            continue;
                        }
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        if (image.Height <= size)
                        {
                            continue;
                        }
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    var resized = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    FileInfo file = new FileInfo(inputPath[i]);
                    resized.Save("products/" + file.Name); ;
                }
            }
        }
    }
}
