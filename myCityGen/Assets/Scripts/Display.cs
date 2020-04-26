﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;
    public Mesh Mesh;

    public Display()
    {
        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh = new Mesh();
        MeshFilter.mesh = Mesh;
    }

    public void SetMesh(MeshData data)
    {
        Mesh.Clear();
        Mesh.SetVertices(data.Vertices);
        Mesh.SetTriangles(data.Triangles, 0);
        Mesh.SetUVs(0, data.UVs);
    }

    public void Draw(Texture2D texture)
    {
        MeshRenderer.materials[0].mainTexture = texture;
        MeshRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

}
