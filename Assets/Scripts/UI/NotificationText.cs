using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationText : MonoBehaviour {

    private WaitForSeconds wait;
    private TextMeshProUGUI notificationText;

    private void Awake() {
        notificationText = GetComponent<TextMeshProUGUI>();
        wait = new WaitForSeconds(4f);        
    }
    private void Start() {
        gameObject.SetActive(false);
    }
    public void SetText(string str) {
        gameObject.SetActive(true);
        StartCoroutine(TextAnimation(str));
    }
    private IEnumerator TextAnimation(string str) {
        notificationText.text = str;
        notificationText.color = Color.white;
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
