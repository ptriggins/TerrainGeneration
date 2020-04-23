using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] Vertices;      // Points
    public int[] Triangles;         // Surfaces
    public int Width, Length;

    public MeshData(int width, int length)
    {
        Width = width; Length = length;

        Vertices = new Vector3[Width * Length];

        int numTriangles = (Width - 1) * (Length - 1) * 2;
        Triangles = new int[numTriangles * 3];

        AddVertices();
    }

    private void AddVertices()
    {
        float topX = (Width - 1) / -2f;
        float topZ = (Length - 1) / 2f;

        int iV = 0, iT = 0;
        for (int z = 0; z < Length; z++)    // Cols
        {
            for (int x = 0; x < Width; x++)     // Rows
            {

                Vertices[iV] = new Vector3(topX + x, 0, topZ - z);                        
    
                if (x < Width - 1 && z < Length - 1)
                {
                    AddTriangles(iV, ref iT);
                }

                iV++;
            }
        }

    }

    private void AddTriangles(int v, ref int t)
    {

        // Upper Triangle
        Triangles[t] = v;
        Triangles[t + 1] = v + 1;
        Triangles[t + 2] = v + Width + 1;

        // Lower Triangle
        Triangles[t + 3] = v;
        Triangles[t + 4] = v + Width + 1;
        Triangles[t + 5] = v + Width;

        t += 6;

    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        return mesh;
    }
}
