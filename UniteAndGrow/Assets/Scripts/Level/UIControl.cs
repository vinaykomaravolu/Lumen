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

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            showPause(true);
//        }
//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            showWin();
//        }
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            showLose();
//        }
//        if (Input.GetKeyDown(KeyCode.Z))
//        {
//            pauseMenu.SetActive(false);
//            winMenu.SetActive(false);
//            loseMenu.SetActive(false);
//        }
//    }

    public void showPause(bool show){
        pauseMenu.SetActive(show);
    }

    public void showWin(){
        winMenu.SetActive(true);
    }

    public void showLose(){
        loseMenu.SetActive(true);
        StartCoroutine(_showLose());
    }

    IEnumerator _showLose(){
        print(1);
        loseBackground.targetAlpha = 1;
        yield return new WaitForSeconds(loseTextDelay);
        loseText.targetAlpha = 1;
        print(2);
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