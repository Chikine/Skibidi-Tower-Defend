using UnityEngine;
public interface IDragBehavior
{
    bool startRequire();
    void onStartDrag();
    void onDragging();
    void onDrop();
}
