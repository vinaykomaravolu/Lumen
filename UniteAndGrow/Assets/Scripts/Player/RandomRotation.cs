using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour { 

    public float rotation_x = 10f;
    public float rotation_y = 30f;
    public float rotation_z = 20f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0), rotation_x * Time.deltaTime);
        transform.Rotate(new Vector3(0, 1, 0), rotation_y * Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, 1), rotation_z * Time.deltaTime);
    }
}
