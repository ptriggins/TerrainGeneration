using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineHelper
{

    public static bool DoSegmentsIntersect(Vector3 l1s, Vector3 l1e, Vector3 l2s, Vector3 l2e)
    {
        Vector2 
            p1 = new Vector2(l1s.x, l1s.z),
            p2 = new Vector2(l1e.x, l1e.z),
            p3 = new Vector2(l2s.x, l2s.z),
            p4 = new Vector2(l2e.x, l2e.z);

        float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

        if (denominator != 0)
        {
            float u_a = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
            float u_b = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

            if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                return true;
        }
        return false;
    }

    public static Vector3 GetIntersection(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
    {
        float a1 = e1.z - s1.z;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.z;

        float a2 = e2.z - s2.z;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.z;

        float delta = a1 * b2 - a2 * b1;
        if (delta == 0)
            return new Vector3(0, 0, 0);
        return new Vector3((b2 * c1 - b1 * c2) / delta, 0, (a1 * c2 - a2 * c1) / delta);
    }

    public static float GetDistanceToLine(Vector3 point, Vector3 lineA, Vector3 lineB)
    {
        return (ProjectPointToLine(point, lineA, lineB) - point).magnitude;
    }

    public static Vector3 ProjectPointToLine(Vector3 point, Vector3 lineS, Vector3 lineE)
    {
        Vector3 rhs = point - lineS;
        Vector3 difference = lineE - lineS;
        float magnitude = difference.magnitude;
        Vector3 lhs = difference;

        if (magnitude > 1E-06f)
            lhs = lhs / magnitude;

        float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineS + lhs * num2);
    }
}
