using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class UIControl : MonoBehaviour{
    
    public Text debugInfo;
    public Image sizeIndicator;
    
    [Header("Collectible")]
    public GameObject[] collectiblesIcon;
    public GameObject[] collectiblesIconBack;
    public float rotateSpeed;
    public float rotationTime;
    private bool[] rotateColl;
    
    [Header("Pause")]
    public GameObject pauseMenu;
    
    [Header("Win")]
    public GameObject winMenu;
    public TMP_Text scoreBoard;

    [Header("Lose")] 
    public FadingText loseBackground;

    private void Start(){
        rotateColl = new bool[collectiblesIcon.Length];
    }

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
            if (Input.GetKeyDown("5"))
            {
                List<Score> scores = LeaderBoard.get();
                for (int i = 0; i < scores.Count; i++)
                {
                    Score score = scores[i];
                    string time = score.getTimeString();
                    Debug.Log(score.ToString());
                    Debug.Log(time);
                    Debug.Log(name);
                }
            }
            if (Input.GetKeyDown("6"))
            {
                addToLeaderBoard();
            }
        }
        for (int i = 0; i < Global.gameControl.collected; i++){
            Transform collTrans = collectiblesIcon[i].transform;
            if (rotateColl[i]){
                collTrans.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            } else{
                collTrans.rotation = Quaternion.RotateTowards(
                    collTrans.rotation,
                    Quaternion.identity,
                    rotateSpeed * Time.deltaTime);
            }
        }
    }
    //leader: name:time:score:timestamp

    public IEnumerator showCollectible(int index){
        rotateColl[index] = true;
        yield return new WaitForSeconds(rotationTime);
        rotateColl[index] = false;
    }

    public void showPause(bool show){
        pauseMenu.SetActive(show);
    }

    public void showWin(){
        winMenu.SetActive(true);
        this.setScoreBoard();
    }

    public void showLose(){
        StartCoroutine(_showLose());
    }

    IEnumerator _showLose(){
        loseBackground.targetAlpha = 1;
        yield return new WaitForSecondsRealtime(1 / loseBackground.speed);
        respawn();
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
        StartCoroutine(loadNext());
    }

    IEnumerator loadNext(){
        var loading = SceneManager.LoadSceneAsync(Global.gameControl.nextScene);
        while (!loading.isDone){
            yield return null;
        }
    }

    private void setScoreBoard(){
        scoreBoard.SetText("Total Score: " + Global.gameControl.getScore() + "\nTotal Time: " + Time.timeSinceLevelLoad);
        float time = Time.timeSinceLevelLoad;
        int collectible = Global.gameControl.collected;
    }

    private void setLeaderBoard(){
        List<Score> scores = LeaderBoard.get();
        for (int i = 0; i < scores.Count; i++){
            Score score = scores[i];
            string time = score.getTimeString();
            string name = score.name;
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