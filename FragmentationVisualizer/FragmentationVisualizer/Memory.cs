using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FragmentationVisualizer
{
    class Memory
    {
        private const int N = 100;
        private Block[] blocks;

        public Memory()
        {
            blocks = new Block[N];
            blocks[3] = new Block();
            blocks[4] = new Block();
            blocks[6] = new Block();
            blocks[7] = new Block();
        }

        public void Draw(Canvas canvasToDraw)
        {
            for (int i = 0; i< N; i++)
            {
                if (blocks[i]!=null)
                    blocks[i].Draw(canvasToDraw, i);
            }
        }
    }
}
