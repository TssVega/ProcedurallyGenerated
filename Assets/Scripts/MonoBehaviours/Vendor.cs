using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;

    private VendorUI vendorUI;

    public Sprite interactImage;
    public ItemCreator itemCreator;

    private Item[] items;

    private const int shopSize = 8;

    private System.Random shopRandom;

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
    private void OnEnable() {
        shopRandom = new System.Random(Random.Range(0, 999999999).GetHashCode());
        items = new Item[shopSize];
        for(int i = 0; i < items.Length; i++) {
            items[i] = itemCreator.GetRandomItem(shopRandom.Next().ToString());
        }
    }
    public void Interact() {
        uiCanvas.vendorPanel.SetActive(!uiCanvas.vendorPanel.activeSelf);
        uiCanvas.inventoryIcon.SetActive(!uiCanvas.vendorPanel.activeSelf);
        if(uiCanvas.vendorPanel.activeSelf) {
            vendorUI.SetItems(items);
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
