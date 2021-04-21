using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestObject : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;
    private ChestUI chestUI;

    public Sprite interactImage;

    public Chest chestContent;

    public Sprite UISprite {
        get;
        set;
    }

    private void Awake() {
        uiCanvas = FindObjectOfType<UICanvas>();
        UISprite = interactImage;
        chestUI = uiCanvas.chestPanel.GetComponent<ChestUI>();
    }
    public void SetChest(Chest chest) {
        chestContent = new Chest();
        chestContent = chest;
    }
    public Chest GetChest() {
        return chestContent;
    }
    public void Interact() {
        // Open and close chest here
        uiCanvas.chestPanel.SetActive(!uiCanvas.chestPanel.activeSelf);
        if(uiCanvas.chestPanel.activeSelf) {
            chestUI.SetChestUI(this);
        }        
        uiCanvas.playerUI.SetActive(!uiCanvas.chestPanel.activeSelf);
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
