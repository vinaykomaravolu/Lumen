using UnityEngine;
using UnityEngine.UI;

// a general game control that can be used for all levels
//Singleton
public class GameControl : MonoBehaviour{

    [Header("General")]
    public bool debug;
    public GameObject startPoint;
    public int checkPointIndex;
    public string nextScene;
    public float gravity;
    
    [Header("Prefabs")]
    public GameObject canvas;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public SoundControl soundControlPrefab;

    [Header("Score")]
    public float timeScoreBase;
    public float timeScoreFactor;
    public float collectScoreBase;

    private GameObject player;
    private Text debugInfo;
    private UIControl uiControl;
    
    private FormControl playerForm;
    private Rigidbody playerBody;
    private ContactHandler playerContact;
    private GameObject camera;
    public bool paused;
    public bool end;

    private int collected = 0;

    private void Start(){
        Global.gameControl = this;
        Global.soundControl = Instantiate(soundControlPrefab);
        Global.gravity = gravity;
        
        spawn();
    }

    private void spawn(){
        uiControl = Instantiate(canvas).GetComponent<UIControl>();
        debugInfo = uiControl.debugInfo;
        
        player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        camera = Instantiate(cameraPrefab, startPoint.transform.position, startPoint.transform.rotation);
        
        playerForm = player.GetComponent<FormControl>();
        playerBody = player.GetComponent<Rigidbody>();
        player.GetComponent<MovementControl>().playerCamera = camera;;
        playerContact = player.GetComponent<ContactHandler>();
        
        camera.GetComponentInChildren<CameraDistance>().form = playerForm;
        camera.GetComponent<CameraRotation>().player = player;

        SizeIndicator.form = playerForm;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
        end = false;
        paused = false;
    }

    private void Update(){
        if (debug) updateDebugInfo();
        if (!end && (Input.GetButtonDown(Global.pauseButton) || 
            Input.GetButtonDown(Global.altPauseButton))) pause();
    }

    public void respawn(){
        Destroy(player);
        Destroy(camera);
        Destroy(uiControl.gameObject);
        spawn();
    }

    public void win(){
        end = true;
        uiControl.showWin();
        Global.soundControl.win();
        Time.timeScale = 0;
    }

    public void lose(){
        end = true;
        uiControl.showLose();
        Global.soundControl.lose();
        Time.timeScale = 0;
    }

    public void pause(){
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        uiControl.showPause(paused);
    }

    public void collect(){
        collected++;
    }

    public int getScore(float time){
        return (int)(collected * collectScoreBase + getTimeScore(time));
    }

    private float getTimeScore(float time){
        return timeScoreFactor / (time + timeScoreFactor / timeScoreBase);
    }

    private void updateDebugInfo(){
        Vector3 groundSpeed = playerBody.velocity;
        groundSpeed.y = 0;
        debugInfo.text = "Time: " + Time.timeSinceLevelLoad +
                         "\nScore: " + getScore(Time.timeSinceLevelLoad) +
                         "\nSize: " + playerForm.size +
                         "\nVolume: " + playerForm.volume +
                         "\nDimension: " + player.transform.localScale +
                         "\nVelocity: " + playerBody.velocity +
                         "\nGround Speed: " + groundSpeed.magnitude +
                         "\nContact: " + playerContact.contactMode +
                         "\nNorm: " + playerContact.contactNorm +
                         "\nSurface: " + playerContact.contactSurface;
    }
}