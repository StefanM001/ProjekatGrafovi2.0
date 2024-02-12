using ProjekatGrafovi.Model;
using ProjekatGrafovi.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;

namespace ProjekatGrafovi.ViewModel
{
    public class AddNewGraphViewModel : BindableBase
    {
        #region Initialization

        private ResourceManager resourceManager;
        private string selectedItemLayoutOption;

        private string verticlesString;
        private string edgesString;
        private bool verticlesStringBool = true;
        private bool edgesStringBool = true;
        private bool chooseCoordinatedCommandEnabled = false;
        public static Dictionary<int, Cvor> allVerticles = new Dictionary<int, Cvor>();
        public static List<Cvor> verticlesList = new List<Cvor>();
        public static List<Grana> edgesList = new List<Grana>();
        private List<string> layoutOptions = new List<string>();
        public static CanvasWindow newWindow = new CanvasWindow();

        public MyICommand GenerisiCommand { get; set; }
        public MyICommand CoordinatesChooseCommand { get; set; }
        public MyICommand FastGenerateCommand { get; set; }
        public MyICommand ComboBoxSelectionChangedCommand { get; set; }
        #endregion Initialization

        #region BindingResexValues
        public string MainWindow_Title
        {
            get { return GetResourceString("MainWindow_Title"); }
        }
        public string MainWindow_NodesDefinition
        {
            get { return GetResourceString("MainWindow_NodesDefinition"); }
        }
        public string MainWindow_LayoutOptions
        {
            get { return GetResourceString("MainWindow_LayoutOptions"); }
        }
        public string MainWindow_Generate
        {
            get { return GetResourceString("MainWindow_Generate"); }
        }
        public string MainWindow_FastGenerateText
        {
            get { return GetResourceString("MainWindow_FastGenerateText"); }
        }
        public string MainWindow_FastGenerate
        {
            get { return GetResourceString("MainWindow_FastGenerate"); }
        }
        public string MainWindow_EdgeDefinition
        {
            get { return GetResourceString("MainWindow_EdgeDefinition"); }
        }
        public string MainWindow_CorrectFormNodes
        {
            get { return GetResourceString("MainWindow_CorrectFormNodes"); }
        }

        public string MainWindow_CorrectFormEdges
        {
            get { return GetResourceString("MainWindow_CorrectFormEdges"); }
        }
        public string MainWindow_CoordinatesText
        {
            get { return GetResourceString("MainWindow_CoordinatesText"); }
        }
        public string MainWindow_ChooseCoordinates
        {
            get { return GetResourceString("MainWindow_ChooseCoordinates"); }
        }
        #endregion BindingResexValues

        #region ClassicGettersAndSetters
        public bool ChooseCoordinatedCommandEnabled
        {
            get { return chooseCoordinatedCommandEnabled; }
            set { chooseCoordinatedCommandEnabled = value; OnPropertyChanged("ChooseCoordinatedCommandEnabled"); }
        }
        public bool EdgesStringBool
        {
            get { return edgesStringBool; }
            set { edgesStringBool = value; OnPropertyChanged("EdgesStringBool"); }
        }
        public bool VerticlesStringBool
        {
            get { return verticlesStringBool; }
            set { verticlesStringBool = value; OnPropertyChanged("VerticlesStringBool"); }
        }
        public List<string> LayoutOptions
        {
            get { return layoutOptions; }
            set { layoutOptions = value; OnPropertyChanged(nameof(LayoutOptions)); }
        }
        public string VerticlesString
        {
            get { return verticlesString; }
            set { verticlesString = value; OnPropertyChanged("VerticlesString"); }
        }

        public string EdgesString
        {
            get { return edgesString; }
            set { edgesString = value; OnPropertyChanged("EdgesString"); }
        }

        public string SelectedItemLayoutOption
        {
            get { return selectedItemLayoutOption; }
            set { selectedItemLayoutOption = value; OnPropertyChanged("SelectedItemLayoutOption"); }
        }
        #endregion ClassicGettersAndSetters


