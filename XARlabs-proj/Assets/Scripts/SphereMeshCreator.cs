using System.Collections.Generic;
using UnityEngine;

public class Sphere : ProceduralMesh
{
    public Sphere(int horizontalDivisions, int verticalDivisions, float radius)
    {
        List<Vector3> vertices;
        List<int> triangles;
        List<Vector3> normals;

        (vertices, normals, triangles) = GenerateSphere(horizontalDivisions, verticalDivisions, radius);

        Mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            normals = normals.ToArray()
        };
    }
    
    private static (List<Vector3>, List<Vector3>, List<int>) GenerateSphere(int horizontalDivisions, int verticalDivisions, float radius)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        // Sphere Vertices: Generate vertices using spherical coordinates
        for (int v = 0; v <= verticalDivisions; v++)
        {
            float phi = Mathf.PI * v / verticalDivisions;  // Latitude angle (0 to PI)
            for (int h = 0; h < horizontalDivisions; h++)
            {
                float theta = 2 * Mathf.PI * h / horizontalDivisions;  // Longitude angle (0 to 2*PI)

                // Convert spherical coordinates to Cartesian coordinates
                float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Cos(phi);
                float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);

                vertices.Add(new Vector3(x, y, z));  // Add vertex
                normals.Add(new Vector3(x, y, z).normalized);  // Normal is the same as the position (normalized)
            }
        }

        // Sphere Triangles: Generate triangles (connect the vertices to form faces)
        for (int v = 0; v < verticalDivisions; v++)
        {
            for (int h = 0; h < horizontalDivisions; h++)
            {
                int current = v * horizontalDivisions + h;
                int next = (h + 1) % horizontalDivisions + v * horizontalDivisions;
                int below = (v + 1) * horizontalDivisions + h;
                int belowNext = (h + 1) % horizontalDivisions + (v + 1) * horizontalDivisions;

                // Create two triangles for each quad on the sphere
                triangles.Add(current);
                triangles.Add(next);
                triangles.Add(below);

                triangles.Add(next);
                triangles.Add(belowNext);
                triangles.Add(below);
            }
        }
        
        return (vertices, normals, triangles);
    }
}


