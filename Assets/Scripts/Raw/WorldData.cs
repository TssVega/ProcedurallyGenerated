using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData {

    public string[,] worldData;
    public int[] currentCoordinates;
    public int[] lastCoordinates;

    public WorldData(string[,] data, int[] current) {
        worldData = data;
        currentCoordinates = current;
        lastCoordinates = new int[] { -1, -1 };
    }
}
