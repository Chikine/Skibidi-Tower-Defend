using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfirmBox : MonoBehaviour
{
    public GameObject confirmBox;
    public TextMeshProUGUI confirmText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
    }

    public void setText(string text)
    {
        confirmText.text = text;
    }

    public void Show()
    {
        confirmBox.SetActive(true);
    }
    public void Hide()
    {
        confirmBox.SetActive(false);
    }
}
