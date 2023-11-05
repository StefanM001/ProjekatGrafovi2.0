using System;
using System.Collections.Generic;
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
        public CanvasWindow()
        {
            InitializeComponent();
            canvasWindow.RenderTransform = translateTransform;
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
