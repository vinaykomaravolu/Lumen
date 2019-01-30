using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    void Update() {
        transform.Rotate(new Vector3(0, 30, 0) * 5 * Time.deltaTime);
    }
}