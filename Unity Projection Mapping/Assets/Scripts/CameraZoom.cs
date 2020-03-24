using UnityEngine;

/// <summary>
/// Camera zoom in and out with the left and right arrow keys
/// </summary>
public class CameraZoom : MonoBehaviour
{
    public float _scrollSpeed = 2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) // Increase camera size
        {
            Camera.main.orthographicSize += _scrollSpeed;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)) // Decrease camera size
        {
            Camera.main.orthographicSize -= _scrollSpeed;
        }
    }
}
