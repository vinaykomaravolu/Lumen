using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour{

    public GameObject playerCamera;
    public float speed; // movement speed on ground
    public float jumpSpeed;
    public float pushForce; // push force when in mid air
    public float gravity; // gravity scale
    public bool isWater; // player form
    public bool jumpBack; // jump back scale during wall jump
    
    private Rigidbody body;

    private bool canDoubleJump = true;
    private Vector3 contactNorm; // contactNorm.y = -2 if in mid air

    private float wallSlope = 0.8f; // if contactNorm.y is less than this value, it's a wall
    private ContactMode contactMode {
        get{
            if (contactNorm.y < -1) return ContactMode.Air;
            if (contactNorm.y < wallSlope) return ContactMode.Wall;
            return ContactMode.Ground;
        }
    }
    
    // Start is called before the first frame update
    void Start(){
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        movement();
        body.AddForce(Vector3.down * gravity); // add gravity
        
    }

    // IMPORTANT! contactNorm is not reliable in FixedUpdate
    void FixedUpdate(){
        
    }

    private void LateUpdate(){
        contactNorm = new Vector3(0, -2, 0);
    }

    private void movement(){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        
        //horizontal movement
        if (contactNorm.y < wallSlope){
            body.AddForce(controlStick * pushForce);
        } else{
            velocity.x = setSpeed(controlStick.x * speed, velocity.x);
            velocity.z = setSpeed(controlStick.z * speed, velocity.z);
        }
        
        //jump
        if (Input.GetButtonDown("Jump")){
            switch (contactMode){
                case ContactMode.Air:
                    if (canDoubleJump){
                        canDoubleJump = false;
                        velocity.y = jumpSpeed;
                    }
                    break;
                case ContactMode.Ground:
                    velocity.y = jumpSpeed;
                    break;
                case ContactMode.Wall:
                    if (!isWater){
                        if (!canDoubleJump) break;
                        canDoubleJump = false;
                    }
                    velocity.y = 0;
                    Vector3 norm = contactNorm;
                    norm.y = 0;
                    norm = norm.normalized * 0.5f;
                    norm.y = 1;
                    velocity += jumpSpeed * norm;
                    break;
            }
        }

        body.velocity = velocity;
    }

    // get the vector3 of the movement input, relative to the camera
    private Vector3 getControl(){
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        right.Normalize();
        return right * Input.GetAxis("Horizontal") + forward * Input.GetAxis("Vertical");
    }

    // return the correct velocity based on input control and current velocity
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

        
    private void OnCollisionStay(Collision other){
        setWallSlope(other);
        //reset double jump when on ground
        if (contactMode == ContactMode.Ground) canDoubleJump = true;
    }

    private void setWallSlope(Collision other){
        //looking for the flattest contact surface
        for (int i = 0; i < other.contactCount; i++){
            Vector3 norm = other.GetContact(i).normal;
            if (norm.y > contactNorm.y) contactNorm = norm;
        }
    }
}

// surface contacting with the player
public enum ContactMode{Air, Ground, Wall}
