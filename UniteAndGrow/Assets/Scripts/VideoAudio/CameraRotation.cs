using UnityEngine;

public class CameraRotation : MonoBehaviour {

    public GameObject player;
    public float clampAngle;
    public float inputSensitivity;
    public bool invertCamera;
    public float initVertical;
    private Vector3 rotation;

    private void Start(){
        rotation = transform.eulerAngles;
        rotation.x = initVertical;
    }

    void Update() {
        float deltaHorizontal = Input.GetAxis("Mouse X") + Input.GetAxis("Controller X");
        float deltaVertical = Input.GetAxis("Mouse Y") + Input.GetAxis("Controller Y");
        if (invertCamera) deltaVertical *= -1;
        
        rotation.y += deltaHorizontal * inputSensitivity * Time.deltaTime;
        rotation.x += deltaVertical * inputSensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
    }

    private void LateUpdate(){
        transform.position = player.transform.position;
    }
}
