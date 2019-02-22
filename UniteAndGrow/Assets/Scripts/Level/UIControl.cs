using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour{
    
    public Text debugInfo;
    public GameObject pauseMenu;
    public GameObject endMenu;
    public GameObject dieMenu;
    public Text scoreBoard;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            showPause(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            showWin();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            showLose();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pauseMenu.SetActive(false);
            endMenu.SetActive(false);
            dieMenu.SetActive(false);
        }
    }

    public void showPause(bool show){
        pauseMenu.SetActive(show);
    }

    public void showWin(){
        endMenu.SetActive(true);
    }

    public void showLose(){
        dieMenu.SetActive(true);
        StartCoroutine(dieMenu.GetComponent<DeathUIFade>().FadeTextToFullAlpha(1f));
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