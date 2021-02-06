using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

    public float duration;
    private float counter;

    private void OnEnable() {
        counter = duration;
    }

    private void Update() {
        if(counter > 0) {
            counter -= Time.deltaTime;
        }
        else {
            Deactivate();
        }
    }

    public void Deactivate() {
        transform.parent = null;
        gameObject.SetActive(false);
    }
}
