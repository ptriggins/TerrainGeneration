using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Road : MonoBehaviour
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

    public Vector3 GetExtension(float variation)
    {
        Quaternion rotation = Quaternion.Euler(0, Angle + variation, 0);
        Vector3 direction = rotation * Vector3.right;
        return Start + rotation * direction;
    }

    public void DrawRoad()
    {
        GameObject line = new GameObject();
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
