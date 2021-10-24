using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenItemInfo : MonoBehaviour {

    public ItemInfoPanel itemInfoPanel;
    private Inventory inventory;

    private void Awake() {
        inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
    }
    public void SetItemInfoPanelFromInventory(int index) {
        if(inventory.inventory[index] != null) {
            itemInfoPanel.SetItem(inventory.inventory[index], index, true);
            itemInfoPanel.gameObject.SetActive(true);
        }
        AudioSystem.audioManager.PlaySound("inGameButton", 0f);
    }
    public void SetItemInfoPanelFromEquipment(int index) {
        if(inventory.equipment[index]) {
            itemInfoPanel.SetItem(inventory.equipment[index], index, false);
            itemInfoPanel.gameObject.SetActive(true);
        }
        AudioSystem.audioManager.PlaySound("inGameButton", 0f);
    }
}
