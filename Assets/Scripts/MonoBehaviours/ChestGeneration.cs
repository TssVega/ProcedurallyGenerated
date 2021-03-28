using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestGeneration : MonoBehaviour {

    public ChestObject chestObject;
    public Chest[] chests;

    private List<ChestObject> chestObjects = new List<ChestObject>();
    private System.Random pseudoRandomForChests;
    private readonly int maxChestCount = 5;
    private readonly int maxItemCountInChest = 10;

    // Load chest data from binary file
    public void LoadChests(int slot, string seed, LevelGeneration levelGen) {
        pseudoRandomForChests = new System.Random(seed.GetHashCode());
        int chestCount = pseudoRandomForChests.Next(0, maxChestCount);        
        int itemCount = pseudoRandomForChests.Next(0, maxItemCountInChest);
        if(levelGen.layout.worldCoordinates.x == 0 && levelGen.layout.worldCoordinates.y == 2) {
            Debug.Log($"Chest count: {chestCount}");
            Debug.Log($"Item count: {itemCount}");
            Debug.Log($"Seed: {seed}");
        }        
        chests = new Chest[chestCount];
        ChestData chestData = SaveSystem.LoadChests(slot, levelGen.layout.worldCoordinates);
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
                        chests[i].items[j] = Time.time.ToString();
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
                    chests[i].items[j] = Time.time.ToString();
                }
            }
        }
        PutChests(chestCount, levelGen);
    }
    public void PutChests(int count, LevelGeneration levelGen) {
        for(int i = 0; i < count; i++) {
            GameObject chestClone = ObjectPooler.objectPooler.GetPooledObject("Chest");
            chestClone.transform.position = GetRandomLocationForChest(pseudoRandomForChests, levelGen.layout.height, levelGen);
            chestClone.transform.rotation = Quaternion.identity;
            chestClone.SetActive(true);
            chestObjects.Add(chestClone.GetComponent<ChestObject>());
            if(levelGen.layout.worldCoordinates.x == 0 && levelGen.layout.worldCoordinates.y == 2) {
                Debug.Log($"Chest position: {chestClone.transform.position}");
            }
        }
    }
    public void ClearChests() {
        for(int i = 0; i < chestObjects.Count; i++) {
            chestObjects[i].gameObject.SetActive(false);
        }
        chestObjects.Clear();
    }
    public Vector3Int GetRandomLocationForChest(System.Random pseudoRnd, int mapSize, LevelGeneration levelGen) {
        bool valid = false;
        Vector3Int location = Vector3Int.zero;
        // System.Random pseudoRandomForLevel = new System.Random(seed.GetHashCode());
        while(!valid) {
            location = new Vector3Int(pseudoRnd.Next(3, 60), pseudoRnd.Next(3, 60), 0);
            if(levelGen.IsValidLocation(new Vector2Int(location.x, location.y))) {
                valid = true;
            }
        }
        location = new Vector3Int(
                    (location.x - mapSize / 2) + levelGen.layout.worldCoordinates.x * mapSize,
                    (location.y - mapSize / 2) + levelGen.layout.worldCoordinates.y * mapSize, 0);
        return location;
    }
}
