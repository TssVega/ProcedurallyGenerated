using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour {

    public GameObject loadingPanel;
    public GameObject playerUI;

    private void Start() {
        loadingPanel.SetActive(true);
        playerUI.SetActive(true);
    }
}
