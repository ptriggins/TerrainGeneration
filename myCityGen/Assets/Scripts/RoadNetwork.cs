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

    public Vector3 TopLeft;
    public Tile[,] Tiles;
    public float[,] Values;
    public List<Road> Accepted;

    private Transform Transform;

    public void Initialize(Tile[,] tiles, float[,] values, Vector3 topleft)
    {
        TopLeft = topleft;
        Tiles = tiles;
        Values = values;
    }

    public void Instantiate()
    {
        Accepted = new List<Road>();
    }

    public void Generate(Vector3 start)
    {
        Vector3 firstStart = TopLeft + start;
        Vector2 random = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(random.x, 0, random.y);
        Vector3 firstEnd = start + direction * Length;

        Queue<Road> candidates = new Queue<Road>();
        candidates.Enqueue(new Road(firstStart, firstEnd));

        int i = 0;
        int limit = 10;

        while (candidates.Count > 0 && i < limit)
        {
            Road current = candidates.Dequeue();
            Accepted.Add(current);
            GetCandidates(current, candidates);
            i++;
        }
        
    }

    public void GetCandidates(Road current, Queue<Road> queue)
    {
        Vector3 start = current.End;

        Vector3[] variants = new Vector3[3];
        variants[0] = current.GetExtension(-20, Length);
        variants[1] = current.GetExtension(0, Length);
        variants[2] = current.GetExtension(20, Length);

        float[] values = new float[3];
        values[0] = Values[(int)variants[0].x, (int)variants[0].z];
        values[1] = Values[(int)variants[1].x, (int)variants[1].z];
        values[2] = Values[(int)variants[2].x, (int)variants[2].z];

        for (int i = 0; i < 3; i++)
        {
            if (values[i] == values.Max())
                queue.Enqueue(new Road(start, variants[i]));
        }

        DensityType type = Tiles[(int)start.x, (int)start.z].Type;
  
        if (Random.Range(0, 1) < type.Percentile / 2)
            queue.Enqueue(new Road(start, current.GetExtension(90, Length)));
        if (Random.Range(0, 1) < type.Percentile / 2)
            queue.Enqueue(new Road(start, current.GetExtension(90, Length)));
    }

    public void Draw()
    {
        for (int i = 0; i < Accepted.Count; i++)
        {

        }
    }
}
