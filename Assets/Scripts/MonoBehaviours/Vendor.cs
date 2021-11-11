using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;

    private VendorUI vendorUI;

    public Sprite interactImage;
    public ItemCreator itemCreator;
    public ItemDatabase itemDatabase;

    private Item[] items;

    private const int shopSize = 8;

    private System.Random shopRandom;

    private List<int> indices;

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
        indices = new List<int>();
        for(int i = 0; i < shopSize; i++) {
            int index;
            do {
                index = shopRandom.Next(0, itemDatabase.blacksmithItems.Count);
            }
            while(indices.Contains(index));
            indices.Add(index);
        }
        for(int i = 0; i < items.Length; i++) {
            items[i] = itemCreator.GetItemByIndex(indices[i]);
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
