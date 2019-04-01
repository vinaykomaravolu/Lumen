using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a general game control that can be used for all levels
//Singleton
public class GameControl : MonoBehaviour{

    [Header("General")]
    public bool debug;
    public GameObject startPoint;
    public string nextScene;
    public float gravity;
    public bool canDash;
    public int totalCollectibles;
    
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
    private MovementControl playerMovement;
    private CameraBase cameraBase;
    
    [HideInInspector] public int checkPointIndex;
    [HideInInspector] public List<Respawnable> respawnObjs = new List<Respawnable>();
    public bool paused{ get; private set; }
    private bool end;

    public int collected{ get; private set; }

    private void Awake(){
        Global.gameControl = this;
        Global.soundControl = Instantiate(soundControlPrefab);
        Global.gravity = gravity;
    }

    private void Start(){
        spawn();
    }

    private void spawn(){
        uiControl = Instantiate(canvas).GetComponent<UIControl>();
        debugInfo = uiControl.debugInfo;
        for (int i = 0; i < totalCollectibles; i++){
            uiControl.collectiblesIconBack[i].SetActive(true);
        }

        for (int i = 0; i < collected; i++){
            uiControl.collectiblesIcon[i].transform.rotation = Quaternion.identity;
        }

        player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        cameraBase = Instantiate(cameraPrefab, startPoint.transform.position, startPoint.transform.rotation)
            .GetComponent<CameraBase>();
        
        playerForm = player.GetComponent<FormControl>();
        playerForm.sizeIndicator = uiControl.sizeIndicator;
        playerBody = player.GetComponent<Rigidbody>();
        playerContact = player.GetComponent<ContactHandler>();
        playerMovement = player.GetComponent<MovementControl>();
        playerMovement.playerCamera = cameraBase;
        
        cameraBase.player = player;

        SizeIndicator.form = playerForm;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
        end = false;
        paused = false;
    }

    private void Update(){
        if (debug){
            updateDebugInfo();
            if (Input.GetKeyDown("c") && Input.GetKeyDown("l")) PlayerPrefs.DeleteAll();
        }
        if (!end && (Input.GetButtonDown(Global.pauseButton) || 
            Input.GetButtonDown(Global.altPauseButton))) pause();
    }

    public void respawn(){
        Destroy(player);
        Destroy(cameraBase.gameObject);
        Destroy(uiControl.gameObject);
        spawn();
        foreach (var obj in respawnObjs) {
            obj.spawn();
        }
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
    }

    public void pause(){
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        uiControl.showPause(paused);
    }

    public void collect(){
        StartCoroutine(uiControl.showCollectible(collected));
        collected++;
    }

    public int getScore(){
        return (int)(collected * collectScoreBase + getTimeScore(Time.timeSinceLevelLoad));
    }

    private float getTimeScore(float time){
        return timeScoreFactor / (time + timeScoreFactor / timeScoreBase);
    }

    private void updateDebugInfo(){
        Vector3 groundSpeed = playerBody.velocity;
        groundSpeed.y = 0;
        debugInfo.text = "Time: " + Time.timeSinceLevelLoad +
                         "\nControl" + playerMovement.getControl() + 
                         "\nScore: " + getScore() +
                         "\nSize: " + playerForm.size +
                         "\nVolume: " + playerForm.volume +
                         "\nDimension: " + player.transform.localScale +
                         "\nVelocity: " + playerBody.velocity +
                         "\nGround Speed: " + groundSpeed.magnitude +
                         "\nContact: " + playerContact.contactMode +
                         "\nNorm: " + playerContact.contactNorm +
                         "\nJumping: " + playerMovement.jumping + 
                         "\nSurface: " + playerContact.contactSurface;
    }
}