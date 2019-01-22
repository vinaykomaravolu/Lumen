using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    private int count;
    public Text countText;
    public Text winText;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
        if (count == 3)
        {
            winText.gameObject.SetActive(true);
            countText.gameObject.SetActive(false);
        }

    }
}
