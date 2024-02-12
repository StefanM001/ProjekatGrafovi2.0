using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ProjekatGrafovi.Model;

namespace ProjekatGrafovi
{
    public class AlgorithmClass
    {
        public void NewLayoutGraph(List<Model.Cvor> verticlesList, List<Grana> edges, Canvas canvas)
        {
            int layerSpacing = 70;
            // int nodeSpacing = 50;

            Dictionary<int, Cvor> nodeDictionary = verticlesList.ToDictionary(node => node.Id);

            foreach (Grana edge in edges)
            {
                verticlesList.Find(verticle => verticle.Id == edge.prvi.Id).SourceTarget++;
                verticlesList.Find(verticle => verticle.Id == edge.drugi.Id).TargetSource++;
                edge.prvi.SourceTarget++;
                edge.drugi.TargetSource++;
            }

            verticlesList = verticlesList.OrderByDescending(verticle => verticle.SourceTarget)
                                          .ThenBy(verticle => verticle.TargetSource)
                                          .ToList();
            int currentX = 30;

            Cvor firstNode = verticlesList[0];
            firstNode.Layer = 1;
            verticlesList[0].Layer = 1;

            for (int i = 0; i < verticlesList.Count; i++)
            {
                verticlesList[i].Layer = i;
                verticlesList[i].X = currentX * verticlesList[i].Id;
                verticlesList[i].Y = i * layerSpacing;
                if (i > 1)
                {
                    if ((verticlesList[i].SourceTarget == verticlesList[i - 1].SourceTarget) &&
                        (verticlesList[i].TargetSource == verticlesList[i - 1].TargetSource))
                    {
                        verticlesList[i].Layer = i - 1;
                        verticlesList[i].X = currentX * verticlesList[i].Id;
                        verticlesList[i].Y = i * layerSpacing;
                    }
                }
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
            DrawingGraph drawingGraph = new DrawingGraph();

            drawingGraph.DrawGraph(canvas, verticlesList, edges);

        }

        public void OldLayoutGraph(List<Cvor> verticlesList, List<Grana> edges, Canvas canvas)
        {
            int layerSpacing = 100;
            int nodeSpacing = 50;

            Dictionary<int, Cvor> nodeDictionary = verticlesList.ToDictionary(node => node.Id);

            bool changed;
            do
            {
                changed = false;
                HashSet<int> processedNodes = new HashSet<int>();

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
                            // targetNode.Layer = sourceNode.Layer + 1;
                            //changed = true;
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

            DrawingGraph drawingGraph = new DrawingGraph();

            drawingGraph.DrawGraph(canvas, verticlesList, edges);
        }

        public void CircularLayout(List<Cvor> nodes, List<Grana> edges, List<Cvor> verticlesList, Canvas canvas )
        {
            double centerX = canvas.Width / 2; 
            double centerY = canvas.Height / 2;
            double radius = Math.Min(canvas.Width, canvas.Height) / 2 - 50;

            double angle = 2 * Math.PI / nodes.Count;

            for (int i = 0; i < nodes.Count; i++)
            {
                double theta = i * angle;

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

            DrawingGraph drawingGraph = new DrawingGraph();

            drawingGraph.DrawGraph(canvas, verticlesList, edges);
        }

        public void CustomCoordinates(List<Cvor> verticlesList, List<Grana> edges, Canvas canvas)
        {
            DrawingGraph dg = new DrawingGraph();
            dg.DrawGraph(canvas, verticlesList, edges);
        }
    }
}
