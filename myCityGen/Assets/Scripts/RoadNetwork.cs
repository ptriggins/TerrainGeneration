using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum XingType
{
    Crossing,
    Ending,
    Free,
    Redundant
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
        Vector3 direction = GetRandOnUnitCircle() * SegmentLength;
        Vector3 testPos = GetExtension(startPos, direction, 0);

        Node parent = new Node(startPos, mapdata.GetValue(startPos), null);
        Node test = new Node(testPos, mapdata.GetValue(testPos), parent);

        int i = 1;
        Queue<Node> candidates = new Queue<Node>();
        Segment segment = new Segment(parent, test, i);
        GetNodes(segment, candidates, mapdata, true);
        Segments.Add(segment);

        while (candidates.Count > 0 && i < NumSegments)
        {
            test = candidates.Dequeue();
            parent = test.ParentNode;

            XingType type = GetXingType(parent, test, i + 1);

            /*
            Vector3 tPosition = parent.Position + direction / 4;
            tPosition.y += .1f;
            Node t = new Node(tPosition, mapdata.GetValue(tPosition), parent);
            Segment t1 = new Segment(parent, t);
            t1.Color = Color.blue;
            Segments.Add(t1);
            */

            /*
            segment = new Segment(parent, test);
            GetNodes(segment, candidates, mapdata, true);
            Segments.Add(segment);
            */

            if (type != XingType.Redundant)
            {
                segment = new Segment(parent, test, i + 1);
                Debug.Log((i + 1) + ": " + type);

                if (type == XingType.Free)
                    GetNodes(segment, candidates, mapdata, true);
                else if (type == XingType.Crossing)
                    GetNodes(segment, candidates, mapdata, false);

                Segments.Add(segment);
                i++;
            }
        }
    }

    public void GetNodes(Segment previous, Queue<Node> queue, MapData mapdata, bool branchesAllowed)
    {
        List<Node> pNodes = new List<Node>();
        List<Node> aNodes = new List<Node>();

        Vector3 sPos = previous.EndNode.Position;
        Vector3 direction = previous.Direction;

        int rotation = -Variation;
        float min = float.MaxValue;
        int iMin = 0;

        for (int i = 0; i < 3; i++)
        {
            Vector3 pPos = GetExtension(sPos, direction, rotation);
            float val = mapdata.GetValue(pPos);

            if (val < min)
            {
                min = val;
                iMin = i;
            }

            Node tNode = new Node(pPos, val, previous.EndNode);
            //Segment seg = new Segment(previous.EndNode, tNode);
            //seg.Color = Color.yellow;
            //Segments.Add(seg);

            pNodes.Add(tNode);
            rotation += Variation;
        }
        queue.Enqueue(pNodes[iMin]);

        if (branchesAllowed == true)
        {
            float p = previous.EndNode.Value;
            Vector3 bPos;

            if (Random.Range(0, 1) < p / 16f)
            {
                bPos = GetExtension(sPos, direction, -90);
                queue.Enqueue(new Node(bPos, mapdata.GetValue(bPos), previous.EndNode));
            }
            if (Random.Range(0, 1) < p / 16f)
            {
                bPos = GetExtension(sPos, direction, 90);
                queue.Enqueue(new Node(bPos, mapdata.GetValue(bPos), previous.EndNode));
            }
        }
    }

    public XingType GetXingType(Node parent, Node test, int j)
    {
        XingType type = XingType.Free;

        List<float> distances = new List<float>();
        for (int i = 0; i < Segments.Count; i++)
        {
            Node s = Segments[i].StartNode,
                 e = Segments[i].EndNode;

            if (parent == s || parent == e)
              distances.Add(float.MaxValue);
            else
              distances.Add(LineHelper.GetDistanceToLine(test.Position, s.Position, e.Position));
        }

        float distance = distances.Min();
        Segment closestSegment = Segments[distances.IndexOf(distance)];

        Vector3
                tPos = test.Position,
                pPos = parent.Position,
                sPos = closestSegment.StartNode.Position,
                ePos = closestSegment.EndNode.Position,
                lPos = LineHelper.ProjectPointToLine(tPos, sPos, ePos);

        /*
        Vector3 tst = LineHelper.ProjectPointToLine(tPos, sPos, ePos);
        Node t = new Node(tst, 0f, null);
        Segment seg = new Segment(test, t, -1);
        seg.Color = Color.red;
        Segments.Add(seg);
        */

        //bool t = LineHelper.DoSegmentsIntersect(pPos, tPos, sPos, ePos);
        //Debug.Log(j + " intersects " + closestSegment.name + ": " + t);
        Debug.Log(j + " is " + distance + " away from " + closestSegment.name);
        if (distance < float.MaxValue)
        {
            if (LineHelper.DoLinesIntersect(pPos, tPos, sPos, ePos))
            {
                if (distance < SegmentLength / 2)
                {
                    tPos = lPos;
                    if ((tPos - sPos).magnitude < SegmentLength / 4)
                        tPos = sPos;
                    else if ((tPos - ePos).magnitude < SegmentLength / 4)
                        tPos = ePos;

                    type = XingType.Crossing;
                }
                else
                    type = XingType.Redundant;
            }
            else if (distance < SegmentLength / 2 && distance < float.MaxValue)
            {
                tPos = lPos;
                if ((tPos - sPos).magnitude < SegmentLength / 4)
                    tPos = sPos;
                else if ((tPos - ePos).magnitude < SegmentLength / 4)
                    tPos = ePos;

                type = XingType.Ending;
            }

        }
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

    Vector3 GetExtension(Vector3 position, Vector3 direction, int degreeRotation)
    {
        Quaternion rotation = Quaternion.Euler(0, degreeRotation, 0);
        return position + rotation * direction;
    }
}
