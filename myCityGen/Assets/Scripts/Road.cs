using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Road
{
    public Vector3 Start;
    public Vector3 End;
    public float Angle;

    public Road(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
        Angle = Vector3.Angle(Vector3.right, end - start);
    }

    public Vector3 GetExtension(float variation, float length)
    {
        Quaternion rotation = Quaternion.Euler(0, Angle + variation, 0);
        Vector3 direction = rotation * Vector3.right;
        return Start + rotation * direction * length;
    }

    public void Draw(Transform transform)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        LineRenderer l = line.GetComponent<LineRenderer>();

        l.startColor = Color.black;
        l.endColor = Color.black;
        l.startWidth = 1f;
        l.endWidth = 1f;
        l.SetPosition(0, Start);
        l.SetPosition(1, End);
    }

}
