using UnityEngine;

[System.Serializable]
public class LevelLayout {

    // Use other seeds than given seed
    public bool useRandomSeed = false;
    public string seed;

    public Vector2Int worldCoordinates;

    public int levelSize = 32;

    public bool generateWalls = true;
    public bool generateGrass = true;
    [Range(0, 3)] public int grassDensity = 3;
    public bool generatePlants = true;
    [Range(0, 50)] public int plantDensity = 25;

    [Range(0, 100)]
    public int fillPercent = 50;
    // Radius of the passages between rooms
    public int passageRadius = 2;
    // Generate exit points
    public bool generateExitPoints = true;
    // Random fill
    public bool randomFill = true;
    // Smoothing
    public bool smooth = true;
    // Process
    public bool process = true;
    // Draw
    public bool draw = true;
    // How many levels of smoothing to apply
    public int smoothLevel = 4;
    // This will be equal to two all times
    public int addSideCount = 4;
    // Testing fix side count variable
    public int fixSideCount = 2;
    // Any wall region smaller than this int will get removed
    public int wallThresholdSize = 16;
    // Any ground region smaller than this int will get filled
    public int groundThresholdSize = 0;
    // Connect seperate rooms?
    public bool connectRooms = true;
    // Use the marching squares algorithm to place sprites to neccessary places
    public bool marchingSquares = true;

    public int worldSize = 0;

    public LevelLayout(string seed) {
        this.seed = seed;
    }
}
