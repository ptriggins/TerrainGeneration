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
        int limit = 10;
        int num;

        Road previous = null;
        while (candidates.Count > 0 && i < limit)
        {
            Road current = candidates.Dequeue();
            if (previous != null)
                previous.Relations.Add(current);
            Roads.Add(current);

            //int t = CheckIntersections(current);
            //Debug.Log(t);

            if (limit - i > 3)
                num = 3;
            else
                num = limit - i;

            GetBranches(num, current, candidates, tiles, true);

            previous = current;
            i ++;
            /*
            if (t == 0)
                GetCandidates(current, candidates, tiles, true);
            else if (t == 1)
                GetCandidates(current, candidates, tiles, false);
            */
        }
        CheckIntersect(Roads[Roads.Count - 1]);
    }

    public int CheckIntersect(Road current)
    {
        for (int i = 0; i < Roads.Count; i++)
        {
            Roads[i].Visited = false;
        }

        current.Color = Color.blue;
        List<Road> roads = new List<Road> {current};

        roads = GetNextLevel(roads, 1);
        roads = GetNextLevel(roads, 2);
        int level = 2;

        while (roads.Count > 0)
        {
            List<float> distances = new List<float>();
            for (int i = 0; i < roads.Count; i++)
            {
                distances.Add(DistanceToLine(current.End, roads[i].Start, roads[i].End));
                //Debug.Log(DistanceToLine(current.End, roads[i].Start, roads[i].End));
            }

            for (int i = 0; i < Roads.Count; i++)
            {
                int iMin = distances.IndexOf(distances.Min());
                Road road = roads[iMin];

                if (distances[iMin] < 2)
                    return CreateIntersection(road);
                distances[iMin] = float.MaxValue;
            }
            roads = GetNextLevel(roads, level);
            level++;
        }
        return 0;

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

    public void GetBranches(int num, Road current, Queue<Road> queue, Tile[,] tiles, bool perps)
    {
        List<Road> pBranches = new List<Road>();
        List<Road> aBranches = new List<Road>();
        List<float> densities = new List<float>();

        DensityType type = tiles[(int)current.End.x, (int)current.End.z].Type;

        Vector3 start = current.End;
        pBranches.Add(GetBranch(current, -15, 1));
        pBranches.Add(GetBranch(current, 0, 1));
        pBranches.Add(GetBranch(current, 15, 1));
        
        if (perps == true)
        {
            pBranches.Add(GetBranch(current, -90, type.Percentile / 2));
            pBranches.Add(GetBranch(current, -90, type.Percentile / 2));
        }

        for (int i = 0; i < pBranches.Count; i++)
        {
            int x = (int)pBranches[i].End.x;
            int z = (int)pBranches[i].End.z;
            densities.Add(tiles[x, z].Value);
        }

        for (int i = 0; i < num; i++)
        {
            int minI = densities.IndexOf(densities.Min());
            aBranches.Add(pBranches[minI]);
            pBranches.Remove(pBranches[minI]);
        }

        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                aBranches[i].Relations.Add(current);
                if (i != j)
                    aBranches[i].Relations.Add(aBranches[j]);
            }
        }

        for (int i = 0; i < num; i++)
        {
            //Debug.Log(aBranches[i].Start);
            queue.Enqueue(aBranches[i]);
        }
        
        Road GetBranch(Road road, float rotation, float probability)
        {
            if (Random.Range(0, 1) < probability)
                return new Road(road.End, road.Extend(rotation));
            else
                return null;
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
