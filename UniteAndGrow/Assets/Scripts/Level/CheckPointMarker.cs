using UnityEngine;

public class CheckPointMarker : MonoBehaviour{

    public Light pointLight;
    public float deltaTime;
    public float randomStartTime;
    public CheckPointOrb[] orbs;
    private float targetLightIntensity;
    private float activationTime = float.PositiveInfinity;
    
    private void Start(){
        targetLightIntensity = pointLight.intensity;
        pointLight.intensity = 0;
    }

    private void Update(){
        float progress = Time.timeSinceLevelLoad - activationTime;
        if (progress > 0){
            progress = Mathf.Clamp01(progress / deltaTime);
            pointLight.intensity = Mathf.Lerp(
                pointLight.intensity, 
                targetLightIntensity, 
                progress);
            
        }
    }

    public void activate(){
        activationTime = Time.timeSinceLevelLoad + Random.Range(0, randomStartTime);
        foreach (var orb in orbs){
            orb.activate();
        }
    }

}