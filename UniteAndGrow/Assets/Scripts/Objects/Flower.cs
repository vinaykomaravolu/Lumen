using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    Animator flower;
    // Start is called before the first frame update
    void Start()
    {
        flower = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            flower.SetBool("inter", true);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            flower.SetBool("inter", false);
        }
    }
}