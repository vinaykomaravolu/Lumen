using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour{

    public GameObject playerCamera;
    public float speed;
    public float jumpSpeed;
    public float force;
    private Rigidbody body;

    public float maxJumpCount;
    private float jumpCount = 0;
    private Vector3 contactNorm;

    private float wallSlope = 0.7f;
    
    // Start is called before the first frame update
    void Start(){
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        
        //movement
        if (contactNorm.y < wallSlope){
            body.AddForce(controlStick * force);
        } else{
            print(contactNorm + " con " + controlStick);
            velocity.x = setSpeed(controlStick.x * speed, velocity.x);
            velocity.z = setSpeed(controlStick.z * speed, velocity.z);
        }
        
        //jump
        if (canJump()){
            velocity.y = 0;
            Vector3 norm = new Vector3(0, 1, 0);
            if (contactNorm.y < wallSlope){
                norm = contactNorm;
                norm.y = 0;
                norm = norm.normalized * 0.5f;
                norm.y = 1;
            }
            velocity += jumpSpeed * norm;
            jumpCount--;
        }

        body.velocity = velocity;
        body.AddForce(Vector3.down * 100); // add gravity
    }

    private Vector3 getControl(){
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        right.Normalize();
        return right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");
    }

    private float setSpeed(float control, float current){
        if (control * current < 0){
            // control is trying to move to the opposite direction
            return current + control;
        }

        if (Mathf.Abs(control) < Mathf.Abs(current)){
            return current;
        }

        return control;
    }

    private bool canJump(){
        return Input.GetButtonDown("Jump") && jumpCount > 0;
    }

    private void OnCollisionStay(Collision other){
        checkWallSlope(other);
    }

    private void checkWallSlope(Collision other){
        contactNorm = new Vector3(0, -1, 0);
        
        //looking for the flattest contact surface
        for (int i = 0; i < other.contactCount; i++){
            Vector3 norm = other.GetContact(i).normal;
            if (norm.y > contactNorm.y) contactNorm = norm;
        }
        
        //reset jump count when contact
        if (contactNorm.y >= 0) jumpCount = maxJumpCount;
    }
}
