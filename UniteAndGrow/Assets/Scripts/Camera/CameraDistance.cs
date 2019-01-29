using UnityEngine;

public class CameraDistance : MonoBehaviour{

    public FormControl form;
    public float minDistance => form.size;
    public float maxDistanceBase;
    public float maxDistance => maxDistanceBase + form.size * sizeFactor;
    public float sizeFactor;
    public float smooth;
    public float distance;
    
    void Update(){
        Vector3 maxPose = transform.parent.position - transform.forward * maxDistance;
        RaycastHit hit;
        distance = Physics.Linecast(transform.parent.position, maxPose, out hit)
            ? Mathf.Clamp(hit.distance, minDistance, maxDistance)
            : maxDistance;
        transform.localPosition = 
            Vector3.Lerp(transform.localPosition, Vector3.back * distance, Time.deltaTime * smooth);
    }
}
