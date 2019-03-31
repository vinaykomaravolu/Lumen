using UnityEngine;

public class SizeChanger : MonoBehaviour{

    public bool grow;
    public bool killOnDry;
    public float volume;
    public float volumePerSecond;
    private Vector3 initScale;
    private float initVolume;
    [HideInInspector] public ParticleEmissionControl effect;

    private void Start(){
        initScale = transform.localScale;
        initVolume = volume;
    }

    // call this when contact with the size changer, return change in volume of the player
    public float contact(){
        float deltaVolume = volumePerSecond * Time.deltaTime;
        if (deltaVolume > volume) deltaVolume = volume;
        volume -= deltaVolume;
        if (killOnDry) transform.localScale = initScale * (volume / initVolume);
        return grow ? deltaVolume : -deltaVolume;
    }

    public void checkDeath(){
        if (killOnDry && volume <= 0){
            Destroy(gameObject);
            effect?.kill();
        }
    }
}