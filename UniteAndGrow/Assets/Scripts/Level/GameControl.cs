using UnityEngine;
using UnityEngine.UI;

// a general game control that can be used for all levels
public class GameControl : MonoBehaviour{

    public GameObject player;
    private FormControl playerForm;
    private Rigidbody playerBody;
    private MovementControl playerMovement;
    public Text debugInfo;

    private void Start(){
        playerForm = player.GetComponent<FormControl>();
        playerBody = player.GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<MovementControl>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        if (playerForm.volume < playerForm.minVolume) gameOver();
        updateDebugInfo();
    }

    private void gameOver(){
        
    }

    private void updateDebugInfo(){
        Vector3 groundSpeed = playerBody.velocity;
        groundSpeed.y = 0;
        debugInfo.text = "Time: " + Time.timeSinceLevelLoad +
                         "\nSize: " + playerForm.size +
                         "\nVolume: " + playerForm.volume +
                         "\nDimension: " + player.transform.localScale +
                         "\nVelocity: " + playerBody.velocity +
                         "\nGround Speed: " + groundSpeed.magnitude +
                         "\nContact: " + playerMovement.contactMode +
                         "\nNorm: " + playerMovement.contactNorm;
    }
}