using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FragmentationVisualizer
{
    class Block
    {
        private int index;
        private Color color;

        public void Draw(Canvas canvasToDraw, int pos)
        {
            int size = 20;
            int margin = 5;
            int columns = Convert.ToInt32(canvasToDraw.ActualWidth) / (size + margin);

            Rectangle rectangle = new Rectangle
            {
                Height = size,
                Width = size,
            };
            rectangle.Fill = Brushes.Green;
            canvasToDraw.Children.Add(rectangle);

            int row = pos / columns;
            int column = pos - (row * columns);

            Canvas.SetLeft(rectangle, column * (size + margin));
            Canvas.SetTop(rectangle, row * (size + margin));
        }
    }
}
