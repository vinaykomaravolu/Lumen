using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathUIFade : MonoBehaviour
{
    public Text uiText;
    public Image backgroundImage;

    public IEnumerator FadeTextToFullAlpha(float t)
    {
        Text j = this.uiText;
        Image i = this.backgroundImage;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        j.color = new Color(j.color.r, j.color.g, j.color.b, 0);
        while (i.color.a < 0.85f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        j.color = new Color(j.color.r, j.color.g, j.color.b, 0);
        while (j.color.a < 1.0f)
        {
            j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}