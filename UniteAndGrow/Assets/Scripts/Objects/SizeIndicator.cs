using UnityEngine;

public class SizeIndicator : MonoBehaviour{

    public float massLimit;
    public float sizeLimit;
    public Gradient overLimitGradient;
    public Gradient underLimitGradient;
    
    public ParticleSystem effect;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    public static FormControl form{ set; private get; }

    private void Start(){
        colorModule = effect.colorOverLifetime;
    }

    private void Update(){
        float limit = Mathf.Max(FormControl.sizeToVolume(sizeLimit), massLimit);
        // volume is the same as mass, one of the limit can be 0
        if (form.volume > limit){
            colorModule.color = overLimitGradient.Evaluate(
                (form.volume - limit) / (form.maxSize - limit));
        } else{
            colorModule.color = underLimitGradient.Evaluate(
                form.volume / limit);
        }
    }
}