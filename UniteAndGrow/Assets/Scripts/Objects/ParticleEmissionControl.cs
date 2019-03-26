using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleEmissionControl : MonoBehaviour{

    public ParticleSystem particle;
    public Quaternion targetRotation;
    public float rotationDelta;
    public float size;

    private void Update(){
//        particle.transform.localScale = Vector3.one * size;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationDelta * Time.deltaTime);
    }

    public void kill(){
        StartCoroutine(_kill());
    }

    private IEnumerator _kill(){
        particle.Stop();
        yield return new WaitForSeconds(particle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}