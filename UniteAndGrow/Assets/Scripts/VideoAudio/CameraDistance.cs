﻿using UnityEngine;

public class CameraDistance : MonoBehaviour{

    [HideInInspector]public FormControl form;
    public float minDistance => form.size;
    public float maxDistanceBase;
    public float gap; // gap to obstacle
    public float maxDistance => maxDistanceBase + form.size * sizeFactor;
    public float sizeFactor;
    public float smooth;
    private float distance;
    
    void Update(){
        var transform_ = transform;
        var position = transform_.parent.position;
        Vector3 maxPose = position - transform_.forward * maxDistance;
        // ignore layer 9 2
        distance = Physics.Linecast(position, maxPose, out var hit, -1 - (1 << 9) - (1 << 2))
            ? Mathf.Clamp(hit.distance - gap, minDistance, maxDistance)
            : maxDistance;
        transform.localPosition = 
            Vector3.Lerp(transform.localPosition, Vector3.back * distance, Time.deltaTime * smooth);
    }
}
