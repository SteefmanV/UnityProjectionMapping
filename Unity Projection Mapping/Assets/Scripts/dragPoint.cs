using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Draggable point for the ImageProjector
/// </summary>
public class dragPoint : MonoBehaviour, IDragHandler
{
    private const int _borderOffset = 10;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenPoint = Input.mousePosition;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(screenPoint);
        newPos.z = -80;

        if (newPos.x > Screen.width - _borderOffset) newPos.x = Screen.width - _borderOffset;
        if (newPos.x < _borderOffset) newPos.x = _borderOffset;
        if (newPos.y > Screen.height - _borderOffset) newPos.y = Screen.height - _borderOffset;
        if (newPos.y < _borderOffset) newPos.y = _borderOffset;

        transform.position = newPos;
        GetComponentInParent<ImageProjector>().UpdateDragPositions();
    }
}
