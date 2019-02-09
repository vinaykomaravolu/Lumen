using System.Collections;
using UnityEngine;

public class SoundControl : MonoBehaviour{

    [Header("Fading")] 
    public float fadeSlow;
    public float fadeFast;
    public float bgmShiftDelay;

    [Header("Effects")]
    public GameObject landing;
    
    [Header("BGMs")]
    public GameObject gameBgm;
    public GameObject loseBgm;
    public GameObject winBgm;
    public Sound currentBgm;

    public void win(){
        setBgm(winBgm, fadeFast);
        currentBgm = Instantiate(winBgm).GetComponent<Sound>();
    }

    public void lose(){
        setBgm(loseBgm, fadeFast);
        currentBgm = Instantiate(loseBgm).GetComponent<Sound>();
    }

    private void setBgm(GameObject newBgm, float fade){
        Sound bgm = newBgm.GetComponent<Sound>();
        if (bgm == null) return;
        currentBgm.startFading(fade);
    }

    public void setBgm(GameObject newBgm){
        setBgm(newBgm, fadeSlow);
        StartCoroutine(startNewBgm(newBgm));
    }

    private IEnumerator startNewBgm(GameObject newBgm){
        yield return new WaitForSecondsRealtime(bgmShiftDelay);
        currentBgm = Instantiate(newBgm).GetComponent<Sound>();
    }
}