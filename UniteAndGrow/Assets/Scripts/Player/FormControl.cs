using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

// in charge of size and liquid or solid
public class FormControl : MonoBehaviour{

    public float volume;
    public float minVolume => sizeToVolume(minSize);
    public float maxVolume => sizeToVolume(maxSize);
    public float size => volumeToSize(volume);
    public float minSize;
    public float maxSize;

    private float volumeToSize(float volume){
        return Mathf.Pow(volume, 1f/3f);
    }

    private float sizeToVolume(float size){
        return size * size * size;
    }

    private void changeVolume(float change){
        volume += change;
        if (volume > maxVolume) volume = maxVolume;
        transform.localScale = new Vector3(size, size, size);
    }

    private void OnTriggerEnter(Collider other){
        checkSizeChange(other.gameObject);
    }

    private void OnCollisionStay(Collision collision){
        checkSizeChange(collision.gameObject);
    }

    private void checkSizeChange(GameObject other){
        if (!other.CompareTag(Global.sizeChangerTag)) return;
        SizeChanger sizeChanger = other.GetComponent<SizeChanger>();
        changeVolume(sizeChanger.contact());
        sizeChanger.postContact();
    }
}
