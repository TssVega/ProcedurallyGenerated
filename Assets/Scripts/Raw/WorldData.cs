using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData {

    public int[,] worldMap;
    public string[,] worldData;
    public int[] currentCoordinates;
    public int[] lastCoordinates;

    public WorldData(string[,] data, int[] current, int[,] worldMap) {
        worldData = data;
        currentCoordinates = current;
        lastCoordinates = new[] { -1, -1 };
        this.worldMap = worldMap;
    }
}
