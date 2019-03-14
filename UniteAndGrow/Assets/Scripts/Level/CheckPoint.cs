using Objects;
using UnityEngine;

class CheckPoint : MonoBehaviour{

    public int checkPointIndex;
    public GameObject spawnPoint;
    public CheckPointMarker[] markers;
        
    private void OnTriggerEnter(Collider other){
        if (Global.gameControl.checkPointIndex < checkPointIndex){
            Global.gameControl.startPoint = spawnPoint;
            Global.gameControl.checkPointIndex = checkPointIndex;
        }
    }
    
}