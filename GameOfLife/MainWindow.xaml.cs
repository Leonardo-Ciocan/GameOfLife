using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
            var dt = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.01)};
            dt.Tick += (a, b) => Game.CalculateNextGeneration();
            btnStart.Click += (a, b) => dt.Start();
            MouseLeftButtonDown += (a, b) => MouseDown = true;
            MouseLeftButtonUp += (a, b) => MouseDown = false;

            root.MouseMove += (a, b) =>
            {
                if (MouseDown)
                {
                    int i = (int)Math.Round( (b.GetPosition(root).X/8));
                    int j = (int)Math.Round((b.GetPosition(root).Y/ 8));
                    UniverseImage.FillRectangle(i*8, j*8, i*8 + 8, j*8 + 8, Colors.Black);
                    Game.Universe[i, j].Alive = !Game.Universe[i, j].Alive;
                }
            };
        }

        private WriteableBitmap UniverseImage = new WriteableBitmap(1200,800,96,96, PixelFormats.Pbgra32,null);
        public void DrawInitialUniverse()
        {
            UniverseImage.Clear(Colors.White);
            root.Background = new ImageBrush(UniverseImage);
        }

        public void UpdateUniverse()
        {
            UniverseImage.Clear(Colors.White);
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    //Color c = Color.FromRgb((byte)Math.Min(255, Math.Floor(Game.Universe[i, j].Generation/1000.0)), 0, 0);
                    UniverseImage.FillRectangle(i*8, j*8, i*8 + 8, j*8 + 8,
                        (Game.Universe[i, j].Alive) ? Colors.Black : Colors.White);
                }
            }
        }

#region Alternative Method Using User Controls
        //public CellControl[,] DrawnUniverse = new CellControl[150,100];
        //public void DrawInitialUniverse()
        //{
        //    root.Children.Clear();
        //    for (int i = 0; i < 150; i++)
        //    {
        //        for (int j = 0; j < 100; j++)
        //        {
        //            var rect = new CellControl();
        //            rect.Tag = new Point(i, j);
        //            rect.MouseEnter += (a, b) =>
        //            {
        //                if (MouseDown)
        //                {
        //                    Point pos = (Point) rect.Tag;
        //                    (Game.Universe[(int) pos.X, (int) pos.Y].Alive) =
        //                        !(Game.Universe[(int) pos.X, (int) pos.Y].Alive);
        //                    rect.AliveMask.Visibility =
        //                        (Game.Universe[(int) pos.X, (int) pos.Y].Alive)
        //                            ? Visibility.Visible
        //                            : Visibility.Collapsed;
        //                }

        //            };
        //            rect.AliveMask.Visibility =
        //                        (Game.Universe[i,j].Alive)
        //                            ? Visibility.Visible
        //                            : Visibility.Collapsed;
        //            root.Children.Add(rect);
        //            DrawnUniverse[i, j] = rect;
        //            Canvas.SetLeft(rect , i* 8);
        //            Canvas.SetTop(rect, j * 8);
        //        }
        //    }
        //}

        
        //public void UpdateUniverse()
        //{
        //    for (int i = 0; i < 150; i++)
        //    {
        //        for (int j = 0; j < 100; j++)
        //        {
        //            //Color col = (Game.Universe[i, j].Alive) ? Colors.Black : Colors.White;
        //            //if (col != (rect.Fill as SolidColorBrush).Color) rect.Fill = new SolidColorBrush(col);
        //            DrawnUniverse[i, j].AliveMask.Visibility = ((Game.Universe[i, j].Alive)) ? Visibility.Visible : Visibility.Collapsed;
        //        }
        //    }
        //    //txtGeneration.Text = Game.TotalGenerations.ToString();
        //}
#endregion

    }
}
