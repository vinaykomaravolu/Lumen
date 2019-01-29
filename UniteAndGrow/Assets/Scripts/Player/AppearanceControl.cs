using UnityEngine;

public class AppearanceControl: MonoBehaviour {
    
    // Start is called before the first frame update
    public GameObject outer;
    public GameObject inner;
    public float jumpStretch;
    public float outerRestoreSpeed;

    private MovementControl movementControl;
    private FormControl form;
    private Rigidbody body;
    
    void Start(){
        movementControl = GetComponent<MovementControl>();
        form = GetComponent<FormControl>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        inner.transform.rotation = Quaternion.identity;
        restoreOuterShape();
    }

    public void jump(Vector3 direction){
        outer.transform.localScale = Vector3.one + direction * jumpStretch;
    }

    private void restoreOuterShape(){
        outer.transform.localScale = 
            Vector3.one + (outer.transform.localScale - Vector3.one) * outerRestoreSpeed;
    }
}
