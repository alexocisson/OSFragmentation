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
        public int index
            { get; }
        private Color color;

        public Block(Color col, int ind)
        {
            color = col;
            index = ind;
        }

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
            rectangle.Fill = new SolidColorBrush(color);
            canvasToDraw.Children.Add(rectangle);

            int row = pos / columns;
            int column = pos - (row * columns);

            Canvas.SetLeft(rectangle, column * (size + margin));
            Canvas.SetTop(rectangle, row * (size + margin));
        }
    }
}
