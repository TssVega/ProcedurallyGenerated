using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomObject : MonoBehaviour, IInteractable {

    private Mushroom mushroom;
    private SpriteRenderer spriteRen;
    private Player player;
    private Vector2Int mushroomCoordinates;
    private MushroomGeneration mushroomGeneration;

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
    public void SetMushroom(Mushroom mushroom, Vector2Int mushroomCoordinates, MushroomGeneration mushroomGeneration) {
        this.mushroom = mushroom;
        if(this.mushroom && spriteRen && mushroomGeneration) {
            spriteRen.sprite = this.mushroom.firstSprite;
            this.mushroomCoordinates = mushroomCoordinates;
            this.mushroomGeneration = mushroomGeneration;
        }
    }
    private void TakeMushroom() {
        spriteRen.sprite = null;
        this.mushroomGeneration.TakeMushroom(new Vector2Int(this.mushroomCoordinates.x, this.mushroomCoordinates.y));
        gameObject.SetActive(false);
    }
    public Mushroom GetMushroom() {
        return this.mushroom;
    }
    public void Interact() {
        if(gameObject.activeSelf) {
            if(player.inventory.CanAddToInventory()) {
                player.inventory.AddToInventory(GetMushroom(), true);
                TakeMushroom();
            }            
        }        
    }
}
