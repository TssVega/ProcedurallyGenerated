using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationText : MonoBehaviour {

    private WaitForSeconds wait;
    private TextMeshProUGUI notificationText;

    private Coroutine textCoroutine;

    private void Awake() {
        notificationText = GetComponent<TextMeshProUGUI>();
        wait = new WaitForSeconds(3f);        
    }
    private void Start() {
        gameObject.SetActive(false);
    }
    private void OnDisable() {
        StopAllCoroutines();
        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 0f);
    }
    public void SetText(string str, Color color) {
        gameObject.SetActive(true);
        if(textCoroutine != null && notificationText.color.a > 0f) {
            StopCoroutine(textCoroutine);
        }        
        textCoroutine = StartCoroutine(TextAnimation(str, color));
    }
    private IEnumerator TextAnimation(string str, Color color) {
        notificationText.text = str;
        notificationText.color = color;
        yield return wait;
        var alpha = 1f;
        while(notificationText.color.a > 0f) {
            alpha -= Time.deltaTime;
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, alpha);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
