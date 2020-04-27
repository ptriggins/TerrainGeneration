using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Road : MonoBehaviour
{
    public Vector3 Start;
    public Vector3 End;

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
