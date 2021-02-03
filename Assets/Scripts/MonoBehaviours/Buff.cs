using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    private BuffData buffData;
    private float countdown;

    private void Update() {
        countdown -= Time.deltaTime;
        if(countdown <= 0f) {
            EndBuff();
        }
    }
    public void SetBuff(BuffData data) {
        buffData = data;
        countdown = buffData.lifetime;
    }
    private void EndBuff() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.DetachChildren();
        gameObject.SetActive(false);
    }
}
