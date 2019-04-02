using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AppearanceControl: MonoBehaviour {
    
    [Header("General")]
    public float idleDuration;
    public float jumpDuration;
    public float landDuration;
    
    [Header("Outer")]
    public GameObject outer;
    public float jumpStretch;
    public float landStretch;
    public float outerRestoreSpeed;
    
    [Header("Inner")]
    public GameObject inner;
    public float lerpSpeed;
    public float rotateSpeed;
    public float rotateDuration;
    public Vector2 idlePosRange;
    public float jumpPos;
    public float landPos;
    public float moveSpeedFast;
    public float moveSpeedSlow;
    
    [Header("Shell")] 
    public GameObject left;
    public GameObject right;
    public Vector2 idleAngleRange;
    public float jumpAngle;
    public float landAngle;
    public float deltaAngleFast;
    public float deltaAngleSlow;

    private Rigidbody body;
    private MovementControl movement;
    private bool doubleJumping;
    private float jumpEnd = float.NegativeInfinity;
    private float landEnd = float.NegativeInfinity;
    
    void Start(){
        body = GetComponent<Rigidbody>();
        movement = GetComponent<MovementControl>();
    }

    // Update is called once per frame
    void Update(){

        if (movement.jumping){
            jumpEnd = Time.timeSinceLevelLoad + jumpDuration;
        }

        if (doubleJumping){
            doubleJumpRotate();
        } else{
            rotateInner();
        }
        
        restoreOuterShape();

        float deltaAngle = deltaAngleFast;
        float moveSpeed = moveSpeedFast;
        float shellAngle;
        float innerPos;

        if (Time.timeSinceLevelLoad < landEnd){
            shellAngle = landAngle;
            innerPos = landPos;
        } else if (Time.timeSinceLevelLoad < jumpEnd){
            shellAngle = jumpAngle;
            innerPos = jumpPos;
        } else{
            shellAngle = idleAngle();
            innerPos = idlePos();
            deltaAngle = deltaAngleSlow;
            moveSpeed = moveSpeedSlow;
        }
        
        Quaternion shellRotation = Quaternion.Euler(0, 0, shellAngle);
        shellRotation = Quaternion.RotateTowards(
            right.transform.localRotation,
            shellRotation,
            deltaAngle * Time.deltaTime);
        left.transform.localRotation = Quaternion.Inverse(shellRotation);
        right.transform.localRotation = shellRotation;
        innerPos = Mathf.MoveTowards(
            inner.transform.localPosition.y,
            innerPos,
            moveSpeed * Time.deltaTime);
        inner.transform.localPosition = Vector3.up * innerPos;
    }

    private float idleProcess(){
        float relativeTime = (Time.timeSinceLevelLoad) % idleDuration;
        float cosValue = Mathf.Cos(2 * Mathf.PI * 
                                   relativeTime / idleDuration);
        return -(cosValue - 1) / 2;
    }

    private float idleAngle(){
        return Mathf.Lerp(idleAngleRange.x, idleAngleRange.y, idleProcess());
    }

    private float idlePos(){
        return Mathf.Lerp(idlePosRange.x, idlePosRange.y, idleProcess());
    }

    public void land(){
        landEnd = Time.timeSinceLevelLoad + landDuration;
        outer.transform.rotation = Quaternion.LookRotation(Vector3.up);
        outer.transform.localScale = Vector3.one - Vector3.forward * landStretch;
    }

    public void jump(Vector3 direction){
        outer.transform.rotation = Quaternion.LookRotation(direction);
        outer.transform.localScale = Vector3.one + Vector3.forward * jumpStretch;
    }

    public void doubleJump(){
        StartCoroutine(_doubleJump());
    }

    private IEnumerator _doubleJump(){
        doubleJumping = true;
        Vector3 forward = transform.forward;
        yield return new WaitForSeconds(rotateDuration);
        doubleJumping = false;
    }

    private void doubleJumpRotate(){
        inner.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void rotateInner(){
        Vector3 forward = movement.getControl();
        forward.y = 0;
        if (forward == Vector3.zero) return;
        inner.transform.rotation = Quaternion.Lerp(
            inner.transform.rotation,
            Quaternion.LookRotation(forward, Vector3.up),
            lerpSpeed * Time.deltaTime);
    }

    private void restoreOuterShape(){
        outer.transform.localScale = 
            Vector3.one + (outer.transform.localScale - Vector3.one) * outerRestoreSpeed;
    }
}
