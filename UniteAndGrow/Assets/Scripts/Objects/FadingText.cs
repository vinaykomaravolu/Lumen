using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingText : MonoBehaviour{
    
    private CanvasGroup canvasGroup;
    public GameObject uiElement;
    public float initAlpha;
    public float targetAlpha;
    public float speed;

    private void Start(){
        canvasGroup = uiElement.AddComponent<CanvasGroup>();
        canvasGroup.alpha = initAlpha;
    }

    private void Update(){
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.unscaledDeltaTime);
    }

    public void OnTriggerEnter(Collider other){
        targetAlpha = 1;
    }
    
    public void OnTriggerExit(Collider other){
        targetAlpha = 0;
    }
}
