using System.Collections;
using UnityEngine;

public class Sound : MonoBehaviour{
    
    public float loopTime; // instantiate self to loop the music
    public float killDelay; // after loop time
    public SoundType type;
    
    private bool fading;
    private float fadingLength;
    private float fadingStart;
    private bool instantiateNew; // true if want to instantiate new
    private float initialVolume;
    [HideInInspector] public AudioSource audio;

    private void Start(){
        audio = GetComponent<AudioSource>();
        initialVolume = audio.volume;
        instantiateNew = type != SoundType.Effect;
        StartCoroutine(life());
    }

    private void Update(){
        if (fading) fade();
    }

    IEnumerator life(){
        yield return new WaitForSecondsRealtime(loopTime);
        if (instantiateNew) Global.soundControl.updateCurrent(Instantiate(gameObject));
        yield return new WaitForSecondsRealtime(killDelay);
        Destroy(gameObject);
    }

    public bool startFading(float length){
        if (fading) return false;
        fadingStart = Time.realtimeSinceStartup;
        fadingLength = length;
        fading = true;
        instantiateNew = false;
        return true;
    }

    private void fade(){
        float volume = initialVolume * (1 - (Time.realtimeSinceStartup - fadingStart) / fadingLength);
        audio.volume = volume;
        if (volume <= 0) Destroy(gameObject);
    }
}

public enum SoundType {Ambient, Music, Effect}