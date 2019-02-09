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
    public GameObject loseBgm;
    public GameObject winBgm;
    private Sound currentAmbient;
    private Sound currentMusic;

    public void win(){
        changeCurrentFast(winBgm);
    }

    public void lose(){
        changeCurrentFast(loseBgm);
    }

    private Sound getCurrent(SoundType type){
        if (type == SoundType.Music) return currentMusic;
        if (type == SoundType.Ambient) return currentAmbient;
        return null;
    }

    public void updateCurrent(GameObject newSound){
        updateCurrent(newSound.GetComponent<Sound>());
    }

    public void updateCurrent(Sound newSound){
        SoundType type = newSound.type;
        if (type == SoundType.Music) currentMusic = newSound;
        if (type == SoundType.Ambient) currentAmbient = newSound;
    }

    // return true if current is null
    private bool fadeCurrent(SoundType type, float fade){
        Sound current = getCurrent(type);
        if (current != null) current.startFading(fade);
        return current == null;
    }

    private void changeCurrentFast(GameObject newSound){
        fadeCurrent(newSound.GetComponent<Sound>().type, fadeFast);
        updateCurrent(Instantiate(newSound));
    }

    public void changeCurrentSlow(GameObject newSound){
        if (fadeCurrent(newSound.GetComponent<Sound>().type, fadeSlow)){
            updateCurrent(Instantiate(newSound));
        } else{
            StartCoroutine(startNew(newSound));
        }
    }

    private IEnumerator startNew(GameObject newSound){
        yield return new WaitForSecondsRealtime(bgmShiftDelay);
        updateCurrent(Instantiate(newSound));
    }
}