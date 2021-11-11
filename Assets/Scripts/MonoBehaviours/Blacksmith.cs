using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : MonoBehaviour, IInteractable {

    private Player globalPlayer;
    private UICanvas uiCanvas;
    private VendorUI vendorUI;

    private ParticleSystem partSys;

    private const float animationDuration = 0.5f;

    private WaitForSeconds waitDuration;

    private Item[] items;

    private const int shopSize = 8;

    private System.Random shopRandom;

    private List<int> indices;

    public Sprite interactImage;
    public ItemCreator itemCreator;
    public ItemDatabase itemDatabase;

    private WorldGeneration worldGen;

    private float DistanceToPlayer {
        get {
            return globalPlayer ? Vector3.Distance(globalPlayer.transform.position, transform.position) : 100f;
        }
    }
    public Sprite UISprite {
        get;
        set;
    }
    public bool Seller {
        get;
        set;
    }

    private void Awake() {
        worldGen = FindObjectOfType<WorldGeneration>();
        globalPlayer = FindObjectOfType<Player>();
        uiCanvas = FindObjectOfType<UICanvas>();
        partSys = GetComponent<ParticleSystem>();
        waitDuration = new WaitForSeconds(animationDuration);
        UISprite = interactImage;
        vendorUI = uiCanvas.vendorPanel.GetComponent<VendorUI>();
        Seller = true;
    }
    public void SetBlacksmith() {
        shopRandom = new System.Random(worldGen.WorldSeed.GetHashCode());
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
            items[i] = itemCreator.GetBlacksmithItemByIndex(indices[i]);
        }
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
    public void Burst() {
        StartCoroutine(BurstParticles());
    }
    private IEnumerator BurstParticles() {
        partSys.Play();
        AudioSystem.audioManager.PlaySound("anvilHit", DistanceToPlayer);
        yield return waitDuration;
        partSys.Stop();
    }
    public void Interact() {
        uiCanvas.vendorPanel.SetActive(!uiCanvas.vendorPanel.activeSelf);
        uiCanvas.inventoryIcon.SetActive(!uiCanvas.vendorPanel.activeSelf);
        if(uiCanvas.vendorPanel.activeSelf) {
            vendorUI.SetItems(items);
        }
        uiCanvas.playerUI.SetActive(!uiCanvas.vendorPanel.activeSelf);
    }
}
