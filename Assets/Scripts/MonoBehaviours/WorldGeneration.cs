using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.IO;

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
    public List<LevelGeneration> levels;
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
        SetupLevels();
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
    public void SetupLevels() {
        groundTilemap.ClearAllTiles();
        tilemap.ClearAllTiles();
        LoadWorldData();        
        GenerateCurrentLevels();
    }

    public int WorldSize => worldSize;

    public int[,] WorldMap => world.worldMap;

    public string WorldSeed => world.seed;

    public int[,] ExplorationData => world.explorationData;

    public int[] CurrentCoordinates => world.currentCoordinates;

    public int[] LastCoordinates => world.lastCoordinates;

    public bool[,] PortalData { get => world?.portalData;
        set { } 
    }

    private void GetAutosaveFiles() {
        List<string> chestFiles = PersistentData.GetAllFilesWithKey($"ChestData{PersistentData.saveSlot}", $"", $"");
        List<string> mushroomFiles = PersistentData.GetAllFilesWithKey($"MushroomData{PersistentData.saveSlot}", $"", $"");
        List<string> poolFiles = PersistentData.GetAllFilesWithKey($"PoolData{PersistentData.saveSlot}", $"", $"");
        for(int i = 0; i < chestFiles.Count; i++) {
            File.Copy(chestFiles[i], chestFiles[i].Replace($"Data{PersistentData.saveSlot}", "Data0"));
        }
        for(int i = 0; i < mushroomFiles.Count; i++) {
            File.Copy(mushroomFiles[i], mushroomFiles[i].Replace($"Data{PersistentData.saveSlot}", "Data0"));
        }
        for(int i = 0; i < poolFiles.Count; i++) {
            File.Copy(poolFiles[i], poolFiles[i].Replace($"Data{PersistentData.saveSlot}", "Data0"));
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
            world = new WorldData(data.worldData, data.currentCoordinates, data.worldMap, data.seed, data.explorationData, data.portalData);
        }
        else {
            world = new WorldData(new string[worldSize, worldSize], new[] { 0, 0 }, new int[worldSize, worldSize],
                Random.Range(0, 99999999).ToString(), new int[worldSize, worldSize], new bool[worldSize, worldSize]) {
                lastCoordinates = new[] { -1, -1 }
            };
        }
    }
    public void ChangeCurrentCoordinates(Vector2Int coordinates) {
        //ChangeLastCoordinates(new Vector2Int(world.currentCoordinates[0], world.currentCoordinates[1]));
        world.currentCoordinates = new[] { coordinates.x, coordinates.y };
        GenerateCurrentLevels();
        SetSideLevels();
    }
    public void ChangeLastCoordinates(Vector2Int coordinates) {
        world.lastCoordinates = new[] { coordinates.x, coordinates.y };
    }
    // Generate adjacent levels and unload farther ones
    private void GenerateCurrentLevels() {
        for(int x = world.currentCoordinates[0] - 1; x <= world.currentCoordinates[0] + 1; x++) {
            for(int y = world.currentCoordinates[1] - 1; y <= world.currentCoordinates[1] + 1; y++) {
                if(world.currentCoordinates[0] == world.lastCoordinates[0] && world.currentCoordinates[1] == world.lastCoordinates[1]) {
                    return;
                }
                if(CheckBounds(x, y)) {
                    if(world.explorationData[x, y] == 0) {
                        world.explorationData[x, y] = 1;
                        mapPanel.NewExploration(new Vector2Int(x, y), WorldMap);
                    }
                }
                if(CheckBounds(x, y) && world.worldMap[x, y] < 1) {
                    continue;
                }
                if((x == world.currentCoordinates[0] - 1 && y == world.currentCoordinates[1] - 1)
                    || (x == world.currentCoordinates[0] - 1 && y == world.currentCoordinates[1] + 1)
                    || (x == world.currentCoordinates[0] + 1 && y == world.currentCoordinates[1] - 1)
                    || (x == world.currentCoordinates[0] + 1 && y == world.currentCoordinates[1] + 1)) {
                    continue;
                }
                if(!loadingPanel.gameObject.activeInHierarchy) {
                    loadingPanel.gameObject.SetActive(true);
                    loadingPanel.LoadingLevels();
                }
                if(!CheckBounds(x, y)) {
                    continue;
                }
                if(x == world.currentCoordinates[0] && y == world.currentCoordinates[1]) {
                    mapPanel.SetCursor(new Vector2Int(x, y));
                    aStarPath.graphs[0].active.data.gridGraph.center = new Vector3Int(levelSize * x, levelSize * y, 0);
                    StartCoroutine(ScanPath());
                    //ScanPath();
                }
                if(currentRenderedLevels.Contains(new Vector2Int(x, y))) {
                    continue;
                }
                //PersistentData.AddWorkingThread();
                if(world.worldData[x, y] == null) {
                    //pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());
                    world.worldData[x, y] = $"{WorldSeed}{Insignify(x)}{Insignify(y)}";
                }
                // Create a level with object pooling
                GameObject levelClone = ObjectPooler.objectPooler.GetPooledObject(level.name);
                levelClone.transform.position = new Vector3(x * levelSize, y * levelSize, 0);
                levelClone.transform.rotation = Quaternion.identity;
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levels.Add(levelGen);
                /*
                if(x != world.currentCoordinates[0] || y != world.currentCoordinates[1]) {
                    Spawner.spawner.AddSideLevel(levelGen);
                }
                else if(x == world.currentCoordinates[0] && y == world.currentCoordinates[1]) {
                    Spawner.spawner.RemoveSideLevel(levelGen);
                }*/
                currentRenderedLevels.Add(new Vector2Int(x, y));
                levelGen.layout = new LevelLayout(world.worldData[x, y]) {
                    worldCoordinates = new Vector2Int(x, y),
                    worldSize = worldSize,
                    biomeIndex = WorldMap[x, y]
                };
                levelClone.SetActive(true);
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
                Spawner.spawner.RemoveSideLevel(levels[i]);
                levels[i].UnloadLevel();
                currentRenderedLevels.Remove(levels[i].layout.worldCoordinates);
                levels.Remove(levels[i]);
                --i;
                // levels.RemoveAt(i);
                //currentRenderedLevels.RemoveAt(i);
            }
        }
    }
    private void SetSideLevels() {
        Spawner.spawner.ClearSideLevels();
        for(int i = 0; i < levels.Count; i++) {
            if(world.currentCoordinates[0] != levels[i].layout.worldCoordinates.x || world.currentCoordinates[1] != levels[i].layout.worldCoordinates.y) {
                Spawner.spawner.AddSideLevel(levels[i]);
            }
        }
    }
    // Returns the specified number with up to 2 insignificant digits in front of it until it reaches 3 digits
    // Eg: 9 returns "009", 25 returns "025"
    private string Insignify(int value) {
        string newValue;
        if((float)value / 100f >= 1f) {
            newValue = value.ToString();
        }
        else if((float)value / 10f >= 1f) {
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
            aStarPath.Scan(aStarPath.graphs[0]);
        }
        loadingPanel.FadeOut();
    }
}
