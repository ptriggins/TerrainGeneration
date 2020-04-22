using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] Vertices;
    public int[] Triangles;         
    public Vector2[] UVs;           // Position of vertex on a 2D texture

    public MeshData(float[,] vals)
    {
        int numCols = vals.GetLength(0);
        int numRows = vals.GetLength(1);

        int numVertices = numCols * numRows;
        int numTriangles = (numCols - 1) * (numRows - 1) * 2;

        Vertices = new Vector3[numVertices];
        Triangles = new int[numTriangles * 3];
        UVs = new Vector2[numVertices];

        float topX = (numCols - 1) / -2f;
        float topY = (numRows - 1) / 2f;

        int iVertex = 0;
        int iTriangle = 0;

        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)       // Sets vertices in rows
            {
                Vertices[iVertex] = new Vector3(topX + x, topY + y, vals[x, y]);                        // Position of each vertex in 3D space
                UVs[iVertex] = new Vector2(x / (float)numCols, y / (float)numRows);                          // Position of each vertex in a 2D texture

                // Adds two triangles for each box in the grid
                if (x < numCols - 1 && y < numRows - 1)
                {
                    AddTriangle(iVertex, iVertex + 1, iVertex + numCols + 1);
                    AddTriangle(iVertex, iVertex + numCols + 1, iVertex + numCols);
                }

                iVertex++;
            }
        }

        // Sets a triangle given its vertices
        void AddTriangle(int v1, int v2, int v3)
        {
            Triangles[iTriangle] = v1;
            Triangles[iTriangle + 1] = v2;
            Triangles[iTriangle + 2] = v3;
            iTriangle += 3;
        }

    }

    // Returns a mesh from the meshdata
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
