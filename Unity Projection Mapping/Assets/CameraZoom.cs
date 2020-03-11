using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float _scrollSpeed = 2;

    void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += d * _scrollSpeed;
    }
}
