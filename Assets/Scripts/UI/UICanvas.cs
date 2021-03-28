﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour {

    public GameObject loadingPanel;
    public GameObject playerUI;
    public GameObject skillTreeUI;
    public GameObject inventoryUI;
    public GameObject chestPanel;
    public Image interactImage;
    public SelectedItem selectedItem;

    private void Start() {
        loadingPanel.SetActive(true);
        playerUI.SetActive(true);
        skillTreeUI.SetActive(false);
        inventoryUI.SetActive(false);
        chestPanel.SetActive(false);
        ClearInteractButton();
    }
    public void TogglePlayerUI() {
        playerUI.SetActive(!playerUI.activeSelf);
    }
    public void ChangeInteractButton(Sprite interactImage) {
        this.interactImage.gameObject.SetActive(true);
        this.interactImage.sprite = interactImage;
        this.interactImage.color = Color.white;
    }
    public void ClearInteractButton() {        
        interactImage.sprite  = null;
        interactImage.color = Color.clear;
        interactImage.gameObject.SetActive(false);
    }
}
