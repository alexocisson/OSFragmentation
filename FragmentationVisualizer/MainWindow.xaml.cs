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

        private DispatcherTimer dispatcherTimer;
        private Color colorTempMemory;

        public MainWindow()
        {
            InitializeComponent();
            Memory = new Memory(300, MemoryCanevas);
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
                PlusButton.IsEnabled = false;
                MinusButton.IsEnabled = false;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(writeNTFS);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                dispatcherTimer.Start();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("EXT4");

                Memory.indexToEXT4(tempMemory.nbBlocks);
                PlusButton.IsEnabled = false;
                MinusButton.IsEnabled = false;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(writeEXT4);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }
            repaintCanevas();
        }

        public void writeNTFS(object sender, EventArgs e)
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
                PlusButton.IsEnabled = true;
                MinusButton.IsEnabled = true;
                if (tempMemory.index == 0)
                    colorTempMemory = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
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
                PlusButton.IsEnabled = true;
                MinusButton.IsEnabled = true;
                if (tempMemory.index == 0)
                    colorTempMemory = Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255));
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
            rectangle.Fill = Brushes.Blue;
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, 0);
            MemoryCanevas.Children.Add(rectangle);

            Memory.addIndexRectangle(MemoryCanevas);
            tempMemory.addIndexRectangle(PreviewCanevas);

            Memory.Draw(MemoryCanevas);
            tempMemory.Draw(PreviewCanevas);
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

        private void DefragButton_Click(object sender, RoutedEventArgs e)
        {
            Memory.fillRandom();
            repaintCanevas();
        }
    }
}
