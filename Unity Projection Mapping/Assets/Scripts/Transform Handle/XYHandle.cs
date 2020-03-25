using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class XYHandle : MonoBehaviour, IDragHandler
{
    [SerializeField] private TransformHandle _transform;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 oldPos = transform.position;

        Vector3 screenPoint = Input.mousePosition;
        Vector3 deltaPos =  Camera.main.ScreenToWorldPoint(screenPoint) - oldPos;

        _transform.TranslateX(deltaPos.x);
        _transform.TranslateY(deltaPos.y);
    }
}
