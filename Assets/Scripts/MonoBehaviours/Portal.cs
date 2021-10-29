using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable {

    private UICanvas uiCanvas;
    private PortalUI portalUI;

    public Sprite interactIcon;

    private bool bonusTaken;
    private List<int> indices;
    private string seed;

    private const int choiceCount = 3;

    private Player player;
    private PoolGeneration generator;
    private WorldGeneration worldGen;

    private GameObject mapPanel;

    private System.Random pseudoRandom;

    public Sprite UISprite {
        get;
        set;
    }
    public bool Seller {
        get;
        set;
    }
    public void Interact() {
        if(!mapPanel) {
            mapPanel = FindObjectOfType<UICanvas>().mapPanel;
        }
        if(bonusTaken) {
            mapPanel.SetActive(true);
            return;
        }
        if(seed == null) {
            bonusTaken = false;
            seed = Random.Range(0, 99999).ToString();
        }
        pseudoRandom = new System.Random(seed.GetHashCode());
        uiCanvas.portalPanel.SetActive(!uiCanvas.portalPanel.activeSelf);
        uiCanvas.inventoryIcon.SetActive(!uiCanvas.portalPanel.activeSelf);
        if(uiCanvas.portalPanel.activeSelf) {
            indices = new List<int>();
            for(int i = 0; i < choiceCount; i++) {
                int index;
                do {
                    index = pseudoRandom.Next(0, player.npcBonusesLength);
                }
                while(player.npcBonuses[index] || indices.Contains(index));
                indices.Add(index);
            }
            portalUI.SetBonuses(indices, this);
        }
        uiCanvas.playerUI.SetActive(!uiCanvas.portalPanel.activeSelf);
    }
    public void SetPortal(bool bonusTaken, string seed, PoolGeneration generator) {
        this.seed = seed;
        this.bonusTaken = bonusTaken;
        this.generator = generator;
    }
    public void GetBonus(int index) {
        bonusTaken = true;
        generator.usedPortal = true;
        AudioSystem.audioManager.PlaySound("bookRead", 0f);
        player.AddNPCBonus(indices[index]);
        player.ClearInteraction(this);
        worldGen.PortalData[worldGen.CurrentCoordinates[0], worldGen.CurrentCoordinates[1]] = true;
        uiCanvas.portalPanel.SetActive(!uiCanvas.portalPanel.activeSelf);
        uiCanvas.inventoryIcon.SetActive(!uiCanvas.portalPanel.activeSelf);
        uiCanvas.playerUI.SetActive(!uiCanvas.portalPanel.activeSelf);
    }
    private void Awake() {
        worldGen = FindObjectOfType<WorldGeneration>();
        player = FindObjectOfType<Player>();
        
        bonusTaken = false;
        uiCanvas = FindObjectOfType<UICanvas>();
        mapPanel = uiCanvas.mapPanel;
        portalUI = uiCanvas.portalPanel.GetComponent<PortalUI>();
        Seller = false;
        UISprite = interactIcon;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            player.SetInteraction(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            player.ClearInteraction(this);
        }
    }
}
