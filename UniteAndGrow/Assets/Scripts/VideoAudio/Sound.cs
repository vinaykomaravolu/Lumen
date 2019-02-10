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
    private AudioSource audio;

    private void Start(){
        audio = GetComponent<AudioSource>();
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

    public void startFading(float length){
        if (fading) return;
        fadingStart = Time.realtimeSinceStartup;
        fadingLength = length;
        fading = true;
    }

    private void fade(){
        float volume = 1 - (Time.realtimeSinceStartup - fadingStart) / fadingLength;
        audio.volume = volume < 0 ? 0 : volume;
        instantiateNew = false;
    }
}

public enum SoundType {Ambient, Music, Effect}