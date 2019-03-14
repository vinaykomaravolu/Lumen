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
    public Text loseTextContent;
    private string[] deathDialogueOptions = { "Better luck next time!", "Yikes!", "You're not very good at this are you?", "Wowzers!", "Why are you still trying?",
        "Are you playing with your hands or your feet?" };

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
            }
        }
    }

    public void showPause(bool show){
        pauseMenu.SetActive(show);
    }

    public void showWin(){
        winMenu.SetActive(true);
    }

    public void showLose(){
        int rnd = Random.Range(0, deathDialogueOptions.Length);
        string dialogue = deathDialogueOptions[rnd];
        loseTextContent.text = dialogue;
        StartCoroutine(_showLose());
    }

    IEnumerator _showLose(){
        loseMenu.SetActive(true);
        loseBackground.targetAlpha = 1;
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + loseTextDelay)
        {
            yield return null;
        }
        loseText.targetAlpha = 1;
        yield return null;
    }

    public void pause(){
        Global.gameControl.pause();
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void respawn(){
        Global.gameControl.respawn();
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