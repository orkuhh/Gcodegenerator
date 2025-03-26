﻿using HelixToolkit.Wpf;
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

            // Precompute all triangles with their transformations
            var triangles = new List<(Point3D v1, Point3D v2, Point3D v3)>();
            
            foreach (var geometryModel in modelGroup.Children.OfType<GeometryModel3D>())
            {
                if (geometryModel.Geometry is MeshGeometry3D meshGeometry)
                {
                    var localTransform = geometryModel.Transform;
                    
                    // Process each triangle
                    for (int i = 0; i < meshGeometry.TriangleIndices.Count; i += 3)
                    {
                        if (i + 2 < meshGeometry.TriangleIndices.Count)
                        {
                            int idx1 = meshGeometry.TriangleIndices[i];
                            int idx2 = meshGeometry.TriangleIndices[i + 1];
                            int idx3 = meshGeometry.TriangleIndices[i + 2];
                            
                            // Get transformed vertices
                            var v1 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx1]));
                            var v2 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx2]));
                            var v3 = groupTransform.Transform(localTransform.Transform(meshGeometry.Positions[idx3]));
                            
                            triangles.Add((v1, v2, v3));
                        }
                    }
                }
            }

            // Compute bounds once
            var positions = triangles.SelectMany(t => new[] { t.v1, t.v2, t.v3 });
            double minX = positions.Min(p => p.X);
            double maxX = positions.Max(p => p.X);
            double minY = positions.Min(p => p.Y);
            double maxY = positions.Max(p => p.Y);
            double maxZ = positions.Max(p => p.Z) + 10; // Start rays from above the highest point

            // Build a spatial index for triangles to optimize ray casting
            var triangleIndex = BuildTriangleSpatialIndex(triangles, step);

            // Generate grid points using ray casting
            Parallel.For(0, (int)((maxX - minX) / step) + 1, xIndex =>
            {
                double x = minX + xIndex * step;

                for (double y = minY; y <= maxY; y += step)
                {
                    // Cast ray from above down to the model
                    double? intersectionZ = CastRay(new Point3D(x, y, maxZ), new Vector3D(0, 0, -1), 
                                                   triangleIndex, triangles, x, y, step);

                    if (intersectionZ.HasValue)
                    {
                        lock (gridPoints)
                        {
                            gridPoints.Add(new Point3D(x, y, intersectionZ.Value));
                        }
                    }
                }
            });

            return gridPoints;
        }

        // Build a spatial index for triangles to make ray casting faster
        private Dictionary<(int, int), List<int>> BuildTriangleSpatialIndex(
            List<(Point3D v1, Point3D v2, Point3D v3)> triangles, double gridSize)
        {
            var index = new Dictionary<(int, int), List<int>>();
            
            for (int i = 0; i < triangles.Count; i++)
            {
                var triangle = triangles[i];
                
                // Find the bounds of the triangle in grid space
                int minCellX = (int)Math.Floor(Math.Min(Math.Min(triangle.v1.X, triangle.v2.X), triangle.v3.X) / gridSize);
                int maxCellX = (int)Math.Ceiling(Math.Max(Math.Max(triangle.v1.X, triangle.v2.X), triangle.v3.X) / gridSize);
                int minCellY = (int)Math.Floor(Math.Min(Math.Min(triangle.v1.Y, triangle.v2.Y), triangle.v3.Y) / gridSize);
                int maxCellY = (int)Math.Ceiling(Math.Max(Math.Max(triangle.v1.Y, triangle.v2.Y), triangle.v3.Y) / gridSize);
                
                // Add the triangle to all cells it overlaps
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

        // Cast a ray and find intersection with triangles
        private double? CastRay(
            Point3D rayOrigin, 
            Vector3D rayDirection,
            Dictionary<(int, int), List<int>> triangleIndex,
            List<(Point3D v1, Point3D v2, Point3D v3)> triangles,
            double x, 
            double y, 
            double gridSize)
        {
            // Normalize ray direction
            rayDirection.Normalize();
            
            // Get cell coordinates
            int cellX = (int)Math.Floor(x / gridSize);
            int cellY = (int)Math.Floor(y / gridSize);
            
            // Search in a 3x3 grid around the point to catch triangles that might be near
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
                            
                            // Check ray-triangle intersection
                            if (RayIntersectsTriangle(rayOrigin, rayDirection, 
                                                      triangle.v1, triangle.v2, triangle.v3, 
                                                      out double t))
                            {
                                // The intersection point is rayOrigin + t * rayDirection
                                // We're only interested in the Z value for grid height
                                if (t > 0 && t < closestIntersection) // t > 0 ensures we only get intersections in ray direction
                                {
                                    closestIntersection = t;
                                    foundIntersection = true;
                                }
                            }
                        }
                    }
                }
            }
            
            if (foundIntersection)
            {
                // Calculate the Z value at intersection point
                double intersectionZ = rayOrigin.Z + closestIntersection * rayDirection.Z;
                return intersectionZ;
            }
            
            return null; // No intersection found
        }

        // Calculate ray-triangle intersection using Möller–Trumbore algorithm
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
            
            // If the determinant is near zero, ray lies in plane of triangle or ray is parallel to plane of triangle
            if (a > -EPSILON && a < EPSILON)
                return false;
            
            double f = 1.0 / a;
            Vector3D s = new Vector3D(rayOrigin.X - vertex0.X, rayOrigin.Y - vertex0.Y, rayOrigin.Z - vertex0.Z);
            double u = f * Vector3D.DotProduct(s, h);
            
            // If u is not between 0 and 1, the intersection point is outside the triangle
            if (u < 0.0 || u > 1.0)
                return false;
            
            Vector3D q = Vector3D.CrossProduct(s, edge1);
            double v = f * Vector3D.DotProduct(rayDirection, q);
            
            // If v is negative or u + v is greater than 1, the intersection point is outside the triangle
            if (v < 0.0 || u + v > 1.0)
                return false;
            
            // At this stage we can compute t to find out where the intersection point is on the line
            t = f * Vector3D.DotProduct(edge2, q);
            
            // If t is greater than 0, we have a ray intersection
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