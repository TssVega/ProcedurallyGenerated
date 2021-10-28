using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Pool : MonoBehaviour, IInteractable {

    private StatusEffects playerStatus;
    private Stats playerStats;
    private Inventory inventory;

    private PoolGeneration poolGeneration;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSys;

    private bool full;
    private bool healingPool;

    private Light2D light2D;

    public Sprite emptyPoolSprite;
    public Sprite healthPoolSprite;
    public Sprite manaPoolSprite;

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
        playerStatus = FindObjectOfType<Player>().GetComponent<StatusEffects>();
        inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
        playerStats = FindObjectOfType<Player>().stats;
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSys = GetComponent<ParticleSystem>();
        light2D = GetComponent<Light2D>();
        UISprite = interactImage;
        Seller = false;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            if(full) {
                collision.GetComponent<Player>().SetInteraction(this);
            }            
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().ClearInteraction(this);          
        }
    }
    public void SetPool(bool healthPool, bool full, bool exists, PoolGeneration poolGeneration) {
        if(exists) {            
            this.healingPool = healthPool;
            this.full = full;
            this.poolGeneration = poolGeneration;
            if(!full) {
                spriteRenderer.sprite = emptyPoolSprite;
                light2D.intensity = 0f;
                particleSys.Stop();
            }
            else if(healingPool) {
                spriteRenderer.sprite = healthPoolSprite;
                light2D.color = Color.red;
                light2D.intensity = 1f;
                var main = particleSys.main;
                main.startColor = Color.red;
                particleSys.Play();
            }
            else {
                spriteRenderer.sprite = manaPoolSprite;
                light2D.color = Color.blue;
                light2D.intensity = 1f;
                var main = particleSys.main;
                main.startColor = Color.blue;
                particleSys.Play();
            }
        }        
        /*
        this.mushroom = mushroom;
        if(mushroom && spriteRen) {
            spriteRen.sprite = mushroom.firstSprite;
            this.mushroomCoordinates = mushroomCoordinates;
            this.mushroomGeneration = mushroomGeneration;
        }
        else if(spriteRen) {
            spriteRen.sprite = null;
            this.mushroomGeneration.TakeMushroom(new Vector2Int(this.mushroomCoordinates.x, this.mushroomCoordinates.y));
            gameObject.SetActive(false);
        }*/
    }
    public bool GetPoolType() {
        return healingPool;
    }
    public bool GetFill() {
        return full;
    }
    public void Interact() {
        if(gameObject.activeSelf) {
            if(!full) {
                return;
            }
            if(healingPool) {
                playerStatus.Heal(playerStats.trueMaxHealth);                
            }
            else {
                playerStatus.GiveMana(playerStats.maxMana);
            }
            // Milona wonder
            if(playerStats.player.raceIndex == 8) {
                int randomIndex = Random.Range(0, 9);
                switch(randomIndex) {
                    case 0:
                        playerStats.strength++;
                        break;
                    case 1:
                        playerStats.agility++;
                        break;
                    case 2:
                        playerStats.dexterity++;
                        break;
                    case 3:
                        playerStats.intelligence++;
                        break;
                    case 4:
                        playerStats.faith++;
                        break;
                    case 5:
                        playerStats.wisdom++;
                        break;
                    case 6:
                        playerStats.vitality++;
                        break;
                    case 7:
                        playerStats.charisma++;
                        break;
                }
                inventory.CheckCrafting();
                playerStats.player.UpdateMaxHealth();
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
            }
            FindObjectOfType<Player>().ClearInteraction(this);
            SetPool(healingPool, false, true, poolGeneration);
            poolGeneration.full = false;
        }
    }
}
