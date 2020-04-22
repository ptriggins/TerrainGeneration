using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] heightMap, float heightMultiplier)
    {
        int numCols = heightMap.GetLength(0);
        int numRows = heightMap.GetLength(1);

        // Coords of 1st vertex in mesh (top left corner)
        float topX = (numCols - 1) / -2f;
        float topY = (numRows - 1) / 2f;

        MeshData meshData = new MeshData(width, height);

        // Calculates coords of remaining vertices relative to the top left
        int i = 0;
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {
                int height = heightMap[x, y] * heightMultiplier;        // Gets height value (z)
                meshData.vertices[i] = new Vector3(topX + x, topY + y, height);
                meshData.uvs[i] = new Vector2(x / (float)numCols, y / (float)numRows);

                if (x < numCols - 1 && y < numRows - 1)
                {
                    meshData.AddTriangle(index, vertexIndex + width + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }


                vertexIndex++;
            }
        }

        return meshData;
    }
}
