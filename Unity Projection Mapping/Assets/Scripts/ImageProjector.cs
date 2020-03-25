using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class ImageProjector : MonoBehaviour
{
    public string projectorName { get; set; } = "Image Projector";

    [SerializeField] private TransformHandle _transformHandle = null;

    [Header("Corner positions")]
    public Vector3 topLeft = new Vector3(0, 100, 0);
    public Vector3 topRight = new Vector3(100, 100, 0);
    public Vector3 bottomLeft = new Vector3(0, 0, 0);
    public Vector3 bottomRight = new Vector3(100, 0, 0);
    public Transform[] dragPoints = new Transform[4];

    public Sprite materialThumbnail = null;

    [Header("Settings")]
    [SerializeField] private bool _drawGizmos = false;
    [SerializeField] private int _subdevisionIteration = 3;

    private Mesh _mesh;
    private PolygonCollider2D _collider = null;

    // Mesh data:
    private List<Vector3> _vertices = new List<Vector3>();
    private List<Vector2> _uvs = new List<Vector2>();
    private int[] _triangles;

    private int _vert = 0;
    private int _tris = 0;
    private int _quadSize = 0;


    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
        UpdateDragPositions();
        _transformHandle.TransformChanged += OnTransformChanged;
    }


    /// <summary>
    /// Updates the rectangle corner position to the draggable corner positions. 
    /// </summary>
    public void UpdateDragPositions()
    {
        topLeft = dragPoints[0].position;
        topRight = dragPoints[1].position;
        bottomLeft = dragPoints[2].position;
        bottomRight = dragPoints[3].position;

        DrawPerspectiveQuad();

        updateCollider();
    }


    /// <summary>
    /// Enabled/Disables the draggable corners
    /// </summary>
    public void ToggleSelected(bool pActive)
    {
        _transformHandle.gameObject.SetActive(pActive);

        foreach (Transform dragPoint in dragPoints)
        {
            dragPoint.gameObject.SetActive(pActive);
        }
    }


    /// <summary>
    /// Change the quad material
    /// </summary>
    public void ChangeMaterial(Material pMat)
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material = pMat;
    }


    /// <summary>
    /// Updates the drag point position based on the transform translation
    /// </summary>
    private void OnTransformChanged(object sender, TransformChangedEventArgs e)
    {
        foreach (Transform dragPoint in dragPoints)
        {
            dragPoint.transform.position += e.translation;
        }

        UpdateDragPositions();
    }


    /// <summary>
    /// 1. Creates a rectangle. 2. subdivides rectangle. 3. generate mesh of subdivided rectangle
    /// </summary>
    private void DrawPerspectiveQuad()
    {
        NullChecks();
        ResetData();

        Rectangle rect = new Rectangle(topLeft, topRight, bottomRight, bottomLeft); //Create new Rectangle
        SubdivideRectangle(rect, new Vector2(0, 0), new Vector2(1, 1), 0); //Start recursive algorithm

        _transformHandle.SetPosition(rect.GetOrthographicCenter());

        GenerateMesh();
    }


    /// <summary>
    /// Subdived the rectangle in 4 new rectangles
    /// The method is recursive
    /// </summary>
    private void SubdivideRectangle(Rectangle pRec, Vector2 pUVStart, Vector2 pUVRange, int pIteration)
    {
        if (pIteration++ >= _subdevisionIteration) //if max sub devision, add rectangle to mesh
        {
            InsertRectangleInMesh(pRec, pUVStart, pUVRange);
            return;
        }

        Vector3 vanishPoint1 = pRec.getHorizontalVanishPoint();
        Vector3 vanishPoint2 = pRec.getVerticalVanishPoint();
        Vector3 quadCenter = pRec.GetPerspectiveCenter();
        //     V1 = Where line A-D and B-C intersect
        Vector3 intersection1 = Rectangle.LineIntersection(quadCenter, vanishPoint1, pRec.pointA, pRec.pointB);     // A ------- B                                                                                                                
        Vector3 intersection2 = Rectangle.LineIntersection(quadCenter, vanishPoint1, pRec.pointC, pRec.pointD);     // | \     / |
        Vector3 intersection3 = Rectangle.LineIntersection(quadCenter, vanishPoint2, pRec.pointB, pRec.pointC);     // |    O    |  V2 = Where line A-B and C-D intersect
        Vector3 intersection4 = Rectangle.LineIntersection(quadCenter, vanishPoint2, pRec.pointA, pRec.pointD);     // | /     \ |
                                                                                                                    // D ------- C
        //if (_drawGizmos)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(intersection1, intersection2);
        //    Gizmos.DrawLine(intersection3, intersection4);
        //    pRec.DrawGizmoOutline();
        //}

        // Subdivide rectangle in 4 more rectangles
        Vector2 newUVRange = pUVRange / 2;
        SubdivideRectangle(new Rectangle(pRec.pointA, intersection1, quadCenter, intersection4), new Vector2(pUVStart.x, pUVStart.y + newUVRange.y), newUVRange, pIteration); //Top Left
        SubdivideRectangle(new Rectangle(intersection1, pRec.pointB, intersection3, quadCenter), pUVStart + newUVRange, newUVRange, pIteration); // Top Right
        SubdivideRectangle(new Rectangle(quadCenter, intersection3, pRec.pointC, intersection2), new Vector2(pUVStart.x + newUVRange.x, pUVStart.y), newUVRange, pIteration); // Bottom right
        SubdivideRectangle(new Rectangle(intersection4, quadCenter, intersection2, pRec.pointD), pUVStart, newUVRange, pIteration); // Bottom Left
    }


    /// <summary>
    /// Adds a rectangle in the mesh
    /// Sets: vertices, triangles and uv cordinates
    /// </summary>
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
        _triangles[_tris + 0] = _vert + 0;
        _triangles[_tris + 1] = _vert + 1;
        _triangles[_tris + 2] = _vert + 2;

        // Tris 2
        _triangles[_tris + 3] = _vert + 3;
        _triangles[_tris + 4] = _vert + 0;
        _triangles[_tris + 5] = _vert + 2;

        _vert += 4;
        _tris += 6;
    }


    /// <summary>
    /// Generates mesh out of mesh data
    /// </summary>
    private void GenerateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs.ToArray();

        _mesh.Optimize();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }


    /// <summary>
    /// Resets transform and mesh data
    /// </summary>
    private void ResetData()
    {
        //Reset transform
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        //Clean Data
        _mesh.Clear();
        _vertices.Clear();
        _uvs.Clear();
        _vert = 0;
        _tris = 0;

        //Set new values
        _quadSize = (int)Mathf.Pow(2, _subdevisionIteration);
        _triangles = new int[(_quadSize + 1) * (_quadSize + 1) * 6];
    }


    /// <summary>
    /// Generates the polygon collider for the mesh
    /// </summary>
    private void updateCollider()
    {
        Vector2[] path = { topLeft, topRight, bottomRight, bottomLeft };
        _collider.SetPath(0, path);
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
}








