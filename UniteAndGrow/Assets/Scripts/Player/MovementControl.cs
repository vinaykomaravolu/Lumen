using UnityEngine;

public class MovementControl : MonoBehaviour{

    public CameraBase playerCamera;
    public GameObject dashEffectPrefab;
    public AnimationCurve sensitivityCurve;
    [Header("Ground")]
    public float groundSpeed; // movement speed on ground
    public float dashSpeed;
    public float dragForce;
    public bool forceOnly; //use force even on ground
    [Header("Wall")]
    public float wallDrag;
    public float jumpBackFactor; // jump back scale during wall jump
    public float wallMoveThreshold;
    [Header("Air")]
    public float pushForce; // push force when in mid air
    public float dashForce; // dash force when in mid air
    [Header("Jump")]
    public float jumpSpeed;
    public float superJumpFactor;
    public float maxJumpSpeed;
    public float holdJumpDuration;

    private bool canWallJump => contact.contactSurface != ContactSurface.Ground;
    private bool canDoubleJump = true;
    private bool canDash => form.volume > form.minVolume;
    private Vector3 jumpNorm;
    private float jumpStartTime;
    private ParticleEmissionControl dashEffect;
    
    private Rigidbody body;
    private AppearanceControl appearance;
    private ContactHandler contact;
    private ContactMode contactMode => contact.contactMode;
    private Vector3 contactNorm => contact.contactNorm;
    private FormControl form;
    
    // Start is called before the first frame update
    void Start(){
        body = GetComponent<Rigidbody>();
        appearance = GetComponent<AppearanceControl>();
        contact = GetComponent<ContactHandler>();
        form = GetComponent<FormControl>();
    }

    // Update is called once per frame
    void Update(){
        if (Global.gameControl.paused) return;
        //reset double jump when on ground
        if (contactMode == ContactMode.Ground) canDoubleJump = true;
        jumpMove();
        wallSlide();
        // button down is only reliable in update
        if (canDash && Input.GetButton(Global.dashButton)){
            if (dashEffect is null){
                dashEffect = Instantiate(dashEffectPrefab,
                    transform.position,
                    Quaternion.identity,
                    transform).GetComponent<ParticleEmissionControl>();
            }
            dashEffect.transform.rotation = Quaternion.LookRotation(body.velocity);
        } else {
            dashEffect?.kill();
            dashEffect = null;
        }
    }

    // IMPORTANT! contact is not reliable in FixedUpdate
    void FixedUpdate(){
        body.AddForce(Vector3.down * Global.gravity * body.mass); // add gravity
        if (canDash && Input.GetButton(Global.dashButton)){
            horizontalMove(dashSpeed, dashForce);
            form.dash();
            playerCamera.dashing = true;
        } else{
            horizontalMove(groundSpeed, pushForce);
            playerCamera.dashing = false;
        }
    }

    private void wallSlide(){
        if (contactMode != ContactMode.Wall 
            || contact.contactSurface == ContactSurface.Ground 
            || body.velocity.y > -wallDrag) return;
        Vector3 velocity = body.velocity;
        velocity.y = -wallDrag;
        body.velocity = velocity;
    }

    private void horizontalMove(float speed, float force){
        Vector3 velocity = body.velocity;
        Vector3 controlStick = getControl();
        velocity.y = 0;
        float originalSpeed = velocity.magnitude;

        if (contactMode == ContactMode.Ground && !forceOnly){
            velocity.x = getSpeed(controlStick.x * speed, velocity.x);
            velocity.z = getSpeed(controlStick.z * speed, velocity.z);
            velocity = Vector3.ClampMagnitude(velocity, speed);
        } else{
            if (contactMode == ContactMode.Wall){
                Vector3 sideControl =
                    Vector3.Project(controlStick, Vector3.Cross(jumpNorm, Vector3.up));
                Vector3 forwardSpeed = Vector3.Project(velocity, jumpNorm);
                float magDiff = sideControl.magnitude - wallMoveThreshold;
                if (magDiff > 0){
                    velocity = sideControl.normalized * magDiff * speed + forwardSpeed;
                } else{
                    velocity = forwardSpeed;
                }
            } else{
                velocity += controlStick * force * Time.fixedDeltaTime;
            }
            velocity = Vector3.ClampMagnitude(velocity, Mathf.Max(speed, originalSpeed));
        }

        //set drag
        velocity.x += getDrag(controlStick.x, velocity.x);
        velocity.z += getDrag(controlStick.z, velocity.z);
        
        body.velocity = new Vector3(velocity.x, body.velocity.y, velocity.z);
    }

    private void jumpMove(){
        if (contactMode == ContactMode.Ground && contact.contactSurface == ContactSurface.SuperJump){
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
        float horizontal = Input.GetAxis(Global.moveHorizontalButton);
        float vertical = Input.GetAxis(Global.moveVerticalButton);
        return right * sensitivityCurve.Evaluate(Mathf.Abs(horizontal)) * Mathf.Sign(horizontal) 
               + forward * sensitivityCurve.Evaluate(Mathf.Abs(vertical)) * Mathf.Sign(vertical);
    }

    // return control if control is trying to move towards the opposite direction
    // or control's magnitude if greater
    private float getSpeed(float control, float current){
        if (control * current < 0 || Mathf.Abs(control) >= Mathf.Abs(current)) return control;
        return current;
    }

    private float getDrag(float control, float speed){
        if (Mathf.Abs(control) > 0) return 0;
        float deltaSpeed = dragForce * Time.fixedDeltaTime;
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
