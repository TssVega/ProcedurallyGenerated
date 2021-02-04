using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour {

    public GameObject loadingPanel;
    public GameObject playerUI;
    public GameObject skillTreeUI;
    public GameObject inventoryUI;
    public SelectedItem selectedItem;

    private void Start() {
        loadingPanel.SetActive(true);
        playerUI.SetActive(true);
        skillTreeUI.SetActive(false);
        inventoryUI.SetActive(false);
    }
}
