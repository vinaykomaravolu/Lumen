using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathUIFade : MonoBehaviour
{
    public Text uiText;
    public Image backgroundImage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeTextToFullAlpha(1f, this.uiText, this.backgroundImage));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            uiText.color = new Color(uiText.color.r, uiText.color.g, uiText.color.b, 0);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0);
        }
    }



    public IEnumerator FadeTextToFullAlpha(float t, Text j, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 0.85f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        j.color = new Color(j.color.r, j.color.g, j.color.b, 0);
        while (j.color.a < 1.0f)
        {
            j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}