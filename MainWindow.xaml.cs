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
using System.Linq;
using System.IO;
using System.Diagnostics;


namespace WpfApp1
{
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

        private BitmapSource maskBitmap = null;
        private byte[] maskPixelData = null;
        private int maskWidth = 0;
        private int maskHeight = 0;
        private int maskStride = 0;

        private void ApplyProjectedTexture(string textureFilePath)
        {
            double maxZ = MaxZ();
            double minX = MinX();
            double minY = MinY();
            double maxX = MaxX();
            double maxY = MaxY();

            // Load and preprocess the mask bitmap for fast pixel access
            var bitmap = new BitmapImage(new Uri(textureFilePath, UriKind.Absolute));
            maskBitmap = bitmap;

            // Convert to a format we can easily read pixels from
            var convertedBitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0);
            maskWidth = convertedBitmap.PixelWidth;
            maskHeight = convertedBitmap.PixelHeight;
            maskStride = maskWidth * 4; // 4 bytes per pixel (BGRA)

            // Load all pixel data into memory once
            maskPixelData = new byte[maskHeight * maskStride];
            convertedBitmap.CopyPixels(maskPixelData, maskStride, 0);

            var texture = new DiffuseMaterial(new ImageBrush(bitmap));

            // ... rest of your existing ApplyProjectedTexture code remains the same
            var planeMesh = new MeshGeometry3D();

            planeMesh.Positions.Add(new Point3D(minX, maxY, maxZ));
            planeMesh.Positions.Add(new Point3D(maxX, maxY, maxZ));
            planeMesh.Positions.Add(new Point3D(maxX, minY, maxZ));
            planeMesh.Positions.Add(new Point3D(minX, minY, maxZ));

            planeMesh.TriangleIndices.Add(0);
            planeMesh.TriangleIndices.Add(1);
            planeMesh.TriangleIndices.Add(2);
            planeMesh.TriangleIndices.Add(0);
            planeMesh.TriangleIndices.Add(2);
            planeMesh.TriangleIndices.Add(3);

            planeMesh.TextureCoordinates.Add(new Point(0, 0));
            planeMesh.TextureCoordinates.Add(new Point(1, 0));
            planeMesh.TextureCoordinates.Add(new Point(1, 1));
            planeMesh.TextureCoordinates.Add(new Point(0, 1));

            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(texture);
            materialGroup.Children.Add(texture);

            var planeModel = new GeometryModel3D(planeMesh, materialGroup)
            {
                Transform = new TranslateTransform3D(0, 0, 0)
            };

            planeModel.BackMaterial = texture;
            planeModel.Material = texture;

            var modelVisual = new ModelVisual3D();
            modelVisual.Content = planeModel;

