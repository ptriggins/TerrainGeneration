using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position;
    public List<Node> Neighbors;
    public bool Visited = false;
    public bool Intersection = false;
    public bool Crossing = false;

    public Node(Vector3 position)
    {
        Position = position;
        Neighbors = new List<Node>();
    }
}
