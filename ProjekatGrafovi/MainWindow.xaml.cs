﻿using Microsoft.Win32;
using QuickGraph;
using QuickGraph.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

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
		public static Random rand = new Random();
        Dictionary<int, object> nodes = new Dictionary<int, object>();

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
			DataContext = this;
		}


		public bool ValidationNoEmptySpace()
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

		private void AddNewVertex(string[] verticlesSplit)
		{
			for (int i = 0; i < verticlesSplit.Length; i++)
			{
				int id;
				if (!Int32.TryParse(verticlesSplit[i], out id))
				{
					MessageBox.Show("Input was not in correct form", "Add new vertex", MessageBoxButton.OK, MessageBoxImage.Error);
					VerticlesString = "";
					EdgesString = "";
					verticles.BorderBrush = Brushes.Red;
					verticles.BorderThickness = new Thickness(3);
					return;
				}
				else
				{
					//int x = rand.Next(50, 700);
					//int y = rand.Next(1, 200);
					int x = 0;
					int y = 0;
					allVerticles.Add(id, new Cvor(id, x, y));
					verticlesList.Add(new Cvor(id, x, y));
				}
			}
		}

		private void AddNewEdge(string[] edgesSplit)
		{
			for (int i = 0; i < edgesSplit.Count(); i++)
			{
				string[] numbers = edgesSplit[i].Split(',');

				int prviID;
				int drugiID;

				if (!Int32.TryParse(numbers[0], out prviID) || !Int32.TryParse(numbers[1], out drugiID))
				{
					MessageBox.Show("Input was not in correct form", "Add new edge", MessageBoxButton.OK, MessageBoxImage.Error);
					VerticlesString = "";
					EdgesString = "";
					edges.BorderBrush = Brushes.Red;
					edges.BorderThickness = new Thickness(5);
					return;
				}
				else
				{
					Cvor prvi = new Cvor();
					Cvor drugi = new Cvor();

					if(prviID == drugiID)
					{
						Grana g = new Grana(allVerticles[prviID], allVerticles[prviID]);
						edgesList.Add(g);
					}
					else
					{
                        if (!allVerticles.ContainsKey(prviID) || !allVerticles.ContainsKey(drugiID))
                        {
                            MessageBox.Show($"Grana sa cvorovima {prviID} i {drugiID} sadrzi neinicijalizovan cvor!");
                            VerticlesString = "";
                            EdgesString = "";
                            return;
                        }
                        else
                        {
                            Grana g = new Grana(allVerticles[prviID], allVerticles[drugiID]);
                            edgesList.Add(g);
                        }
                    }					
                }
            }
        }

        private void LayoutGraph(List<Cvor> verticlesList, List<Grana> edges)
        {
            int layerSpacing = 100;
            int nodeSpacing = 50;

            Dictionary<int, Cvor> nodeDictionary = verticlesList.ToDictionary(node => node.Id);

            bool changed;
            do
            {
                changed = false;
                HashSet<int> processedNodes = new HashSet<int>(); // obradjeni cvorovi

                foreach (Grana edge in edges)
                {
                    if (nodeDictionary.TryGetValue(edge.prvi.Id, out Cvor sourceNode) &&
                        nodeDictionary.TryGetValue(edge.drugi.Id, out Cvor targetNode) &&
                        sourceNode.Layer >= targetNode.Layer)
                    {
                        if (processedNodes.Contains(targetNode.Id))
                        {
                            targetNode.Layer = sourceNode.Layer + 1;
                            changed = true;
                        }
                        else
                        {
                            processedNodes.Add(targetNode.Id);
                        }
                    }
                }
            } while (changed);

            int currentX = 30;
            foreach (Cvor node in verticlesList)
            {
                node.X = currentX;
                node.Y = node.Layer * layerSpacing;

                currentX += nodeSpacing;
            }

            foreach (Grana edge in edges)
            {
                if (nodeDictionary.TryGetValue(edge.prvi.Id, out Cvor sourceNode) &&
                    nodeDictionary.TryGetValue(edge.drugi.Id, out Cvor targetNode))
                {
                    edge.prvi.X = sourceNode.X;
                    edge.prvi.Y = sourceNode.Y;
                    edge.drugi.X = targetNode.X;
                    edge.drugi.Y = targetNode.Y;
                }
            }
            FileClass.DrawGraph(canvas, verticlesList, edges);
        }    

        private void Generisi_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidationNoEmptySpace())
			{
				MessageBox.Show("You can't leave empty spaces for verticles or edges!", "Error - empty spaces", MessageBoxButton.OK, MessageBoxImage.Warning) ;
			}
			else
			{
				canvas.Children.Clear();

				string[] cvoroviSplit = VerticlesString.Split(',');

				AddNewVertex(cvoroviSplit);

				string[] graneSplit = edgesString.Split(';');

				AddNewEdge(graneSplit);

				LayoutGraph(verticlesList, edgesList);

                verticles.BorderBrush = Brushes.AliceBlue;
				edges.BorderBrush = Brushes.AliceBlue;

                FileClass fc = new FileClass();

                int idSCV = fc.ReadNumberTxt();
                fc.SaveToNewFastGenerateTxt(edgesString, idSCV);
                MessageBox.Show($"Your graph parameters are succesfully saved in file FastGraph{idSCV}.txt");

                StartAgain();
			}
		}

        private void FastVertexAdd(string[] edgesSplit)
        {
            Dictionary<int, Cvor> uniqueVertex = new Dictionary<int, Cvor>();

            for(int i = 0; i < edgesSplit.Count(); i++)
			{
                string[] numbers = edgesSplit[i].Split(',');

                int prviID;
                int drugiID;

                if (!Int32.TryParse(numbers[0], out prviID) || !Int32.TryParse(numbers[1], out drugiID))
                {
                    MessageBox.Show("We found error while parsing your Txt file in FastGenerate.");
                    return;
                }
                else
                {
                    int x = 0;
                    int y = 0;
                    if(!uniqueVertex.ContainsKey(prviID))
                    {
                        allVerticles.Add(prviID, new Cvor(prviID, x, y));
                        verticlesList.Add(new Cvor(prviID, x, y));
                        uniqueVertex.Add(prviID, new Cvor(prviID, x, y));
                    }
                    if(!uniqueVertex.ContainsKey(drugiID))
                    {
                        allVerticles.Add(drugiID, new Cvor(drugiID, x, y));
                        verticlesList.Add(new Cvor(drugiID, x, y));
                        uniqueVertex.Add(drugiID, new Cvor(drugiID, x, y));
                    }
                }
            }
        }

        private void FastGenerate_Click(object sender, RoutedEventArgs e)
        {
			canvas.Children.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();
            string fileName = "";

            if (result == true)
            {
                fileName = openFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("You didn't choose file correctly. Try again");
                StartAgain();
            }

            string edgesFromTxtFile = File.ReadAllText(fileName);

            string[] graneSplit = edgesFromTxtFile.Split(';');

            FastVertexAdd(graneSplit);
            AddNewEdge(graneSplit);

            LayoutGraph(verticlesList, edgesList);
            StartAgain();
        }



    /*public void CircularLayout(List<Cvor> nodes, List<Grana> edges)
		{
            double centerX = canvas.Width / 2; // X koordinata centra kruga
            double centerY = canvas.Height / 2; // Y koordinata centra kruga
            double radius = Math.Min(canvas.Width, canvas.Height) / 2 - 50; // Rastojanje čvorova od centra kruga

            double angle = 2 * Math.PI / nodes.Count; // Ugao između dva susedna čvora na krugu

			for (int i = 0; i < nodes.Count; i++)
			{
				double theta = i * angle; // Ugao za trenutni čvor

				// Izračunavanje koordinata za čvor na krugu
				double x = centerX + radius * Math.Cos(theta);
				double y = centerY + radius * Math.Sin(theta);

				nodes[i].X = x;
				nodes[i].Y = y;

				if (edges.Find(e => e.prvi.Id == nodes[i].Id) != null)
				{
					edges.Find(e => e.prvi.Id == nodes[i].Id).prvi.X = x;
					edges.Find(e => e.prvi.Id == nodes[i].Id).prvi.Y = y;
				}

				if (edges.Find(e => e.drugi.Id == nodes[i].Id) != null) 
				{
                    edges.Find(e => e.drugi.Id == nodes[i].Id).drugi.X = x;
                    edges.Find(e => e.drugi.Id == nodes[i].Id).drugi.Y = y;
                }
            }

            DrawGraph(nodes, edges);
		} */
}
}