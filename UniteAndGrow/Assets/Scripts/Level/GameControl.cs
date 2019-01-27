using UnityEngine;
    
// a general game control that can be used for all levels
public class GameControl : MonoBehaviour{

    public GameObject player;
    private FormControl formControl;

    private void Start(){
        formControl = player.GetComponent<FormControl>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        if (formControl.volume < formControl.minVolume) gameOver();
    }

    private void gameOver(){
        
    }
}