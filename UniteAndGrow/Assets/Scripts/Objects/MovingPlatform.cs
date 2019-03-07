using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour{

    public Vector2 randomStart;
    public Vector4[] locations;
    [Header("Quick Setting")]
    public Vector4 shift;
    private float delta;
    private float totalTime;

    private void Start(){
        delta = Random.Range(randomStart.x, randomStart.y);
        if (locations.Length < 2){
            Vector4 startPos = transform.position;
            Vector4 midPos = startPos + shift;
            Vector4 endPos = startPos;
            endPos.w = midPos.w * 2;
            locations = new[]{startPos, midPos, endPos};
        }
        totalTime = locations[locations.Length - 1].w;
    }

    private void Update(){
        float relativeTime = (Time.timeSinceLevelLoad - delta + totalTime) % totalTime;
        int start = 0;
        while (locations[start + 1].w < relativeTime) start++;
        float cosValue = Mathf.Cos(Mathf.PI * 
            (relativeTime - locations[start].w) / (locations[start + 1].w - locations[start].w));
        cosValue = -(cosValue - 1) / 2;
        transform.position = (Vector4.Lerp(locations[start], locations[start + 1], cosValue));
    }

    private void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)){
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)){
            other.transform.parent = null;
        }
    }
}
