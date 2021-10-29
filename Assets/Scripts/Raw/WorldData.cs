using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData {

    public int[,] worldMap;
    public int[,] explorationData;
    public bool[,] portalData;
    public string[,] worldData;
    public int[] currentCoordinates;
    public int[] lastCoordinates;
    public string seed;

    public WorldData(string[,] data, int[] current, int[,] worldMap, string seed, int[,] explorationData, bool[,] portalData) {
        worldData = data;
        currentCoordinates = current;
        lastCoordinates = new[] { -1, -1 };
        this.worldMap = worldMap;
        this.seed = seed;
        this.explorationData = explorationData;
        this.portalData = portalData;
    }
}
