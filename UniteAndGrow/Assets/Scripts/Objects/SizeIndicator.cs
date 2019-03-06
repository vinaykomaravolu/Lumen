using UnityEngine;

public class SizeIndicator : MonoBehaviour{

    public float massLimit;
    public float sizeLimit;
    public Gradient overLimitGradient;
    public Gradient underLimitGradient;
    public Gradient alphaGradient;
    
    public ParticleSystem effect;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    public static FormControl form{ set; private get; }
    private GradientColorKey[] colorKeys = new GradientColorKey[1];

    private void Start(){
        colorModule = effect.colorOverLifetime;
    }

    private void Update(){
        float limit = Mathf.Max(FormControl.sizeToVolume(sizeLimit), massLimit);
        // volume is the same as mass, one of the limit can be 0
        if (form.volume > limit){
            colorKeys[0].color = overLimitGradient.Evaluate((form.volume - limit) / (form.maxSize - limit));
            alphaGradient.SetKeys(colorKeys, alphaGradient.alphaKeys);
            colorModule.color = alphaGradient;
        } else{
            colorKeys[0].color = underLimitGradient.Evaluate(form.volume / limit);
            alphaGradient.SetKeys(colorKeys, alphaGradient.alphaKeys);
            colorModule.color = alphaGradient;
        }
    }
}