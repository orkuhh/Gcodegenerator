using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ModelVisual3D modelVisual = new ModelVisual3D();
        private Model3DGroup model = new Model3DGroup();

        public MainWindow()
        {
            InitializeComponent();
        }        
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "OBJ Files (*.obj)|*.obj",
                Title = "Wybierz plik OBJ"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadOBJ(openFileDialog.FileName);
            }
        }
        private void LoadOBJ(string filePath)
        {
            var reader = new ObjReader();
            model = reader.Read(filePath);

            // Usuwamy tylko poprzedni model
            viewport.Children.Remove(modelVisual);

            // Aktualizujemy model i dodajemy go z powrotem
            modelVisual.Content = model;
            viewport.Children.Add(modelVisual);
        }
        private void ApplyRotationToModel(Vector3D axis, double angle)
        {
            if (modelVisual != null && modelVisual.Content != null)
            {
                // Pobranie bieżącej macierzy transformacji
                Matrix3D transformationMatrix = modelVisual.Transform.Value;

                // Obrót macierzy o podany kąt wokół danej osi
                transformationMatrix.Rotate(new Quaternion(axis, angle));

                // Ustawienie nowej macierzy transformacji
                modelVisual.Transform = new MatrixTransform3D(transformationMatrix);

                // Zapisanie nowego modelu bazowego
                Model3D originalModel = modelVisual.Content;
                viewport.Children.Remove(modelVisual);
                modelVisual = new ModelVisual3D { Content = originalModel, Transform = new MatrixTransform3D(transformationMatrix) };
                viewport.Children.Add(modelVisual);
            }
        }
            private void x90minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(1, 0, 0), -90);
        }
        private void x15minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(1, 0, 0), -15);
        }
        private void x15plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(1, 0, 0), 15);
        }
        private void x90plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(1, 0, 0), 90);
        }
        private void y90minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 1, 0), -90);
        }
        private void y15minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 1, 0), -15);
        }
        private void y15plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 1, 0), 15);
        }
        private void y90plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 1, 0), 90);
        }
        private void z90minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 0, 1), -90);
        }
        private void z15minus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 0, 1), -15);
        }
        private void z15plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 0, 1), 15);
        }
        private void z90plus(object sender, RoutedEventArgs e)
        {
            ApplyRotationToModel(new Vector3D(0, 0, 1), 90);
        }
        private double MinZ()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Min(p => p.Z) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private double MaxZ()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Max(p => p.Z) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private double MinX()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Min(p => p.X) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private double MinY()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Min(p => p.Y) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private double MaxX()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Max(p => p.X) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private double MaxY()
        {
            if (modelVisual?.Content is Model3DGroup modelGroup)
            {
                // Pobranie globalnej transformacji (np. obroty, przesunięcia)
                var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

                var positions = modelGroup.Children
                    .OfType<GeometryModel3D>()
                    .Where(m => m.Geometry is MeshGeometry3D) // Filtrujemy tylko modele z geometrią
                    .SelectMany(m => ((MeshGeometry3D)m.Geometry).Positions
                        .Select(p => m.Transform.Transform(p)) // Transformacja lokalna
                        .Select(p => groupTransform.Transform(p))) // Transformacja globalna (całego modelu)
                    .ToList();

                return positions.Any() ? positions.Max(p => p.Y) : double.NaN;
            }

            return double.NaN; // Jeśli modelVisual jest pusty
        }
        private void OpenMask_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG Files (*.png)|*.png",
                Title = "Wybierz plik tekstury"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ApplyProjectedTexture(openFileDialog.FileName);
            }
        }
        private void Generate(object sender, RoutedEventArgs e)
        {
            double minZ = MinZ();
            MinZTextBox.Text = minZ.ToString("F3");
            double minX = MinX();
            MinXTextBox.Text = minX.ToString("F3");
            double minY = MinY();
            MinYTextBox.Text = minY.ToString("F3");
            double maxX = MaxX();
            MaxXTextBox.Text = maxX.ToString("F3");
            double maxY = MaxY();
            MaxYTextBox.Text = maxY.ToString("F3");
            List<Point3D> gridPoints = GenerateGridWithMaxZ();
            viewport.Children.Remove(modelVisual);
            DisplayGridPoints(gridPoints);
        }

        private void ApplyProjectedTexture(string textureFilePath)
        {
            double maxZ = MaxZ();
            double minX = MinX();
            double minY = MinY();
            double maxX = MaxX();
            double maxY = MaxY();
            // Tworzymy teksturę z pliku PNG
            var bitmap = new BitmapImage(new Uri(textureFilePath, UriKind.Absolute));
            var texture = new DiffuseMaterial(new ImageBrush(bitmap));

            // Tworzymy płaszczyznę w układzie XY (rozciągniętą na jednostkowy kwadrat)
            var planeMesh = new MeshGeometry3D();

            // Określamy wierzchołki płaszczyzny w układzie XY (Z = 0)
            planeMesh.Positions.Add(new Point3D(minX, maxY, maxZ)); // Lewy dolny róg
            planeMesh.Positions.Add(new Point3D(maxX, maxY, maxZ));  // Prawy dolny róg
            planeMesh.Positions.Add(new Point3D(maxX, minY, maxZ));   // Prawy górny róg
            planeMesh.Positions.Add(new Point3D(minX, minY, maxZ));  // Lewy górny róg

            // Tworzymy trójkąty z wierzchołków
            planeMesh.TriangleIndices.Add(0);
            planeMesh.TriangleIndices.Add(1);
            planeMesh.TriangleIndices.Add(2);
            planeMesh.TriangleIndices.Add(0);
            planeMesh.TriangleIndices.Add(2);
            planeMesh.TriangleIndices.Add(3);

            // Dodajemy współrzędne tekstur dla wierzchołków
            planeMesh.TextureCoordinates.Add(new Point(0, 0));
            planeMesh.TextureCoordinates.Add(new Point(1, 0));
            planeMesh.TextureCoordinates.Add(new Point(1, 1));
            planeMesh.TextureCoordinates.Add(new Point(0, 1));

            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(texture); // Przód
            materialGroup.Children.Add(texture); // Tył (identyczna tekstura)

            // Tworzymy model 3D dla płaszczyzny i przypisujemy teksturę
            var planeModel = new GeometryModel3D(planeMesh, materialGroup)
            {
                // Ustawiamy transformację, aby płaszczyzna była na wysokości Z = 0
                Transform = new TranslateTransform3D(0, 0, 0) // Z = 0, na płaszczyźnie XY
            };

            // Ustawiamy culling mode, aby nie ukrywać tylnej strony płaszczyzny
            planeModel.BackMaterial = texture;
            planeModel.Material = texture;

            // Tworzymy wizualizację modelu 3D i dodajemy ją do widoku
            var modelVisual = new ModelVisual3D();
            modelVisual.Content = planeModel;

            // Dodajemy płaszczyznę do sceny
            viewport.Children.Add(modelVisual);
        }
        private List<Point3D> GenerateGridWithMaxZ()
        {
            double step = double.Parse(Gestoscsiatki.Text);
            var gridPoints = new List<Point3D>();

            if (modelVisual?.Content is not Model3DGroup modelGroup)
                return gridPoints;

            // Optimize transformations
            var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

            // Precompute transformed positions
            var transformedPositions = modelGroup.Children
                .OfType<GeometryModel3D>()
                .Where(m => m.Geometry is MeshGeometry3D)
                .SelectMany(model =>
                {
                    var localTransform = model.Transform;
                    var meshGeometry = (MeshGeometry3D)model.Geometry;
                    return meshGeometry.Positions.Select(p =>
                        groupTransform.Transform(localTransform.Transform(p))
                    );
                })
                .ToArray();

            // Compute bounds once
            double minX = transformedPositions.Min(p => p.X);
            double maxX = transformedPositions.Max(p => p.X);
            double minY = transformedPositions.Min(p => p.Y);
            double maxY = transformedPositions.Max(p => p.Y);

            // Create spatial index for faster lookup
            var spatialIndex = CreateSpatialIndex(transformedPositions, step);

            // Parallel grid generation with optimized point finding
            Parallel.For(0, (int)((maxX - minX) / step) + 1, xIndex =>
            {
                double x = minX + xIndex * step;

                for (double y = minY; y <= maxY; y += step)
                {
                    // Optimized point finding using spatial index
                    double maxZ = FindMaxZFromSpatialIndex(spatialIndex, x, y, step);

                    if (!double.IsNaN(maxZ))
                    {
                        lock (gridPoints)
                        {
                            gridPoints.Add(new Point3D(x, y, maxZ));
                        }
                    }
                }
            });

            return gridPoints;
        }

        // Create a spatial index for faster point lookup
        private Dictionary<(int, int), List<Point3D>> CreateSpatialIndex(IEnumerable<Point3D> points, double gridSize)
        {
            var spatialIndex = new Dictionary<(int, int), List<Point3D>>();

            foreach (var point in points)
            {
                // Compute grid cell coordinates
                int cellX = (int)Math.Floor(point.X / gridSize);
                int cellY = (int)Math.Floor(point.Y / gridSize);
                var cell = (cellX, cellY);

                // Add point to corresponding cell
                if (!spatialIndex.ContainsKey(cell))
                {
                    spatialIndex[cell] = new List<Point3D>();
                }
                spatialIndex[cell].Add(point);
            }

            return spatialIndex;
        }

        // Find max Z using spatial index
        private double FindMaxZFromSpatialIndex(
            Dictionary<(int, int), List<Point3D>> spatialIndex,
            double x,
            double y,
            double gridSize)
        {
            // Compute the cell for the current point
            int cellX = (int)Math.Floor(x / gridSize);
            int cellY = (int)Math.Floor(y / gridSize);

            // Check neighboring cells
            double maxZ = double.NaN;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    var cell = (cellX + dx, cellY + dy);

                    if (spatialIndex.TryGetValue(cell, out var cellPoints))
                    {
                        // Find max Z in points close to (x,y)
                        var zValues = cellPoints
                            .Where(p =>
                                Math.Abs(p.X - x) < gridSize / 2 &&
                                Math.Abs(p.Y - y) < gridSize / 2)
                            .Select(p => p.Z);

                        if (zValues.Any())
                        {
                            double currentMaxZ = zValues.Max();
                            maxZ = double.IsNaN(maxZ) ? currentMaxZ : Math.Max(maxZ, currentMaxZ);
                        }
                    }
                }
            }

            return maxZ;
        }
        private void DisplayGridPoints(List<Point3D> gridPoints)
        {
            var pointsVisual = new PointsVisual3D
            {
                Color = Colors.Black,   // Czarny kolor punktów
                Size = 4,               // Rozmiar punktów (można zwiększyć)
                Points = new Point3DCollection(gridPoints)
            };
            viewport.Children.Add(pointsVisual); // Dodajemy punkty do sceny
        }
    }
}