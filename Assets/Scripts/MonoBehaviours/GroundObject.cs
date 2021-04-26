using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : MonoBehaviour, IInteractable {

    private Item groundObj;
    private SpriteRenderer spriteRen;
    private Player player;
    private Vector2Int itemCoordinates;
    private MushroomGeneration itemGeneration;

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
    public void SetObject(Item item, Vector2Int itemCoordinates, MushroomGeneration itemGeneration) {
        this.groundObj = item;
        if(item && spriteRen) {
            spriteRen.sprite = item.firstSprite;
            this.itemCoordinates = itemCoordinates;
            this.itemGeneration = itemGeneration;
        }
        else if(spriteRen) {
            spriteRen.sprite = null;
            this.itemGeneration.TakeMushroom(new Vector2Int(this.itemCoordinates.x, this.itemCoordinates.y));
            gameObject.SetActive(false);
        }
    }
    public Item GetObject() {
        return this.groundObj;
    }

    public void Interact() {
        if(gameObject.activeSelf) {
            if(player.inventory.CanAddToInventory()) {
                player.inventory.AddToInventory(GetObject());
                SetObject(null, new Vector2Int(), null);
            }
        }
    }
}
