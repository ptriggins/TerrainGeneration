using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Road
{
    public Vector3 Start;
    public Vector3 End;
    public float Angle;
    public Color Color = Color.black;

    public List<Road> Next;
    public List<Road> Last;
    public Road Previous;

    public Road(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;

        Next = new List<Road>();
        Last = new List<Road>();
        Previous = null;
    }

    public Vector3 GetExtension(float degrees)
    {
        Quaternion rotation = Quaternion.Euler(0, degrees, 0);
        return End + rotation * (End - Start);
    }

    public GameObject Draw(Transform transform)
    {
        Start.z *= -1;
        End.z *= -1;

        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();

        LineRenderer l = line.GetComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Unlit/Color"));
        l.sharedMaterial.color = Color;
        l.startWidth = 1f;
        l.endWidth = 1f;
        l.SetPosition(0, 10 * Start);
        l.SetPosition(1, 10 * End);

        return line;
    }
}
