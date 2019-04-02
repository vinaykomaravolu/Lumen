using System.Net.Mime;
using UnityEngine;

public class CameraBase : MonoBehaviour {

    public Vector2 clampAngle;
    public float inputSensitivity;
    public bool invertCamera;
    public float initVertical;
    public CameraDistance innerCamera;
    public Camera camera;
    public float dashFieldOfView;
    public float deltaFieldOfView;
    public GameObject rainEffect;
    [HideInInspector] public GameObject player;
    [HideInInspector] public bool dashing;
    
    private Vector3 rotation;
    private float initFieldOfView;

    private void Start(){
        rotation = transform.eulerAngles;
        rotation.x = initVertical;
        innerCamera.form = player.GetComponent<FormControl>();
        initFieldOfView = camera.fieldOfView;
        rainEffect.SetActive(Global.gameControl.rain);
    }

    void Update() {
        float deltaHorizontal = Input.GetAxis(Global.camHorizontalMouse)
                                + Input.GetAxis(Global.camHorizontalStick);
        float deltaVertical = Input.GetAxis(Global.camVerticalMouse)
                              + Input.GetAxis(Global.camVerticalStick);
        if (invertCamera) deltaVertical *= -1;
        
        rotation.y += deltaHorizontal * inputSensitivity * Time.deltaTime;
        rotation.x += deltaVertical * inputSensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, clampAngle.x, clampAngle.y);

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
