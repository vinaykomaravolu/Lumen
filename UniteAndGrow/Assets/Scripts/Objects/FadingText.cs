using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    public CanvasGroup uiElement;
    public float targetAlpha;
    public float speed;

    private void Update(){
        uiElement.alpha = Mathf.MoveTowards(uiElement.alpha, targetAlpha, speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other){
        targetAlpha = 1;
    }
    
    public void OnTriggerExit(Collider other){
        targetAlpha = 0;
    }
}
