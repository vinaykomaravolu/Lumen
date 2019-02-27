using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        StartCoroutine(_showLose());
    }

    IEnumerator _showLose(){
        loseMenu.SetActive(true);
        loseBackground.targetAlpha = 1;
        yield return new WaitForSeconds(loseTextDelay);
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