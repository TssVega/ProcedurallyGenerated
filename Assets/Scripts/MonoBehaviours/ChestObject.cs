using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestObject : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;

    public Sprite interactImage;

    public Sprite UISprite {
        get;
        set;
    }

    private void Awake() {
        uiCanvas = FindObjectOfType<UICanvas>();
        UISprite = interactImage;
    }
    public void Interact() {
        // Open and close chest here
        uiCanvas.chestPanel.SetActive(!uiCanvas.chestPanel.activeSelf);
        uiCanvas.playerUI.SetActive(!uiCanvas.chestPanel.activeSelf);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().SetInteraction(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().ClearInteraction();
        }
    }
}
