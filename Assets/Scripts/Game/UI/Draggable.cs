using UnityEngine;

public class Draggable : MonoBehaviour
{
    private IDragBehavior dragBehavior;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        dragBehavior = GetComponent<IDragBehavior>();
    }
    public bool startReq()
    {
        return dragBehavior == null || dragBehavior.startRequire();
    }
    public void startDrag()
    {
        dragBehavior?.onStartDrag();
    }

    public void dragging()
    {
        dragBehavior?.onDragging();
    }

    public void drop()
    {
        dragBehavior?.onDrop();
    }
}
