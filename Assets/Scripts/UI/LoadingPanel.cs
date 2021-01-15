using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingPanel : MonoBehaviour {

    private Image panel;
    private Image cube;
    private bool fadingIn = false;
    public LoadingScreenTips tips;
    public TextMeshProUGUI loadingTip;

    private void Awake() {
        panel = GetComponent<Image>();
        cube = transform.GetChild(0).GetComponent<Image>();
    }
    private void Start() {
        loadingTip.text = tips.tips[Random.Range(0, tips.tips.Count)];
    }
    private void Update() {
        if(gameObject.activeInHierarchy) {
            CubeAnimation();
        }
    }
    public void LoadingLevels() {
        panel.color = Color.clear;
        cube.color = Color.white;
        loadingTip.color = Color.clear;
    }
    private void CubeAnimation() {
        float newAlpha;
        if(fadingIn) {
            newAlpha = cube.color.a + 0.01f;
            if(newAlpha >= 1f) {
                fadingIn = false;
            }
        }
        else {
            newAlpha = cube.color.a - 0.01f;
            if(newAlpha <= 0f) {
                fadingIn = true;
            }
        }
        cube.color = new Color(cube.color.r, cube.color.g, cube.color.b, newAlpha);
    }
    // Slowly make screen clear from black
    public void FadeOut() {
        if(gameObject.activeInHierarchy) {
            StartCoroutine(Fade(false));
        }        
    }
    // Slowly make the screen black from clear
    public void FadeIn() {
        if(gameObject.activeInHierarchy) {
            StartCoroutine(Fade(true));
        }
    }
    private IEnumerator Fade(bool fadeIn) {
        /*
        panel.color = Color.black;
        cube.color = Color.white;
        loadingTip.color = Color.white;*/
        float newAlpha;
        if(fadeIn) {
            while(panel.color.a < 1) {
                newAlpha = panel.color.a + 0.01f;
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, newAlpha);
                yield return null;
            }
            gameObject.SetActive(false);
        }
        else {
            while(panel.color.a > 0) {
                newAlpha = panel.color.a - 0.01f;
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, newAlpha);
                yield return null;
            }
            gameObject.SetActive(false);
        }        
    }
}
