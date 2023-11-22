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
				"Custom Coordinates",
                "Circular Layout",
                "New Sugiyama layout",
                "Old Sugiyama layout",
            };
            layout.ItemsSource = LayoutOptions;
            DataContext = this;
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
			verticles.IsEnabled = true;
			edges.IsEnabled = true;
			verticles.Text = "";
			verticles.Text = "";
			edgesList.Clear();
			allVerticles.Clear();
			verticlesList.Clear();
		}

        private void Generisi_Click(object sender, RoutedEventArgs e)
		{
            newWindow = new CanvasWindow();
            if (verticles.Text == "Nodes generated from file")
			{
                ClearCanvas(newWindow.canvasWindow);
                if (layout.SelectedItem != null)
                {
                    ChooseLayoutOption(layout.SelectedItem.ToString());

                    verticles.BorderBrush = Brushes.AliceBlue;
                    edges.BorderBrush = Brushes.AliceBlue;

                  /*  FileClass fc = new FileClass();

                    int idSCV = fc.ReadNumberTxt();
                    fc.SaveToNewFastGenerateTxt(edgesString, idSCV);
                    MessageBox.Show($"Your graph parameters are succesfully saved in file FastGraph{idSCV}.txt"); */

					//otkomentarisi kasnije

					newWindow.Show();

                    StartAgain();
                }
                else
                {
                    MessageBox.Show("You need to choose layout first!");
                }
            }
			else
			{
                if (!ValidationNoEmptySpace())
                {
                    MessageBox.Show("You can't leave empty spaces for verticles or edges!", "Error - empty spaces", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                  /*  FileClass fc = new FileClass();

                    int idSCV = fc.ReadNumberTxt();
                    fc.SaveToNewFastGenerateTxt(edgesString, idSCV);
                    MessageBox.Show($"Your graph parameters are succesfully saved in file FastGraph{idSCV}.txt"); */

					//otkomentarisati kasnije

					newWindow.Show();

                    StartAgain();
                }
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

			/*if(layout.SelectedItem != null)
			{
                ChooseLayoutOption(layout.SelectedItem.ToString());
            }
			else
			{
				MessageBox.Show("You need to choose layout first!");
			}
            newWindow.Show();
            
            StartAgain(); */

			verticles.Text = "Nodes generated from file";
			verticles.IsEnabled = false;
			edges.Text = "Edges generated from file";
			edges.IsEnabled = false;
        }

        private void CoordinatesChoose_Click(object sender, RoutedEventArgs e)
        {
			ClearCanvas(newWindow.canvasWindow);

			string fileName = OpenFile();
			if (fileName == null) return;

			string allCoordinates = File.ReadAllText(fileName);

            GraphParameterRead gpr = new GraphParameterRead();

          /*  string[] cvoroviSplit = VerticlesString.Split(',');

            gpr.AddNewVertex(cvoroviSplit, VerticlesString, EdgesString, verticles);

            string[] graneSplit = edgesString.Split(';');

            gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString, edges); */

			string[] coordinateSplit = allCoordinates.Split(';');
			gpr.SetNodesCoordinates(coordinateSplit);

			/*DrawingGraph dg = new DrawingGraph();
			dg.DrawGraph(newWindow.canvasWindow, verticlesList, edgesList);

            newWindow.Show(); */
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
				case "Custom Coordinates":
					//do something
					algorithmClass.CustomCoordinates(verticlesList, edgesList, newWindow.canvasWindow);
					break;
                default:
                    MessageBox.Show("You have to choose layout first!");
                    break;
            }
        }

        private void layout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if(layout.SelectedItem.ToString() == "Custom Coordinates")
			{
				CoordinatesChoose.IsEnabled = true;
			}
			else
			{
				CoordinatesChoose.IsEnabled = false;
			}
        }
    }
}
