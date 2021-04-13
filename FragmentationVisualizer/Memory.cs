using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FragmentationVisualizer
{
    class Memory
    {
        private int N;
        public int nbBlocks
            { get; set; }
        private Block[] blocks;

        public int index
            {get; set; }

        Rectangle indexRectangle;

        public Memory(int size, Canvas canvasToDraw)
        {
            N = size;
            blocks = new Block[N];
            index = 0;
            nbBlocks = 0;
            indexRectangle = new Rectangle
            {
                Height = 20,
                Width = 20,
                StrokeThickness = 1,
                Stroke = Brushes.Red,
            };
            canvasToDraw.Children.Add(indexRectangle);
        }

        public void addIndexRectangle(Canvas canvasToDraw)
        {
            canvasToDraw.Children.Add(indexRectangle);
        }

        public void push(Block block)
        {
            if (index < N)
            {
                blocks[index] = block;
                index++;
                nbBlocks++;
            }
        }
        public void pushTemp(Block block)
        {
            if (index < N)
            {
                for (int i = index; i>0; i--)
                {
                    blocks[i] = blocks[i - 1];
                }
                blocks[0] = block;
                index++;
                nbBlocks++;
            }
        }

        public Block popTemp()
        {
            if (index > 0)
            {
                index--;
                nbBlocks--;
                Block block = blocks[0];
                blocks[index] = null;
                for (int i = 0; i < index; i++)
                    blocks[i] = blocks[i + 1];
                return block;
            }
            return new Block(new Color(), -1);
        }

        public Block pop()
        {
            if (index>0)
            {
                index--;
                nbBlocks--;
                Block block = blocks[index];
                blocks[index] = null;
                return block;
            }
            return new Block(new Color(), -1);
        }

        public void writeNTFS(Block block)
        {
            findNextFree();
            push(block);
        }

        public void indexToNTFS()
        {
            index = N / 2;
        }
        public void indexToEXT4(int size)
        {
            index = 0;
            int cnt = 0;
            while (!hasEnoughtSpeceFor(size)&&cnt<(N-nbBlocks))
            {
                findNextFree();
                cnt++;
            }
            System.Diagnostics.Debug.WriteLine("Im out nigger " + index);
        }
        public Boolean hasEnoughtSpeceFor(int size)
        {
            Boolean answere = true;
            if(index>=0 && index < (N-size))
            {
                int tmpIndex = index;
                for (int i = 0; i<size; i++)
                {
                    System.Diagnostics.Debug.WriteLine("Testing block at" + tmpIndex);
                    if (blocks[tmpIndex] != null)
                    {
                        answere = false;
                        index = tmpIndex;
                        System.Diagnostics.Debug.WriteLine("It's taken yo!");
                    }
                    tmpIndex++;
                }
            }
            else
            {
                answere = false;
            }
            return answere;
        }

        public void findNextFree()
        {
            int cnt = 0;
            if (index == N)
                index = 0;
            while (blocks[index]!=null&&cnt<(N-nbBlocks))
            {
                System.Diagnostics.Debug.WriteLine("Looking at index: " + index);
                index++;
                cnt++;
                if (index == N)
                    index = 0;
            }
        }

        public void fillRandom()
        {
            Random rd = new Random();
            for(int i = 0; i<rd.Next(3,6); i++)
            {
                if (nbBlocks<(N-10))
                {
                    System.Diagnostics.Debug.WriteLine("yo");
                    Color color = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
                    index = rd.Next(0, N);
                    for (int j = 0; j < rd.Next(2, 10); j++)
                    {
                        findNextFree();
                        blocks[index] = new Block(color, j);
                        index++;
                    }
                }
            }
        }

        public void Draw(Canvas canvasToDraw)
        {
            for (int i = 0; i< N; i++)
            {
                if (blocks[i]!=null)
                    blocks[i].Draw(canvasToDraw, i);
            }

            DrawIndex(canvasToDraw, index);

        }

        public void DrawIndex(Canvas canvasToDraw, int pos)
        {
            int size = 20;
            int margin = 5;
            int columns = Convert.ToInt32(canvasToDraw.ActualWidth) / (size + margin);

            int row = pos / columns;
            int column = pos - (row * columns);

            Canvas.SetLeft(indexRectangle, column * (size + margin));
            Canvas.SetTop(indexRectangle, row * (size + margin));
        }
    }
}
