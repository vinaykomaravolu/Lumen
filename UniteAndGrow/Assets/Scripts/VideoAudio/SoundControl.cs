using System.Collections;
using UnityEngine;

public class SoundControl : MonoBehaviour{

    [Header("Fading")] 
    public float fadeSlow;
    public float fadeFast;
    public float shiftDelay;

    [Header("Effects")]
    public GameObject landing;
    public GameObject collecting;
    public GameObject jumping;
    
    [Header("BGMs")]
    public GameObject winBgm;
    private readonly CurrentSound currentAmbient = new CurrentSound();
    private readonly CurrentSound currentMusic = new CurrentSound();

    public void win(){
        changeCurrent(winBgm, fadeFast, 0);
    }

    public void jump(){
        Instantiate(jumping);
    }

    public void collect(){
        Instantiate(collecting);
    }

    public void land(){
        Instantiate(landing);
    }

    private void Update(){
        currentAmbient.checkStartNew();
        currentMusic.checkStartNew();
    }

    public void updateCurrent(GameObject newSound){
        Sound sound = newSound.GetComponent<Sound>();
        getCurrent(sound.type).sound = sound;
    }

    public void changeCurrentSlow(GameObject newSound){
        changeCurrent(newSound, fadeSlow, shiftDelay);
    }

    private CurrentSound getCurrent(SoundType type){
        if (type == SoundType.Music) return currentMusic;
        if (type == SoundType.Ambient) return currentAmbient;
        return null;
    }

    private void changeCurrent(GameObject newSound, float fade, float delay){
        Sound sound = newSound.GetComponent<Sound>();
        getCurrent(sound.type).setStartNew(newSound, fade, delay);
    }

    private class CurrentSound{
        public Sound sound;
        public float shiftTime = float.PositiveInfinity;
        public GameObject next;

        public void setStartNew(GameObject newSound, float fade, float delay){
            if (sound != null && 
                sound.audio.clip.name == newSound.GetComponent<AudioSource>().clip.name) return;
            next = newSound;
            if (sound == null){
                shiftTime = Time.realtimeSinceStartup;
            } else{
                if (sound.startFading(fade)) shiftTime = Time.realtimeSinceStartup + delay;
            }
        }

        public void checkStartNew(){
            if (Time.realtimeSinceStartup > shiftTime){
                sound = Instantiate(next).GetComponent<Sound>();
                shiftTime = float.PositiveInfinity;
            }
        }
    }
}