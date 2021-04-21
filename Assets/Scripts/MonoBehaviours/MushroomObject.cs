using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomObject : MonoBehaviour, IInteractable {

    private Mushroom mushroom;
    private SpriteRenderer spriteRen;
    private Player player;
    public Sprite interactImage;

    public Sprite UISprite {
        get;
        set;
    }

    private void Awake() {
        player = FindObjectOfType<Player>();
        spriteRen = GetComponent<SpriteRenderer>();
        UISprite = interactImage;
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
    public void SetMushroom(Mushroom mushroom) {
        this.mushroom = mushroom;
        if(mushroom) {
            spriteRen.sprite = mushroom.firstSprite;
        }
        else {
            spriteRen.sprite = null;
            gameObject.SetActive(false);
        }
    }
    public Mushroom GetMushroom() {
        return this.mushroom;
    }

    public void Interact() {
        if(gameObject.activeSelf) {
            if(player.inventory.CanAddToInventory()) {
                player.inventory.AddToInventory(GetMushroom());
                SetMushroom(null);
            }            
        }        
    }
}
