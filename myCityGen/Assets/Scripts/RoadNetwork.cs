using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum XingType
{
    Crossing,
    Ending,
    Free,
    Trivial
}

public class RoadNetwork : MonoBehaviour
{
    [Header("Propeties")]
    [SerializeField]
    public float SegmentLength = 4;
    [SerializeField]
    public float SegmentThickness = 4;
    [SerializeField]
    public float SegmentHeight = .1f;
    [SerializeField]
    public int Variation = 15;
    [SerializeField]
    public float BranchProbability = .1f;
    [SerializeField]
    public float MergeTolerance = 4;
    [SerializeField]
    public int NumSegments = 10;

    public List<Segment> Segments;
    public List<GameObject> Lines;

    public void Instantiate()
    {
        Segments = new List<Segment>();
        Lines = new List<GameObject>();
    }

    public void Generate(Vector3 startPos, MapData mapdata)
    {
        startPos.y += SegmentHeight;
        Vector3 direction = GetRandOnUnitCircle();
        Vector3 testPos = GetExtension(startPos, direction, 0, mapdata);

        Node parent = new Node(startPos, mapdata.GetValue(startPos), null);
        Node test = new Node(testPos, mapdata.GetValue(testPos), parent);

        int i = 1;
        Stack<Node> candidates = new Stack<Node>();
        Segment segment = new Segment(parent, test, i);
        GetNodes(segment, candidates, mapdata, true);
        Segments.Add(segment);

        while (candidates.Count > 0 && i < NumSegments)
        {
            test = candidates.Pop();
            parent = test.ParentNode;

            XingType type = GetXingType(parent, test, i + 1);

            if (type != XingType.Trivial)
            {
                segment = new Segment(parent, test, i + 1);

                if (type == XingType.Free)
                    GetNodes(segment, candidates, mapdata, true);
                else if (type == XingType.Crossing)
                    GetNodes(segment, candidates, mapdata, false);

                Segments.Add(segment);
                //Debug.Log(i + 1 + ": " + (segment.StartNode.Position - segment.EndNode.Position).magnitude);
                i++;
            }
        }
    }

    public void GetNodes(Segment previous, Stack<Node> stack, MapData mapdata, bool branchesAllowed)
    {
        List<Node> pNodes = new List<Node>();
        List<Node> aNodes = new List<Node>();

        Vector3 sPos = previous.EndNode.Position;
        Vector3 direction = previous.Direction;

        int rotation = -Variation;
        float min = float.MaxValue;
        int iMin = -1;

        for (int i = 0; i < 3; i++)
        {
            Vector3 pPos = GetExtension(sPos, direction, rotation, mapdata);
            float val = mapdata.GetValue(pPos);

            if (val != -1)
            {
                if (val < min)
                {
                    min = val;
                    iMin ++;
                }

                Node tNode = new Node(pPos, val, previous.EndNode);
                pNodes.Add(tNode);
            }
            rotation += Variation;
        }

        Debug.Log(pNodes.Count + ": " + iMin);
        if (min < float.MaxValue)
            stack.Push(pNodes[iMin]);

        if (branchesAllowed == true)
        {
            float p = previous.EndNode.Value;
            Vector3 bPos;

            if (Random.Range(0, 1) < BranchProbability)
            {
                bPos = GetExtension(sPos, direction, -90, mapdata);
                if (mapdata.GetValue(bPos) != -1)
                    stack.Push(new Node(bPos, mapdata.GetValue(bPos), previous.EndNode));
            }
            if (Random.Range(0, 1) < BranchProbability)
            {
                bPos = GetExtension(sPos, direction, 90, mapdata);
                if (mapdata.GetValue(bPos) != -1)
                    stack.Push(new Node(bPos, mapdata.GetValue(bPos), previous.EndNode));
            }
        }
    }

    public XingType GetXingType(Node parent, Node test, int j)
    {
        XingType type = XingType.Free;

        List<float> dLines = new List<float>();
        List<float> dIntersects = new List<float>();

        float dMinIntersect = float.MaxValue;
        int iIntersect = -1;

        List<float> dP1toIntersects = new List<float>();
        List<int> iIntersects = new List<int>();

        Vector3 
            tPos = test.Position, 
            pPos = parent.Position, 
            sPos, ePos;

        for (int i = 0; i < Segments.Count; i++)
        {
            if (Segments[i].StartNode != parent && Segments[i].EndNode != parent)
            {
                sPos = Segments[i].StartNode.Position;
                ePos = Segments[i].EndNode.Position;
                dLines.Add(LineHelper.GetDistanceToLine(tPos, sPos, ePos));

                if (LineHelper.DoSegmentsIntersect(pPos, tPos, sPos, ePos))
                {
                    float dIntersect = LineHelper.GetDistanceToLine(pPos, sPos, ePos);

                    if (dIntersect < dMinIntersect)
                    {
                        dMinIntersect = dIntersect;
                        iIntersect = i;
                    }
                }
            }
            else
                dLines.Add(float.MaxValue);     
        }

        if (iIntersect != -1)
        {
            type = XingType.Crossing;

            Segment intersect = Segments[iIntersect];
            sPos = intersect.StartNode.Position;
            ePos = intersect.EndNode.Position;
            tPos = LineHelper.ProjectPointToLine(tPos, sPos, ePos);

            if ((tPos - sPos).magnitude < (sPos - ePos).magnitude / 4)
            {
                type = XingType.Ending;
                tPos = sPos;
            }
            else if ((tPos - ePos).magnitude < (sPos - ePos).magnitude / 4)
            {
                type = XingType.Ending;
                tPos = ePos;
            }
        }
        else
        {
            float minDistance = dLines.Min();
            Segment closestSegment = Segments[dLines.IndexOf(minDistance)];
            sPos = closestSegment.StartNode.Position;
            ePos = closestSegment.EndNode.Position;

            if (minDistance < (sPos - ePos).magnitude / 2)
            {
                type = XingType.Ending;
                tPos = LineHelper.ProjectPointToLine(tPos, sPos, ePos);

                if ((tPos - sPos).magnitude < (sPos - ePos).magnitude / 4)
                    tPos = sPos;
                else if ((tPos - ePos).magnitude < (sPos - ePos).magnitude / 4)
                    tPos = ePos;
            }
        }

        if ((tPos - pPos).magnitude < SegmentLength / 2)
            type = XingType.Trivial;

        test.Position = tPos;
        return type;
    }

    public void Draw()
    {
        Transform Transform = (Transform)gameObject.GetComponent(typeof(Transform));
        for (int i = 0; i < Segments.Count; i++)
        {
            Lines.Add(Segments[i].Draw(Transform));
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

    Vector3 GetExtension(Vector3 position, Vector3 direction, int degreeRotation, MapData mapdata)
    {
        Quaternion rotation = Quaternion.Euler(0, degreeRotation, 0);
        return position + rotation * direction * GetLength(position, mapdata);
    }

    float GetLength(Vector3 position, MapData mapdata)
    {
        return SegmentLength * (1.0001f - mapdata.GetValue(position)) * 10;
    }
}
