using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour {

    public GameObject loadingPanel;
    public GameObject playerUI;
    public GameObject skillTreeUI;

    private void Start() {
        loadingPanel.SetActive(true);
        playerUI.SetActive(true);
        skillTreeUI.SetActive(true);
    }
}
