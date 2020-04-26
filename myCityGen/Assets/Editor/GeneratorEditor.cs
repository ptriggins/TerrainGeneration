using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Generator))]
public class MapGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        Generator generator = (Generator)target;

        DrawDefaultInspector();

        if (generator.Initialized)
        {
            if (GUILayout.Button("Generate"))
            {
                generator.Generate();
            }
        }
        else
        {
            if (GUILayout.Button("Initialize"))
            {
                generator.Initialize();
            }
        }

    }
}
