using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIControl : MonoBehaviour{
    
    public Text debugInfo;
    
    [Header("Pause")]
    public GameObject pauseMenu;
    
    [Header("Win")]
    public GameObject winMenu;
    public Text scoreBoard;
    
    [Header("Lose")]
    public GameObject loseMenu;
    public FadingText loseBackground;
    public float loseTextDelay;
    public FadingText loseText;
    public GrowingText loseTextGrow;

    void Update() {
        if (Debug.isDebugBuild) {
            if (Input.GetKeyDown("1")) {
                showPause(true);
            }
            if (Input.GetKeyDown("2")) {
                showWin();
            }
            if (Input.GetKeyDown("3")) {
                showLose();
            }
            if (Input.GetKeyDown("4")) {
                pauseMenu.SetActive(false);
                winMenu.SetActive(false);
                loseMenu.SetActive(false);
                loseBackground.reset();
                loseText.reset();
                loseTextGrow.reset();
            }
        }
    }

    public void showPause(bool show){
        pauseMenu.SetActive(show);
        if(show)
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Resume"));
            Button resumeBtn = GameObject.Find("Resume").GetComponent<Button>();
            resumeBtn.onClick.AddListener(() => {
                pauseMenu.SetActive(false);
                Global.gameControl.pause();
            });
            Button returnBtn = GameObject.Find("Return").GetComponent<Button>();
            returnBtn.onClick.AddListener(() => {
                pauseMenu.SetActive(false);
                this.exit();
            });
        }
    }

    public void showWin(){
        winMenu.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Try Again"));
        Button tryBtn = GameObject.Find("Try Again").GetComponent<Button>();
        tryBtn.onClick.AddListener(() => {
            winMenu.SetActive(false);
            this.restart();
        });
        Button returnBtn = GameObject.Find("Return").GetComponent<Button>();
        returnBtn.onClick.AddListener(() => {
            winMenu.SetActive(false);
            this.exit();
        });
    }

    public void showLose(){
        StartCoroutine(_showLose());
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Try Again"));
        Button tryBtn = GameObject.Find("Try Again").GetComponent<Button>();
        tryBtn.onClick.AddListener(() => {
            loseMenu.SetActive(false);
            this.restart();
        });
        Button returnBtn = GameObject.Find("Return").GetComponent<Button>();
        returnBtn.onClick.AddListener(() => {
            loseMenu.SetActive(false);
            this.exit();
        });
    }

    IEnumerator _showLose(){
        loseMenu.SetActive(true);
        loseBackground.targetAlpha = 1;
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + loseTextDelay)
        {
            yield return null;
        }
        loseTextGrow.targetSize = 120;
        loseText.targetAlpha = 1;
        yield return null;
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exit(){
        SceneManager.LoadScene(Global.mainMenuName);
    }

    public void next(){
        SceneManager.LoadScene(Global.gameControl.nextScene);
    }

    private void setScoreBoard(){
        scoreBoard.text = "Total Score: " + Global.gameControl.getScore(Time.timeSinceLevelLoad);
    }
}