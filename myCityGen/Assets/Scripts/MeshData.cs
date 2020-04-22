using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] Vertices;      // Points that will be drawn in 3D space
    public int[] Triangles;         // Triangular surfaces
    public Vector2[] UVs;           // Position of a vertex in a 2D texture

    int triangleIndex;

    // Constructs meshdata using a heightmap
    public static MeshData MeshData(float[,] heightMap, float heightMultiplier)
    {
        int numCols = heightMap.GetLength(0);
        int numRows = heightMap.GetLength(1);

        // Calculates size of arrays
        numVertices = numCols * numRows;
        numTriangles = (numCols - 1) * (numRows - 1) * 2;

        Vertices = new Vector3[numVertices];
        Triangles = new int[numTriangles * 3];
        UVs = new Vector2[numVertices];

        float topX = (numCols - 1) / -2f;
        float topY = (numRows - 1) / 2f;

        int i = 0;
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {
                // Sets position and height of each vertex
                meshData.vertices[i] = new Vector3(topX + x, topY + y, heightMap[x, y] * heightMultiplier);     // Position of each vertex in 3D space
                meshData.uvs[i] = new Vector2(x / (float)numCols, y / (float)numRows);                          // Position of each vertex in a 2D texture

                // Adds two triangles for each box in the grid
                if (x < numCols - 1 && y < numRows - 1)
                {
                    meshData.AddTriangle(i, i + numCols + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                i++;
            }
        }

        return meshData;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
