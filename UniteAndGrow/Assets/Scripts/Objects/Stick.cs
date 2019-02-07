using UnityEngine;

public class Stick : MonoBehaviour{

    public float force;
    public float forceRange;
    private GameObject target;

    private void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag(Global.playerTag)) target = other.gameObject;
    }

    private void FixedUpdate(){
        if (target == null) return;
        if ((target.transform.position - transform.position).magnitude > forceRange){
            target = null;
            return;
        }
        Vector3 direction = transform.position - target.transform.position;
        target.GetComponent<Rigidbody>().AddForce(direction.normalized * force);
    }
}