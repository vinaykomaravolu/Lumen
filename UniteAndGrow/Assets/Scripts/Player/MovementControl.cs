using UnityEngine;

public class MovementControl : MonoBehaviour{

    public GameObject playerCamera;
    public float speed; // movement speed on ground
    public float jumpSpeed;
    public float superJumpFactor;
    public float maxJumpSpeed;
    public float pushForce; // push force when in mid air
    public float gravity; // gravity scale
    public float jumpBackFactor; // jump back scale during wall jump
    public float dragForce;
    public float wallDrag;
    public float holdJumpDuration;
    public bool forceOnly; //use force even on ground

    private bool canWallJump => contact.contactSurface != ContactSurface.Ground;
    private bool canDoubleJump = true;
    private Vector3 jumpNorm;
    private float jumpStartTime;
    
    private Rigidbody body;
    private AppearanceControl appearance;
    private ContactHandler contact;
    private ContactMode contactMode => contact.contactMode;
    private Vector3 contactNorm => contact.contactNorm;
    
    // Start is called before the first frame update
    void Start(){
        body = GetComponent<Rigidbody>();
        appearance = GetComponent<AppearanceControl>();
        contact = GetComponent<ContactHandler>();
    }

    // Update is called once per frame
    void Update(){
        //reset double jump when on ground
        if (contactMode == ContactMode.Ground) canDoubleJump = true;
        jumpMove();
        wallSlide();
    }

    // IMPORTANT! contact is not reliable in FixedUpdate
    void FixedUpdate(){
        body.AddForce(Vector3.down * gravity * body.mass); // add gravity
        horizontalMove();
    }

    private void wallSlide(){
        if (contactMode != ContactMode.Wall || body.velocity.y > -wallDrag) return;
        Vector3 velocity = body.velocity;
        velocity.y = -wallDrag;
        body.velocity = velocity;
    }

    private void horizontalMove(){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        velocity.y = 0;
        
        if (contactMode == ContactMode.Ground && !forceOnly){
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

    private void jumpMove(){
        if (contactMode == ContactMode.Ground && contact.contactSurface == ContactSurface.Mushroom){
            float speed = Mathf.Clamp(contact.contactVelocity.y * superJumpFactor, jumpSpeed, maxJumpSpeed);
            initialJump(speed);
        }
        if (Input.GetButtonDown(Global.jumpButton)) initialJump(jumpSpeed);
        if (Input.GetButton(Global.jumpButton)) holdJump();
        if (Input.GetButtonUp(Global.jumpButton)) jumpStartTime = float.NegativeInfinity; //disable hold jump
    }

    private void initialJump(float jumpSpeed){
        Vector3 velocity = body.velocity;
        if (jumpSpeed <= body.velocity.y) return;
        
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
                norm = norm.normalized * jumpBackFactor;
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
        if (jumpSpeed < velocity.y) return;
        if (jumpNorm == Vector3.up){
            velocity.y = jumpSpeed;
        } else{
            // magic, don't touch
            Vector3 sideVelocity = Vector3.Project(velocity, Vector3.Cross(jumpNorm, Vector3.up));
            velocity = sideVelocity + jumpNorm * jumpSpeed;
        }

        body.velocity = velocity;
    }

    // get the vector3 of the movement input, relative to the world
    public Vector3 getControl(){
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

    //debug
    private void OnDrawGizmos(){
        if (contact == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, contactNorm * 2);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.Cross(jumpNorm, Vector3.up));
    }
}
