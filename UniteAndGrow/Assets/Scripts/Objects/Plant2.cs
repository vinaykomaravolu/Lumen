using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant2 : MonoBehaviour
{
    Animator plant2;
    // Start is called before the first frame update
    void Start()
    {
        plant2 = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            plant2.SetTrigger("pass");
        }
    }
}
