using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowingText : MonoBehaviour
{
    private Text text;
    public int initSize;
    public int targetSize;
    public int delay;
    private int counter;

    private void Start()
    {
        text = GetComponent<Text>();
        counter = 0;
    }

    private void Update()
    {
        counter += 1;
        if (counter == delay)
        {
            counter = 0;
            if (text.fontSize < targetSize)
            {
                text.fontSize += 1;
            }
            else if (text.fontSize > targetSize)
            {
                text.fontSize -= 1;
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        text.fontSize = initSize;
    }

    public void OnTriggerExit(Collider other)
    {
        text.fontSize = targetSize;
    }

    public void reset()
    {
        text.fontSize = initSize;
        targetSize = initSize;
    }
}
