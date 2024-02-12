using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using ProjekatGrafovi.Model;

namespace ProjekatGrafovi
{
    public class DrawingGraph
    { 
        public void DrawGraph(Canvas canvas, List<Cvor> nodes, List<Grana> edges)
        {
            DrawEdge(canvas, nodes, edges);

            DrawNode(canvas, nodes, edges);

            FileClass fc = new FileClass();

            int idSCV = fc.ReadNumberTxt();
            fc.SaveToSvg(canvas, idSCV);
            idSCV++;
            fc.WriteNumberText(idSCV);
        }

        public void DrawNode(Canvas canvas, List<Cvor> nodes, List<Grana> edges)
        {
            foreach (var node in nodes)
            {
                Ellipse ellipse1 = new Ellipse();
                ellipse1.Width = 30;
                ellipse1.Height = 30;
                ellipse1.StrokeThickness = 5;
                ellipse1.Stroke = Brushes.Red;
                ellipse1.Fill = Brushes.Red;

                Canvas.SetLeft(ellipse1, node.X - ellipse1.Width / 2);
                Canvas.SetTop(ellipse1, node.Y - ellipse1.Height / 2);

                canvas.Children.Add(ellipse1);

                TextBlock prviText = new TextBlock();
                prviText.Text = node.Id.ToString();
                prviText.Foreground = Brushes.White;
                prviText.FontWeight = FontWeights.Bold;
                prviText.FontSize = 25;
                prviText.TextAlignment = TextAlignment.Center;
                prviText.HorizontalAlignment = HorizontalAlignment.Center;
                prviText.VerticalAlignment = VerticalAlignment.Center;
                prviText.Width = 30;
                prviText.Height = 30;

                double prviX = Canvas.GetLeft(ellipse1) + ellipse1.ActualWidth / 2;
                double prviY = Canvas.GetTop(ellipse1) + ellipse1.ActualHeight / 2;

                Canvas.SetLeft(prviText, prviX - ellipse1.ActualWidth / 2);
                Canvas.SetTop(prviText, prviY - ellipse1.ActualHeight / 2);

                canvas.Children.Add(prviText);
            }
        }

        public void DrawEdge(Canvas canvas, List<Cvor> nodes, List<Grana> edges)
        {
            foreach (var edge in edges)
            {
                bool isReverseEdge = edges.Any(e => e.prvi == edge.drugi && e.drugi == edge.prvi);

                if (!isReverseEdge)
                {
                    DrawLineEdge(canvas, edge);
                }
                else
                {
                    if (edge.prvi.Id < edge.drugi.Id)
                    {
                        DrawLineEdge(canvas, edge);
                    }
                    else
                    {
                        DrawBackEdge(canvas, edge);
                    }
                }
            }
        }

        public void DrawLineEdge(Canvas canvas, Grana edge)
        {
            Line line = new Line();
            line.X1 = edge.prvi.X;
            line.Y1 = edge.prvi.Y;
            line.X2 = edge.drugi.X;
            line.Y2 = edge.drugi.Y;
            line.StrokeThickness = 5;
            line.Stroke = Brushes.Red;
            canvas.Children.Add(line);
        }

        public void DrawBackEdge(Canvas canvas, Grana edge)
        {
            double centerX = (edge.prvi.X + edge.drugi.X) / 2.0;
            double centerY = (edge.prvi.Y + edge.drugi.Y) / 2.0;
            double radius = Math.Abs(edge.drugi.X - edge.prvi.X) / 2.0;

            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Red;
            path.StrokeThickness = 5;

            // Definisanje polukružnog luka
            System.Windows.Media.PathGeometry pathGeometry = new System.Windows.Media.PathGeometry();
            System.Windows.Media.ArcSegment arcSegment = new System.Windows.Media.ArcSegment(new Point(edge.drugi.X, edge.drugi.Y), new Size(radius, radius), 0, false, System.Windows.Media.SweepDirection.Counterclockwise, true);
            System.Windows.Media.PathFigure pathFigure = new System.Windows.Media.PathFigure();
            pathFigure.StartPoint = new Point(edge.prvi.X, edge.prvi.Y);
            pathFigure.IsClosed = false;
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);

            path.Data = pathGeometry;
            canvas.Children.Add(path);
        }
    }
}
