using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
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
