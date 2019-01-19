using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSize : MonoBehaviour
{
    public float increaseFactor_X = 0.1f;
    public float increaseFactor_Y = 0.1f;
    public float increaseFactor_Z = 0.1f;
    public string increaseObjectsTag;
    public bool increaseSizeOnMovement;

    private Rigidbody objectRigidbody;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == increaseObjectsTag)
        {
            if(increaseSizeOnMovement)
            {
                // Only increase size if moving on an object
                if(objectRigidbody.velocity.magnitude != 0)
                {
                    transform.localScale = transform.localScale + (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f));
                }
            }
            else
            {
                // Always increase size when touching an object
                transform.localScale = transform.localScale + (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f));

            }

        }
    }

    
}
