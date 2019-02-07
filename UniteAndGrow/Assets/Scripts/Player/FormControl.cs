using UnityEngine;

// in charge of size and liquid or solid
public class FormControl : MonoBehaviour{

    public float volume;
    public float minVolume => sizeToVolume(minSize);
    public float maxVolume => sizeToVolume(maxSize);
    public float size => volumeToSize(volume);
    public float minSize;
    public float maxSize;

    private float volumeToSize(float volume){
        return Mathf.Sqrt(volume);
    }

    private float sizeToVolume(float size){
        return size * size;
    }

    private void Update(){
        if (volume < minVolume) Global.gameControl.lose();
    }

    private void changeVolume(float change){
        volume = Mathf.Clamp(volume + change, 0, maxVolume);
        
        transform.localScale = new Vector3(size, size, size);
    }

    public void checkSizeChange(GameObject other){
        SizeChanger sizeChanger = other.GetComponent<SizeChanger>();
        changeVolume(sizeChanger.contact());
        sizeChanger.postContact();
    }
}
