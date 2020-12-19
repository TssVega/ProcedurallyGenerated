using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelLayout{

    public string seed;

    public int width = 64;
    public int height = 64;

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
    // Testing fix ide count variable
    public int fixSideCount = 2;
    // Any wall region smaller than this int will get removed
    public int wallThresholdSize = 16;
    // Any ground region smaller than this int will get filled
    public int groundThresholdSize = 16;
}
