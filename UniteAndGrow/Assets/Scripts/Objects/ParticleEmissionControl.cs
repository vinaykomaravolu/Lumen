using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleEmissionControl : MonoBehaviour{

    public ParticleSystem particle;
    public Quaternion targetRotation;
    public float rotationDelta;
    private bool killed;

    private void Update(){
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationDelta * Time.deltaTime);
    }

    public void kill(){
        if (killed) return;
        StartCoroutine(_kill());
        killed = true;
    }

    private IEnumerator _kill(){
        particle.Stop();
        yield return new WaitForSeconds(particle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}