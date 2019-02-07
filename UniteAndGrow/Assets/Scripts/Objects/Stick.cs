using UnityEngine;

public class Stick : MonoBehaviour{

    public float force;
    public float forceRange;
    private GameObject target;
    private Rigidbody targetBody;
    private MovementControl targetMovement;

    private void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)){
            target = other.gameObject;
            targetBody = target.GetComponent<Rigidbody>();
            targetMovement = target.GetComponent<MovementControl>();
            targetMovement.forceOnly = true;
        }
    }

    private void FixedUpdate(){
        if (target == null) return;
        if ((target.transform.position - transform.position).magnitude > forceRange){
            target = null;
            targetMovement.forceOnly = false;
            return;
        }
        Vector3 direction = transform.position - target.transform.position;
        targetBody.AddForce(direction.normalized * force);
    }
}