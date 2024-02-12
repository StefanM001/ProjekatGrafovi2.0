using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using ProjekatGrafovi.Model;
using ProjekatGrafovi.ViewModel;

namespace ProjekatGrafovi
{
    public class GraphParameterRead
    {
        public void AddNewVertex(string[] verticlesSplit, string VerticlesString, string EdgesString)
        {
            for (int i = 0; i < verticlesSplit.Length; i++)
            {
                int id;
                if (!Int32.TryParse(verticlesSplit[i], out id))
                {
                    MessageBox.Show("Input was not in correct form", "Add new vertex", MessageBoxButton.OK, MessageBoxImage.Error);
                    VerticlesString = "";
                    EdgesString = "";
                    //verticles.BorderBrush = Brushes.Red;
                    //verticles.BorderThickness = new Thickness(3);
                    return;
                }
                else
                {
                    int x = 0;
                    int y = 0;
                    AddNewGraphViewModel.allVerticles.Add(id, new Cvor(id, x, y));
                    AddNewGraphViewModel.verticlesList.Add(new Cvor(id, x, y));
                }
            }
        }

        public void AddNewEdge(string[] edgesSplit, string VerticlesString, string EdgesString)
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
                  //  edges.BorderBrush = Brushes.Red;
                  //  edges.BorderThickness = new Thickness(5);
                    return;
                }
                else
                {
                    if (prviID == drugiID)
                    {
                        Grana g = new Grana(AddNewGraphViewModel.allVerticles[prviID], AddNewGraphViewModel.allVerticles[prviID]);
                        AddNewGraphViewModel.edgesList.Add(g);
                    }
                    else
                    {
                        if (!AddNewGraphViewModel.allVerticles.ContainsKey(prviID) || !AddNewGraphViewModel.allVerticles.ContainsKey(drugiID))
                        {
                            MessageBox.Show($"Grana sa cvorovima {prviID} i {drugiID} sadrzi neinicijalizovan cvor!");
                            VerticlesString = "";
                            EdgesString = "";
                            return;
                        }
                        else
                        {
                            Grana g = new Grana(AddNewGraphViewModel.allVerticles[prviID], AddNewGraphViewModel.allVerticles[drugiID]);
                            AddNewGraphViewModel.edgesList.Add(g);
                        }
                    }
                }
            }
        }

        public void FastVertexAdd(string[] edgesSplit)
        {
            Dictionary<int, Cvor> uniqueVertex = new Dictionary<int, Cvor>();

            for (int i = 0; i < edgesSplit.Count(); i++)
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
                    if (!uniqueVertex.ContainsKey(prviID))
                    {
                        AddNewGraphViewModel.allVerticles.Add(prviID, new Cvor(prviID, x, y));
                        AddNewGraphViewModel.verticlesList.Add(new Cvor(prviID, x, y));
                        uniqueVertex.Add(prviID, new Cvor(prviID, x, y));
                    }
                    if (!uniqueVertex.ContainsKey(drugiID))
                    {
                        AddNewGraphViewModel.allVerticles.Add(drugiID, new Cvor(drugiID, x, y));
                        AddNewGraphViewModel.verticlesList.Add(new Cvor(drugiID, x, y));
                        uniqueVertex.Add(drugiID, new Cvor(drugiID, x, y));
                    }
                }
            }
        }

        public void SetNodesCoordinates(string[] coordinateSplit)
        {
            for(int i = 0; i < coordinateSplit.Length; i++)
            {
                string[] numbers = coordinateSplit[i].Split(',');

                if (!Double.TryParse(numbers[0], out double X) || !Double.TryParse(numbers[1], out double Y))
                {
                    MessageBox.Show("Error in parsing coordinates!");
                }
                else
                {
                    X = Math.Abs(X - CanvasWindow.startPointX);
                    Y = Math.Abs(Y - CanvasWindow.startPointY);
                    AddNewGraphViewModel.verticlesList[i].X = X;
                    AddNewGraphViewModel.verticlesList[i].Y = Y;
                    AddNewGraphViewModel.allVerticles[AddNewGraphViewModel.verticlesList[i].Id].X = X;
                    AddNewGraphViewModel.allVerticles[AddNewGraphViewModel.verticlesList[i].Id].Y = Y;

                    foreach (Grana edge in AddNewGraphViewModel.edgesList)
                    {
                        if (AddNewGraphViewModel.allVerticles.TryGetValue(edge.prvi.Id, out Cvor sourceNode) &&
                            AddNewGraphViewModel.allVerticles.TryGetValue(edge.drugi.Id, out Cvor targetNode))
                        {
                            edge.prvi.X = sourceNode.X;
                            edge.prvi.Y = sourceNode.Y;
                            edge.drugi.X = targetNode.X;
                            edge.drugi.Y = targetNode.Y;
                        }
                    }
                }
            }
        }
    }
}
