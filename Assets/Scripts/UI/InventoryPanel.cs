using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {

    private Inventory inventory;
    private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();
        inventory = player.GetComponent<Inventory>();
        inventory.SetInventory();
    }
    private void OnEnable() {
        inventory.SetInventoryImages();
    }
    public void SaveButton() {
        player.SavePlayer();
    }
}
