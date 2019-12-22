using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UTEC.Controls
{
    [TemplatePart(Name = LineGraph.PART_Canvas, Type = typeof(Canvas))]
    public class LineGraph : Canvas
    {
        #region Template Parts
        protected const string PART_Canvas = "PART_Canvas";
        #endregion

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(LineGraph),
                                        new FrameworkPropertyMetadata(OnDataChanged));

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs args)
        {
            var lineGraph = dObj as LineGraph;
            if (lineGraph != null)
            {
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var grid = base.GetTemplateChild("PART_Canvas") as Canvas;
        }

        private void DrawRow(double[] row, double min, double max)
        {
            var value = row[0];
            var lastX = this.Width / row.Length;
            var lastY = GetRowY(min, max, value, this.Height);

            for (var i = 0; i < row.Length; i++)
            {
                value = row[i];

                var nextX = GetRowX(i, row.Length, this.Width);
                var nextY = GetRowY(min, max, value, this.Height);

                if (i != 0)
                {
                    this.Children.Add(new Line
                    {
                        X1 = lastX,
                        Y1 = lastY,
                        X2 = nextX,
                        Y2 = nextY,
                        Stroke = new SolidColorBrush() { Color = Colors.Red }
                    });
                }

                lastX = nextX;
                lastY = nextY;
            }
        }

        private double GetRowX(int i, int length, double width)
        {
            var offset = (width / (length * 2));

            return ((width * i) / (length)) + offset;
        }

        private int GetRowY(double min, double max, double value, double height)
        {
            var difference = max - min;
            var magnitude = difference == 0 ? 0.5 : (max - value) / difference;
            var result = magnitude * (height * 0.8);
            return (int)result + (int)(height * 0.1);
        }

        private void GetMinMax(out double min, out double max, double[][] data)
        {
            min = double.MaxValue;
            max = double.MinValue;
            double value;
            for (var row = 0; row < data.Length; row++)
            {
                for (var column = 0; column < data[0].Length; column++)
                {
                    value = data[row][column];

                    min = Math.Min(min, value);
                    max = Math.Max(max, value);
                }
            }
        }
    }
}