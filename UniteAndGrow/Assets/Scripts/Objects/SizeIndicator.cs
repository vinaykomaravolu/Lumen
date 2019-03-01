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
        // volume is the same as mass, one of the limit can be 0
        if (form.size > sizeLimit && form.volume > massLimit){
            //over limit
            colorModule.color = overLimitGradient;
        } else{
            colorModule.color = underLimitGradient;
        }
    }
}