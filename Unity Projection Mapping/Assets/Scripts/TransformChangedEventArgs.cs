using System;
using UnityEngine;

public class TransformChangedEventArgs : EventArgs
{
    public Vector3 translation;

    public TransformChangedEventArgs(Vector3 pTranslation)
    {
        translation = pTranslation;
    }
}
