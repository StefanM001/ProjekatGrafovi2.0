using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjekatGrafovi
{
    /// <summary>
    /// Interaction logic for CanvasWindow.xaml
    /// </summary>
    public partial class CanvasWindow : Window
    {
        private TranslateTransform translateTransform = new TranslateTransform();
        private Point startPoint;
        private Point startOffset;
        private double scale = 1.0;
        public static double startPointX = 0;
        public static double startPointY = 0;
        public CanvasWindow()
        {
            InitializeComponent();
            canvasWindow.RenderTransform = translateTransform;
            AddMapImagesOnCanvas();
            zoomScrollViewer.PreviewMouseWheel += ZoomScrollViewer_PreviewMouseWheel;
        }

        private void AddMapImagesOnCanvas()
        {
            ImageGeoreferenced sajmiste = new ImageGeoreferenced("Sajmiste");
            ImageGeoreferenced liman1 = new ImageGeoreferenced("Liman1");
            ImageGeoreferenced stariGrad = new ImageGeoreferenced("StariGrad");
            ImageGeoreferenced grbavica = new ImageGeoreferenced("Grbavica");

            List<ImageGeoreferenced> listOfDistricts = new List<ImageGeoreferenced> { sajmiste,  liman1, stariGrad, grbavica };

            startPointX = listOfDistricts.Min(item => item.X);
            startPointY = listOfDistricts.Max(item => item.Y);

            PlaceDistrictsOnMap(listOfDistricts, startPointX, startPointY);
        }

        private void PlaceDistrictsOnMap(List<ImageGeoreferenced> listOfDistricts, double startPointX, double startPointY)
        {
            foreach(var district in listOfDistricts)
            {
                Canvas.SetTop(district.georeferencedImage, Math.Abs(district.Y - startPointY));
                Canvas.SetLeft(district.georeferencedImage, Math.Abs(district.X - startPointX));
                canvasWindow.Children.Add(district.georeferencedImage);
            }
        }

        private void ZoomScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
            scale *= zoomFactor;

            if (scale < 0.1)
            {
                scale = 0.1;
            }

            var scaleTransform = new ScaleTransform(scale, scale);
            zoomScrollViewer.LayoutTransform = scaleTransform;

            e.Handled = true;
        }

        private void canvasWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvasWindow.IsMouseCaptured)
            {
                Point currentPoint = e.GetPosition(canvasWindow);
                Vector delta = currentPoint - startPoint;
                translateTransform.X = startOffset.X + delta.X;
                translateTransform.Y = startOffset.Y + delta.Y;
            }
        }

        private void canvasWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvasWindow);
            startOffset = new Point(translateTransform.X, translateTransform.Y);
            canvasWindow.CaptureMouse();
        }

        private void canvasWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvasWindow.ReleaseMouseCapture();
        }
    }
}
