using UnityEngine;

// start menu control, and game initialization
public class StartMenu : MonoBehaviour{
    
    private void Start(){
        Global.loadCollectionStatus();
    }

    private void quit(){
        Application.Quit();
        Global.saveCollectionStatus();
    }
}