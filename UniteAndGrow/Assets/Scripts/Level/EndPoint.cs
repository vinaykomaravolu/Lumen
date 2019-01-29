using DefaultNamespace;
using UnityEngine;

public class EndPoint : MonoBehaviour{
    
    public GameControl gameControl;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(Global.playerTag)) gameControl.win();
    }
}
