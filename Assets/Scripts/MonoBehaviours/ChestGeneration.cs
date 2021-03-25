using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGeneration : MonoBehaviour {

    public Chest[] chests;

    private System.Random pseudoRandomForChests;
    private readonly int maxChestCount = 10;
    private readonly int maxItemCountInChest = 10;
    // Load chest data from binary file
    public void LoadChests(int slot, string seed, LevelGeneration levelGen) {
        pseudoRandomForChests = new System.Random(seed.GetHashCode());
        int chestCount = pseudoRandomForChests.Next(0, maxChestCount);
        chests = new Chest[chestCount];
        ChestData chestData = SaveSystem.LoadChests(slot, levelGen.layout.worldCoordinates);
        for(int i = 0; i < chestCount; i++) {
            chests[i].items = new string[maxItemCountInChest];
            for(int j = 0; j < maxItemCountInChest; j++) {
                chests[i].items[j] = chestData.chests[i, j];
            }
        }
    }
    public void GenerateChests(string seed, LevelGeneration levelGen) {
        pseudoRandomForChests = new System.Random(seed.GetHashCode());
        int chestCount = pseudoRandomForChests.Next(0, maxChestCount);
        chests = new Chest[chestCount];
        for(int i = 0; i < chestCount; i++) {

            //chests[i].items = 
            //levelGen.GetRandomLocation();
        }
    }
    public void PutChests() {
    
    }
    public void ClearChests() {
    
    }
}
