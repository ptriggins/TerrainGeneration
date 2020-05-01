using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum XType
{
    Crossing,
    Ending,
    Free,
    Redundant
}

public class Roads : MonoBehaviour
{
    [Header("Propeties")]
    [SerializeField]
    public float SegmentLength = 4;
    [SerializeField]
    public float SegmentThickness = 4;
    [SerializeField]
    public float SegmentHeight = .1f;
    [SerializeField]
    public int ForwardVariation = 15;
    [SerializeField]
    public int NumSegments = 10;

    public List<Segment> Segments;
    public List<GameObject> Lines;

    public void Instantiate()
    {
        Segments = new List<Segment>();
        Lines = new List<GameObject>();
    }

    public void Generate(Vector3 startPosition, Tile[,] tiles)
    {
        startPosition.y += SegmentHeight;
        Vector3 direction = GetRandOnUnitCircle();
        Node parentNode = new Node(startPosition, null);
        Node testNode = new Node(startPosition + direction * SegmentLength, parentNode);

        Queue<Node> candidateNodes = new Queue<Node>();
        candidateNodes.Enqueue(testNode);

        int i = 0;
        while (candidateNodes.Count > 0 && i < NumSegments)
        {
            testNode = candidateNodes.Dequeue();
            XType type = GetXType(parentNode, testNode);

            
            i ++;
        }
    }

    public  XType GetXType(Node test, Node parent)
    {
        XType type = XType.Free;

        List<float> distances = new List<float>();
        for (int i = 0; i < Segments.Count; i++)
        {
            Vector3 s = Segments[i].StartNode.Position,
                    e = Segments[i].EndNode.Position;
            distances.Add(LineHelper.GetDistanceToLine(test.Position, s, e));
        }

        float distance = distances.Min();
        Segment closestSegment = Segments[distances.IndexOf(distance)];

        Vector3
                tPos = test.Position,
                pPos = parent.Position,
                sPos = closestSegment.StartNode.Position,
                ePos = closestSegment.EndNode.Position,
                lPos = LineHelper.ProjectPointToLine(tPos, sPos, ePos);

        if (LineHelper.DoLinesIntersect(pPos, tPos, sPos, ePos))
        {
            if (distance < SegmentLength / 2)
            {
                tPos = lPos;
                if ((tPos - sPos).magnitude < SegmentLength / 4)
                    tPos = sPos;
                else if ((tPos - ePos).magnitude < SegmentLength / 4)
                    tPos = ePos;

                type = XType.Crossing;
            }
            else
                type = XType.Redundant;
        }
        else if (distance < SegmentLength / 2)
        {
            tPos = lPos;
            if ((tPos - sPos).magnitude < SegmentLength / 4)
                tPos = sPos;
            else if ((tPos - ePos).magnitude < SegmentLength / 4)
                tPos = ePos;

            type = XType.Ending;
        }
        return type;

    }

    public void GetNodes(Segment previous, Queue<Node> queue, Tile[,] tiles, bool branchesAllowed)
    {
        List<Node> potentialNodes = new List<Node>();
        List<Node> acceptedNodes = new List<Node>();
        List<float> densities = new List<float>();

        Vector3 startPosition = previous.EndNode.Position;
        Vector3 direction = previous.Direction;

        for (int i = -ForwardVariation; i <= ForwardVariation; i+= ForwardVariation)
        {
            Quaternion rotation = Quaternion.Euler(0, ForwardVariation, 0);
            Vector3 potentialPosition = startPosition + rotation * direction;
            potentialNodes.Add(new Node(potentialPosition, previous.EndNode));
        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = potentialNodes[i].Position;
            int x1 = GetFloor(pos.x);
            int z1 = GetFloor(pos.z);
            densities.Add(tiles[x1, z1].Value);
        }

        int iMin = densities.IndexOf(densities.Min());
        acceptedNodes.Add(potentialNodes[iMin]);

        if (branchesAllowed == true)
        {
            int x = GetFloor(startPosition.x);
            int z = GetFloor(startPosition.z);
            DensityType type = tiles[x, z].Type;
            float probability = type.Percentile / 2f;

            if (Random.Range(0, 1) < probability)
                acceptedNodes.Add(current.Extend(-90));
            if (Random.Range(0, 1) < probability)
                acceptedNodes.Add(current.Extend(90));
        }

        for (int i = 0; i < acceptedNodes.Count; i++)
        {
            acceptedNodes[i].Position.z *= -1;
            acceptedNodes[i].Partner = current.End;
            queue.Enqueue(acceptedNodes[i]);
        }

        int GetFloor(float val)
        {
            return (int)Mathf.Floor(startPosition.x);
        }
    }

    public void Draw()
    {
        Transform Transform = (Transform)gameObject.GetComponent(typeof(Transform));
        for (int i = 0; i < Roads.Count; i++)
        {
            Lines.Add(Roads[i].Draw(Transform));
        }
    }

    public void Clear()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            DestroyImmediate(Lines[i]);
        }
    }

    Vector3 GetRandOnUnitCircle()
    {
        Vector2 randDir = Random.insideUnitCircle.normalized;
        return new Vector3(randDir.x, 0, randDir.y);
    }
}
