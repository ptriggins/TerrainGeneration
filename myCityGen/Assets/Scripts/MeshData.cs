using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    private int Width, Length;
    private List<Vector3> Vertices;
    private List<int> Triangles;
    private List<Vector2> UVs;

    public MeshData(int width, int length)
    {
        Width = width / 100;
        Length = length / 100;

        Vertices = new List<Vector3>(width * length);
        Triangles = new List<int>((Width - 1) * (Length - 1) * 6);
        UVs = new List<Vector2>(width * length);
    }

    public void Calculate(float[,] values)
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        int i = 0;
        for (int z = 0; z < Length + 1; z++)
        {
            for (int x = 0; x < Width + 1; x++)
            {
                Vertices.Add(new Vector3(x, 0, -z));
                UVs.Add(new Vector2(x / ((float)Width), z / ((float)Length)));

                if (x < Width && z < Length)
                    AddTriangles(i);
                i++;

            }
        }

    }

    private void AddTriangles(int i)
    {
        Triangles.Add(i);
        Triangles.Add(i + 1);
        Triangles.Add(i + Width + 2);
        Triangles.Add(i);
        Triangles.Add(i + Width + 2);
        Triangles.Add(i + Width + 1);
    }

    public void RefreshMesh(Mesh mesh)
    {
        mesh.Clear();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetUVs(0, UVs);
    }
}