using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCenter
{
    public Vector2 Location;
    public float Density;

    public PopCenter(Vector2 pos, float density)
    {
        Location = pos;
        Density = density;
    }
}
