using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData {
    // string[chestIndex, itemIndex]
    public string[,] chests;

    public ChestData(ChestGeneration chestGen) {
        for(int i = 0; i < chests.GetUpperBound(0); i++) {
            for(int j = 0; j < chests.GetUpperBound(1); j++) {
                this.chests[i, j] = chestGen.chests[i].items[j];
            }
        }
    }
}
