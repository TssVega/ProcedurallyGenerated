using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Tilemaps;
using System.IO;
using System;
using System.Globalization;

public class WorldGeneration : MonoBehaviour {
    // Should be an odd number so it can have one center
    public const int worldSize = 128;
    private const int levelSize = 32;
    // Seed for world generation
    // A 2 dimentional map of string seeds
    // private string[,] worldMap = new string[worldSize, worldSize];
    // Pathfinding for entities
    private AstarPath aStarPath;
    // So each world can be same with same seed
    private System.Random pseudoRandomForWorld;
    private List<Vector2Int> currentRenderedLevels;
    private List<LevelGeneration> levels;
    private LoadingPanel loadingPanel;
    public GameObject level;
    private Player player;
    [Header("Tilemaps")]
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap tilemap;
    [HideInInspector] public Tilemap debugTilemap;
    [HideInInspector] public Tilemap plantTilemap;

    private WorldData world;

    public MapPanel mapPanel;

    private void Awake() {
        player = FindObjectOfType<Player>();
        levels = new List<LevelGeneration>();
        currentRenderedLevels = new List<Vector2Int>();
        aStarPath = FindObjectOfType<AstarPath>();
        loadingPanel = FindObjectOfType<UICanvas>().loadingPanel.GetComponent<LoadingPanel>();
    }
    private void Start() {
        //player.LoadPlayer();
        // Load world data here
        /*
        world = new WorldData {
            worldData = new string[worldSize, worldSize]
        };*/
        PersistentData.ClearAutosaveFiles();
        GetAutosaveFiles();
        if(!groundTilemap) {
            groundTilemap = GameObject.FindWithTag("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        }
        if(!tilemap) {
            tilemap = GameObject.FindWithTag("Grid").transform.GetChild(1).GetComponent<Tilemap>();
        }
        groundTilemap.ClearAllTiles();
        tilemap.ClearAllTiles();
        LoadWorldData();
        //world = new WorldData(new string[worldSize, worldSize], new int[] { currentCoordinates.x, currentCoordinates.y});
        pseudoRandomForWorld = new System.Random(WorldSeed.GetHashCode());        
        GenerateCurrentLevels();
        /*
        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                worldMap[x, y] = pseudoRandomForWorld.Next().ToString();
                GameObject levelClone = Instantiate(level, new Vector3(x * levelSize, y * levelSize, 0), Quaternion.identity);
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levelGen.layout = new LevelLayout(worldMap[x, y]) {
                    worldCoordinates = new Vector2Int(x, y)
                };
                levelGen.SetLayout();
            }
        }*/
    }

    public int WorldSize => worldSize;

    public int[,] WorldMap => world.worldMap;

    public string WorldSeed => world.seed;

    public int[,] ExplorationData => world.explorationData;

    private void GetAutosaveFiles() {
        List<string> chestFiles = PersistentData.GetAllFilesWithKey($"ChestData{PersistentData.saveSlot}", $"", $"");
        List<string> mushroomFiles = PersistentData.GetAllFilesWithKey($"MushroomData{PersistentData.saveSlot}", $"", $"");
        for(int i = 0; i < chestFiles.Count; i++) {
            File.Copy(chestFiles[i], chestFiles[i].Replace($"Data{PersistentData.saveSlot}", "Data0"));
        }
        for(int i = 0; i < mushroomFiles.Count; i++) {
            File.Copy(mushroomFiles[i], mushroomFiles[i].Replace($"Data{PersistentData.saveSlot}", "Data0"));
        }   
    }
    public bool CanSave() {
        bool canSave = !(world.currentCoordinates[0] == world.lastCoordinates[0]
            && world.currentCoordinates[1] == world.lastCoordinates[1]);
        return canSave;
    }
    /*
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            worldSeed = Time.time.ToString();
            pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());
            StartCoroutine(GenerateCurrentLevels());
        }
    }*/
    public void SaveWorldData() {
        ChangeLastCoordinates(new Vector2Int(-1, -1));
        SaveSystem.SaveWorld(world, PersistentData.saveSlot);
    }
    public void LoadWorldData() {
        WorldData data = SaveSystem.LoadWorld(PersistentData.saveSlot);
        if(data != null) {
            world = new WorldData(data.worldData, data.currentCoordinates, data.worldMap, data.seed, data.explorationData);
        }
        else {
            world = new WorldData(new string[worldSize, worldSize], new[] { 0, 0 }, new int[worldSize, worldSize],
                UnityEngine.Random.Range(0, 99999999).ToString(), new int[worldSize, worldSize]) {                
                lastCoordinates = new[] { -1, -1 }
            };
        }
    }
   
    public void ChangeCurrentCoordinates(Vector2Int coordinates) {
        //ChangeLastCoordinates(new Vector2Int(world.currentCoordinates[0], world.currentCoordinates[1]));
        world.currentCoordinates = new[] { coordinates.x, coordinates.y};
        GenerateCurrentLevels();
    }
    public void ChangeLastCoordinates(Vector2Int coordinates) {
        world.lastCoordinates = new[] { coordinates.x, coordinates.y};
    }
    // Generate adjacent levels and unload farther ones
    private void GenerateCurrentLevels() {
        for(int x = world.currentCoordinates[0] - 1; x <= world.currentCoordinates[0] + 1; x++) {
            for(int y = world.currentCoordinates[1] - 1; y <= world.currentCoordinates[1] + 1; y++) {                
                if(world.currentCoordinates[0] == world.lastCoordinates[0] && world.currentCoordinates[1] == world.lastCoordinates[1]) {
                    return;
                }
                if(CheckBounds(x, y) && world.worldMap[x, y] < 1) {
                    continue;
                }
                if(x == world.currentCoordinates[0] - 1 && y == world.currentCoordinates[1] - 1
                    || x == world.currentCoordinates[0] - 1 && y == world.currentCoordinates[1] + 1
                    || x == world.currentCoordinates[0] + 1 && y == world.currentCoordinates[1] - 1
                    || x == world.currentCoordinates[0] + 1 && y == world.currentCoordinates[1] + 1) {
                    continue;
                }
                if(!loadingPanel.gameObject.activeInHierarchy) {
                    loadingPanel.gameObject.SetActive(true);
                    loadingPanel.LoadingLevels();
                }                
                if(x == world.currentCoordinates[0] && y == world.currentCoordinates[1]) {
                    mapPanel.SetCursor(new Vector2Int(x, y));
                    aStarPath.graphs[0].active.data.gridGraph.center = new Vector3Int(levelSize * x, levelSize * y, 0);
                    StartCoroutine(ScanPath());
                    //ScanPath();
                }
                if(x < 0 || y < 0 || x > worldSize - 1 || y > worldSize - 1) {
                    continue;
                }
                if(currentRenderedLevels.Contains(new Vector2Int(x, y))) {
                    continue;                    
                }
                PersistentData.AddWorkingThread();
                if(world.worldData[x, y] == null) {
                    //pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());
                    world.worldData[x, y] = pseudoRandomForWorld.Next(x + 1, y * worldSize + worldSize + 1).ToString();
                }
                // Create a level with object pooling
                GameObject levelClone = ObjectPooler.objectPooler.GetPooledObject(level.name);
                levelClone.transform.position = new Vector3(x * levelSize, y * levelSize, 0);
                levelClone.transform.rotation = Quaternion.identity;
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levels.Add(levelGen);
                currentRenderedLevels.Add(new Vector2Int(x, y));
                string xCoord = GetNumberWithZeroesInIgsignificantBits(x);
                string yCoord = GetNumberWithZeroesInIgsignificantBits(y);
                string levelSeed = $"{WorldSeed}{xCoord}{yCoord}";
                levelGen.layout = new LevelLayout(levelSeed) {
                    worldCoordinates = new Vector2Int(x, y),
                    worldSize = worldSize
                };
                levelClone.SetActive(true);
                if(world.explorationData[x, y] == 0) {
                    world.explorationData[x, y] = 1;
                    mapPanel.NewExploration(new Vector2Int(x, y), WorldMap);
                }                
                /*
                Thread th = new Thread(() => levelGen.SetLayout(this));
                th.Start();*/
                Connections connections = new Connections() {
                    bottom = CheckBounds(x, y - 1) && world.worldMap[x, y - 1] > 0,
                    top = CheckBounds(x, y + 1) && world.worldMap[x, y + 1] > 0,
                    right = CheckBounds(x + 1, y) && world.worldMap[x + 1, y] > 0,
                    left = CheckBounds(x - 1, y) && world.worldMap[x - 1, y] > 0
                };

                levelGen.SetLayout(connections);
            }
        }
        /*
        // Unload far away levels
        for(int i = 0; i < levels.Count; i++) {
            if(levels[i].layout.worldCoordinates.x < world.currentCoordinates[0] - 1 ||
                levels[i].layout.worldCoordinates.x > world.currentCoordinates[0] + 1 ||
                levels[i].layout.worldCoordinates.y < world.currentCoordinates[1] - 1 ||
                levels[i].layout.worldCoordinates.y > world.currentCoordinates[1] + 1) {
                levels[i].UnloadLevel();
                levels.RemoveAt(i);
                currentRenderedLevels.RemoveAt(i);
            }
        }*/
        // Unload far away levels
        for(int i = 0; i < levels.Count; i++) {
            int xDifference = Mathf.Abs(levels[i].layout.worldCoordinates.x - world.currentCoordinates[0]);
            int yDifference = Mathf.Abs(levels[i].layout.worldCoordinates.y - world.currentCoordinates[1]);
            if(xDifference + yDifference > 1) {
                levels[i].UnloadLevel();
                levels.RemoveAt(i);
                currentRenderedLevels.RemoveAt(i);
            }
        }
    }
    private string GetNumberWithZeroesInIgsignificantBits(int value) {
        string newValue;
        if((float)value / 100f > 1f) {
            newValue = value.ToString();
        }
        else if((float)value / 10f > 1f) {
            newValue = $"0{value}";
        }
        else {
            newValue = $"00{value}";
        }
        return newValue;
    }
    private static bool CheckBounds(int x, int y) {
        return x >= 0 && y >= 0 && x < worldSize && y < worldSize;
    }
    private IEnumerator ScanPath() {
        yield return new WaitForSeconds(0.5f);        
        if(aStarPath) {
            aStarPath.Scan();
        }
        loadingPanel.FadeOut();
    }
    
}
