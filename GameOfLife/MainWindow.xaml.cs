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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Game.InitializeUniverse();
            DrawUniverse();
            Game.Redraw += DrawUniverse;
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(0.1);
            dt.Tick += (a, b) => Game.CalculateNextGeneration();
            this.KeyDown += (a, b) =>
            {
                if (b.Key == Key.Space) dt.Start();
            };
        }

        public void DrawUniverse()
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
                    rect.MouseLeftButtonDown += (a, b) =>
                    {
                        Point pos = (Point)rect.Tag;
                        (Game.Universe[(int) pos.X, (int) pos.Y].Alive) =
                            !(Game.Universe[(int) pos.X, (int) pos.Y].Alive);
                        rect.Fill = new SolidColorBrush((Game.Universe[(int)pos.X, (int)pos.Y].Alive) ? Colors.Black : Colors.White);
                    };
                    rect.Fill = new SolidColorBrush((Game.Universe[i, j].Alive) ?  Colors.Black:Colors.White );
                    root.Children.Add(rect);
                    Canvas.SetLeft(rect , i* 8);
                    Canvas.SetTop(rect, j * 8);
                }
            }
        }

        

        
    }
}
