using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragPoint : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPoint = Input.mousePosition;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(screenPoint);
        transform.position = newPos;
        GetComponentInParent<ImageProjector>().UpdatePosition();
    }
}
