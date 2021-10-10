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
using Microsoft.Win32;
using System.IO;

namespace Graffoid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public struct ItemEllipse
    {
        public Point coord;
        public int radius;
        public ItemEllipse(int x, int y, int rad)
        {
            coord.X = x;
            coord.Y = y;
            radius = rad;
        }
    }
    public partial class MainWindow : Window
    {
        public List<ItemEllipse> ItemsStack;
        public int radius, rend_range, currItem_indx, size;
        public bool move_enable;
        public ItemEllipse currItem;
        public int[,] matrix;
        public MainWindow()
        {
            radius = 25;
            rend_range = 3;
            move_enable = false;
            ItemsStack = new List<ItemEllipse>();
            currItem = new ItemEllipse(0, 0, 0);
            InitializeComponent();
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (move_enable)
            {
                int PositionX = (int)Math.Floor(e.GetPosition(myCanvas).X - radius);
                int PositionY = (int)Math.Floor(e.GetPosition(myCanvas).Y - radius);
                move_enable = false;
                currItem = new ItemEllipse(PositionX, PositionY, radius);
                ItemsStack[currItem_indx] = currItem;
            }
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.ToString() == "Pressed")
            {
                int PositionX = (int)Math.Floor(e.GetPosition(myCanvas).X - radius);
                int PositionY = (int)Math.Floor(e.GetPosition(myCanvas).Y - radius);
                for (int i = 0; i < ItemsStack.Count; i++)
                {
                    if (Math.Sqrt(Math.Pow((double)(PositionX - ItemsStack.ElementAt(i).coord.X), 2.0) + Math.Pow((double)(PositionY - ItemsStack.ElementAt(i).coord.Y), 2.0)) <= Math.Sqrt(radius * radius))
                    {
                        move_enable = true;
                        currItem_indx = i;
                        break;
                    }
                }
                if (!move_enable)
                {
                    var newEllipse = new Ellipse();
                    newEllipse.Fill = Brushes.Green;
                    newEllipse.Width = radius * 2; //RectWidth;
                    newEllipse.Height = radius * 2; //RectHeight;
                    Canvas.SetLeft(newEllipse, PositionX);
                    Canvas.SetTop(newEllipse, PositionY);
                    myCanvas.Children.Add(newEllipse);
                    ItemsStack.Add(new ItemEllipse(PositionX, PositionY, radius));
                }
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            int PositionX = (int)Math.Floor(e.GetPosition(myCanvas).X - radius);
            int PositionY = (int)Math.Floor(e.GetPosition(myCanvas).Y - radius);
            if (e.LeftButton.ToString() == "Pressed")
            { 
                if (move_enable)
                {
                    //var newEllipse = (myCanvas.Children[currItem_indx]);
                    Canvas.SetLeft(myCanvas.Children[currItem_indx], PositionX);
                    Canvas.SetTop(myCanvas.Children[currItem_indx], PositionY);
                }
            }
        }

        private void LoadGraffButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            string filePath;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    filePath = openFileDialog.FileName;
                    StreamReader sr = new StreamReader(filePath);
                    string str = sr.ReadToEnd();
                    string [] rowmass = str.Split((char)13);
                    size = rowmass.Length;
                    matrix = new int[size,size];
                    for(int i = 0; i < size; i++)
                    {
                        string[] colmass = rowmass[i].Split(',');
                        for (int j = 0; j < size; j++)
                        {
                            matrix[i,j] = Int32.Parse(colmass[j]);
                        }
                    }
                }
                catch (System.ArgumentException)
                {
                    MessageBox.Show("Error: Invalid file");
                    return;
                }
            }
            for (double angle = 0; angle < (2.0 * Math.PI); angle = angle + (2 * Math.PI) / size)
            {
                int PositionX = (int)((myCanvas.Width / 2) + ((radius * size) * Math.Cos(angle + (Math.PI / size)/2)));
                int PositionY = (int)((myCanvas.Height / 2) - ((radius * size) * Math.Sin(angle + (Math.PI / size)/2)));
                
                var newEllipse = new Ellipse();
                newEllipse.Fill = Brushes.Green;
                newEllipse.Width = radius * 2; //RectWidth;
                newEllipse.Height = radius * 2; //RectHeight;
                Canvas.SetLeft(newEllipse, PositionX);
                Canvas.SetTop(newEllipse, PositionY);
                myCanvas.Children.Add(newEllipse);
                ItemsStack.Add(new ItemEllipse(PositionX, PositionY, radius));
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        Line myLine = new Line();
                        myLine.Stroke = Brushes.LightSteelBlue;
                        myLine.X1 = (int)ItemsStack[i].coord.X + radius;
                        myLine.X2 = (int)ItemsStack[j].coord.X + radius;
                        myLine.Y1 = (int)ItemsStack[i].coord.Y + radius;
                        myLine.Y2 = (int)ItemsStack[j].coord.Y + radius;
                        myLine.HorizontalAlignment = HorizontalAlignment.Left;
                        myLine.VerticalAlignment = VerticalAlignment.Center;
                        myLine.StrokeThickness = 2;
                        myCanvas.Children.Add(myLine);
                    }
                }
            }
        }
    }
}
