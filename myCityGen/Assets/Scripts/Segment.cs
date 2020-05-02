using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Segment
{
    public Node StartNode;
    public Node EndNode;
    public int name;

    public Vector3 Direction;
    public Color Color = Color.black;

    public Segment(Node s, Node e, int i)
    {
        StartNode = s;
        EndNode = e;
        Direction = (EndNode.Position - StartNode.Position).normalized;
        name = i;
    }

    public GameObject Draw(Transform transform)
    {

        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        line.name = name.ToString();

        LineRenderer l = line.GetComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Unlit/Color"));
        l.sharedMaterial.color = Color;
        l.startWidth = 1f;
        l.endWidth = 1f;
        l.SetPosition(0, 10 * StartNode.Position);
        l.SetPosition(1, 10 * EndNode.Position);
        //Debug.Log(Start + " : " + End);

        return line;
    }
}
