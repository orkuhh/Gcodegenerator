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
    }
}