using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

    public float duration;
    private float counter;
    private ParticleSystem part;

    private void Awake() {
        part = GetComponent<ParticleSystem>();
    }
    private void OnEnable() {
        counter = duration;
        part.Play();
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
        part.Stop();
        gameObject.SetActive(false);
    }
}
