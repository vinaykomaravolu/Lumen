using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour{
    
    public Text debugInfo;
    public GameObject pauseMenu;
    public GameObject endMenu;
    public GameObject dieMenu;
    
    public void showPause(bool show){
        if (show) ;
    }

    public void showWin(){
    }

    public void showLose(){
        restart();
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
}