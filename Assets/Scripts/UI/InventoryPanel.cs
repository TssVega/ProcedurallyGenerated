using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {

    private Inventory inventory;

    private void Awake() {
        inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
    }
    private void OnEnable() {
        inventory.SetInventoryImages();
    }
}
