using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class Generator : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator generator = (TerrainGenerator)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }

    }
}
