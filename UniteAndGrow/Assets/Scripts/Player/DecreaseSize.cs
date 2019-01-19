using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseSize : MonoBehaviour
{
    public float decreaseFactor_X = 0.1f;
    public float decreaseFactor_Y = 0.1f;
    public float decreaseFactor_Z = 0.1f;
    public string decreaseObjectsTag;
    public bool decreaseSizeOnMovement;

    private Rigidbody objectRigidbody;

    private void Start()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == decreaseObjectsTag)
        {
            if (decreaseSizeOnMovement)
            {
                // Only decrease size if moving on an object
                if (objectRigidbody.velocity.magnitude != 0)
                {
                    transform.localScale = transform.localScale - (new Vector3(decreaseFactor_X / 100.0f, decreaseFactor_Y / 100.0f, decreaseFactor_Z / 100.0f));
                }
            }
            else
            {
                // Always decrease size when touching an object
                transform.localScale = transform.localScale - (new Vector3(decreaseFactor_X / 100.0f, decreaseFactor_Y / 100.0f, decreaseFactor_Z / 100.0f));

            }

        }
    }
}
