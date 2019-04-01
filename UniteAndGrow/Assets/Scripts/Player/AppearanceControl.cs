using System.Collections;
using UnityEngine;

public class AppearanceControl: MonoBehaviour {
    
    // Start is called before the first frame update
    public GameObject outer;
    public GameObject inner;
    public GameObject left;
    public GameObject right;
    public float jumpStretch;
    public float outerRestoreSpeed;
    public float lerpSpeed;
    public float rotationSpeed;
    public float rotateDuration;

    private Rigidbody body;
    private MovementControl movement;
    private bool doubleJumping;
    private Vector3 preForward;
    
    void Start(){
        body = GetComponent<Rigidbody>();
        movement = GetComponent<MovementControl>();
    }

    // Update is called once per frame
    void Update(){
        if (doubleJumping){
            doubleJumpRotate();
        } else{
            rotateInner();
        }
        restoreOuterShape();
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
        preForward = forward;
        doubleJumping = false;
    }

    private void doubleJumpRotate(){
        inner.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
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
