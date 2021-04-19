using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace FragmentationVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Memory Memory;
        private Memory tempMemory;
        private Random rd;
        private int indexReading;
        private int indexDefragLecture;
        private int indexDefragEcriture;
        private int indexFileDefrag;
        private Color colorFileDefrag;
        private int defragState;


        private DispatcherTimer dispatcherTimer;
        private Color colorTempMemory;

        public MainWindow()
        {
            InitializeComponent();
            Memory = new Memory(216, MemoryCanevas);
            tempMemory = new Memory(10, PreviewCanevas);
            rd = new Random();
            colorTempMemory = Color.FromRgb((byte)rd.Next(0,255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
        }


        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            if(FormatComboBox.SelectedIndex==0)
            {
                System.Diagnostics.Debug.WriteLine("NTFS");

                Memory.indexToNTFS();
                disableButtons();
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(writeNTFS);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                dispatcherTimer.Start();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("EXT4");

                Memory.indexToEXT4(tempMemory.nbBlocks);
                disableButtons();
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(writeEXT4);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                dispatcherTimer.Start();
            }
            repaintCanevas();
        }

        public void writeNTFS(object sender, EventArgs e)
        {
            if (tempMemory.index > 1)
            {
                Memory.writeNTFS(tempMemory.popTemp());
            }
            else if(tempMemory.index==1)
            {
                tempMemory.getAtIndex(0).isLast = true;
                Memory.writeNTFS(tempMemory.popTemp());
            }
            else
            {
                dispatcherTimer.Stop();
                if (tempMemory.index == 0)
                    colorTempMemory = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
                enableButtons();
                updateFileBox();
            }
            repaintCanevas();
        }

        public void writeEXT4(object sender, EventArgs e)
        {
            if (tempMemory.index > 0)
            {
                System.Diagnostics.Debug.WriteLine("in");
                Memory.writeNTFS(tempMemory.popTemp());
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("out");
                dispatcherTimer.Stop();
                if (tempMemory.index == 0)
                    colorTempMemory = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
                enableButtons();
                updateFileBox();
            }
            repaintCanevas();
        }

        private void repaintCanevas()
        {
            MemoryCanevas.Children.Clear();
            PreviewCanevas.Children.Clear();

            Rectangle rectangle = new Rectangle
            {
                Height = MemoryCanevas.ActualHeight,
                Width = MemoryCanevas.ActualWidth,
            };
            rectangle.Fill = Brushes.White;
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, 0);
            MemoryCanevas.Children.Add(rectangle);

            Memory.Draw(MemoryCanevas);
            tempMemory.Draw(PreviewCanevas); 
            Memory.addIndexRectangle(MemoryCanevas);
            tempMemory.addIndexRectangle(PreviewCanevas);
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            tempMemory.push(new Block(colorTempMemory, tempMemory.index));
            repaintCanevas();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            tempMemory.pop();
            if(tempMemory.index == 0) 
                colorTempMemory = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
            repaintCanevas();
        }

        private void AddRandomButton_Click(object sender, RoutedEventArgs e)
        {
            Memory.fillRandom();
            repaintCanevas();
            updateFileBox();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            Memory.clear();
            repaintCanevas();
            updateFileBox();
        }

        private void DefragButton_Click(object sender, RoutedEventArgs e)
        {
            indexDefragLecture = 0;
            indexDefragEcriture = 0;
            indexFileDefrag = 1;
            defragState = 0;
            tempMemory.clear();
            tempMemory = new Memory(100, PreviewCanevas);
            if (Memory.nbBlocks>0)
            {
                disableButtons();
                tempMemory.clear();
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(defrag);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                dispatcherTimer.Start();
            }
            else
            {

            }
            repaintCanevas();
        }
        public void defrag(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Im in yo <3");
            switch (defragState)
            {
                case 0:
                    indexFileDefrag = 0;
                    if (Memory.getAtIndex(indexDefragLecture) != null)
                    {
                        if (Memory.getAtIndex(indexDefragLecture).index == indexFileDefrag)
                        {
                            defragState = 1;
                            colorFileDefrag = Memory.getAtIndex(indexDefragLecture).color;
                            Memory.setAtIndex(indexDefragEcriture, Memory.getAtIndex(indexDefragLecture));
                            Memory.removeAt(indexDefragLecture);
                            indexDefragEcriture++;
                            indexFileDefrag++;
                        }
                        else
                        {
                            tempMemory.pushDefrag(Memory.getAtIndex(indexDefragLecture));
                            Memory.removeAt(indexDefragLecture);
                        }
                    }
                    indexDefragLecture++;
                    break;
                case 1:
                    if (Memory.getAtIndex(indexDefragLecture) != null)
                    {
                        tempMemory.pushDefrag(Memory.getAtIndex(indexDefragLecture));
                        Memory.removeAt(indexDefragLecture);
                        if(tempMemory.getColorBlock(colorFileDefrag, indexFileDefrag)!=-1)
                        {
                            int i = tempMemory.getColorBlock(colorFileDefrag, indexFileDefrag);
                            Block c = tempMemory.getAtIndex(i);
                            tempMemory.removeAtTemp(i);
                            Memory.setAtIndex(indexDefragEcriture, c);
                            indexDefragEcriture++;
                            indexFileDefrag++;
                            if (c.isLast)
                                defragState = 2;
                        }
                    }
                    indexDefragLecture++;
                    break;
                case 2:
                    if(tempMemory.getANumberOne()!=-1)
                    {
                        colorFileDefrag = tempMemory.getAtIndex(tempMemory.getANumberOne()).color;
                        indexFileDefrag = 0;
                        defragState = 3;
                    }
                    else
                    {
                        defragState = 0;
                    }
                    break;

                case 3:
                    if(tempMemory.getColorBlock(colorFileDefrag, indexFileDefrag)!=-1)
                    {
                        int i = tempMemory.getColorBlock(colorFileDefrag, indexFileDefrag);
                        Block c = tempMemory.getAtIndex(i);
                        tempMemory.removeAtTemp(i);
                        Memory.setAtIndex(indexDefragEcriture, c);
                        indexDefragEcriture++;
                        indexFileDefrag++;
                        if (c.isLast)
                            defragState = 2;
                    }
                    else
                    {
                        defragState = 1;
                    }
                    break;
                default:
                    break;
            }
            Memory.index = indexDefragLecture;
            repaintCanevas();

        }

            private void updateFileBox()
        {
        FileComboBox.Items.Clear();
        Memory.getFiles().ToList().ForEach(x => {
            ComboBoxItem it;
            it = new ComboBoxItem();
            it.Content = x;
            it.Background = new SolidColorBrush(x);
            FileComboBox.Items.Add(it);
        }) ;
        FileComboBox.SelectedIndex = 0;
        }

        private void FileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            var item = (ComboBoxItem)(sender as ComboBox).SelectedItem;
            //System.Diagnostics.Debug.WriteLine(item.Content);
            if(item != null)
                FileComboBox.Background = new SolidColorBrush((Color)(item.Content));
            FileComboBox.UpdateLayout();
            */
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            Memory.startReading();
            indexReading = 0;
            var item = (ComboBoxItem)FileComboBox.SelectedItem;
            if (item != null)
            {
                disableButtons();
                tempMemory.clear();
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(readFile);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                dispatcherTimer.Start();
            }
            else
            {

            }
            /*
            disableButtons()
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(writeNTFS);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();*/
        }
        public void readFile(object sender, EventArgs e)
        {
            var item = (ComboBoxItem)FileComboBox.SelectedItem;
            if (Memory.getAtIndex(Memory.index) != null)
            {
                System.Diagnostics.Debug.WriteLine((Color)item.Content);
                System.Diagnostics.Debug.WriteLine(Memory.getAtIndex(Memory.index).color);
                if (Memory.getAtIndex(Memory.index).color == (Color)item.Content && Memory.getAtIndex(Memory.index).index == indexReading)
                {
                    indexReading++;
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                    if (Memory.getAtIndex(Memory.index).isLast)
                    {
                        dispatcherTimer.Stop();
                        updateFileBox();
                        enableButtons();
                    }
                    Memory.removeAt(Memory.index);
                }
                else
                {
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
                }
                
            }
            else
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
            }
            Memory.index++;
            if (Memory.index >= Memory.N)
                Memory.index = 0;
            repaintCanevas();
        }


        public void disableButtons()
        {
            PlusButton.IsEnabled = false;
            MinusButton.IsEnabled = false;
            DefragButton.IsEnabled = false;
            WriteButton.IsEnabled = false;
            ReadButton.IsEnabled = false;
            AddRandomButton.IsEnabled = false;
            ResetButton.IsEnabled = false;
            FormatComboBox.IsEnabled = false;
            FileComboBox.IsEnabled = false;
        }

        public void enableButtons()
        {
            PlusButton.IsEnabled = true;
            MinusButton.IsEnabled = true;
            DefragButton.IsEnabled = true;
            WriteButton.IsEnabled = true;
            ReadButton.IsEnabled = true;
            AddRandomButton.IsEnabled = true;
            ResetButton.IsEnabled = true;
            FileComboBox.IsEnabled = true;
        }
    }
}
