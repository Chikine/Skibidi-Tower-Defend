using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollCoinWhenClick : MonoBehaviour
{
    public Sprite[] dongToStar;
    public Sprite[] starToDong;
    public float rollTime = 0.5f;
    private bool head = true;
    private Coroutine rollCoroutine;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void rollCoin()
    {
        if (rollCoroutine != null) return; 
        head = !head;
        StartCoroutine(roll(rollTime, head ? starToDong : dongToStar));
    }

    private IEnumerator roll(float time, Sprite[] sprites)
    {
        float interval = time / sprites.Length;
        int index = -1;
        while (++index < sprites.Length)
        {
            image.sprite = sprites[index];
            yield return new WaitForSeconds(interval);
        }
        rollCoroutine = null;
    }
}
