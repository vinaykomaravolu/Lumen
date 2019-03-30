﻿using UnityEditor;
 using UnityEngine;
 using UnityEngine.UI;

// in charge of size and liquid or solid
public class FormControl : MonoBehaviour{

    public float volume;
    public float minVolume => sizeToVolume(minSize);
    public float maxVolume => sizeToVolume(maxSize);
    public float size => volumeToSize(volume);
    public float minSize;
    public float maxSize;
    public float killSize;
    public float volumeSizeFactor;
    public float sizeChangeDistance;
    public float dashShrink;
    public Image sizeIndicator;

    private static float _volumeSizeFactor;
    private bool sizeChanged;
    private bool contactedChanger;
    private Rigidbody body;

    private void Start(){
        body = GetComponent<Rigidbody>();
        body.mass = volume;
        _volumeSizeFactor = volumeSizeFactor;
        changeVolume(0, 0);
    }

    private void Update(){
        checkSizeChange();
        sizeChanged = false;
    }

    private void FixedUpdate(){
        if (size < killSize) Global.gameControl.lose(); // don't check when time scale is 0
    }

    public static float volumeToSize(float volume){
        return Mathf.Pow(volume, 1 / _volumeSizeFactor);
    }

    public static float sizeToVolume(float size){
        return Mathf.Pow(size, _volumeSizeFactor);
    }

    private void changeVolume(float min, float change){
        volume = Mathf.Clamp(volume + change, min, maxVolume);
        body.mass = volume;
        transform.localScale = new Vector3(size, size, size);
        sizeIndicator.fillAmount = Mathf.Clamp01((volume - minVolume) / (maxVolume - minVolume));
    }

    private void checkSizeChange(){
        if (contactedChanger && !sizeChanged &&
            Physics.Raycast(transform.position, Vector3.down, out var hit, sizeChangeDistance)
            && hit.collider.CompareTag(Global.sizeChangerTag)){
            sizeChange(hit.collider.gameObject);
        }
    }

    public void dash(){
        changeVolume(minVolume, dashShrink * Time.deltaTime);
    }

    public void sizeChange(GameObject other){
        if (other is null){
            contactedChanger = false;
            return;
        }
        SizeChanger sizeChanger = other.GetComponent<SizeChanger>();
        changeVolume(other.CompareTag(Global.instantKillTag) ? 0 : minVolume, sizeChanger.contact());
        sizeChanger.checkDeath();
        contactedChanger = true;
        sizeChanged = true;
    }
}