            viewport.Children.Add(modelVisual);
        }
        private List<Point3D> GenerateGridWithMaxZ()
        {
            double step = double.Parse(Gestoscsiatki.Text);
            var gridPoints = new List<Point3D>();

            if (modelVisual?.Content is not Model3DGroup modelGroup)
                return gridPoints;

            // Optymalizacja transformacji
            var groupTransform = modelVisual.Transform ?? Transform3D.Identity;

            // Wstępne obliczenie wszystkich trójkątów z transformacjami
            var triangles = new List<(Point3D v1, Point3D v2, Point3D v3)>();
            
            foreach (var geometryModel in modelGroup.Children.OfType<GeometryModel3D>())
            {
                if (geometryModel.Geometry is MeshGeometry3D meshGeometry)
                {
                    var localTransform = geometryModel.Transform;
                    
                    // Przetwarzanie każdego trójkąta
                    for (int i = 0; i < meshGeometry.TriangleIndices.Count; i += 3)
                    {
                        if (i + 2 < meshGeometry.TriangleIndices.Count)
                        {
                            int idx1 = meshGeometry.TriangleIndices[i];
                            int idx2 = meshGeometry.TriangleIndices[i + 1];
                            int idx3 = meshGeometry.TriangleIndices[i + 2];
                            
                            // Pobieranie przekształconych wierzchołków
                            var v1 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx1]));
                            var v2 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx2]));
                            var v3 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx3]));
                            
                            triangles.Add((v1, v2, v3));
                        }
                    }
                }
            }

            // Jednorazowe obliczenie granic
            var positions = triangles.SelectMany(t => new[] { t.v1, t.v2, t.v3 });
            double minX = positions.Min(p => p.X);
            double maxX = positions.Max(p => p.X);
            double minY = positions.Min(p => p.Y);
            double maxY = positions.Max(p => p.Y);
            double maxZ = positions.Max(p => p.Z) + 10; // Rozpoczęcie promieni z pozycji powyżej najwyższego punktu

            // Budowanie indeksu przestrzennego dla trójkątów w celu optymalizacji rzucania promieni
            var triangleIndex = BuildTriangleSpatialIndex(triangles, step);

            // Obliczanie granic siatki z małym marginesem, aby uniknąć efektów krawędzi
            // Zapobiega to generowaniu punktów dokładnie na granicach modelu
            double gridMinX = Math.Ceiling(minX / step) * step;
            double gridMaxX = Math.Floor(maxX / step) * step;
            double gridMinY = Math.Ceiling(minY / step) * step;
            double gridMaxY = Math.Floor(maxY / step) * step;

            // Generowanie punktów siatki przy użyciu rzucania promieni
            Parallel.For(0, (int)((gridMaxX - gridMinX) / step) + 1, xIndex =>
            {
                double x = gridMinX + xIndex * step;

                for (double y = gridMinY; y <= gridMaxY; y += step)
                {
                    // Rzucenie promienia z góry w dół do modelu
                    double? intersectionZ = CastRay(new Point3D(x, y, maxZ), new Vector3D(0, 0, -1), 
                                                   triangleIndex, triangles, x, y, step);

                    if (intersectionZ.HasValue)
                    {
                        // Dodatkowy filtr: ignorowanie punktów, które są bardzo blisko krawędzi
                        if (x > minX + 0.01 && x < maxX - 0.01 && 
                            y > minY + 0.01 && y < maxY - 0.01)
                        {
                            lock (gridPoints)
                            {
                                gridPoints.Add(new Point3D(x, y, intersectionZ.Value));
                            }
                        }
                    }
                }
            });

            return gridPoints;
        }

        // Budowanie indeksu przestrzennego dla trójkątów, aby przyspieszyć rzucanie promieni
        private Dictionary<(int, int), List<int>> BuildTriangleSpatialIndex(
            List<(Point3D v1, Point3D v2, Point3D v3)> triangles, double gridSize)
        {
            var index = new Dictionary<(int, int), List<int>>();
            
            for (int i = 0; i < triangles.Count; i++)
            {
                var triangle = triangles[i];
                
                // Znajdowanie granic trójkąta w przestrzeni siatki
                int minCellX = (int)Math.Floor(Math.Min(Math.Min(triangle.v1.X, triangle.v2.X), triangle.v3.X) / gridSize);
                int maxCellX = (int)Math.Ceiling(Math.Max(Math.Max(triangle.v1.X, triangle.v2.X), triangle.v3.X) / gridSize);
                int minCellY = (int)Math.Floor(Math.Min(Math.Min(triangle.v1.Y, triangle.v2.Y), triangle.v3.Y) / gridSize);
                int maxCellY = (int)Math.Ceiling(Math.Max(Math.Max(triangle.v1.Y, triangle.v2.Y), triangle.v3.Y) / gridSize);
                
                // Dodawanie trójkąta do wszystkich komórek, które nachodzi
                for (int x = minCellX; x <= maxCellX; x++)
                {
                    for (int y = minCellY; y <= maxCellY; y++)
                    {
                        var cell = (x, y);
                        if (!index.ContainsKey(cell))
                        {
                            index[cell] = new List<int>();
                        }
                        index[cell].Add(i);
                    }
                }
            }
            
            return index;
        }

        // Rzucanie promienia i szukanie przecięcia z trójkątami
        private double? CastRay(
            Point3D rayOrigin, 
            Vector3D rayDirection,
            Dictionary<(int, int), List<int>> triangleIndex,
            List<(Point3D v1, Point3D v2, Point3D v3)> triangles,
            double x, 
            double y, 
            double gridSize)
        {
            // Normalizacja kierunku promienia
            rayDirection.Normalize();
            
            // Uzyskanie współrzędnych komórki
            int cellX = (int)Math.Floor(x / gridSize);
            int cellY = (int)Math.Floor(y / gridSize);
            
            // Szukanie w siatce 3x3 wokół punktu, aby wychwycić trójkąty, które mogą być w pobliżu
            double closestIntersection = double.MaxValue;
            bool foundIntersection = false;
            
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    var cell = (cellX + dx, cellY + dy);
                    
                    if (triangleIndex.TryGetValue(cell, out var triangleIndices))
                    {
                        foreach (var index in triangleIndices)
                        {
                            var triangle = triangles[index];
                            
                            // Sprawdzanie przecięcia promienia z trójkątem
                            if (RayIntersectsTriangle(rayOrigin, rayDirection, 
                                                      triangle.v1, triangle.v2, triangle.v3, 
                                                      out double t))
                            {
                                // Obliczanie normalnej trójkąta
                                Vector3D edge1 = triangle.v2 - triangle.v1;
                                Vector3D edge2 = triangle.v3 - triangle.v1;
                                Vector3D normal = Vector3D.CrossProduct(edge1, edge2);
                                normal.Normalize();
                                
                                // Pomijanie prawie pionowych powierzchni (boki modelu)
                                // Pomaga to uniknąć generowania punktów na pionowych ścianach
                                double verticalThreshold = 0.3; // Dostosuj w razie potrzeby
                                if (Math.Abs(normal.Z) > verticalThreshold)
                                {
                                    // Punkt przecięcia to rayOrigin + t * rayDirection
                                    // Interesuje nas tylko wartość Z dla wysokości siatki
                                    if (t > 0 && t < closestIntersection) // t > 0 zapewnia, że bierzemy tylko przecięcia w kierunku promienia
                                    {
                                        closestIntersection = t;
                                        foundIntersection = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            if (foundIntersection)
            {
                // Obliczanie wartości Z w punkcie przecięcia
                double intersectionZ = rayOrigin.Z + closestIntersection * rayDirection.Z;
                return intersectionZ;
            }
            
            return null; // Nie znaleziono przecięcia
        }

        // Obliczanie przecięcia promienia z trójkątem przy użyciu algorytmu Möllera–Trumbore'a
        private bool RayIntersectsTriangle(
            Point3D rayOrigin, 
            Vector3D rayDirection, 
            Point3D vertex0, 
            Point3D vertex1, 
            Point3D vertex2, 
            out double t)
        {
            t = 0;
            const double EPSILON = 0.0000001;
            
            Vector3D edge1 = new Vector3D(vertex1.X - vertex0.X, vertex1.Y - vertex0.Y, vertex1.Z - vertex0.Z);
            Vector3D edge2 = new Vector3D(vertex2.X - vertex0.X, vertex2.Y - vertex0.Y, vertex2.Z - vertex0.Z);
            
            Vector3D h = Vector3D.CrossProduct(rayDirection, edge2);
            double a = Vector3D.DotProduct(edge1, h);
            
            // Jeśli wyznacznik jest bliski zeru, promień leży w płaszczyźnie trójkąta lub jest równoległy do płaszczyzny trójkąta
            if (a > -EPSILON && a < EPSILON)
                return false;
            
            double f = 1.0 / a;
            Vector3D s = new Vector3D(rayOrigin.X - vertex0.X, rayOrigin.Y - vertex0.Y, rayOrigin.Z - vertex0.Z);
            double u = f * Vector3D.DotProduct(s, h);
            
            // Jeśli u nie jest między 0 a 1, punkt przecięcia jest poza trójkątem
            if (u < 0.0 || u > 1.0)
                return false;
            
            Vector3D q = Vector3D.CrossProduct(s, edge1);
            double v = f * Vector3D.DotProduct(rayDirection, q);
            
            // Jeśli v jest ujemne lub u + v jest większe niż 1, punkt przecięcia jest poza trójkątem
            if (v < 0.0 || u + v > 1.0)
                return false;
            
            // Na tym etapie możemy obliczyć t, aby dowiedzieć się, gdzie znajduje się punkt przecięcia na linii
            t = f * Vector3D.DotProduct(edge2, q);
            
            // Jeśli t jest większe niż 0, mamy przecięcie promienia
            return t > EPSILON;
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

        private void GenerateGcode_Click(object sender, RoutedEventArgs e)
        {
            var gridPoints = GenerateGridWithMaxZ();

            if (gridPoints.Count == 0)
            {
                MessageBox.Show("No grid points were generated. Please load a model first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "G-code Files (*.gcode)|*.gcode",
                Title = "Save G-code File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                GenerateGcodeLineByLine(gridPoints, saveFileDialog.FileName);
                MessageBox.Show($"G-code successfully saved to {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerateGcodeLineByLine(List<Point3D> points, string filePath)
        {
            // Cache model bounds once
            CacheModelBounds();

            // Get printer settings from UI or use defaults
            double feedRate = double.Parse(predkosc.Text); // mm/min
            double travelFeedRate = double.Parse(predkosc.Text); // mm/min
            double retractDistance = 5; // mm
            double extrusionFactor = 0.05; // extrusion per mm of movement
            double xOffset = double.Parse(xoffset.Text);
            double yOffset = double.Parse(yoffset.Text);
            double zOffset = double.Parse(zoffset.Text) - MinZ(); // Z offset for first layer

            StringBuilder gcode = new StringBuilder();

            // Start G-code (same as before)
            gcode.AppendLine("; G-code generated from WPF 3D Model Viewer");
            gcode.AppendLine("; Generated: " + DateTime.Now.ToString());
            gcode.AppendLine("; Number of points: " + points.Count);
            gcode.AppendLine();
            gcode.AppendLine("; Start G-code");

            gcode.AppendLine("G28"); //home
            gcode.AppendLine("G92 E0"); //zapis współrzędnych
            gcode.AppendLine("G1 Z50 F" + travelFeedRate);
            gcode.AppendLine("G1 X0 Y0 F" + travelFeedRate);

            // Sort points in lines
            var sortedPoints = SortPointsInLines(points);

            // PRE-COMPUTE all color states to avoid repeated calculations
            var colorStates = new bool[sortedPoints.Count];
            Parallel.For(0, sortedPoints.Count, i =>
            {
                colorStates[i] = IsPointOnWhitePixelOptimized(sortedPoints[i], cachedMinX, cachedMaxX, cachedMinY, cachedMaxY);
            });

            // Generate path G-code
            double currentX = 0, currentY = 0, currentZ = 5;
            double extrusionAmount = 0;
            bool firstPoint = true;
            bool currentFanState = false; // Track current fan state to avoid redundant commands

            gcode.AppendLine($"; Processing {sortedPoints.Count} points");

            for (int i = 0; i < sortedPoints.Count; i++)
            {
                var point = sortedPoints[i];
                bool isWhitePixel = colorStates[i];

                double x = point.X + xOffset;
                double y = point.Y + yOffset;
                double z = point.Z + zOffset;

                if (firstPoint)
                {
                    // First point - just move to position

                    gcode.AppendLine($"G1 Z{z+40:F3} F{feedRate}"); 
                    gcode.AppendLine($"G1 X{x:F3} Y{y:F3} F{travelFeedRate}");
                    gcode.AppendLine($"G1 Z{z:F3} F{feedRate}");
                    gcode.AppendLine("G1 E0");

                    // Set initial fan state
                    if (isWhitePixel)
                    {
                        gcode.AppendLine("M106 S255");
                        currentFanState = true;
                    }
                    else
                    {
                        gcode.AppendLine("M107");
                        currentFanState = false;
                    }

                    firstPoint = false;
                }
                else
                {
                    // Move to next point
                    gcode.AppendLine($"G1 X{x:F3} Y{y:F3} Z{z:F3} F{feedRate}");

                    // Only change fan state if it's different from current state
                    if (isWhitePixel && !currentFanState)
                    {
                        gcode.AppendLine("M106 S255");
                        gcode.AppendLine("G4 P50");
                        currentFanState = true;
                    }
                    else if (!isWhitePixel && currentFanState)
                    {
                        gcode.AppendLine("M107");
                        gcode.AppendLine("G4 P50");
                        currentFanState = false;
                    }
                }

                currentX = x;
                currentY = y;
                currentZ = z;
            }

            // End G-code (same as before)
            gcode.AppendLine("; End G-code");
            gcode.AppendLine($"G1 Z{currentZ + 10} F{feedRate}");
            gcode.AppendLine($"G1 E{extrusionAmount - retractDistance} F{feedRate * 2}");
            gcode.AppendLine("M104 S0");
            gcode.AppendLine("M140 S0");
            gcode.AppendLine("G28 X0 Y0");
            gcode.AppendLine("M84");

            // Write to file
            File.WriteAllText(filePath, gcode.ToString());
        }

        private List<Point3D> SortPointsInLines(List<Point3D> points)
        {
            double gridStep = double.TryParse(Gestoscsiatki.Text, out double step) ? step : 1.0;

            // Group points by their rounded X coordinate (to handle floating point precision)
            var pointsByX = points.GroupBy(p => Math.Round(p.X / gridStep) * gridStep)
                                  .OrderBy(g => g.Key)
                                  .ToDictionary(g => g.Key, g => g.ToList());

            List<Point3D> sortedPoints = new List<Point3D>(points.Count);
            bool ascending = true; // Direction flag for Y sorting

            // Process each X line
            foreach (var xLine in pointsByX)
            {
                List<Point3D> pointsInXLine;

                if (ascending)
                {
                    // Sort Y ascending (low to high)
                    pointsInXLine = xLine.Value.OrderBy(p => p.Y).ToList();
                    ascending = false; // Flip direction for next line
                }
                else
                {
                    // Sort Y descending (high to low)
                    pointsInXLine = xLine.Value.OrderByDescending(p => p.Y).ToList();
                    ascending = true; // Flip direction for next line
                }

                sortedPoints.AddRange(pointsInXLine);
                Debug.WriteLine($"Completed X line {xLine.Key} with {pointsInXLine.Count} points, direction: {(ascending ? "ascending" : "descending")}");
            }

            // Log the result for verification
            Debug.WriteLine($"Total sorted points: {sortedPoints.Count} (original: {points.Count})");

            return sortedPoints;
        }
        private bool IsPointOnWhitePixelOptimized(Point3D point, double minX, double maxX, double minY, double maxY)
        {
            if (maskPixelData == null || maskWidth == 0 || maskHeight == 0)
                return true; // Default to white if no mask loaded

            // Convert 3D world coordinates to texture UV coordinates (0-1 range)
            double u = (point.X - minX) / (maxX - minX);
            double v = 1.0 - (point.Y - minY) / (maxY - minY); // Flip V coordinate

            // Clamp UV coordinates to [0,1] range
            u = Math.Max(0, Math.Min(1, u));
            v = Math.Max(0, Math.Min(1, v));

            // Convert UV to pixel coordinates
            int pixelX = (int)(u * (maskWidth - 1));
            int pixelY = (int)(v * (maskHeight - 1));

            // Ensure pixel coordinates are within bounds
            pixelX = Math.Max(0, Math.Min(maskWidth - 1, pixelX));
            pixelY = Math.Max(0, Math.Min(maskHeight - 1, pixelY));

            // Calculate pixel index in the byte array (BGRA format = 4 bytes per pixel)
            int pixelIndex = pixelY * maskStride + pixelX * 4;

            // Read RGB values directly from the byte array
            byte blue = maskPixelData[pixelIndex];
            byte green = maskPixelData[pixelIndex + 1];
            byte red = maskPixelData[pixelIndex + 2];

            // Calculate brightness (simple average of RGB values)
            double brightness = (red + green + blue) / 3.0;

            // Return true if pixel is more white than black (threshold at 128)
            return brightness > 128;
        }

        // Add these fields to your MainWindow class
        private double cachedMinX, cachedMaxX, cachedMinY, cachedMaxY;
        private bool boundsAreCached = false;

        // Optimized method to cache model bounds
        private void CacheModelBounds()
        {
            if (!boundsAreCached)
            {
                cachedMinX = MinX();
                cachedMaxX = MaxX();
                cachedMinY = MinY();
                cachedMaxY = MaxY();
                boundsAreCached = true;
            }
        }
        private void InvalidateBoundsCache()
        {
            boundsAreCached = false;
        }
    }
}