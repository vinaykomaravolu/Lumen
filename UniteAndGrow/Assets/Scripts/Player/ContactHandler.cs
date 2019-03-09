using UnityEngine;

public class ContactHandler : MonoBehaviour{

    public float landSoundThreshold;
    private const float wallSlope = 0.7f; // if contactNorm.y is less than this value, it's a wall

    public ContactSurface contactSurface{ get; private set; }
    public Vector3 contactNorm{ get; private set; } // contactNorm.y = -2 if in mid air
    public Vector3 contactVelocity{ get; private set; }
    public GameObject shrinkEffectPrefab;
    public GameObject growEffectPrefab;
    private GameObject shrinkEffect;
    private GameObject growEffect;

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
        if (other.CompareTag(Global.sizeChangerTag)){
            form.setSizeChange(other.gameObject);
            contactSurface = ContactSurface.SizeChanger;
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(Global.endPointTag)) Global.gameControl.win();
        if (other.CompareTag(Global.killZoneTag)) Global.gameControl.lose();
        if (other.CompareTag(Global.collectibleTag)) Global.gameControl.collect();
    }

    private void OnCollisionExit(Collision collision){
        contactNorm = new Vector3(0, -2, 0);
        contactSurface = ContactSurface.Other;
        
        if (collision.gameObject.CompareTag(Global.sizeChangerTag)){
            (collision.gameObject.GetComponent<SizeChanger>().grow ? 
                growEffect : shrinkEffect).GetComponent<ParticleEmissionControl>().kill();
        }
    }

    private void OnCollisionEnter(Collision collision){
        getContactInfo(collision);
        contactVelocity = collision.relativeVelocity;
        if (collision.impulse.magnitude > landSoundThreshold) Instantiate(Global.soundControl.landing);
        
        if (collision.gameObject.CompareTag(Global.sizeChangerTag)){
            if (collision.gameObject.GetComponent<SizeChanger>().grow){
                if (growEffect != null) Destroy(growEffect.gameObject);
                growEffect = Instantiate(growEffectPrefab,
                    transform.position,
                    Quaternion.LookRotation(collision.GetContact(0).normal),
                    transform);
            } else{
                if (shrinkEffect != null) Destroy(shrinkEffect.gameObject);
                shrinkEffect = Instantiate(shrinkEffectPrefab,
                    transform.position,
                    Quaternion.LookRotation(collision.GetContact(0).normal),
                    transform);
            }
        }
    }

    private void OnCollisionStay(Collision collision){
        getContactInfo(collision);
        if (collision.gameObject.CompareTag(Global.sizeChangerTag)){
            form.setSizeChange(collision.gameObject);
            if (collision.gameObject.GetComponent<SizeChanger>().grow){
                growEffect.transform.rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
            } else{
                shrinkEffect.transform.rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
            }
        }
    }

    private void getContactInfo(Collision collision){
        //looking for the flattest contact surface
        for (int i = 0; i < collision.contactCount; i++){
            Vector3 norm = collision.GetContact(i).normal;
            if (norm.y > contactNorm.y) contactNorm = norm;
            switch (collision.gameObject.tag){
                case Global.groundTag:
                    if (contactSurface == ContactSurface.Other)
                        contactSurface = ContactSurface.Ground; // Ground has lower priority
                    break;
                case Global.sizeChangerTag:
                    contactSurface = ContactSurface.SizeChanger; // Top priority
                    break;
                case Global.superJumpTag:
                    contactSurface = ContactSurface.SuperJump; // Top priority
                    break;
                case Global.wallTag:
                    if (contactSurface == ContactSurface.Ground)
                        contactSurface = ContactSurface.Other; // Other has lower priority
                    break;
                default:
                    if (contactSurface == ContactSurface.Other)
                        contactSurface = ContactSurface.Ground; // Ground has lower priority
                    break;
            }
        }
    }
}

// surface contacting with the player
public enum ContactMode{Air, Ground, Wall}

public enum ContactSurface{Ground, SizeChanger, SuperJump, Other}