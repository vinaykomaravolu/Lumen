using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour{

    public Light light;
    public float deltaIntensity;
    public float targetIntensity;
    public ParticleSystem collectEffect;
    public ParticleSystem ambientEffect;
    public float killDelay;
    public GameObject model;
    private bool killed;

    void Update() {
        light.intensity = Mathf.MoveTowards(
            light.intensity,
            targetIntensity,
            deltaIntensity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        if (killed) return;
        if (other.CompareTag(Global.playerTag)){
            StartCoroutine(kill());
            killed = true;
        }
    }

    private IEnumerator kill(){
        ambientEffect?.Stop();
        Destroy(model);          
        Global.gameControl.collect();
        targetIntensity = 0;
        collectEffect.Play();
        yield return new WaitForSeconds(killDelay);
        Destroy(gameObject);
    }
}