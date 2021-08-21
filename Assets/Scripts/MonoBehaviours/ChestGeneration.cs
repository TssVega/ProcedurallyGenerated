using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGeneration : MonoBehaviour {

    public ChestObject chestObject;
    public Chest[] chests;

    private List<ChestObject> chestObjects = new List<ChestObject>();
    private System.Random pseudoRandomForChests;
    private readonly int maxChestCount = 5;
    private readonly int maxItemCountInChest = 16;

    private LevelGeneration levelGeneration;
    private WorldGeneration worldGeneration;

    private void Awake() {
        levelGeneration = GetComponent<LevelGeneration>();
        worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    // Load chest data from binary file
    public void LoadChests(int slot, string seed) {
        pseudoRandomForChests = new System.Random(seed.GetHashCode());
        int chestCount = pseudoRandomForChests.Next(0, maxChestCount) / 4;        
        int itemCount = pseudoRandomForChests.Next(1, maxItemCountInChest);      
        chests = new Chest[chestCount];
        ChestData chestData = SaveSystem.LoadChests(slot, levelGeneration.layout.worldCoordinates);
        // Get the chest content data from file. If there is no data, create a new file
        if(chestData != null && chestCount != 0) {
            for(int i = 0; i < chestCount; i++) {
                chests[i] = new Chest {
                    items = new string[itemCount]
                };
                for(int j = 0; j < itemCount; j++) {
                    if(chestData.chests[i, j] != null) {
                        chests[i].items[j] = chestData.chests[i, j];
                    }
                    else {
                        chests[i].items[j] = null;
                    }
                }
            }
        }
        else if(chestCount != 0) {
            for(int i = 0; i < chestCount; i++) {
                chests[i] = new Chest {
                    items = new string[itemCount]
                };
                for(int j = 0; j < itemCount; j++) {
                    //chests[i].items[j] = Random.Range(0, 99999999).ToString();
                    string xWorld = GetNumberWithZeroesInIgsignificantBits(levelGeneration.layout.worldCoordinates.x);
                    string yWorld = GetNumberWithZeroesInIgsignificantBits(levelGeneration.layout.worldCoordinates.y);
                    string chestIndex = GetNumberWithZeroesInIgsignificantBits(i);
                    string itemIndex = GetNumberWithZeroesInIgsignificantBits(j);
                    chests[i].items[j] = $"{worldGeneration.WorldSeed}{xWorld}{yWorld}{chestIndex}{itemIndex}";
                }
            }
        }
        PutChests(chestCount, levelGeneration);
    }
    private string GetNumberWithZeroesInIgsignificantBits(int value) {
        string newValue;
        if(value / 100 >= 1) {
            newValue = value.ToString();
        }
        else if(value / 10 >= 1) {
            newValue = $"0{value}";
        }
        else {
            newValue = $"00{value}";
        }
        return newValue;
    }
    public void PutChests(int count, LevelGeneration levelGen) {
        if(count < 1) {
            return;
        }
        for(int i = 0; i < count; i++) {
            GameObject chestClone = ObjectPooler.objectPooler.GetPooledObject("Chest");
            chestClone.transform.position = GetRandomLocationForChest(pseudoRandomForChests, levelGen.layout.levelSize, levelGen);
            chestClone.transform.rotation = Quaternion.identity;
            chestClone.GetComponent<ChestObject>().SetChest(chests[i]);
            chestClone.SetActive(true);
            chestObjects.Add(chestClone.GetComponent<ChestObject>());
        }
    }
    public void ClearChests() {
        SaveChests(0);
        for(int i = 0; i < chestObjects.Count; i++) {            
            chestObjects[i].gameObject.SetActive(false);
        }
        chestObjects.Clear();
        levelGeneration.occupiedCoordinates.Clear();
    }
    public void SaveChests(int slot) {
        SaveSystem.SaveChests(this, slot, levelGeneration.layout.worldCoordinates);
    }
    public Vector3Int GetRandomLocationForChest(System.Random pseudoRnd, int mapSize, LevelGeneration levelGen) {
        bool valid = false;
        Vector3Int location = Vector3Int.zero;
        // System.Random pseudoRandomForLevel = new System.Random(seed.GetHashCode());
        while(!valid) {
            location = new Vector3Int(pseudoRnd.Next(3, levelGen.layout.levelSize - 4), pseudoRnd.Next(3, levelGen.layout.levelSize - 4), 0);
            if(levelGen.IsValidLocation(new Vector2Int(location.x, location.y)) && !levelGeneration.occupiedCoordinates.Contains(location)) {
                valid = true;
            }
        }
        location = new Vector3Int(
                    (location.x - mapSize / 2) + levelGen.layout.worldCoordinates.x * mapSize,
                    (location.y - mapSize / 2) + levelGen.layout.worldCoordinates.y * mapSize, 0);
        return location;
    }
}
