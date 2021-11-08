using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour {

    public TextMeshPro damageNumber;

    private float timer;
    private const float animationDuration = 0.75f;
    private const float easingConstant = 2f;

    public void SetNumber(float amount, Color color) {
        damageNumber.text = amount.ToString("0");
        damageNumber.color = color;
        timer = animationDuration;
        damageNumber.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        StartCoroutine(Animate());
    }
    private IEnumerator Animate() {        
        while(timer > 0f) {
            damageNumber.transform.localScale = new Vector3(Mathf.Clamp01(easingConstant * timer / animationDuration), Mathf.Clamp01(easingConstant * timer / animationDuration), 1f);
            damageNumber.color = new Color(damageNumber.color.r, damageNumber.color.g, damageNumber.color.b, Mathf.Clamp01(easingConstant * timer / animationDuration));
            timer -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
