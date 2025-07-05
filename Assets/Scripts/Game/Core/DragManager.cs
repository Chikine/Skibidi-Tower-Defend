using UnityEngine;

public class DragManager : MonoBehaviour
{
    private Draggable draggable;
    private bool isDragging = false;
    private Vector2 position;
    private Vector3 worldPosition;

    public void Update()
    {
        if (isDragging)
        {
            if (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                drop();
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            position = new(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount >= 1)
        {
            position = Input.GetTouch(0).position;
        }
        else return;

        worldPosition = Camera.main.ScreenToWorldPoint(position);
        if (!isDragging)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (raycastHit2D.collider != null)
            {
                Draggable draggableObj = raycastHit2D.transform.gameObject.GetComponent<Draggable>();
                if (draggableObj != null)
                {
                    draggable = draggableObj;
                    if (draggable.startReq())
                    {
                        startDrag();
                    }
                }
            }
        }
        else
        {
            dragging();
        }
    }
    private void startDrag()
    {
        isDragging = true;
        draggable.startDrag();
    }

    private void dragging()
    {
        draggable.transform.position = new(worldPosition.x, worldPosition.y);
        draggable.dragging();
    }

    private void drop()
    {
        isDragging = false;
        draggable.drop();
    }
}
