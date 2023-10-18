using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace ProjekatGrafovi
{
    public class GraphParameterRead
    {
        public void AddNewVertex(string[] verticlesSplit, string VerticlesString, string EdgesString, TextBox verticles)
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
                    int x = 0;
                    int y = 0;
                    MainWindow.allVerticles.Add(id, new Cvor(id, x, y));
                    MainWindow.verticlesList.Add(new Cvor(id, x, y));
                }
            }
        }

        public void AddNewEdge(string[] edgesSplit, string VerticlesString, string EdgesString, TextBox edges)
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
                    if (prviID == drugiID)
                    {
                        Grana g = new Grana(MainWindow.allVerticles[prviID], MainWindow.allVerticles[prviID]);
                        MainWindow.edgesList.Add(g);
                    }
                    else
                    {
                        if (!MainWindow.allVerticles.ContainsKey(prviID) || !MainWindow.allVerticles.ContainsKey(drugiID))
                        {
                            MessageBox.Show($"Grana sa cvorovima {prviID} i {drugiID} sadrzi neinicijalizovan cvor!");
                            VerticlesString = "";
                            EdgesString = "";
                            return;
                        }
                        else
                        {
                            Grana g = new Grana(MainWindow.allVerticles[prviID], MainWindow.allVerticles[drugiID]);
                            MainWindow.edgesList.Add(g);
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
                        MainWindow.allVerticles.Add(prviID, new Cvor(prviID, x, y));
                        MainWindow.verticlesList.Add(new Cvor(prviID, x, y));
                        uniqueVertex.Add(prviID, new Cvor(prviID, x, y));
                    }
                    if (!uniqueVertex.ContainsKey(drugiID))
                    {
                        MainWindow.allVerticles.Add(drugiID, new Cvor(drugiID, x, y));
                        MainWindow.verticlesList.Add(new Cvor(drugiID, x, y));
                        uniqueVertex.Add(drugiID, new Cvor(drugiID, x, y));
                    }
                }
            }
        }
    }
}
