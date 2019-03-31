using UnityEngine;

public class Respawnable : MonoBehaviour{

    [HideInInspector] public bool instantiated;

    private void Start(){
        if (instantiated) return;
        instantiated = true;
        Global.gameControl.respawnObjs.Add(this);
        gameObject.SetActive(false);
        spawn();
    }

    public void spawn(){
        Instantiate(gameObject, transform.parent).SetActive(true);
    }
}