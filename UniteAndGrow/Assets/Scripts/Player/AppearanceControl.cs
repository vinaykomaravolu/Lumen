using UnityEngine;

public class AppearanceControl: MonoBehaviour {
    
    // Start is called before the first frame update
    public GameObject waterShell;

    private MovementControl movementControl;
    private FormControl form;
    
    void Start(){
        movementControl = GetComponent<MovementControl>();
        form = GetComponent<FormControl>();
    }

    // Update is called once per frame
    void Update() {
        waterShell.transform.rotation = Quaternion.identity;
    }
}
