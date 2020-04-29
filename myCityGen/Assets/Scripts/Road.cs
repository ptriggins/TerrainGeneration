using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Road
{
    public Vector3 Start;
    public Vector3 End;
    public float Angle;
    public bool Visited = false;

    public List<Road> Next;
    public List<Road> Last;
    public Road Previous;

    public Road(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
        Angle = Vector3.Angle(Vector3.right, end - start);

        Next = new List<Road>();
        Last = new List<Road>();
        Previous = null;
    }

    public Vector3 GetExtension(float variation, float length)
    {
        Quaternion rotation = Quaternion.Euler(0, Angle + variation, 0);
        return End + rotation * Vector3.right * length;
    }

    public GameObject Draw(Transform transform)
    {
        Start.z *= -1;
        End.z *= -1;

        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();

        LineRenderer l = line.GetComponent<LineRenderer>();
        Material road = Resources.Load("Road Material", typeof(Material)) as Material;
        l.material = road;

        l.startColor = Color.black;
        l.endColor = Color.black;
        l.startWidth = 1f;
        l.endWidth = 1f;
        l.SetPosition(0, 10 * Start);
        l.SetPosition(1, 10 * End);

        return line;
    }
}
