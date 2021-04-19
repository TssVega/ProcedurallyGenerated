﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenItemInfo : MonoBehaviour {

    public ItemInfoPanel itemInfoPanel;
    private Inventory inventory;

    private void Awake() {
        inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
    }
    public void SetItemInfoPanelFromInventory(int index) {
        itemInfoPanel.SetItem(inventory.inventory[index]);
        itemInfoPanel.gameObject.SetActive(true);
    }
    public void SetItemInfoPanelFromEquipment(int index) {
        itemInfoPanel.SetItem(inventory.equipment[index]);
        itemInfoPanel.gameObject.SetActive(true);
    }
}