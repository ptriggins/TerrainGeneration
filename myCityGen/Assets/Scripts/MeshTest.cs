using UnityEngine;

public class MeshTest : MonoBehaviour
{
    MeshFilter meshFilter;

    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
    }

    void Update()
    {
        // Create the Mesh
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];

        // Create vertices of the quad, and add a random position
        vertices[0] = new Vector3(-1, -1, 0) + Random.insideUnitSphere * 0.1f;
        vertices[1] = new Vector3(-1, 1, 0) + Random.insideUnitSphere * 0.1f;
        vertices[2] = new Vector3(1, -1, 0) + Random.insideUnitSphere * 0.1f;
        vertices[3] = new Vector3(1, 1, 0) + Random.insideUnitSphere * 0.1f;

        // Triangles
        int[] triangles = new int[6] {
            0, 1, 2,
            1, 3, 2 };

        // Set vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}
