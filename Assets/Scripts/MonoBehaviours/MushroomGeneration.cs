using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomGeneration : MonoBehaviour {

    public MushroomDatabase mushroomDatabase;
    public ItemDatabase itemDatabase;
    public MushroomData mushroomData;
    public int[,] mushroomValues;
    public List<GameObject> mushrooms;

    private LevelGeneration levelGeneration;
    private System.Random pseudoRandomForMushrooms;

    private const int levelSize = 32;

    private void Awake() {
        levelGeneration = GetComponent<LevelGeneration>();
    }
    public async Task GenerateMushrooms(int slot, string seed) {
        pseudoRandomForMushrooms = new System.Random(seed.GetHashCode());
        mushrooms = new List<GameObject>();
        mushroomValues = new int[levelSize, levelSize];
        for(int i = 0; i < levelSize; i++) {
            for(int j = 0; j < levelSize; j++) {
                mushroomValues[i, j] = -1;
            }
        }
        mushroomData = SaveSystem.LoadMushrooms(slot, levelGeneration.layout.worldCoordinates);
        if(mushroomData != null) {            
            for(int x = 0; x < levelSize; x++) {
                for(int y = 0; y < levelSize; y++) {
                    if(mushroomData.mushroomMap[x, y] >= 0 && levelGeneration.CheckLocation(x, y)) {
                        mushroomValues[x, y] = mushroomData.mushroomMap[x, y];
                        if(mushroomValues[x, y] >= 0 && mushroomValues[x, y] < 12) {
                            PlaceMushroom(x, y, mushroomValues[x, y]);
                        }
                        else if(mushroomValues[x, y] > 0) {
                            PlaceObject(x, y, mushroomValues[x, y]);
                        }
                    }
                }
            }
        }
        else {
            for(int i = 0; i < CalculateMushroomCount(); i++) {
                Vector3Int randomLocation = Vector3Int.zero;
                bool valid = false;
                while(!valid) {
                    // pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4)
                    randomLocation = new Vector3Int(pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4), pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4), 0);
                    if(levelGeneration.CheckLocation(randomLocation.x, randomLocation.y) && !levelGeneration.occupiedCoordinates.Contains(randomLocation)) {
                        valid = true;
                    }
                }
                levelGeneration.occupiedCoordinates.Add(randomLocation);
                Debug.Log($"Occupied coordinates count: {levelGeneration.occupiedCoordinates.Count}");
                mushroomValues[randomLocation.x, randomLocation.y] = await Task.Run(CalculateMushroomValue);
                if(mushroomValues[randomLocation.x, randomLocation.y] >= 0 && mushroomValues[randomLocation.x, randomLocation.y] < 12) {
                    PlaceMushroom(randomLocation.x, randomLocation.y, mushroomValues[randomLocation.x, randomLocation.y]);
                }
                else if(mushroomValues[randomLocation.x, randomLocation.y] > 0) {
                    PlaceObject(randomLocation.x, randomLocation.y, mushroomValues[randomLocation.x, randomLocation.y]);
                }
            }            
        }
        PersistentData.FinishWorkingThread();
    }
    private void PlaceMushroom(int x, int y, int mushroomValue) {
        GameObject mushroomClone = ObjectPooler.objectPooler.GetPooledObject("Mushroom");
        mushroomClone.transform.position = levelGeneration.GetPreciseLocation(x, y);
        mushroomClone.transform.rotation = Quaternion.identity;
        mushrooms.Add(mushroomClone);
        mushroomClone.GetComponent<MushroomObject>().SetMushroom(mushroomDatabase.mushrooms[mushroomValue], new Vector2Int(x, y), this);
        mushroomClone.SetActive(true);
    }
    private void PlaceObject(int x, int y, int objectValue) {
        GameObject objectClone = ObjectPooler.objectPooler.GetPooledObject("GroundObject");
        objectClone.transform.position = levelGeneration.GetPreciseLocation(x, y);
        objectClone.transform.rotation = Quaternion.identity;
        mushrooms.Add(objectClone);
        objectClone.GetComponent<GroundObject>().SetObject(itemDatabase.items[objectValue - 12], new Vector2Int(x, y), this);
        objectClone.SetActive(true);
    }
    public void ClearMushrooms() {
        SaveMushrooms(0);
        for(int i = 0; i < mushrooms.Count; i++) {
            mushrooms[i].SetActive(false);
        }
        mushrooms.Clear();
        levelGeneration.occupiedCoordinates.Clear();
    }
    public void TakeMushroom(Vector2Int coordinates) {
        mushroomValues[coordinates.x, coordinates.y] = -1;
    }
    public void SaveMushrooms(int slot) {
        SaveSystem.SaveMushrooms(this, slot, levelGeneration.layout.worldCoordinates);
    }
    private int CalculateMushroomCount() {
        return Mathf.Clamp(RollDice(2, 6) - 6, 1, 6);
    }
    private int CalculateMushroomValue() {
        int diceRollTotal = RollDice(6, 6);
        int mushroomValue;
        switch(diceRollTotal) {
            case 34:
                // Matsutake mushroom
                mushroomValue = 6;
                break;
            case 33:
                // Morel mushroom
                mushroomValue = 10;
                break;
            case 32:
                // Black trumpet mushroom
                mushroomValue = 3;
                break;
            case 29:
                // Truffle mushroom
                mushroomValue = 1;
                break;
            case 30:
                // Reishi mushroom
                mushroomValue = 5;
                break;
            case 28:
                // Chanterelle mushroom
                mushroomValue = 4;
                break;
            case 27:
                // Fly agaric mushroom
                mushroomValue = 11;
                break;
            case 26:
                // Turkey tail mushroom
                mushroomValue = 2;
                break;
            case 25:
                // Destroying angel mushroom
                mushroomValue = 0;
                break;
            case 20:
                // Porcini mushroom
                mushroomValue = 9;
                break;
            case 23:
                // Enoki mushroom
                mushroomValue = 8;
                break;
            case 24:
                // Puffball mushroom
                mushroomValue = 7;
                break;
            case 21:
            case 18:
                // Wood
                mushroomValue = 12;
                break;
            case 17:
                // Copper
                mushroomValue = 13;
                break;
            case 16:
                // Iron
                mushroomValue = 14;
                break;
            case 15:
                // Silver
                mushroomValue = 15;
                break;
            case 12:
                // Gold
                mushroomValue = 16;
                break;
            case 11:
                // Platinum
                mushroomValue = 17;
                break;
            case 14:
                // Titanium
                mushroomValue = 18;
                break;
            case 13:
                // Tungsten
                mushroomValue = 19;
                break;
            case 10:
                // Sapphire
                mushroomValue = 20;
                break;
            case 7:
                // Ruby
                mushroomValue = 21;
                break;
            case 9:
                // Emerald
                mushroomValue = 22;
                break;
            case 6:
                // Diamond
                mushroomValue = 23;
                break;
            case 8:
                // Musgravite
                mushroomValue = 24;
                break;
            case 35:
            case 36:
                // Taaffeite
                mushroomValue = 25;
                break;
            case 31:
                // Amber
                mushroomValue = 26;
                break;
            case 19:
                // Stone
                mushroomValue = 42;
                break;
            case 22:
                // Coal
                mushroomValue = 44;
                break;
            default:
                mushroomValue = -1;
                break;
        }
        return mushroomValue;
    }    
    private int RollDice(int diceCount, int diceMax) {
        int diceRollTotal = 0;
        // Roll 5d20
        for(int i = 0; i < diceCount; i++) {
            int dieResult = pseudoRandomForMushrooms.Next(1, diceMax + 1);
            diceRollTotal += dieResult;
        }
        return diceRollTotal;
    }
}
