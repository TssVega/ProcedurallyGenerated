using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData {
    // string[chestIndex, itemIndex]
    public string[,] chests;

    private const int maxChestCount = 5;
    private const int maxItemCount = 16;

    public ChestData(ChestGeneration chestGen) {
        chests = new string[maxChestCount, maxItemCount];
        for(int i = 0; i < maxChestCount; i++) {
            for(int j = 0; j < maxItemCount; j++) {
                if(i < chestGen.chests.Length && j < chestGen.chests[i].items.Length) {
                    this.chests[i, j] = chestGen.chests[i].items[j];
                }
                else {
                    this.chests[i, j] = null;
                }
            }
        }
    }
}
