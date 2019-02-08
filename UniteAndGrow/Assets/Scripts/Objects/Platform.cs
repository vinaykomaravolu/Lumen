using UnityEngine;

public class Platform : MonoBehaviour{

    public float massLimit;
    public float overMultiplier; // multiplier to make platform fall near limit
    public float underMultiplier; // multiplier to make the platform float near limit
    public float restoreMultiplier; // multiplier to restore back to original height

    private float baseHeight;
    private Rigidbody body;
    private float multiplier = 1;

    private void Start(){
        baseHeight = transform.position.y;
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate(){
        Vector3 position = transform.position;
        if (position.y < baseHeight){
            body.AddForce(Vector3.up * massLimit * Global.gravity * multiplier);
        }

        if (position.y > baseHeight){
            position.y = baseHeight;
            transform.position = position;
        }
    }

    private void OnCollisionStay(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)){
            multiplier = other.gameObject.GetComponent<Rigidbody>().mass > massLimit
                ? overMultiplier
                : underMultiplier;
        }
    }

    private void OnCollisionExit(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)){
            multiplier = restoreMultiplier;
        }
    }
}