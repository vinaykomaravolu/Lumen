using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MovementControl : MonoBehaviour{
    
    private const float wallSlope = 0.7f; // if contactNorm.y is less than this value, it's a wall

    public GameObject playerCamera;
    public float speed; // movement speed on ground
    public float jumpSpeed;
    public float pushForce; // push force when in mid air
    public float gravity; // gravity scale
    public float jumpBack; // jump back scale during wall jump
    public float dragForce;
    public float wallDrag;
    public float holdJumpDuration;

    private bool canWallJump = true;
    private bool canDoubleJump = true;
    private Vector3 jumpNorm;
    private float jumpStartTime;
    
    public Vector3 contactNorm{ get; private set; } // contactNorm.y = -2 if in mid air
    public ContactMode contactMode {
        get{
            if (contactNorm.y < -1) return ContactMode.Air;
            if (contactNorm.y < wallSlope) return ContactMode.Wall;
            return ContactMode.Ground;
        }
    }
    
    private Rigidbody body;
    private AppearanceControl appearance;
    
    // Start is called before the first frame update
    void Start(){
        body = GetComponent<Rigidbody>();
        appearance = GetComponent<AppearanceControl>();
    }

    // Update is called once per frame
    void Update(){
        jumpMove();
    }

    // IMPORTANT! contactNorm is not reliable in FixedUpdate
    void FixedUpdate(){
        float factor = contactMode == ContactMode.Wall && body.velocity.y <= 0 ? wallDrag : 1;
        body.AddForce(Vector3.down * gravity * body.mass * factor); // add gravity
        horizontalMove();
    }

    private void horizontalMove(){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        velocity.y = 0;
        
        if (contactMode == ContactMode.Ground){
            velocity.x = getSpeed(controlStick.x * speed, velocity.x);
            velocity.z = getSpeed(controlStick.z * speed, velocity.z);
        } else{
            velocity += controlStick * pushForce * Time.deltaTime;
        }

        //max horizontal speed
        velocity = Vector3.ClampMagnitude(velocity, speed);

        //set drag
        velocity.x += getDrag(controlStick.x, velocity.x);
        velocity.z += getDrag(controlStick.z, velocity.z);
        
        body.velocity = new Vector3(velocity.x, body.velocity.y, velocity.z);
    }

    private void OnCollisionExit(Collision other){
        contactNorm = new Vector3(0, -2, 0);
    }

    private void jumpMove(){
        if (Input.GetButtonDown(Global.jumpButton)) initialJump();
        if (Input.GetButton(Global.jumpButton)) holdJump();
        if (Input.GetButtonUp(Global.jumpButton)) jumpStartTime = float.NegativeInfinity; //disable hold jump
    }

    private void initialJump(){
        Vector3 velocity = body.velocity;
        
        switch (contactMode){
            case ContactMode.Air:
                if (!canDoubleJump) return;
                canDoubleJump = false;
                velocity.y = jumpSpeed;
                appearance.jump(Vector3.up);
                jumpNorm = Vector3.up;
                break;
            case ContactMode.Ground:
                velocity.y = jumpSpeed;
                appearance.jump(Vector3.up);
                jumpNorm = Vector3.up;
                break;
            case ContactMode.Wall:
                if (!canWallJump){
                    if (!canDoubleJump) return;
                    canDoubleJump = false;
                }
                velocity.y = 0;
                Vector3 norm = contactNorm;
                norm.y = 0;
                norm = norm.normalized * jumpBack;
                norm.y += 1;
                velocity += jumpSpeed * norm;
                appearance.jump(velocity.normalized);
                jumpNorm = norm;
                break;
        }

        jumpStartTime = Time.timeSinceLevelLoad;
        body.velocity = velocity;
    }

    private void holdJump(){
        if (Time.timeSinceLevelLoad - jumpStartTime > holdJumpDuration) return;
        Vector3 velocity = body.velocity;
        if (jumpNorm == Vector3.up){
            velocity.y = jumpSpeed;
        } else{
            // magic, don't touch
            print(velocity);
            print(Vector3.Cross(jumpNorm, Vector3.up));
            Vector3 sideVelocity = Vector3.Project(velocity, Vector3.Cross(jumpNorm, Vector3.up));
            print(sideVelocity);
            velocity = sideVelocity + jumpNorm * jumpSpeed;
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
        return right * Input.GetAxis(Global.moveHorizontalButton)
               + forward * Input.GetAxis(Global.moveVerticalButton);
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

    //debug
    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, contactNorm * 2);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.Cross(jumpNorm, Vector3.up));
    }
}

// surface contacting with the player
public enum ContactMode{Air, Ground, Wall}
