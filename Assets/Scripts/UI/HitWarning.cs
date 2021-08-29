using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitWarning : MonoBehaviour {

    public Image bloodSplat;
    public byte alpha = 0;

    public void Hit() {
        alpha = 30;
        StopAllCoroutines();
        StartCoroutine(HitEffect());
    }
    private IEnumerator HitEffect() {
        while(alpha > 0) {
            bloodSplat.color = new Color32(200, 0, 0, alpha);
            alpha--;
            yield return null;
        }        
    }
}
