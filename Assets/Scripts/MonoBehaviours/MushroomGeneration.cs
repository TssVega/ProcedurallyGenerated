using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomGeneration : MonoBehaviour {

    public MushroomDatabase mushroomDatabase;
    public GameObject mushroomObject;
    public MushroomData mushroomData;
    public int[,] mushroomValues;
    public MushroomObject mushroom;
    public List<GameObject> mushrooms;

    private LevelGeneration levelGeneration;
    private System.Random pseudoRandomForMushrooms;

    private readonly int levelSize = 64;

    private void Awake() {
        levelGeneration = GetComponent<LevelGeneration>();
    }
    public async Task GenerateMushrooms(int slot, string seed) {
        pseudoRandomForMushrooms = new System.Random(seed.GetHashCode());
        mushrooms = new List<GameObject>();
        mushroomValues = new int[levelSize, levelSize];
        mushroomData = SaveSystem.LoadMushrooms(slot, levelGeneration.layout.worldCoordinates);
        if(mushroomData != null) {
            mushroomValues = mushroomData.mushroomMap;
        }
        else {
            for(int x = 0; x < levelSize; x++) {
                for(int y = 0; y < levelSize; y++) {
                    if(levelGeneration.CheckLocation(x, y)) {
                        mushroomValues[x, y] = await Task.Run(() => CalculateMushroomValue());
                        if(mushroomValues[x, y] >= 0) {
                            PlaceMushroom(x, y, mushroomValues[x, y]);
                        }
                    }                                       
                }
            }
        }        
    }
    private void PlaceMushroom(int x, int y, int mushroomValue) {
        GameObject mushroomClone = ObjectPooler.objectPooler.GetPooledObject("Mushroom");
        mushroomClone.transform.position = levelGeneration.GetPreciseLocation(x, y);
        mushroomClone.transform.rotation = Quaternion.identity;
        mushrooms.Add(mushroomClone);
        mushroomClone.GetComponent<MushroomObject>().SetMushroom(mushroomDatabase.mushrooms[mushroomValue]);
        mushroomClone.SetActive(true);
    }
    public void ClearMushrooms() {
        SaveMushrooms(0);
        for(int i = 0; i < mushrooms.Count; i++) {
            mushrooms[i].SetActive(false);
        }
        mushrooms.Clear();
    }
    public void SaveMushrooms(int slot) {
        SaveSystem.SaveMushrooms(this, slot, levelGeneration.layout.worldCoordinates);
    }
    private int CalculateMushroomValue() {
        int diceRollTotal = Roll3d20();
        int mushroomValue;
        switch(diceRollTotal) {
            case 3:
                // Matsutake mushroom
                mushroomValue = 6;
                break;
            case 4:
                // Morel mushroom
                mushroomValue = 10;
                break;
            case 59:
                // Black trumpet mushroom
                mushroomValue = 3;
                break;
            case 5:
                // Truffle mushroom
                mushroomValue = 1;
                break;
            case 58:
                // Reishi mushroom
                mushroomValue = 5;
                break;
            case 6:
                // Chanterelle mushroom
                mushroomValue = 4;
                break;
            case 57:
                // Fly agaric mushroom
                mushroomValue = 11;
                break;
            case 7:
                // Turkey tail mushroom
                mushroomValue = 2;
                break;
            case 56:
                // Destroying angel mushroom
                mushroomValue = 0;
                break;
            case 8:
                // Porcini mushroom
                mushroomValue = 9;
                break;
            case 55:
                // Enoki mushroom
                mushroomValue = 8;
                break;
            case 11:
                // Puffball mushroom
                mushroomValue = 7;
                break;
            default:
                mushroomValue = -1;
                break;
        }
        return mushroomValue;
    }    
    private int Roll3d20() {
        int diceRollTotal = 0;
        // Roll 3 d 20
        for(int i = 0; i < 3; i++) {
            int dieResult = pseudoRandomForMushrooms.Next(1, 21);
            diceRollTotal += dieResult;
        }
        return diceRollTotal;
    }
}
