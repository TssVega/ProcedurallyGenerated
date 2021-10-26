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
    public bool Seller {
        get;
        set;
    }
    private void Awake() {
        player = FindObjectOfType<Player>();
        spriteRen = GetComponent<SpriteRenderer>();
        UISprite = interactImage;
        Seller = false;
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
        if(item && spriteRen && itemGeneration) {
            spriteRen.sprite = item.firstSprite;
            this.itemCoordinates = itemCoordinates;
            this.itemGeneration = itemGeneration;
        }
    }
    private void TakeObject() {
        spriteRen.sprite = null;
        this.itemGeneration.TakeMushroom(new Vector2Int(this.itemCoordinates.x, this.itemCoordinates.y));
        itemGeneration.mushrooms.Remove(this.gameObject);
        gameObject.SetActive(false);
    }
    public Item GetObject() {
        return this.groundObj;
    }
    public void Interact() {
        if(gameObject.activeSelf) {
            if(player.inventory.CanAddToInventory()) {
                // Othani fortune
                if(player.raceIndex == 3) {
                    const float bonusYieldChance = 0.1f;
                    if(Random.Range(0f, 1f) < bonusYieldChance) {
                        player.inventory.AddToInventory(GetObject(), false);
                    }                    
                }
                player.inventory.AddToInventory(GetObject(), true);
                TakeObject();
            }
        }
    }
}
