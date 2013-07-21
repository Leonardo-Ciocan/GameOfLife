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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private bool MouseDown = false;
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Game.InitializeUniverse();
            DrawInitialUniverse();
            Game.Redraw += UpdateUniverse;
            var dt = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.0001)};
            dt.Tick += (a, b) => Game.CalculateNextGeneration();
            btnStart.Click += (a, b) => dt.Start();
            MouseLeftButtonDown += (a, b) => MouseDown = true;
            MouseLeftButtonUp += (a, b) => MouseDown = false;
        }


        public Rectangle[,] DrawnUniverse = new Rectangle[150,100];
        public void DrawInitialUniverse()
        {
            root.Children.Clear();
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    var rect = new Rectangle
                    {
                        Width =8 ,
                        Height=8,
                        Stroke= new SolidColorBrush(Colors.Gray),
                        StrokeThickness = 0.3
                    };
                    rect.Tag = new Point(i, j);
                    rect.MouseEnter += (a, b) =>
                    {
                        if (MouseDown)
                        {
                            Point pos = (Point) rect.Tag;
                            (Game.Universe[(int) pos.X, (int) pos.Y].Alive) =
                                !(Game.Universe[(int) pos.X, (int) pos.Y].Alive);
                            rect.Fill =
                                new SolidColorBrush((Game.Universe[(int) pos.X, (int) pos.Y].Alive)
                                    ? Colors.Black
                                    : Colors.White);
                        }

                    };
                    rect.Fill = new SolidColorBrush((Game.Universe[i, j].Alive) ?  Colors.Black:Colors.White );
                    root.Children.Add(rect);
                    DrawnUniverse[i, j] = rect;
                    Canvas.SetLeft(rect , i* 8);
                    Canvas.SetTop(rect, j * 8);
                }
            }
        }

        
        public void UpdateUniverse()
        {
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    var rect = DrawnUniverse[i,j];
                    //Color col = (Game.Universe[i, j].Alive) ? Colors.Black : Colors.White;
                    //if (col != (rect.Fill as SolidColorBrush).Color) rect.Fill = new SolidColorBrush(col);
                    rect.Visibility = ((Game.Universe[i, j].Alive)) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            txtGeneration.Text = Game.TotalGenerations.ToString();
        }
        
    }
}
