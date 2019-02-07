using UnityEngine;

public class AppearanceControl: MonoBehaviour {
    
    // Start is called before the first frame update
    public GameObject outer;
    public GameObject inner;
    public float jumpStretch;
    public float outerRestoreSpeed;
    public float rotationSpeed;

    private Rigidbody body;
    private MovementControl movement;
    
    void Start(){
        body = GetComponent<Rigidbody>();
        movement = GetComponent<MovementControl>();
    }

    // Update is called once per frame
    void Update(){
        rotateInner();
        restoreOuterShape();
    }

    public void jump(Vector3 direction){
        outer.transform.rotation = Quaternion.LookRotation(direction);
        outer.transform.localScale = Vector3.one + Vector3.forward * jumpStretch;
    }

    private void rotateInner(){
        Vector3 forward = movement.getControl();
        if (forward == Vector3.zero) forward = body.velocity;
        forward.y = 0;
        if (forward == Vector3.zero) return;
        inner.transform.rotation = Quaternion.Lerp(
            inner.transform.rotation,
            Quaternion.LookRotation(forward, Vector3.up),
            rotationSpeed);
    }

    private void restoreOuterShape(){
        outer.transform.localScale = 
            Vector3.one + (outer.transform.localScale - Vector3.one) * outerRestoreSpeed;
    }
}
