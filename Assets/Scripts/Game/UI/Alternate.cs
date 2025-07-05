//using System.Drawing;
using UnityEngine;

public class Alternate : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color color;
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }
    public void hide()
    {
        color.a = 0f;
        spriteRenderer.color = color;
    }

    public void show()
    {
        color.a = 0.3f;
        spriteRenderer.color = color;
    }
}
