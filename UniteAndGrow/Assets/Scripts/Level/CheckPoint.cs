using UnityEngine;

class CheckPoint : MonoBehaviour{

    public GameObject spawnPoint;
    public CheckPointMarker[] markers;
    private bool activated;
        
    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag(Global.playerTag)) return;
        Global.gameControl.startPoint = spawnPoint;
        if (activated) return;
        activated = true;
        foreach (var marker in markers){
            marker?.activate();
        }
    }
    
}