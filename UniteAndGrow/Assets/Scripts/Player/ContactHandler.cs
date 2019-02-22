using UnityEngine;

public class ContactHandler : MonoBehaviour{

    public float landSoundThreshold;
    private const float wallSlope = 0.7f; // if contactNorm.y is less than this value, it's a wall

    public ContactSurface contactSurface{ get; private set; }
    public Vector3 contactNorm{ get; private set; } // contactNorm.y = -2 if in mid air
    public Vector3 contactVelocity{ get; private set; }

    public ContactMode contactMode {
        get{
            if (contactNorm.y < -1) return ContactMode.Air;
            if (contactNorm.y < wallSlope) return ContactMode.Wall;
            return ContactMode.Ground;
        }
    }

    private FormControl form;

    private void Start(){
        form = GetComponent<FormControl>();
    }

    private void OnTriggerStay(Collider other){
        if (other.CompareTag(Global.sizeChangerTag)) form.checkSizeChange(other.gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(Global.endPointTag)) Global.gameControl.win();
        if (other.CompareTag(Global.killZoneTag)) Global.gameControl.lose();
        if (other.CompareTag(Global.collectibleTag)) Global.gameControl.collect();
    }

    private void OnCollisionExit(Collision collision){
        contactNorm = new Vector3(0, -2, 0);
        contactSurface = ContactSurface.Other;
    }

    private void OnCollisionEnter(Collision collision){
        getContactInfo(collision);
        contactVelocity = collision.relativeVelocity;
        if (collision.impulse.magnitude > landSoundThreshold) Instantiate(Global.soundControl.landing);
        transform.parent = collision.transform.parent;
    }

    private void OnCollisionStay(Collision other){
        getContactInfo(other);
        if (other.gameObject.CompareTag(Global.sizeChangerTag)) form.checkSizeChange(other.gameObject);
    }

    private void getContactInfo(Collision collision){
        //looking for the flattest contact surface
        for (int i = 0; i < collision.contactCount; i++){
            Vector3 norm = collision.GetContact(i).normal;
            if (norm.y <= contactNorm.y) continue;
            contactNorm = norm;
            switch (collision.gameObject.tag){
                case Global.groundTag:
                    contactSurface = ContactSurface.Ground;
                    break;
                case Global.sizeChangerTag:
                    contactSurface = ContactSurface.SizeChanger;
                    break;
                case Global.mushroomTag:
                    contactSurface = ContactSurface.Mushroom;
                    break;
                default:
                    contactSurface = ContactSurface.Other;
                    break;
            }
        }
    }
}

// surface contacting with the player
public enum ContactMode{Air, Ground, Wall}

public enum ContactSurface{Ground, SizeChanger, Mushroom, Other}