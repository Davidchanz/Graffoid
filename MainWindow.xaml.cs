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
        public int radius, rend_range, currItem_indx;
        public bool move_enable;
        public ItemEllipse currItem;
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
             //Проверяем нажата ли клавиша и наличие прямоугольника на этой позиции.
             {

                /* if (!move_enable)
                 {
                     for (int i = 0; i < ItemsStack.Count; i++)
                     {
                         if ((ItemsStack.ElementAt(i).coord.X >= PositionX) && (ItemsStack.ElementAt(i).coord.X <= PositionX + (rend_range * radius)) && (ItemsStack.ElementAt(i).coord.Y >= PositionY) && (ItemsStack.ElementAt(i).coord.Y <= PositionY + (rend_range * radius)))
                         {
                             if ((int)Math.Pow((double)(PositionX - ItemsStack.ElementAt(i).coord.X), 2.0) + (int)Math.Pow((double)(PositionY - ItemsStack.ElementAt(i).coord.Y), 2.0) <= radius * radius)
                             {
                                 move_enable = true;
                                 currItem_indx = i;
                                 currItem.coord.X = ItemsStack.ElementAt(i).coord.X;
                                 currItem.coord.Y = ItemsStack.ElementAt(i).coord.Y;
                                 currItem.radius = ItemsStack.ElementAt(i).radius;
                                 break;
                             }
                         }
                     }
                 }*/
            if (move_enable)
                {
                    //var newEllipse = (myCanvas.Children[currItem_indx]);
                    Canvas.SetLeft((myCanvas.Children[currItem_indx]), PositionX);
                    Canvas.SetTop((myCanvas.Children[currItem_indx]), PositionY);
                }
            }
        }
    }
}
