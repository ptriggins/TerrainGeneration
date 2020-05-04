using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;
    public Mesh Mesh;

    public void Instantiate()
    {
        Mesh = new Mesh();
        MeshFilter.mesh = Mesh;
    }

    public void Draw(Texture2D texture)
    {
        MeshRenderer.sharedMaterials[0].mainTexture = texture;
        MeshRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

}