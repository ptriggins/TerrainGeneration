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
        Vertices = new List<Vector3>(numQuads * 4);
        Triangles = new List<int>(numQuads * 6);
        UVs = new List<Vector2>(numQuads * 4);
    }

    public void SetData()
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        int QuadNum = 0;
        float startX = (Width - 1) / -2f;
        float startZ = (Length - 1) / 2f;
        Vector3 startPos = new Vector3(startX, 0, startZ);

        for (int z = 0; z < Length - 1; z++)
        {
            for (int x = 0; x < Width - 1; x++)
            {
                Vertices.Add(startPos + new Vector3(x, 0, -z));
                UVs.Add(new Vector2(x / (float)Width, z / (float)Length));
                AddTriangles(QuadNum);
                QuadNum++;
            }
        }

    }

    private void AddTriangles(int i)
    {
        Triangles.Add(i * 4);
        Triangles.Add(i * 4 + 1);
        Triangles.Add(i * 4 + Width + 1);
        Triangles.Add(i * 4);
        Triangles.Add(i * 4 + Width + 1);
        Triangles.Add(i * 4 + Width);
    }

}