using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Draggable point for the ImageProjector
/// </summary>
public class dragPoint : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPoint = Input.mousePosition;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(screenPoint);
        transform.position = newPos;
        GetComponentInParent<ImageProjector>().UpdateDragPositions();
    }
}
