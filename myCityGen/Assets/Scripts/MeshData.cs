using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    private int Width, Length;
    public List<Vector3> Vertices;
    public List<int> Triangles;
    public List<Vector2> UVs;

    public MeshData(int width, int length)
    {
        Width = width;
        Length = length;

        int numQuads = (width - 1) * (length - 1);
        Vertices = new List<Vector3>(width * length);
        Triangles = new List<int>(numQuads * 6);
        UVs = new List<Vector2>(width * length);
    }

    public void SetData(float[,] values)
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        float startX = (Width - 1) / -2f;
        float startZ = (Length - 1) / 2f;
        Vector3 startPos = new Vector3(startX, 0, startZ);

        int i = 0;
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vertices.Add(startPos + new Vector3(x, values[x, z] * 5000, -z));
                UVs.Add(new Vector2(x / (float)Width, z / (float)Length));

                if (x < Width - 1 && z < Length - 1)
                {
                    AddTriangles(i);
                }
                i++;

            }
        }

    }

    private void AddTriangles(int i)
    {
        Triangles.Add(i);
        Triangles.Add(i + 1);
        Triangles.Add(i + Width + 1);
        Triangles.Add(i);
        Triangles.Add(i + Width + 1);
        Triangles.Add(i + Width);
    }

}