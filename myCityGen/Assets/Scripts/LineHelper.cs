using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineHelper
{
    public static bool DoLinesIntersect(Vector3 line1a, Vector3 line1b, Vector3 line2a, Vector3 line2b)
    {
        return (((line2b.z - line1a.z) * (line2a.x - line1a.x) > (line2a.z - line1a.z) * (line2b.x - line1a.x))
                != ((line2b.z - line1b.z) * (line2a.x - line1b.x) > (line2a.z - line1b.z) * (line2b.x - line1b.x))
                    && ((line2a.y - line1a.y) * (line1b.x - line1a.x) > (line1b.z - line1a.z) * (line2a.x - line1a.x))
                        != ((line2b.y - line1a.y) * (line1b.x - line1a.x) > (line1b.y - line1a.y) * (line2b.x - line1a.x)));
    }

    public static float GetDistanceToLine(Vector3 point, Vector3 lineA, Vector3 lineB)
    {
        return (ProjectPointToLine(point, lineA, lineB) - point).magnitude;
    }

    public static Vector3 ProjectPointToLine(Vector3 point, Vector3 lineA, Vector3 lineB)
    {
        Vector3 rhs = point - lineA;
        Vector3 difference = lineB - lineA;
        float magnitude = difference.magnitude;
        Vector3 lhs = difference;

        if (magnitude > 1E-06f)
            lhs = lhs / magnitude;

        float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineA + lhs * num2);
    }
}
