using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


struct Rectangle
{
    // A ---- B
    // |      |
    // |      |
    // D ---- C

    public Vector2 pointA { get; set; }
    public Vector2 pointB { get; set; }
    public Vector2 pointC { get; set; }
    public Vector2 pointD { get; set; }


    public Rectangle(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD)
    {
        pointA = pA;
        pointB = pB;
        pointC = pC;
        pointD = pD;
    }


    public void DrawGizmoOutline()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawLine(pointB, pointC);
        Gizmos.DrawLine(pointC, pointD);
        Gizmos.DrawLine(pointD, pointA);

        //Draw center intersection lines
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA, pointC);
        Gizmos.DrawLine(pointB, pointD);
    }


    /// <summary>
    /// Return the perspective center of the Rectangle
    /// </summary>
    public Vector2 GetCenter()
    {
        return LineIntersection(pointA, pointC, pointB, pointD);
    }


    /// <summary>
    /// Returns the position of intersection with AD & BC
    /// </summary>
    public Vector3 getHorizontalVanishPoint()
    {
        return LineIntersection(pointA, pointD, pointB, pointC);
    }


    /// <summary>
    /// Returns position of intersection with AB & CD
    /// </summary>
    public Vector3 getVerticalVanishPoint()
    {
        return LineIntersection(pointA, pointB, pointC, pointD);
    }


    /// <summary>
    /// Returns the position where 2 lines are intersecting
    /// </summary>
    /// <param name="l1Head"> Line 1, point A</param>
    /// <param name="l1Tail"> Line 1, point B</param>
    /// <param name="l2Head"> Line 2, point A</param>
    /// <param name="l2Tail"> Line 2, point B</param>
    public static Vector2 LineIntersection(Vector3 l1Head, Vector3 l1Tail, Vector3 l2Head, Vector3 l2Tail)
    {
        Vector3 lineVec1 = l1Tail - l1Head;
        Vector2 lineVec2 = l2Tail - l2Head;
        Vector3 lineVec3 = l2Tail - l1Head;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parrallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            return l1Head + (lineVec1 * s);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
