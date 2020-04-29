using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    [Header("Road Propeties")]
    [SerializeField]
    public float Length = 4;
    [SerializeField]
    public float Thickness = 4;
    [SerializeField]
    public int NumSegments = 10;

    public List<Node> Nodes;
    public List<Road> Roads;
    public List<GameObject> Lines;

    public void Instantiate()
    {
        Nodes = new List<Node>;
        Roads = new List<Road>();
        Lines = new List<GameObject>();
    }

    public void Generate(Vector3 start, Tile[,] tiles)
    {
        start.y += .1f;
        Node current = new Node(start);
        Nodes.Add(current);

        Vector2 randDir = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randDir.x, 0, randDir.y);
        Node end = new Node(start + direction * Length);

        Queue<Node> candidates = new Queue<Node>();
        candidates.Enqueue(end);

        int i = 0;
        while (candidates.Count > 0 && i < NumSegments)
        {
            Node next = candidates.Dequeue();
            

            int t = CheckIntersect(current);
            if (t == 0)
                GetBranches(current, candidates, tiles, true);
            else if (t == 1)
                GetBranches(current, candidates, tiles, false);

            Roads.Add(current);
            previous = current;
            i ++;
        }
        CheckIntersect(Roads[Roads.Count - 1]);
    }

    public Node CheckForIntersections(Node node, Node end)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].Visited = false;
        }

        for (int i = 0; i < Roads.Count; i++)
        {
            float dStart = Vector3.Distance(node.Position, Roads[i].Start.Position);
            float dEnd = Vector3.Distance(node.Position, Roads[i].End.Position);
            float dLine = DistanceToLine(node.Position, Roads[i].Start.Position, Roads[i].End.Position);

            if (dStart < Length / 4)
                return Roads[i].Start;
            else if (dEnd < Length / 4)
                return Roads[i].End;
            else if (dLine < Length / 2)
                return ProjectToLine()

        }

        roads = GetNextLevel(roads);
        roads = GetNextLevel(roads);

        while (roads.Count > 0)
        {
            List<float> distances = new List<float>();
            for (int i = 0; i < roads.Count; i++)
            {
                distances.Add(DistanceToLine(current.Position, roads[i].Start, roads[i].End));
                Debug.Log(DistanceToLine(current.End, roads[i].Start, roads[i].End));
            }

            for (int i = 0; i < Roads.Count; i++)
            {
                int iMin = distances.IndexOf(distances.Min());
                Road road = roads[iMin];

                if (distances[iMin] < Length / 2)
                    return CreateIntersection(road);
                distances[iMin] = float.MaxValue;
            }
            roads = GetNextLevel(roads, level);
            level++;
        }
        return 0;

        List<Node> GetNeighbors(List<Node> nodes)
        {
            List<Node> neighbors = new List<Node>();
            for (int i = 0; i < list.Count; i++)
            {
                Road road = list[i];
                if (!road.Visited)
                {
                    for (int j = 0; j < road.Relations.Count; j++)
                    {
                        Road nextRoad = road.Relations[j];
                        if (!nextRoad.Visited)
                        {
                            if (lev == 1)
                                nextRoad.Color = Color.yellow;
                            if (lev == 2)
                                nextRoad.Color = Color.red;
                            if (lev == 3)
                                nextRoad.Color = Color.magenta;
                            nextLevel.Add(nextRoad);
                        }
                    }
                }
                road.Visited = true;
            }
            return nextLevel;
        }

        List<Road> GetNextLevel(List<Road> list, int lev)
        {
            List<Road> nextLevel = new List<Road>();
            for (int i = 0; i < list.Count; i++)
            {
                Road road = list[i];
                if (!road.Visited)
                {
                    for (int j = 0; j < road.Relations.Count; j++)
                    {
                        Road nextRoad = road.Relations[j];
                        if (!nextRoad.Visited)
                        {
                            if (lev == 1)
                                nextRoad.Color = Color.yellow;
                            if (lev == 2)
                                nextRoad.Color = Color.red;
                            if (lev == 3)
                                nextRoad.Color = Color.magenta;
                            nextLevel.Add(nextRoad);
                        }
                    }
                }
                road.Visited = true;                 
            }
            return nextLevel;
        }

        int CreateIntersection(Road road)
        {
            current.End = ProjectToLine(current.End, road.Start, road.End);
            if (Intersect(current.Start, current.End, road.Start, road.End))
                return 1;
            else
                return 2;
        }

        bool Intersect(Vector3 oneA, Vector3 oneB, Vector3 twoA, Vector3 twoB)
        {
            return (((twoB.z - oneA.z) * (twoA.x - oneA.x) > (twoA.z - oneA.z) * (twoB.x - oneA.x)) 
                    != ((twoB.z - oneB.z) * (twoA.x - oneB.x) > (twoA.z - oneB.z) * (twoB.x - oneB.x)) 
                        && ((twoA.y - oneA.y) * (oneB.x - oneA.x) > (oneB.z - oneA.z) * (twoA.x - oneA.x)) 
                            != ((twoB.y - oneA.y) * (oneB.x - oneA.x) > (oneB.y - oneA.y) * (twoB.x - oneA.x)));
        }

        float DistanceToLine(Vector3 point, Vector3 start, Vector3 end)
        {
            return Vector3.Magnitude(ProjectToLine(point, start, end) - point);
        }

        Vector3 ProjectToLine(Vector3 point, Vector3 s, Vector3 e)
        {
            Vector3 rhs = point - s;
            Vector3 v2 = end - s;
            float magnitude = v2.magnitude;
            Vector3 lhs = v2;

            if (magnitude > 1E-06f)
                lhs = lhs / magnitude;

            float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
            return (s + lhs * num2);
        }
    }

    public void GetBranches(Road current, Queue<Node> queue, Tile[,] tiles, bool perps)
    {
        List<Node> pNodes = new List<Node>();
        List<Node> aNodes = new List<Node>();
        List<float> densities = new List<float>();

        Node start = current.End;
        int x = (int)Mathf.Floor(start.Position.x);
        int z = (int)Mathf.Floor(start.Position.z);
        DensityType type = tiles[x, z].Type;
        float probability = type.Percentile / 2f;

        pNodes.Add(current.Extend(-15));
        pNodes.Add(current.Extend(0));
        pNodes.Add(current.Extend(15));

        for (int i = 0; i < 3; i++)
        {
            int x1 = (int)Mathf.Floor(pNodes[i].Position.x);
            int z1 = (int)Mathf.Floor(pNodes[i].Position.z);
            //Debug.Log(x + ", " + z);
            densities.Add(tiles[x, z].Value);
        }

        int minI = densities.IndexOf(densities.Min());
        aNodes.Add(pNodes[minI]);

        if (perps == true)
        {
            if (Random.Range(0, 1) < probability)
                aNodes.Add(current.Extend(-90));
            if (Random.Range(0, 1) < probability)
                aNodes.Add(current.Extend(90));
        }

        for (int i = 0; i < aNodes.Count; i++)
        {
            for (int j = 0; j < aNodes.Count; j++)
            {
                aNodes[i].Neighbors.Add(current.End);
            }
            queue.Enqueue(aNodes[i]);
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
}
