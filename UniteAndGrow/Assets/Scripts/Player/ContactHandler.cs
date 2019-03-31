using UnityEngine;

public class ContactHandler : MonoBehaviour{

    public float landSoundThreshold;
    private const float wallSlope = 0.7f; // if contactNorm.y is less than this value, it's a wall

    public ContactSurface contactSurface{ get; private set; }
    public Vector3 contactNorm{ get; private set; } // contactNorm.y = -2 if in mid air
    public Vector3 contactVelocity{ get; private set; }
    public GameObject shrinkEffectPrefab;
    public GameObject growEffectPrefab;
    private GameObject collider;

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
        switch (other.tag){
            case Global.sizeChangerTag:
            case Global.instantKillTag:
                form.sizeChange(other.gameObject);
                break;
        }
    }

    private void OnTriggerEnter(Collider other){
        switch (other.tag){
            case Global.endPointTag:
                Global.gameControl.win();
                break;
            case Global.killZoneTag:
                Global.gameControl.lose();
                break;
        }
    }

    private void OnCollisionExit(Collision collision){
        contactNorm = new Vector3(0, -2, 0);
        contactSurface = ContactSurface.Other;

        GameObject other = collision.gameObject;
        switch (other.tag){
            case Global.sizeChangerTag:
            case Global.instantKillTag:
                other.GetComponent<SizeChanger>().effect.kill();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision){
        getContactInfo(collision);
        contactVelocity = collision.relativeVelocity;
        if (collision.impulse.magnitude > landSoundThreshold) Instantiate(Global.soundControl.landing);

        GameObject other = collision.gameObject;
        switch (other.tag){
            case Global.sizeChangerTag:
            case Global.instantKillTag:
                SizeChanger changer = other.GetComponent<SizeChanger>();
                GameObject effect = changer.grow ? growEffectPrefab : shrinkEffectPrefab;
                changer.effect = Instantiate(effect,
                    transform.position,
                    Quaternion.identity,
                    transform).GetComponent<ParticleEmissionControl>();
                break;
            default:
                form?.sizeChange(null);
                break;
        }
    }

    private void OnCollisionStay(Collision collision){
        getContactInfo(collision);
        GameObject other = collision.gameObject;
        switch (other.tag){
            case Global.sizeChangerTag:
            case Global.instantKillTag:
                form.sizeChange(collision.gameObject);
                ParticleEmissionControl effect = collision.gameObject.GetComponent<SizeChanger>().effect;
                effect.targetRotation = Quaternion.LookRotation(collision.GetContact(0).normal);
                break;
        }
    }

    private void getContactInfo(Collision collision){
        //looking for the flattest contact surface
        for (int i = 0; i < collision.contactCount; i++){
            Vector3 norm = collision.GetContact(i).normal;
            if (norm.y > contactNorm.y || collider == collision.gameObject){
                contactNorm = norm;
                collider = collision.gameObject;
            }
            switch (collision.gameObject.tag){
                case Global.groundTag:
                    if (contactSurface == ContactSurface.Other)
                        contactSurface = ContactSurface.Ground; // Ground has lower priority
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

public enum ContactSurface{Ground, SuperJump, Other}