using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;

    private VendorUI vendorUI;

    public Sprite interactImage;

    public Sprite UISprite {
        get;
        set;
    }
    public bool Seller {
        get;
        set;
    }
    private void Awake() {
        uiCanvas = FindObjectOfType<UICanvas>();
        UISprite = interactImage;
        vendorUI = uiCanvas.vendorPanel.GetComponent<VendorUI>();
        Seller = true;
    }
    public void Interact() {
        uiCanvas.vendorPanel.SetActive(!uiCanvas.vendorPanel.activeSelf);
        uiCanvas.inventoryIcon.SetActive(!uiCanvas.vendorPanel.activeSelf);
        if(uiCanvas.chestPanel.activeSelf) {
            // vendorUI.SetChestUI(this);
        }
        uiCanvas.playerUI.SetActive(!uiCanvas.vendorPanel.activeSelf);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().SetInteraction(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().ClearInteraction(this);
        }
    }
}
