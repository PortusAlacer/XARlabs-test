using System.Collections.Generic;
using UnityEngine;

public class SphereCone : ProceduralMesh
{
    public SphereCone(int horizontalDivisions, int verticalDivisions, float height, float radius)
    {
        List<Vector3> vertices;
        List<int> triangles;
        List<Vector3> normals;

        (vertices, normals, triangles) = GenerateCone(horizontalDivisions, verticalDivisions, height, radius);
        
        (List<Vector3> sphereVertices, List<Vector3> sphereNormals, List<int> sphereTriangles) = GenerateSphere(horizontalDivisions, verticalDivisions, radius);

        int coneVertsCount = vertices.Count;
        
        vertices.AddRange(sphereVertices);
        normals.AddRange(sphereNormals);

        foreach (int t in sphereTriangles)
        {
            triangles.Add(coneVertsCount + t);
        }

        Mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            normals = normals.ToArray()
        };
    }

    private static (List<Vector3>, List<Vector3>, List<int>) GenerateCone(int horizontalDivisions, int verticalDivisions, float height, float radius)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        //Add the tip of the cone
        vertices.Add(new Vector3(0f, height, 0f)); // Cone tip (vertex 0)
        normals.Add(Vector3.up); // Normal pointing up at the tip
        
        // Add vertices for each vertical slice
        for (int v = 1; v <= verticalDivisions; v++)
        {
            float y = Mathf.Lerp(height, 0f, v / (float)verticalDivisions);  // Interpolated height for each slice
            float currentRadius = Mathf.Lerp(0f, radius, v / (float)verticalDivisions);  // Interpolated radius
        
            for (int i = 0; i < horizontalDivisions; i++)
            {
                float angle = i * Mathf.PI * 2f / horizontalDivisions;
                float x = Mathf.Cos(angle) * currentRadius;
                float z = Mathf.Sin(angle) * currentRadius;
                vertices.Add(new Vector3(x, y, z));
                normals.Add(Vector3.up); // Placeholder, normals will be calculated later
            }
        }
        
        // first layer, as it doesn't need to do 2 triangles due to tip
        for (int i = 1; i <= horizontalDivisions; i++)
        {
            int next = (i == horizontalDivisions) ? 1 : i + 1;
            // Triangle from tip to vertices of the first line
            triangles.Add(0);  // Tip vertex
            triangles.Add(next);  // Next vertex
            triangles.Add(i);  //  Current  vertex
        }
        
        // Side triangles: Connect the tip to each ring of vertices
        for (int v = 1; v < verticalDivisions; v++)
        {
            {
                for (int i = 0; i < horizontalDivisions; i++)
                {
                    // Calculate indices for current vertical slice
                    int currentVertIndex = 1 + (v - 1) * horizontalDivisions + i;
                    int nextVertIndex = 1 + (v - 1) * horizontalDivisions + ((i + 1) % horizontalDivisions);
                    int currentVertIndexAbove = 1 + v * horizontalDivisions + i;
                    int nextVertIndexAbove = 1 + v * horizontalDivisions + ((i + 1) % horizontalDivisions);
            
                    // Connect the current slice to the next slice
                    triangles.Add(currentVertIndex); // Current slice vertex
                    triangles.Add(nextVertIndex); // Next base vertex in the current slice
                    triangles.Add(currentVertIndexAbove); // Current slice vertex in the next slice
            
                    triangles.Add(currentVertIndexAbove); // Current slice vertex in the next slice
                    triangles.Add(nextVertIndex); // Next base vertex in the current slice
                    triangles.Add(nextVertIndexAbove); // Next slice vertex in the next slice
                    
                    Vector3 p0 = vertices[0];
                    Vector3 p1 = vertices[currentVertIndex];
                    Vector3 p2 = vertices[nextVertIndex];
                    
                    Vector3 normal = Vector3.Cross(p2 - p1, p1 - p0).normalized;
                    normals[currentVertIndex] = normal;
                }
            }
        }
        
        return (vertices, normals, triangles);
    }
    
    private static (List<Vector3>, List<Vector3>, List<int>) GenerateSphere(int horizontalDivisions, int verticalDivisions, float radius)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        // Sphere Vertices: Generate vertices using spherical coordinates
        for (int v = verticalDivisions / 2; v <= verticalDivisions; v++)
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
        for (int v = 0; v < verticalDivisions / 2; v++)
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


