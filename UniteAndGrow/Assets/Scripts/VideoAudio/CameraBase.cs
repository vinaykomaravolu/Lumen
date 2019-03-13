using UnityEngine;

public class CameraBase : MonoBehaviour {

    public GameObject player;
    public float clampAngle;
    public float inputSensitivity;
    public bool invertCamera;
    public float initVertical;
    public CameraDistance innerCamera;
    public Camera camera;
    public bool dashing;
    public float dashFieldOfView;
    public float deltaFieldOfView;
    
    private Vector3 rotation;
    private float initFieldOfView;

    private void Start(){
        rotation = transform.eulerAngles;
        rotation.x = initVertical;
        innerCamera.form = player.GetComponent<FormControl>();
        initFieldOfView = camera.fieldOfView;
    }

    void Update() {
        float deltaHorizontal = Input.GetAxis(Global.camHorizontalMouse)
                                + Input.GetAxis(Global.camHorizontalStick);
        float deltaVertical = Input.GetAxis(Global.camVerticalMouse)
                              + Input.GetAxis(Global.camVerticalStick);
        if (invertCamera) deltaVertical *= -1;
        
        rotation.y += deltaHorizontal * inputSensitivity * Time.deltaTime;
        rotation.x += deltaVertical * inputSensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);

        camera.fieldOfView = Mathf.MoveTowards(
            camera.fieldOfView,
            dashing ? dashFieldOfView : initFieldOfView,
            deltaFieldOfView * Time.deltaTime);
    }

    private void LateUpdate(){
        transform.position = player.transform.position;
    }
}
