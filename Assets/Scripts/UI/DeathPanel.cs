using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour {

    public Image deathPanel;
    public Transform youDiedText;
    public Transform loadButton;

    private float opacity = 0f;

    public void DeathPanelInit() {
        StartCoroutine(Darken());
    }
    private IEnumerator Darken() {
        while(deathPanel.color.a < 1f) {
            // Change opacity of the panel background
            opacity += 0.002f;
            deathPanel.color = new Color(0f, 0f, 0f, opacity);
            // Lower the "You Died" text
            youDiedText.transform.Translate(Vector3.down);
            // Ascend the "Load Last Save" button
            loadButton.transform.Translate(Vector3.up);
            yield return null;
        }
    }
    public void LoadLastSave() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
