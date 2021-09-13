using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour {

    private Inventory inventory;
    private Player player;

    private bool imagesSet = false;
    private bool firstTime = true;

    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI craftingText;
    public TextMeshProUGUI skillsText;
    public TextMeshProUGUI mapText;

    public GameObject skillBook;
    public GameObject map;

    private void Awake() {
        player = FindObjectOfType<Player>();
        inventory = player.GetComponent<Inventory>();
        inventory.SetInventory();
    }    
    private void OnEnable() {
        if(firstTime) {
            firstTime = false;
            return;
        }
        if(!imagesSet) {
            imagesSet = true;
            inventory.SetInventoryImages();
        }
        CheckButtons();
    }
    public void CheckButtons() {
        inventoryText.text = LocalizationManager.localization.GetText("inventory");
        skillBook.SetActive(player.skillBookUnlocked);
        skillsText.gameObject.SetActive(player.skillBookUnlocked);
        skillsText.text = LocalizationManager.localization.GetText("skills");
        map.SetActive(player.mapUnlocked);
        mapText.gameObject.SetActive(player.mapUnlocked);
        mapText.text = LocalizationManager.localization.GetText("map");
        craftingText.text = LocalizationManager.localization.GetText("crafting");
    }
    public void SaveButton() {
        player.SavePlayer();
    }
}
