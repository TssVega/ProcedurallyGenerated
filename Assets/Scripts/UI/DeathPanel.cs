using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathPanel : MonoBehaviour {

    public Image deathPanel;
    public Transform youDiedText;
    public Transform loadButton;

    public TextMeshProUGUI youDied;
    public TextMeshProUGUI loadText;

    private float opacity = 0f;
    private const float multiplier = 5f;

    private void Start() {
        youDied.text = LocalizationManager.localization.GetText("youDied");
        loadText.text = LocalizationManager.localization.GetText("loadGame");
    }
    public void DeathPanelInit() {
        StartCoroutine(Darken());
    }
    private IEnumerator Darken() {
        while(deathPanel.color.a < 1f) {
            // Change opacity of the panel background
            opacity += 0.01f;
            deathPanel.color = new Color(0f, 0f, 0f, opacity);
            // Lower the "You Died" text
            youDiedText.transform.Translate(Vector3.down * multiplier);
            // Ascend the "Load Last Save" button
            loadButton.transform.Translate(Vector3.up * multiplier);
            yield return null;
        }
    }
    public void LoadLastSave() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
