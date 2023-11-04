using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjekatGrafovi
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>  
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static Dictionary<int, Cvor> allVerticles = new Dictionary<int, Cvor>();
		public static List<Cvor> verticlesList = new List<Cvor>();
		public static List<Grana> edgesList = new List<Grana>();
		public static List<string> LayoutOptions = new List<string>();
        private double scale = 1.0;
        public static CanvasWindow newWindow = new CanvasWindow();

        private string verticlesString;
		public string VerticlesString
		{
			get
			{
				return verticlesString;
			}
			set
			{
				verticlesString = value;
				OnPropertyChanged("VerticlesString");
			}
		}

		private string edgesString;
		public string EdgesString
		{
			get
			{
				return edgesString;
			}
			set
			{
				edgesString = value;
				OnPropertyChanged("EdgesString");
			}
		}


		public MainWindow()
		{
			InitializeComponent();
            LayoutOptions = new List<string>
            {
                "Circular Layout",
                "New Sugiyama layout",
                "Old Sugiyama layout",
            };
            layout.ItemsSource = LayoutOptions;
			AddMapImagesOnCanvas();
            newWindow.zoomScrollViewer.PreviewMouseWheel += ZoomScrollViewer_PreviewMouseWheel;
            DataContext = this;
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
            newWindow.zoomScrollViewer.LayoutTransform = scaleTransform;

            e.Handled = true;
        }

        private void AddMapImagesOnCanvas()
		{
            double xFactor = 800.0 / 2923.0;
            double yFactor = 800.0 / 1906.0;

            double xCoordinateFactor = 800.0 / Math.Abs((2205543.87 - 2210528.30));
            double yCoordinateFactor = 800.0 / Math.Abs((5662452.10 - 5659201.91));

            Uri sajmisteUri = new Uri(@"GMapsImages\Sajmiste_Georeferenced.tif", UriKind.Relative);
            double XSajam = Math.Abs((2205543.87 - 2205543.87));
            double YSajam = Math.Abs((5662452.10 - 5662452.10));
            Image sajmiste = new Image();
            sajmiste.Source = new BitmapImage(sajmisteUri);
            sajmiste.Width = sajmiste.Source.Width * xFactor;
            sajmiste.Height = sajmiste.Source.Height * yFactor;
            Canvas.SetTop(sajmiste, XSajam);
            Canvas.SetLeft(sajmiste, YSajam);
            newWindow.canvasWindow.Children.Add(sajmiste);

             Uri stariGradUri = new Uri(@"GMapsImages\StariGrad_Georeferenced.tif", UriKind.Relative);
             double XStariGrad = Math.Abs((2208082.23 - 2205543.87)) * xCoordinateFactor;
             double YStariGrad = Math.Abs((5662264.45 - 5662452.10)) * yCoordinateFactor;
             Image stariGrad = new Image();
             stariGrad.Source = new BitmapImage(stariGradUri);
             stariGrad.Width = stariGrad.Source.Width * xFactor;
             stariGrad.Height = stariGrad.Source.Height * yFactor;
             Canvas.SetTop(stariGrad, YStariGrad);
             Canvas.SetLeft(stariGrad, XStariGrad);
             newWindow.canvasWindow.Children.Add(stariGrad); 

            double XGrbavica = Math.Abs((2206831.29 - 2205543.87)) * xCoordinateFactor;
            double YGrbavica = Math.Abs((5660900.80 - 5662452.10)) * yCoordinateFactor;

            Uri grbavicaUri = new Uri(@"GMapsImages\Grbavica_Georeferenced.tif", UriKind.Relative);
            Image grbavica = new Image();
            grbavica.Source = new BitmapImage(grbavicaUri);
            grbavica.Width = grbavica.Source.Width * xFactor;
            grbavica.Height = grbavica.Source.Height * yFactor;
            Canvas.SetTop(grbavica, YGrbavica); 
            Canvas.SetLeft(grbavica, XGrbavica - 5);
            newWindow.canvasWindow.Children.Add(grbavica); 

           /* double XLiman1 = Math.Abs((2208442.28 - 2205532.87)) / 6.9;
            double YLiman1 = Math.Abs((5661014.13 - 5662383.91)) / 7;

            Uri liman1Uri = new Uri(@"GMapsImages\Liman1_Georeferenced.tif", UriKind.Relative);
            Image liman1 = new Image();
            liman1.Source = new BitmapImage(liman1Uri);
            liman1.Width = liman1.Source.Width / 4;
            liman1.Height = liman1.Source.Height / 4;
            Canvas.SetTop(liman1, YLiman1);
            Canvas.SetLeft(liman1, XLiman1);
            newWindow.canvasWindow.Children.Add(liman1);  */
        }

        private bool ValidationNoEmptySpace()
		{
			bool valid = true;

			if(VerticlesString.Equals("") || VerticlesString == null)
			{
				valid = false;
				verticles.BorderBrush = Brushes.Red;
				verticles.BorderThickness = new Thickness(3);
			}

			if (EdgesString.Equals("") || EdgesString == null)
			{
				valid = false;
				edges.BorderBrush = Brushes.Red;
				edges.BorderThickness = new Thickness(5);
			} 


			return valid;
		}

		private void StartAgain()
		{
			VerticlesString = "";
			EdgesString = "";
			edgesList.Clear();
			allVerticles.Clear();
			verticlesList.Clear();
		}

        private void Generisi_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidationNoEmptySpace())
			{
				MessageBox.Show("You can't leave empty spaces for verticles or edges!", "Error - empty spaces", MessageBoxButton.OK, MessageBoxImage.Warning) ;
			}
			else
			{
				ClearCanvas(newWindow.canvasWindow);
                GraphParameterRead gpr = new GraphParameterRead();

				string[] cvoroviSplit = VerticlesString.Split(',');

				gpr.AddNewVertex(cvoroviSplit, VerticlesString, EdgesString, verticles);

				string[] graneSplit = edgesString.Split(';');
                
				gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString, edges);

                if (layout.SelectedItem != null)
                {
                    ChooseLayoutOption(layout.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("You need to choose layout first!");
                }

                verticles.BorderBrush = Brushes.AliceBlue;
				edges.BorderBrush = Brushes.AliceBlue;

                FileClass fc = new FileClass();

                int idSCV = fc.ReadNumberTxt();
                fc.SaveToNewFastGenerateTxt(edgesString, idSCV);
                MessageBox.Show($"Your graph parameters are succesfully saved in file FastGraph{idSCV}.txt");

                StartAgain();
			}
		}

        private void FastGenerate_Click(object sender, RoutedEventArgs e)
        {
			ClearCanvas(newWindow.canvasWindow);

			string fileName = OpenFile();
			if(fileName == null) return;

            string edgesFromTxtFile = File.ReadAllText(fileName);

            string[] graneSplit = edgesFromTxtFile.Split(';');
			GraphParameterRead gpr = new GraphParameterRead();

            gpr.FastVertexAdd(graneSplit);
            gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString, edges);

			if(layout.SelectedItem != null)
			{
                ChooseLayoutOption(layout.SelectedItem.ToString());
            }
			else
			{
				MessageBox.Show("You need to choose layout first!");
			}
            newWindow.Show();
            
            StartAgain();
        }

        private void CoordinatesChoose_Click(object sender, RoutedEventArgs e)
        {
			ClearCanvas(newWindow.canvasWindow);

			string fileName = OpenFile();
			if (fileName == null) return;

			string allCoordinates = File.ReadAllText(fileName);

            GraphParameterRead gpr = new GraphParameterRead();

            string[] cvoroviSplit = VerticlesString.Split(',');

            gpr.AddNewVertex(cvoroviSplit, VerticlesString, EdgesString, verticles);

            string[] graneSplit = edgesString.Split(';');

            gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString, edges);

			string[] coordinateSplit = allCoordinates.Split(';');
			gpr.SetNodesCoordinates(coordinateSplit);

			DrawingGraph dg = new DrawingGraph();
			dg.DrawGraph(newWindow.canvasWindow, verticlesList, edgesList);
        }

		private string OpenFile()
		{
            ClearCanvas(newWindow.canvasWindow);
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();
            string fileName = "";

            if (result == true)
            {
                fileName = openFileDialog.FileName;
				return fileName;
            }
            else
            {
                MessageBox.Show("You didn't choose file correctly. Try again");
                StartAgain();
				return null;
            }
        }

		private void ClearCanvas(Canvas canvas)
		{
			canvas.Children.RemoveRange(4, canvas.Children.Count);
		}

        private void ChooseLayoutOption(string layoutSelection)
        {
            AlgorithmClass algorithmClass = new AlgorithmClass();
            switch (layoutSelection)
            {
                case "Circular Layout":
                    algorithmClass.CircularLayout(verticlesList, edgesList, verticlesList, newWindow.canvasWindow);
                    break;
                case "New Sugiyama layout":
                    algorithmClass.NewLayoutGraph(verticlesList, edgesList, newWindow.canvasWindow);
                    break;
                case "Old Sugiyama layout":
                    algorithmClass.OldLayoutGraph(verticlesList, edgesList, newWindow.canvasWindow);
                    break;
                default:
                    MessageBox.Show("You have to choose layout first!");
                    break;
            }
        }
    }
}
