using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSize : MonoBehaviour
{
    public float increaseFactor_X = 0.1f;
    public float increaseFactor_Y = 0.1f;
    public float increaseFactor_Z = 0.1f;
    public float minimumSize;
    public float maximumSize;
    public string increaseObjectsTag;
    public string decreaseObjectsTag;
    public bool increaseSizeOnMovement;

    private Rigidbody objectRigidbody;
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(increaseObjectsTag != null && collision.gameObject.tag == increaseObjectsTag)
        {
            if (transform.localScale.x <= maximumSize && transform.localScale.y <= maximumSize && transform.localScale.z <= maximumSize)
            {
                if (increaseSizeOnMovement)
                {
                    transform.localScale = transform.localScale + (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f)) * objectRigidbody.velocity.magnitude;

                }
                else
                {
                    // Always increase size when touching an object
                    transform.localScale = transform.localScale + (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f));
                }
            }
        else if(decreaseObjectsTag != null && collision.gameObject.tag == decreaseObjectsTag)
            {
                if (transform.localScale.x >= minimumSize && transform.localScale.y >= minimumSize && transform.localScale.z >= minimumSize)
                {
                    if (increaseSizeOnMovement)
                    {
                        transform.localScale = transform.localScale - (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f)) * objectRigidbody.velocity.magnitude;

                    }
                    else
                    {
                        // Always increase size when touching an object
                        transform.localScale = transform.localScale - (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f));
                    }
                }
            }     
        }
    }

    
}
