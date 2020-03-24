using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageProjector))]
public class MoveHandlesEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        ImageProjector example = (ImageProjector)target;
        float size = HandleUtility.GetHandleSize(example.topLeft) * 0.2f;
        Vector3 snap = Vector3.one * 0.5f;

        Vector3 topLeft = Handles.FreeMoveHandle(example.topLeft, Quaternion.identity, size, snap, Handles.SphereHandleCap);
        Vector3 topRight = Handles.FreeMoveHandle(example.topRight, Quaternion.identity, size, snap, Handles.SphereHandleCap);
        Vector3 bottomRight = Handles.FreeMoveHandle(example.bottomRight, Quaternion.identity, size, snap, Handles.SphereHandleCap);
        Vector3 bottomLeft = Handles.FreeMoveHandle(example.bottomLeft, Quaternion.identity, size, snap, Handles.SphereHandleCap);

        example.topLeft = topLeft;
        example.topRight = topRight;
        example.bottomRight = bottomRight;
        example.bottomLeft = bottomLeft;

        example.dragPoints[0].transform.position = topLeft;
        example.dragPoints[1].transform.position = topRight;
        example.dragPoints[2].transform.position = bottomLeft;
        example.dragPoints[3].transform.position = bottomRight;

    }
}