        public AddNewGraphViewModel()
        {
            GenerisiCommand = new MyICommand(OnGeneriClick);
            CoordinatesChooseCommand = new MyICommand(OnChooseCoordinates);
            FastGenerateCommand = new MyICommand(OnFastGenerate);
            ComboBoxSelectionChangedCommand = new MyICommand(OnComboBoxSelectionChanged);
            resourceManager = new ResourceManager("ProjekatGrafovi.ResourceFolder.ParametersResource", typeof(AddNewGraphViewModel).Assembly);
            LayoutOptions = new List<string>
            {
                "Custom Coordinates",
                "Circular Layout",
                "New Sugiyama layout",
                "Old Sugiyama layout",
            }; 
        }

        private string GetResourceString(string key)
        {
            string value = resourceManager.GetString(key, CultureInfo.CurrentCulture);

            return value ?? key;
        }


        private bool ValidationNoEmptySpace()
        {
            bool valid = true;

            if (VerticlesString.Equals("") || VerticlesString == null)
            {
                valid = false;
                //Verticles.BorderBrush = Brushes.Red;
                //verticles.BorderThickness = new Thickness(3);
            }

            if (EdgesString.Equals("") || EdgesString == null)
            {
                valid = false;
                //Edges.BorderBrush = Brushes.Red;
                //Edges.BorderThickness = new Thickness(5);
            }


            return valid;
        }

        private void StartAgain()
        {
            VerticlesString = "";
            EdgesString = "";
            VerticlesStringBool = true;
            EdgesStringBool = true;
            edgesList.Clear();
            allVerticles.Clear();
            verticlesList.Clear();
        }

        private void OnGeneriClick()
        {
            newWindow = new CanvasWindow();
            if (VerticlesString == "Nodes generated from file")
            {
                ClearCanvas(newWindow.canvasWindow);
                if (SelectedItemLayoutOption != null)
                {
                    ChooseLayoutOption(SelectedItemLayoutOption);

                    //Verticles.BorderBrush = Brushes.AliceBlue;
                   // Edges.BorderBrush = Brushes.AliceBlue;

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

                    gpr.AddNewVertex(cvoroviSplit, VerticlesString, EdgesString);

                    string[] graneSplit = EdgesString.Split(';');

                    gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString);

                    if (SelectedItemLayoutOption != null)
                    {
                        ChooseLayoutOption(SelectedItemLayoutOption);
                    }
                    else
                    {
                        MessageBox.Show("You need to choose layout first!");
                    }

                    //Verticles.BorderBrush = Brushes.AliceBlue;
                    //Edges.BorderBrush = Brushes.AliceBlue;

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

        private void OnFastGenerate()
        {
            ClearCanvas(newWindow.canvasWindow);

            string fileName = OpenFile();
            if (fileName == null) return;

            string edgesFromTxtFile = File.ReadAllText(fileName);

            string[] graneSplit = edgesFromTxtFile.Split(';');
            GraphParameterRead gpr = new GraphParameterRead();

            gpr.FastVertexAdd(graneSplit);
            gpr.AddNewEdge(graneSplit, VerticlesString, EdgesString);

            VerticlesString = "Nodes generated from file";
            VerticlesStringBool = false;
            EdgesString = "Edges generated from file";
            EdgesStringBool = false;
        }

        private void OnChooseCoordinates()
        {
            ClearCanvas(newWindow.canvasWindow);

            string fileName = OpenFile();
            if (fileName == null) return;

            string allCoordinates = File.ReadAllText(fileName);

            GraphParameterRead gpr = new GraphParameterRead();

            string[] coordinateSplit = allCoordinates.Split(';');
            gpr.SetNodesCoordinates(coordinateSplit);
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
                    algorithmClass.CustomCoordinates(verticlesList, edgesList, newWindow.canvasWindow);
                    break;
                default:
                    MessageBox.Show("You have to choose layout first!");
                    break;
            }
        }

         private void OnComboBoxSelectionChanged()
         {
             if (SelectedItemLayoutOption == "Custom Coordinates")
             {
                ChooseCoordinatedCommandEnabled = true;
             }
             else
             {
                ChooseCoordinatedCommandEnabled = false;
             }
         } 
    }
}
