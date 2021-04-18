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
        public Color color
        { get; }

        public Boolean isLast = false;

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
            /*
            if (isLast)
            {
                rectangle.StrokeThickness = 5;
                rectangle.Stroke = Brushes.Black;
            }*/
            canvasToDraw.Children.Add(rectangle);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = ""+index;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb((byte)(Math.Abs(255 -color.R)),
                                                                     (byte)(Math.Abs(255 - color.G)),
                                                                     (byte)(Math.Abs(255 - color.B))));
            canvasToDraw.Children.Add(textBlock);

            int row = pos / columns;
            int column = pos - (row * columns);

            Canvas.SetLeft(rectangle, column * (size + margin));
            Canvas.SetTop(rectangle, row * (size + margin));

            Canvas.SetLeft(textBlock, column * (size + margin));
            Canvas.SetTop(textBlock, row * (size + margin));
        }


    }
}
