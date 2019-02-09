using UnityEngine;
using UnityEngine.UI;

// a general game control that can be used for all levels
//Singleton
public class GameControl : MonoBehaviour{

    [Header("General")]
    public bool debug;
    public GameObject startPoint;
    public float gravity;
    public SoundControl soundControl;
    
    [Header("Prefabs")]
    public GameObject canvas;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

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
    private bool paused;

    private int collected = 0;

    private void Start(){
        Global.gameControl = this;
        Global.soundControl = soundControl;
        Global.gravity = gravity;
        
        uiControl = Instantiate(canvas).GetComponent<UIControl>();
        debugInfo = uiControl.debugInfo;

        player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        GameObject camera = Instantiate(cameraPrefab, startPoint.transform.position, startPoint.transform.rotation);
        
        playerForm = player.GetComponent<FormControl>();
        playerBody = player.GetComponent<Rigidbody>();
        player.GetComponent<MovementControl>().playerCamera = camera;;
        playerContact = player.GetComponent<ContactHandler>();
        
        camera.GetComponentInChildren<CameraDistance>().form = playerForm;
        camera.GetComponent<CameraRotation>().player = player;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        if (debug) updateDebugInfo();
        if (Input.GetButtonDown(Global.pauseButton)) pause();
    }

    public void win(){
        uiControl.showWin();
    }

    public void lose(){
        uiControl.showLose();
    }

    public void collect(){
        collected++;
    }

    private int getScore(float time){
        return (int)(collected * collectScoreBase + getTimeScore(time));
    }

    private float getTimeScore(float time){
        return timeScoreFactor / (time + timeScoreFactor / timeScoreBase);
    }

    public void pause(){
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        uiControl.showPause(paused);
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