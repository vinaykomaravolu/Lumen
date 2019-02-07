using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour{
    
    public Text debugInfo;
    public GameObject pauseMenu;
    
    
    public void showPause(bool show){
        if (show) ;
    }

    public void showWin(){
        Time.timeScale = 0;
    }

    public void showLose(){
//        Time.timeScale = 0;
        restart();
    }

    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exit(){
        SceneManager.LoadScene(Global.mainMenuName);
    }
}