using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public int Width, Length;
    public Vector3[] Vertices;      // Points
    public int[] Triangles;         // Surfaces
    public Vector2[] UVs;

    public MeshData(int width, int length)
    {
        Width = 500; Length = 500;
        AddVertices();
        AddTriangles();
    }

    private void AddVertices()
    {
        Vertices = new Vector3[Width * Length];
        UVs = new Vector2[Width * Length];

        float topX = (Width - 1) / -2f;
        float topZ = (Length - 1) / 2f;

        int i = 0;
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vertices[i] = new Vector3(topX + x, 0, topZ - z);
                UVs[i] = new Vector2(x / (float)Width, z / (float)Length);
                i++;
            }
        }
    }

    private void AddTriangles()
    {
        Triangles = new int[(Width - 1) * (Length - 1) * 6];

        int i = 0, t = 0;
        for (int z = 0;  z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (z < Length - 1 && x < Width - 1)
                {
                    AddTriangle(t, i, i + 1, i + Width + 1);            // Top triangle
                    AddTriangle(t + 3, i, i + Width + 1, i + Width);    // Bottom triangle
                    t += 6;
                }
                Debug.Log(i);
                i++;
            }
        }
    }

    private void AddTriangle(int t, int a, int b, int c)
    {
        Triangles[t] = a;
        Triangles[t + 1] = b;
        Triangles[t + 2] = c;
        Debug.Log(Vertices[a] + ": " + Vertices[Triangles[t]] + ", " + Vertices[Triangles[t + 1]] + ", " + Vertices[Triangles[t + 2]]);
    }

}
