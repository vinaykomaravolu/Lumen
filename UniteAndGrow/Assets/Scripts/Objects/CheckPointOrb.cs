using UnityEngine;

public class CheckPointOrb : MonoBehaviour{

    private MeshRenderer orb;
    public Material initMaterial;
    public Material targetMaterial;
    public float deltaTime;
    public float delay;
    private float activationTime = float.PositiveInfinity;
    
    private void Start(){
        orb = GetComponent<MeshRenderer>();
        orb.material = initMaterial;
    }

    private void Update(){
        float progress = Time.timeSinceLevelLoad - activationTime;
        if (progress > 0){
            progress = Mathf.Clamp01(progress / deltaTime);
            orb.material.Lerp(initMaterial, targetMaterial, progress);
        }
    }
    
    public void activate(){
        activationTime = Time.timeSinceLevelLoad + delay;
    }

}