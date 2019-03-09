﻿using UnityEngine;

// in charge of size and liquid or solid
public class FormControl : MonoBehaviour{

    public float volume;
    public float minVolume => sizeToVolume(minSize);
    public float maxVolume => sizeToVolume(maxSize);
    public float size => volumeToSize(volume);
    public float minSize;
    public float maxSize;
    public float volumeSizeFactor;
    public float sizeChangeInterval;

    private static float _volumeSizeFactor;
    private Rigidbody body;
    private SizeChanger sizeChanger;
    private float sizeChangeTime = float.NegativeInfinity;
    private ContactHandler contact;

    private void Start(){
        body = GetComponent<Rigidbody>();
        body.mass = volume;
        contact = GetComponent<ContactHandler>();
        _volumeSizeFactor = volumeSizeFactor;
    }

    private void Update(){
        checkSizeChange();
        if (volume < minVolume) Global.gameControl.lose();
    }

    public static float volumeToSize(float volume){
        return Mathf.Pow(volume, 1 / _volumeSizeFactor);
    }

    public static float sizeToVolume(float size){
        return Mathf.Pow(size, _volumeSizeFactor);
    }

    private void changeVolume(float change){
        volume = Mathf.Clamp(volume + change, 0, maxVolume);
        body.mass = volume;
        transform.localScale = new Vector3(size, size, size);
    }

    private void checkSizeChange(){
        if (contact.contactMode == ContactMode.Ground && contact.contactSurface != ContactSurface.SizeChanger)
            sizeChangeTime = float.NegativeInfinity;
        if (Time.timeSinceLevelLoad > sizeChangeTime + sizeChangeInterval) return;
        changeVolume(sizeChanger.contact());
        if (sizeChanger.checkDeath()) sizeChangeTime = float.NegativeInfinity;
    }

    public void setSizeChange(GameObject other){
        sizeChanger = other.GetComponent<SizeChanger>();
        sizeChangeTime = Time.timeSinceLevelLoad;
    }
}
