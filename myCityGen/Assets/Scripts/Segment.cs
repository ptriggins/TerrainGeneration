using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Segment
{
    public Node StartNode;
    public Vector3 Direction;
    public Node EndNode;
    public float length;

    public Color Color = Color.black;

    public Segment(Vector3 s, Vector3 d, float l)
    {
        Start = s;
        End = s + d * l;
    }

    public Vector3 Rotate(Vector3 startPosition, float rotationDegrees)
    {
        Quaternion rotation = Quaternion.Euler(0, rotationDegrees, 0);
        return new Node(End.Position + rotation * (End.Position - Start.Position));
    }

    public GameObject Draw(Transform transform)
    {

        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();

        LineRenderer l = line.GetComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Unlit/Color"));
        l.sharedMaterial.color = Color;
        l.startWidth = 1f;
        l.endWidth = 1f;
        l.SetPosition(0, 10 * Start.Position);
        l.SetPosition(1, 10 * End.Position);
        //Debug.Log(Start + " : " + End);

        return line;
    }
}
