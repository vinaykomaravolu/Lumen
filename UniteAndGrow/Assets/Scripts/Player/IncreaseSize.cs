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
    public bool increaseSizeOnMovement;
    public bool invert;

    private Rigidbody objectRigidbody;
    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == increaseObjectsTag)
        {
            if (!invert && transform.localScale.x <= maximumSize && transform.localScale.y <= maximumSize && transform.localScale.z <= maximumSize)
            {
                if (increaseSizeOnMovement)
                {
                    // Only increase size if moving on an object
                    if (objectRigidbody.velocity.magnitude != 0)
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
            else if(invert && transform.localScale.x >= minimumSize && transform.localScale.y >= minimumSize && transform.localScale.z >= minimumSize)
            {
                if (increaseSizeOnMovement)
                {
                    // Only increase size if moving on an object
                    if (objectRigidbody.velocity.magnitude != 0)
                    {
                        transform.localScale = transform.localScale - (new Vector3(increaseFactor_X / 100.0f, increaseFactor_Y / 100.0f, increaseFactor_Z / 100.0f));
                    }
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
