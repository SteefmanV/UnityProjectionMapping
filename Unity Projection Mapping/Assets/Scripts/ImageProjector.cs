using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class ImageProjector : MonoBehaviour
{
    public bool drawGizmos = false;
    public int subdevisionIteration = 2;
    public int quadSize = 0;

    public Vector3 topLeft = new Vector3(0, 100, 0);
    public Vector3 topRight = new Vector3(100, 100, 0);
    public Vector3 bottomLeft = new Vector3(0, 0, 0);
    public Vector3 bottomRight = new Vector3(100, 0, 0);

    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private List<Vector2> _uvs = new List<Vector2>();
    private int[] _triangles;

    private int vert = 0;
    private int tris = 0;

    void OnDrawGizmos()
    {
        DrawPerspectiveQuad();
    }

    private void DrawPerspectiveQuad()
    {
        NullChecks();
        ResetData();

        Rectangle rect = new Rectangle(topLeft, topRight, bottomRight, bottomLeft); //Create new Rectangle
        SubdivideRectangle(rect, new Vector2(0, 0), new Vector2(1, 1), 0); //Start recursive algorithm

        GenerateMesh();
    }


    private void SubdivideRectangle(Rectangle pRec, Vector2 pUVStart, Vector2 pUVRange, int pIteration)
    {
        if (pIteration++ >= subdevisionIteration) //if max sub devision, add rectangle to mesh
        {
            InsertRectangleInMesh(pRec, pUVStart, pUVRange);
            return;
        }

        Vector3 vanishPoint1 = pRec.getHorizontalVanishPoint();
        Vector3 vanishPoint2 = pRec.getVerticalVanishPoint();
        Vector3 quadCenter = pRec.GetCenter();

        Vector3 intersection1 = Rectangle.LineIntersection(quadCenter, vanishPoint1, pRec.pointA, pRec.pointB);
        Vector3 intersection2 = Rectangle.LineIntersection(quadCenter, vanishPoint1, pRec.pointC, pRec.pointD);
        Vector3 intersection3 = Rectangle.LineIntersection(quadCenter, vanishPoint2, pRec.pointB, pRec.pointC);
        Vector3 intersection4 = Rectangle.LineIntersection(quadCenter, vanishPoint2, pRec.pointA, pRec.pointD);

        if (drawGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(intersection1, intersection2);
            Gizmos.DrawLine(intersection3, intersection4);
            pRec.DrawGizmoOutline();
        }

        Vector2 newUVRange = pUVRange / 2;
        SubdivideRectangle(new Rectangle(pRec.pointA, intersection1, quadCenter, intersection4), new Vector2(pUVStart.x, pUVStart.y + newUVRange.y), newUVRange, pIteration); //Top Left
        SubdivideRectangle(new Rectangle(intersection1, pRec.pointB, intersection3, quadCenter), pUVStart + newUVRange, newUVRange, pIteration); // Top Right
        SubdivideRectangle(new Rectangle(quadCenter, intersection3, pRec.pointC, intersection2), new Vector2(pUVStart.x + newUVRange.x, pUVStart.y), newUVRange, pIteration); // Bottom right
        SubdivideRectangle(new Rectangle(intersection4, quadCenter, intersection2, pRec.pointD), pUVStart, newUVRange, pIteration); // Bottom Left
    }


    void GenerateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs.ToArray();

        _mesh.Optimize();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }


    private void ResetData()
    {
        //Reset transform
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        //Clean Data
        _mesh.Clear();
        _vertices.Clear();
        _uvs.Clear();
        vert = 0;
        tris = 0;

        //Set new values
        quadSize = (int)Mathf.Pow(2, subdevisionIteration);
        _triangles = new int[(quadSize + 1) * (quadSize + 1) * 6];
    }

    private void NullChecks()
    {
        if (_mesh == null)
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        if (GetComponent<MeshRenderer>().sharedMaterial == null)
        {
            meshRend.material = new Material(Shader.Find("Standard"));
        }
    }


    private void InsertRectangleInMesh(Rectangle pRec, Vector2 pUVStart, Vector2 pUVRange)
    {
        // 4 Vertices
        _vertices.Add(pRec.pointA);
        _vertices.Add(pRec.pointB);
        _vertices.Add(pRec.pointC);
        _vertices.Add(pRec.pointD);

        // Texture Cordinates
        _uvs.Add(new Vector2(pUVStart.x, pUVStart.y + pUVRange.y));
        _uvs.Add(pUVStart + pUVRange);
        _uvs.Add(new Vector2(pUVStart.x + pUVRange.x, pUVStart.y));
        _uvs.Add(pUVStart);

        // Tris 1
        _triangles[tris + 0] = vert + 0;
        _triangles[tris + 1] = vert + 1;
        _triangles[tris + 2] = vert + 2;

        // Tris 2
        _triangles[tris + 3] = vert + 3;
        _triangles[tris + 4] = vert + 0;
        _triangles[tris + 5] = vert + 2;

        vert += 4;
        tris += 6;
    }
}
