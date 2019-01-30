using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// a general game control that can be used for all levels
public class GameControl : MonoBehaviour{

    [Header("Level Dependent")]
    public bool debug;
    public GameObject startPoint;
    public EndPoint endPoint;
    
    [Header("Prefabs")]
    public GameObject canvas;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    private GameObject player;
    private GameObject camera;
    private Text debugInfo;
    private UIControl uiControl;
    
    private FormControl playerForm;
    private Rigidbody playerBody;
    private MovementControl playerMovement;
    private bool paused;

    private void Start(){
        endPoint.gameControl = this;
        
        uiControl = Instantiate(canvas).GetComponent<UIControl>();
        debugInfo = uiControl.debugInfo;

        player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        camera = Instantiate(cameraPrefab, startPoint.transform.position, startPoint.transform.rotation);
        
        playerForm = player.GetComponent<FormControl>();
        playerBody = player.GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<MovementControl>();
        playerMovement.playerCamera = camera;
        camera.GetComponentInChildren<CameraDistance>().form = playerForm;
        camera.GetComponent<CameraRotation>().player = player;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        if (playerForm.volume < playerForm.minVolume) lose();
        if (debug) updateDebugInfo();
    }

    public void pause(){
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        uiControl.show(paused);
    }

    public void win(){
        
    }

    private void lose(){
        restart();
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exit(){
        SceneManager.LoadScene(Global.mainMenuName);
    }

    private void updateDebugInfo(){
        Vector3 groundSpeed = playerBody.velocity;
        groundSpeed.y = 0;
        debugInfo.text = "Time: " + Time.timeSinceLevelLoad +
                         "\nSize: " + playerForm.size +
                         "\nVolume: " + playerForm.volume +
                         "\nDimension: " + player.transform.localScale +
                         "\nVelocity: " + playerBody.velocity +
                         "\nGround Speed: " + groundSpeed.magnitude +
                         "\nContact: " + playerMovement.contactMode +
                         "\nNorm: " + playerMovement.contactNorm;
    }
}