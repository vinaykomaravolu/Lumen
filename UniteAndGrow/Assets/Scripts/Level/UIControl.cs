using System.Collections;
using System.Collections.Generic;
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
    public FadingText loseBackground;
    public FadingText quipText;
    public Text quipTextContent;

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
                loseBackground.reset();
            }
        }
    }
    //leader: name:time:score:timestamp

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
        loseBackground.targetAlpha = 1;
        yield return new WaitForSecondsRealtime(1 / loseBackground.speed);
        respawn();
    }

    IEnumerator _showQuip()
    {
        quipText.targetAlpha = 1;
        yield return new WaitForSecondsRealtime(1 / quipText.speed);
        quipText.targetAlpha = 0;
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
//        SceneManager.LoadScene(Global.loadingSceneName);
//        SceneManager.LoadScene(Global.gameControl.nextScene);
        StartCoroutine(loadNext());
    }

    IEnumerator loadNext(){
        var loading = SceneManager.LoadSceneAsync(Global.gameControl.nextScene);
        while (!loading.isDone){
            yield return null;
        }
    }

    private void setScoreBoard(){
        scoreBoard.text = "Total Score: " + Global.gameControl.getScore();
        float time = Time.timeSinceLevelLoad;
        int collectible = Global.gameControl.collected;
    }

    private void setLeaderBoard(){
        List<Score> scores = LeaderBoard.get();
        for (int i = 0; i < scores.Count; i++){
            Score score = scores[i];
            string time = score.getTimeString();
        }
    }

    public void addToLeaderBoard(){
        LeaderBoard.add(new Score{
            name = "name",
            time = Time.timeSinceLevelLoad,
            score = Global.gameControl.getScore()
        });
    }
}