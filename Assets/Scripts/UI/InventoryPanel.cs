using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {

    private Inventory inventory;
    private Player player;

    private bool imagesSet = false;
    private bool firstTime = true;

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
    }
    public void SaveButton() {
        player.SavePlayer();
    }
}
