using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxWarning : MonoBehaviour {

    public void SetWarning(float duration, float scale) {
        transform.localScale = new Vector3(scale, scale, 1f);
        StartCoroutine(Countdown(duration));
    }
    private IEnumerator Countdown(float duration) {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        transform.SetParent(null);
    }
}
