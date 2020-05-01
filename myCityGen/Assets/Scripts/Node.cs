using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position;
    public float Value;
    public Node ParentNode = null;

    public Node(Vector3 p, float v, Node pn)
    {
        Position = p;
        Value = v;
        ParentNode = pn;
    }
}
