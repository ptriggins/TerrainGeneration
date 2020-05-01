using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position;
    public float Density;
    public Node ParentNode = null;

    public Node(Vector3 p, Node pn)
    {
        Position = p;
        ParentNode = pn;
    }

    public float GetDensity(Tiles[] tiles)
    {
        int x = (int)Mathf.Floor(Position.x);
        int y = (int)Mathf.Floor(Position.y);
    }

    int GetFloor(float val)
    {
        return (int)Mathf.Floor(val);
    }
}
