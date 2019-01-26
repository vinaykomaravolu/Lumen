using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    public float cameraMoveSpeed = 120.0f;
    public GameObject cameraFollowObject;

    private Vector3 followPosition;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject cameraObject;
    public GameObject playerObject;
    public float cameraDistanceToPlayerX;
    public float cameraDistanceToPlayerY;
    public float cameraDistanceToPlayerZ;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    public bool invertCamera = false;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;


    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationY = rotation.y;
        rotationX = rotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");

        if (invertCamera)
        {
            mouseY = -1 * Input.GetAxis("Mouse Y");
        }
        else
        {
            mouseY = Input.GetAxis("Mouse Y");
        }
        
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotationY += finalInputX * inputSensitivity * Time.deltaTime;
        rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        transform.rotation = localRotation;
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        Transform target = cameraFollowObject.transform;
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
