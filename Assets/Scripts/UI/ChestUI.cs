using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour {

    private ChestObject chestObject;

    public GameObject[] chestSlots;

    private Image[] firstIcons;
    private Image[] secondIcons;
    private Image[] thirdIcons;

    private readonly int chestCapacity = 16;

    public ItemCreator itemCreator;

    private Inventory inventory;
    private Stats playerStats;

    private void Awake() {
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
    }

    public void SetChestUI(ChestObject chestObj) {
        firstIcons = new Image[chestCapacity];
        secondIcons = new Image[chestCapacity];
        thirdIcons = new Image[chestCapacity];
        for(int i = 0; i < chestSlots.Length; i++) {
            firstIcons[i] = chestSlots[i].transform.GetChild(1).GetComponent<Image>();
            secondIcons[i] = chestSlots[i].transform.GetChild(2).GetComponent<Image>();
            thirdIcons[i] = chestSlots[i].transform.GetChild(0).GetComponent<Image>();
        }
        inventory = FindObjectOfType<Inventory>();
        chestObject = chestObj;
        for(int i = 0; i < chestCapacity; i++) {
            if(i >= chestObject.chestContent.items.Length) {
                firstIcons[i].sprite = null;
                firstIcons[i].color = Color.clear;
                secondIcons[i].sprite = null;
                secondIcons[i].color = Color.clear;
                thirdIcons[i].sprite = null;
                thirdIcons[i].color = Color.clear;
                continue;
            }
            Item item = null;
            if(string.IsNullOrEmpty(chestObject.chestContent.items[i])) {
                firstIcons[i].sprite = null;
                firstIcons[i].color = Color.clear;
                secondIcons[i].sprite = null;
                secondIcons[i].color = Color.clear;
                thirdIcons[i].sprite = null;
                thirdIcons[i].color = Color.clear;
            }
            else {
                item = itemCreator.CreateItem(chestObject.chestContent.items[i], playerStats.luck);
            }
            if(item == null) {
                continue;
            }
            if(item.firstIcon) {
                firstIcons[i].sprite = item.firstIcon;
                firstIcons[i].color = item.firstColor;
            }
            else {
                firstIcons[i].sprite = null;
                firstIcons[i].color = Color.clear;
            }
            if(item.secondIcon) {
                secondIcons[i].sprite = item.secondIcon;
                secondIcons[i].color = item.secondColor;
            }
            else {
                secondIcons[i].sprite = null;
                secondIcons[i].color = Color.clear;
            }
            if(item.thirdIcon) {
                thirdIcons[i].sprite = item.thirdIcon;
                thirdIcons[i].color = item.thirdColor;
            }
            else {
                thirdIcons[i].sprite = null;
                thirdIcons[i].color = Color.clear;
            }
        }
    }
    public void TakeAllItems() {
        for(int i = 0; i < chestObject.chestContent.items.Length; i++) {
            TakeItemOnSlot(i);
        }
    }
    public void TakeItemOnSlot(int slot) {
        // If the item is successfully added to inventory
        if(slot >= chestObject.chestContent.items.Length) {
            return;
        }
        if(string.IsNullOrEmpty(chestObject.chestContent.items[slot])) {
            return;
        }
        if(!inventory.CanAddToInventory()) {
            return;
        }
        if(inventory.AddToInventory(itemCreator.CreateItem(chestObject.chestContent.items[slot], playerStats.luck), true)) {
            firstIcons[slot].color = Color.clear;
            secondIcons[slot].color = Color.clear;
            thirdIcons[slot].color = Color.clear;
            firstIcons[slot].sprite = null;
            secondIcons[slot].sprite = null;
            thirdIcons[slot].sprite = null;
            chestObject.chestContent.items[slot] = null;
        }
    }
}
