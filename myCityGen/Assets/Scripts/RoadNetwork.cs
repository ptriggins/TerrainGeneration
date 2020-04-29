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

    public List<Road> Roads;
    public List<GameObject> Lines;

    public void Instantiate()
    {
        Roads = new List<Road>();
        Lines = new List<GameObject>();
    }

    public void Generate(Vector3 start, Tile[,] tiles)
    {
        start.y += .1f;
        Vector2 randDir = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randDir.x, 0, randDir.y);
        Vector3 end = start + direction * Length;

        Queue<Road> candidates = new Queue<Road>();
        candidates.Enqueue(new Road(start, end));

        int i = 0;
        int limit = 100;

        while (candidates.Count > 0 && i < limit)
        {
            Road current = candidates.Dequeue();
            Roads.Add(current);

            int t = CheckIntersections(current);
            if (t == 0)
                GetCandidates(current, candidates, tiles, true);
            else if (t == 1)
                GetCandidates(current, candidates, tiles, false);
            i++;
        }
    }

    public int CheckIntersections(Road current)
    {
        List<Road> roads = new List<Road>();
        GetNextRoads(current, roads);

        int level = 1;

        while (roads.Count > 0)
        {
            List<float> distances = new List<float>();
            for (int i = 0; i < roads.Count; i++)
            {
                distances.Add(DistanceToLine(current.End, roads[i].Start, roads[i].End));
                //Debug.Log(DistanceToLine(current.End, roads[i].Start, roads[i].End));
            }

            List<Road> nextRoads = new List<Road>();
            while (roads.Count > 0)
            {
                float min = distances.Min();
                Road road = roads[distances.IndexOf(min)];

                if (min < 2 && level > 1)
                    return CreateIntersection(road);
                GetNextRoads(road, nextRoads);
                distances.Remove(distances[distances.IndexOf(min)]);
                roads.Remove(road);
            }
            level++;
        }
        return 0;

        void GetNextRoads(Road road, List<Road> list)
        {
            if (road.Next.Contains(road.Previous) || road.Previous == null)
                for (int i = 0; i < road.Last.Count; i++)
                {
                    list.Add(road.Last[i]);
                    road.Last[i].Previous = road;
                }
            else if (road.Last.Contains(road.Previous))
                for (int i = 0; i < road.Next.Count; i++)
                {
                    list.Add(road.Next[i]);
                    road.Next[i].Previous = road;
                }
        }

        int CreateIntersection(Road road)
        {
            Debug.Log("test");
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

        Vector3 ProjectToLine(Vector3 point, Vector3 start, Vector3 end)
        {
            Vector3 rhs = point - start;
            Vector3 v2 = end - start;
            float magnitude = v2.magnitude;
            Vector3 lhs = v2;

            if (magnitude > 1E-06f)
                lhs = lhs / magnitude;

            float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
            return (start + lhs * num2);
        }

    }

    public void GetCandidates(Road current, Queue<Road> queue, Tile[,] tiles, bool branches)
    {
        Vector3[] variants = new Vector3[3];
        variants[0] = current.GetExtension(-15, Length);
        variants[1] = current.GetExtension(0, Length);
        variants[2] = current.GetExtension(15, Length);

        Debug.Log((int)variants[0].x + ", " + (int)variants[0].z);
        Debug.Log((int)variants[1].x + ", " + (int)variants[1].z);
        Debug.Log((int)variants[2].x + ", " + (int)variants[2].z);
        Debug.Log("");

        float[] values = new float[3];
        values[0] = tiles[(int)variants[0].x, (int)variants[0].z].Value;
        values[1] = tiles[(int)variants[1].x, (int)variants[1].z].Value;
        values[2] = tiles[(int)variants[2].x, (int)variants[2].z].Value;

        Road straight = null;
        Road branch1 = null;
        Road branch2 = null;

        for (int i = 0; i < 3; i++)
        {
            if (values[i] == values.Max())
            {
                straight = new Road(current.End, variants[i]);
                straight.Last.Add(current);
                current.Next.Add(straight);
            }
        }

        if (branches == true)
        {
            DensityType type = tiles[(int)current.Start.x, (int)current.Start.z].Type;

            if (Random.Range(0, 1) < type.Percentile / 2)
            {
                branch1 = new Road(current.End, current.GetExtension(90, Length));
                straight.Last.Add(branch1);
                current.Next.Add(branch1);
            }
            if (Random.Range(0, 1) < type.Percentile / 2)
            {
                branch2 = new Road(current.End, current.GetExtension(-90, Length));
                straight.Last.Add(branch2);
                current.Next.Add(branch2);
            }

            if (branch1 != null && branch2 != null)
            {
                branch1.Last.Add(branch2);
                branch1.Last.Add(straight);
                branch2.Last.Add(branch1);
                branch2.Last.Add(straight);
                queue.Enqueue(branch1);
                queue.Enqueue(branch2);
            }
            else if (branch1 == null)
            {
                branch2.Last.Add(straight);
                queue.Enqueue(branch2);
            }
            else if (branch2 == null)
            {
                branch1.Last.Add(straight);
                queue.Enqueue(branch2);
            }
        }
        queue.Enqueue(straight);
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
