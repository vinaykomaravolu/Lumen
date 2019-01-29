using UnityEngine;

public class SizeChanger : MonoBehaviour{

    public bool grow;
    public bool killOnDry;
    public float volume;
    public float volumePerSecond;

    // call this when contact with the size changer, return change in volume of the player
    public float contact(){
        float deltaVolume = volumePerSecond * Time.deltaTime;
        if (deltaVolume > volume) deltaVolume = volume;
        volume -= deltaVolume;
        return grow ? deltaVolume : -deltaVolume;
    }

    public void postContact(){
        if (killOnDry && volume <= 0) Destroy(gameObject);
    }
}