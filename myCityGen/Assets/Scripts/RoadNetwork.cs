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

    Vector3 Start;
    public Tile[,] Tiles;
    public List<Road> Roads;

    public void Initialize(Vector3 start, Tile[,] tiles)
    {
        Start = start;
        Tiles = tiles;
        Roads = new List<Road>();
    }

    public void Generate()
    {
        Vector2 randDir = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randDir.x, 0, randDir.y);
        Vector3 end = Start + direction * Length;

        Queue<Road> candidates = new Queue<Road>();
        candidates.Enqueue(new Road(Start, end));

        int i = 0;
        int limit = 10;

        while (candidates.Count > 0 && i < limit)
        {
            Road current = candidates.Dequeue();
            Roads.Add(current);
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
        //Debug.Log(variants[0] + ", " + variants[1] + ", " + variants[2]);
       
        float[] values = new float[3];
        values[0] = Tiles[(int)variants[0].x, (int)variants[0].z].Value;
        values[1] = Tiles[(int)variants[1].x, (int)variants[1].z].Value;
        values[2] = Tiles[(int)variants[2].x, (int)variants[2].z].Value;

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
        Transform Transform = (Transform)gameObject.GetComponent(typeof(Transform));
        for (int i = 0; i < Roads.Count; i++)
        {
            Roads[i].Draw(Transform);
        }
    }
}
