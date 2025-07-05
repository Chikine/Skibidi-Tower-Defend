using System.Collections;
using UnityEngine;

public class AppearRangeWhenPlayAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float duration;
    public float startAlpha;
    private Tower tower;
    public void Start()
    {
        tower = GetComponentInParent<Tower>();
        setRange();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void showRange()
    {
        StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool inout)
    {
        float start = inout ? 0f : startAlpha;
        float end = inout ? startAlpha : 0f;
        float time = 0f;

        Color color = spriteRenderer.color;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, time / duration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(color.r, color.g, color.b, end);
    }

    public void setRange()
    {
        float range = tower.range;
        transform.localScale = new Vector3(range * 20, range * 20, range * 20);
    }
}
