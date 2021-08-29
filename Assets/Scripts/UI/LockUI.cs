using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockUI : MonoBehaviour {

    public Image lockUI;
    public TextMeshProUGUI lockText;

    private PlayerController playerMovement;

    private void Awake() {
        lockUI.color = Color.green;
        lockText.color = Color.green;
        lockText.text = LocalizationManager.localization.GetText("unlocked");
        playerMovement = FindObjectOfType<PlayerController>();
    }
    public void CheckLock() {
        if(playerMovement && !playerMovement.lockedOn) {
            lockUI.color = Color.green;
            lockText.color = Color.green;
            lockText.text = LocalizationManager.localization.GetText("unlocked");
        }
        else {
            lockUI.color = Color.red;
            lockText.color = Color.red;
            lockText.text = LocalizationManager.localization.GetText("locked");
        }
    }
    public void Lock() {
        playerMovement.ToggleLock();
        if(!playerMovement.lockedOn) {
            lockUI.color = Color.green;
            lockText.color = Color.green;
            lockText.text = LocalizationManager.localization.GetText("unlocked");
        }
        else {
            lockUI.color = Color.red;
            lockText.color = Color.red;
            lockText.text = LocalizationManager.localization.GetText("locked");
        }
    }
}
