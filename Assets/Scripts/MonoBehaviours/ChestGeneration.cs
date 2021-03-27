using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGeneration : MonoBehaviour {

    public Chest[] chests;

    private List<GameObject> chestObjects = new List<GameObject>();
    private System.Random pseudoRandomForChests;
    private readonly int maxChestCount = 5;
    private readonly int maxItemCountInChest = 10;

    // Load chest data from binary file
    public void LoadChests(int slot, string seed, LevelGeneration levelGen) {
        pseudoRandomForChests = new System.Random(seed.GetHashCode());
        int chestCount = pseudoRandomForChests.Next(0, maxChestCount);
        int itemCount = pseudoRandomForChests.Next(0, maxItemCountInChest);
        chests = new Chest[chestCount];
        ChestData chestData = SaveSystem.LoadChests(slot, levelGen.layout.worldCoordinates);
        // Get the chest content data from file. If there is no data, create a new file
        if(chestData != null) {
            for(int i = 0; i < chestCount; i++) {
                chests[i].items = new string[itemCount];
                for(int j = 0; j < itemCount; j++) {
                    if(chestData.chests[i, j] != null) {
                        chests[i].items[j] = chestData.chests[i, j];
                    }
                    else {
                        chests[i].items[j] = Time.time.ToString();
                    }
                }
            }
        }
        else {
            for(int i = 0; i < chestCount; i++) {
                chests[i].items = new string[itemCount];
                for(int j = 0; j < itemCount; j++) {
                    chests[i].items[j] = Time.time.ToString();
                }
            }
        }
    }
    public void PutChests() {
        
    }
    public void ClearChests() {
    
    }
}
