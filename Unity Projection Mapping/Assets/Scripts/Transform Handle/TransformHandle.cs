using System;
using UnityEngine;

public class TransformHandle : MonoBehaviour
{
    /// <summary>
    /// Gets called each time when a translation happend
    /// </summary>
    public event EventHandler<TransformChangedEventArgs> TransformChanged; 


    /// <summary>
    /// Translates the handles position over the X axis
    /// </summary>
    public void TranslateX(float pTranslation)
    {
        Vector3 pos = transform.position;
        pos.x += pTranslation;
        transform.position = pos;

        TransformChanged?.Invoke(this, new TransformChangedEventArgs(new Vector3(pTranslation, 0, 0)));
    }


    /// <summary>
    /// Translates the handles position over the Y axis
    /// </summary>
    public void TranslateY(float pTranslation)
    {
        Vector3 pos = transform.position;
        pos.y += pTranslation;
        transform.position = pos;

        TransformChanged?.Invoke(this, new TransformChangedEventArgs(new Vector3(0, pTranslation, 0)));
    }


    /// <summary>
    /// Set the position of the handle
    /// </summary>
    public void SetPosition(Vector3 pPos)
    {
        transform.position = pPos;
    }
}
