using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform dragHandle; 
    RectTransform rootRect;
    Canvas rootCanvas;
    Vector2 pointerOffset;

    void Awake()
    {
        rootRect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas == null) Debug.LogWarning("DraggableWindow: Canvas parent not found.");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rootRect.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootRect, eventData.position, eventData.pressEventCamera, out pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragHandle != null)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(dragHandle, eventData.position, eventData.pressEventCamera))
                return;
        }

        if (rootCanvas == null) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector2 pivotOffset = new Vector2(rootRect.rect.width * rootRect.pivot.x, rootRect.rect.height * rootRect.pivot.y);
            Vector2 newPos = localPointerPosition - pointerOffset;
            rootRect.anchoredPosition = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ClampToWindow();
    }

    void ClampToWindow()
    {
        if (rootCanvas == null) return;
        RectTransform canvasRect = rootCanvas.transform as RectTransform;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        Vector3[] windowCorners = new Vector3[4];
        rootRect.GetWorldCorners(windowCorners);

        Vector2 offset = Vector2.zero;

        if (windowCorners[0].x < canvasCorners[0].x) offset.x = canvasCorners[0].x - windowCorners[0].x;
        if (windowCorners[2].x > canvasCorners[2].x) offset.x = canvasCorners[2].x - windowCorners[2].x;
        if (windowCorners[0].y < canvasCorners[0].y) offset.y = canvasCorners[0].y - windowCorners[0].y;
        if (windowCorners[2].y > canvasCorners[2].y) offset.y = canvasCorners[2].y - windowCorners[2].y;

        rootRect.position += (Vector3)offset;
    }
}
