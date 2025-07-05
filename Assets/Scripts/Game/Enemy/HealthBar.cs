using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //get y scale
    private float yScale;
    //private obj
    private Enemy enemy;
    private float maxHP;
    private float targetMaskScaleX;
    private float speed = 0.2f;
    private float duration = 1f;
    private float maxLen;
    //hp display
    public Transform currentHPObject; 
    public Transform maskHPObject;   
    // origin scale
    private Vector3 maskOriginalScale;
    //show coroutine
    private Coroutine barCoroutine;

    public void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        maxHP = enemy.HP;
        maxLen = currentHPObject.localScale.x;
        maskOriginalScale = maskHPObject.localScale;
        yScale = transform.localScale.y;
        hide();
    }

    public void Update()
    {
        if (maskHPObject.localScale.x >= targetMaskScaleX)
        {
            maskHPObject.localScale = new Vector3(targetMaskScaleX, maskOriginalScale.y, maskOriginalScale.z);
        }
        else
        {
            maskHPObject.localScale = new Vector3(
                Mathf.MoveTowards(maskHPObject.localScale.x, targetMaskScaleX, Time.deltaTime * speed),
                maskOriginalScale.y,
                maskOriginalScale.z
            );
        }
    }

    public void ScaleUpdate()
    {
        if (this == null) return;
        ShowBarWhenHit(duration);
        float healthRatio = Mathf.Clamp01(enemy.HP / maxHP); 
        float currentHPScale = maxLen * healthRatio;
        targetMaskScaleX = maxLen - currentHPScale; 
        currentHPObject.localScale = new Vector3(currentHPScale, currentHPObject.localScale.y, currentHPObject.localScale.z);
    }
    
    private void hide()
    {
        if (this == null) return;
        if (enemy != null) transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    }

    private void show()
    {
        if (this == null) return;
        if (enemy != null) transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
    }
    private void ShowBarWhenHit(float duration)
    {
        if (!this.enabled) return;
        if (this == null) return;
        if (barCoroutine != null)
        {
            StopCoroutine(barCoroutine);
        }
        barCoroutine = StartCoroutine(GetBarInSeconds(duration));

    }

    IEnumerator GetBarInSeconds(float duration)
    {
        show();
        yield return new WaitForSeconds(duration);
        if(this.enabled) hide();
    }
}
