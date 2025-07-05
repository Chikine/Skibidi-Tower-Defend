using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class ScaleButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 hoverScale = new(1.2f, 1.2f, 1f); 
    public float scaleSpeed = 0.2f;
    private Vector3 originalScale;

    public void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(hoverScale * 0.9f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(hoverScale));
    }

    private IEnumerator ScaleButton(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.unscaledDeltaTime * 100);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
