using System.Collections;
using UnityEngine;

public class ParticleEmissionControl : MonoBehaviour{

    public ParticleSystem particle;

    public void kill(){
        StartCoroutine(_kill());
    }

    private IEnumerator _kill(){
        particle.Stop();
        yield return new WaitForSeconds(particle.main.startLifetime.constant);
        Destroy(gameObject);
    }
}