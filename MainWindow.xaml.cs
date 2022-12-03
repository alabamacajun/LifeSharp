using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace LifeSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Size = 50;
        private const int Center = Size / 2;
        private const int MilDelay = 1000 / 8;  // 4 times per second.

        private readonly DiffuseMaterial[,] GridMat = new DiffuseMaterial[Size + 2, Size + 2];
        private readonly LifeCells CellField;
        private readonly DispatcherTimer GameLoopTimer;

        public MainWindow()
        {
            var seeds = MakeGliderSeed();
            CellField = ILifeCells.GetNew(Size, seeds);
            InitializeComponent();
            BuildGrid(this.CellGroup);
            GameLoopTimer = new DispatcherTimer();
            SetupGameLoop(MilDelay);
        }

        private static CellSeed[] MakeGliderSeed()
        {
            return new CellSeed[] {
                new CellSeed() { x = 13, y = 15, value = 1 },
                new CellSeed() { x = 14, y = 14, value = 1 },
                new CellSeed() { x = 12, y = 13, value = 1 },
                new CellSeed() { x = 13, y = 13, value = 1 },
                new CellSeed() { x = 14, y = 13, value = 1 },
            };
        }

        private void BuildGrid(Model3DGroup group)
        {
            // Define 3D mesh object
            var mesh = new MeshGeometry3D();
            // Front face
            mesh.Positions.Add(new Point3D(-0.45, -0.45, 4));
            mesh.Positions.Add(new Point3D(0.45, -0.45, 4));
            mesh.Positions.Add(new Point3D(0.45, 0.45, 4));
            mesh.Positions.Add(new Point3D(-0.45, 0.45, 4));
            // Front face
            foreach (var i in new int[] { 0, 1, 2, 2, 3, 0 })
                mesh.TriangleIndices.Add(i);

            // Geometry creation
            for (int oy = 0; oy < Size; ++oy)
            {
                for (int ox = 0; ox < Size; ++ox)
                {
                    var mat = new DiffuseMaterial(Brushes.DarkGray);
                    GridMat[ox, oy] = mat;
                    var cell = new GeometryModel3D(mesh, mat);
                    var group2 = new Transform3DGroup();
                    group2.Children.Add(new TranslateTransform3D()
                    {
                        OffsetX = ox - Center,
                        OffsetY = oy - Center,
                        OffsetZ = 0,
                    });
                    cell.Transform = group2;
                    group.Children.Add(cell);
                }
            }
        }

        private void SetupGameLoop(int milisecondsDelay)
        {
            GameLoopTimer.Tick += GameLoopTimer_Tick;
            GameLoopTimer.Interval = new TimeSpan(0, 0, 0, 0, milisecondsDelay);
        }
        private void GameLoopTimer_Tick(object sender, EventArgs e)
        {
            CellField.Cycle();
            Brush[] seeds = { Brushes.DarkGray, Brushes.Green, Brushes.Yellow, Brushes.Red };
            var feed = CellField.GeView();
            for (int oy = 0; oy < Size; ++oy)
            {
                for (int ox = 0; ox < Size; ++ox)
                {
                    var mat = GridMat[ox, oy];
                    var r = feed[ox, oy];
                    if (r > 3) r = 0;
                    mat.Brush = seeds[r];
                }
            }
        }

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            if (GameLoopTimer.IsEnabled) GameLoopTimer.Stop();
            else GameLoopTimer.Start();
        }
    }
}
