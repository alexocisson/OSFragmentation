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
        private Block[] blocks;
        public int index
            {get; set; }


        public Memory(int size)
        {
            N = size;
            blocks = new Block[N];
            index = 0;
        }

        public void push(Block block)
        {
            if (index < N)
            {
                blocks[index] = block;
                index++;
            }
        }

        public Block pop()
        {
            if (index>0)
            {
                index--;
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


        public void findNextFree()
        {
            while(blocks[index]!=null)
            {
                index++;
                if (index == N)
                    index = 0;
            }
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
