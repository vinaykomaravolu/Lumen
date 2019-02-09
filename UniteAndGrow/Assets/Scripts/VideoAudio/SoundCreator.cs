using UnityEngine;

public class SoundCreator : MonoBehaviour{

    public GameObject soundPrefab;

    private void OnTriggerEnter(Collider other){
        Global.soundControl.changeCurrentSlow(soundPrefab);
    }
}