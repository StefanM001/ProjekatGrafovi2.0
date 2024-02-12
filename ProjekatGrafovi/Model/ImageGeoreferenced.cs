using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ProjekatGrafovi
{
    public class ImageGeoreferenced : BindableBase
    {
        private double x;
        private double y;
        public Image georeferencedImage;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public ImageGeoreferenced(string imageName)
        {
            var gmapsImagesPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "GMapsImages");
            string imagePath = System.IO.Path.Combine(gmapsImagesPath, imageName + "_Georeferenced.tfw");

            List<double> imageValues = GetValuesFromWorldFile(imagePath);
            double pixelRateImage = imageValues[0];

            X = imageValues[4]; 
            Y = imageValues[5];

            georeferencedImage = new Image();
            Uri georeferencedImageUri = new Uri(@"GMapsImages\" + imageName + "_Georeferenced.tif", UriKind.Relative);
            georeferencedImage.Source = new BitmapImage(georeferencedImageUri);
            georeferencedImage.Width = georeferencedImage.Source.Width * pixelRateImage;
            georeferencedImage.Height = georeferencedImage.Source.Height * pixelRateImage;
        }


        private List<double> GetValuesFromWorldFile(string worldFile)
        {
            string[] values = ReadFromFile(worldFile).Split();
            List<double> valuesInDouble = new List<double>();
            for (int i = 0; i < values.Length - 1; i++)
            {
                valuesInDouble.Add(Double.Parse(values[i]));
            }

            return valuesInDouble;
        }

        private string ReadFromFile(string fileName)
        {
            string ret = "";
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ret += line + " ";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Greška pri čitanju datoteke: " + e.Message);
            }
            return ret;
        }
    }
}
