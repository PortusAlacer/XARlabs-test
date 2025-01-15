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
        
        //add triangles missing to merge both shapes
        for (int h = 0; h < horizontalDivisions; h++)
        {
            int current = coneVertsCount - horizontalDivisions + h;
            int next = coneVertsCount - horizontalDivisions + ((h + 1 == horizontalDivisions) ? 0 : h + 1);
            int below = coneVertsCount + h;
            int belowNext = coneVertsCount + ((h + 1 == horizontalDivisions) ? 0 : h + 1);
        
            // Create two triangles for each quad on the sphere
            triangles.Add(current);
            triangles.Add(next);
            triangles.Add(below);
            
            triangles.Add(next);
            triangles.Add(belowNext);
            triangles.Add(below);
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
        
        //Generate vertices
        for (int v = 1; v <= verticalDivisions; v++)
        {
            float y = Mathf.Lerp(height, 0f, v / (float)verticalDivisions);  // Interpolated height for each slice
            float currentRadius = Mathf.Lerp(0f, radius, v / (float)verticalDivisions);  // Interpolated radius
            
            for (int h = 0; h < horizontalDivisions; h++)
            {
                float angle = h * Mathf.PI * 2f / horizontalDivisions;
                float x = Mathf.Cos(angle) * currentRadius;
                float z = Mathf.Sin(angle) * currentRadius;
                
                vertices.Add(new Vector3(x, y, z));
                normals.Add(Vector3.up); // Placeholder, normals will be calculated later
            }
        }
        
        
        //Generate triangles
        //first layer
        for (int h = 0; h < horizontalDivisions; h++) 
        {
            int currentVert = h + 1; // shift at 1 because 0 is the top vertice
            int nextVert = (h + 1 == horizontalDivisions) ? 0 : h + 1; // this one is the shift to the next vertice
            nextVert++;  // shift at 1 because 0 is the top vertice
            
            // Triangle from tip to vertices of the first line
            triangles.Add(0);  // Tip vertex
            triangles.Add(nextVert);  // Next vertex
            triangles.Add(currentVert);  //  Current  vertex
        }

        
        // other layers, create 2 triangles per square on each layer
        for (int v = 1; v < verticalDivisions; v++)
        {
            for (int h = 0; h < horizontalDivisions; h++)
            {
                // Calculate indices for current vertical slice
                int currentVertIndex = (v - 1) * horizontalDivisions + h;
                int nextVertIndex = (v - 1) * horizontalDivisions + ((h + 1 == horizontalDivisions) ? 0 : h + 1);
                int currentVertIndexAbove = v * horizontalDivisions + h;
                int nextVertIndexAbove = v * horizontalDivisions + ((h + 1 == horizontalDivisions) ? 0 : h + 1);
                currentVertIndex++;
                nextVertIndex++;
                currentVertIndexAbove++;
                nextVertIndexAbove++;
                
                // Connect the current slice to the next slice
                triangles.Add(currentVertIndex); // Current slice vertex
                triangles.Add(nextVertIndex); // Next base vertex in the current slice
                triangles.Add(currentVertIndexAbove); // Current slice vertex in the next slice

                triangles.Add(currentVertIndexAbove); // Current slice vertex in the next slice
                triangles.Add(nextVertIndex); // Next base vertex in the current slice
                triangles.Add(nextVertIndexAbove); // Next slice vertex in the next slice
            }
        }

        //Calculate normals
        for (int v = 0; v < verticalDivisions; v++)
        {
            for (int h = 0; h < horizontalDivisions; h++)
            {
                int currentVertIndex = v * horizontalDivisions + h;
                int nextVertIndex = v * horizontalDivisions + ((h + 1 == horizontalDivisions) ? 0 : h + 1);
                currentVertIndex++;
                nextVertIndex++;
            
                Vector3 p0 = vertices[0];
                Vector3 p1 = vertices[currentVertIndex];
                Vector3 p2 = vertices[nextVertIndex];
                
                Vector3 normal = Vector3.Cross(p2 - p1, p1 - p0).normalized;
                normals[currentVertIndex] = normal;
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


