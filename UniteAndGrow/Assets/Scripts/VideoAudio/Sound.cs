using UnityEngine;

public class Sound : MonoBehaviour{
    
    public float loopTime; // instantiate self to loop the music
    public bool instantiateNew; // true if want to instantiate new
    public float killDelay;
    private float instantiateTime;

    private void Start(){
        instantiateTime = Time.timeSinceLevelLoad + loopTime;
    }

    private void Update(){
        if (instantiateNew && Time.timeSinceLevelLoad > instantiateTime){
            instantiateNew = false;
            Instantiate(gameObject);
        }

        if (Time.timeSinceLevelLoad > instantiateTime + killDelay){
            Destroy(gameObject);
        }
    }
}