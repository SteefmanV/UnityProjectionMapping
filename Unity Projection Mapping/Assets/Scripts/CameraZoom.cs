using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float _scrollSpeed = 2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Camera.main.orthographicSize += _scrollSpeed;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Camera.main.orthographicSize -= _scrollSpeed;
        }
    }
}
