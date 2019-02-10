using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSideToSidePlatform : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player) Player.transform.parent = transform;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player) Player.transform.parent = null;
        //transform.Translate(transform.right * Mathf.Cos(Time.time) * Time.deltaTime * length);
    }
}
