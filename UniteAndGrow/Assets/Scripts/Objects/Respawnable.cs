using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour{

    [HideInInspector] public bool instantiated;
    private GameObject instance;

    private void Start(){
        if (instantiated) return;
        instantiated = true;
        Global.gameControl.respawnObjs.Add(this);
        gameObject.SetActive(false);
        spawn();
    }

    public void spawn(){
        Destroy(instance);
        instance = Instantiate(gameObject, transform.parent);
        instance.SetActive(true);
    }
}