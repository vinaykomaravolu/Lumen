using UnityEngine;

public class ContactHandler : MonoBehaviour{
    
    private const float wallSlope = 0.7f; // if contactNorm.y is less than this value, it's a wall
    
    public Vector3 contactNorm{ get; private set; } // contactNorm.y = -2 if in mid air
    public ContactMode contactMode {
        get{
            if (contactNorm.y < -1) return ContactMode.Air;
            if (contactNorm.y < wallSlope) return ContactMode.Wall;
            return ContactMode.Ground;
        }
    }

    private FormControl form;
    private Rigidbody body;

    private void Start(){
        form = GetComponent<FormControl>();
        body = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(Global.endPointTag)) Global.gameControl.win();
        if (other.gameObject.CompareTag(Global.killZoneTag)) Global.gameControl.lose();
        form.checkSizeChange(other.gameObject);
    }

    private void OnCollisionExit(Collision other){
        contactNorm = new Vector3(0, -2, 0);
    }

    private void OnCollisionStay(Collision other){
        getContactNorm(other);
        form.checkSizeChange(other.gameObject);
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