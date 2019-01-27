using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour{

    public GameObject playerCamera;
    public float speed; // movement speed on ground
    public float jumpSpeed;
    public float pushForce; // push force when in mid air
    public float gravity; // gravity scale
    public float jumpBack; // jump back scale during wall jump
    public float dragForce;

    private bool canWallJump = true;
    private Rigidbody body;

    private bool canDoubleJump = true;
    public Vector3 contactNorm; // contactNorm.y = -2 if in mid air

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
        jumpMove();
    }

    // IMPORTANT! contactNorm is not reliable in FixedUpdate
    void FixedUpdate(){
        body.AddForce(Vector3.down * gravity * body.mass); // add gravity   
        horizontalMove();
    }

    private void horizontalMove(){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        
        if (contactMode == ContactMode.Ground){
            velocity.x = getSpeed(controlStick.x * speed, velocity.x);
            velocity.z = getSpeed(controlStick.z * speed, velocity.z);
        } else{
            body.AddForce(controlStick * pushForce);
        }

        //max horizontal speed
        velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
        velocity.z = Mathf.Clamp(velocity.z, -speed, speed);

        //set drag
        velocity.x += getDrag(controlStick.x, velocity.x);
        velocity.z += getDrag(controlStick.z, velocity.z);

        body.velocity = velocity;
    }

    private void OnCollisionExit(Collision other){
        contactNorm = new Vector3(0, -2, 0);
    }

    private void jumpMove(){
        if (!Input.GetButtonDown("Jump")) return;
        
        Vector3 velocity = body.velocity;
        
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
                if (!canWallJump){
                    if (!canDoubleJump) break;
                    canDoubleJump = false;
                }
                velocity.y = 0;
                Vector3 norm = contactNorm;
                norm.y = 0;
                norm = norm.normalized * jumpBack;
                norm.y = 1;
                velocity += jumpSpeed * norm;
                break;
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

    // return control if control is trying to move towards the opposite direction
    // or control's magnitude if greater
    private float getSpeed(float control, float current){
        if (control * current < 0 || Mathf.Abs(control) >= Mathf.Abs(current)) return control;
        return current;
    }

    private float getDrag(float control, float speed){
        if (Mathf.Abs(control) > 0.1f) return 0;
        float deltaSpeed = dragForce * Time.deltaTime;
        if (Mathf.Abs(speed) < deltaSpeed) return -speed;
        return speed < 0 ? deltaSpeed : -deltaSpeed;
    }

    private void OnCollisionStay(Collision other){
        getContactNorm(other);
        //reset double jump when on ground
        if (contactMode == ContactMode.Ground) canDoubleJump = true;
    }

    private void getContactNorm(Collision other){
        //looking for the flattest contact surface
        for (int i = 0; i < other.contactCount; i++){
            Vector3 norm = other.GetContact(i).normal;
            if (norm.y > contactNorm.y) contactNorm = norm;
        }
    }
}

// surface contacting with the player
public enum ContactMode{Air, Ground, Wall}
