using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour {
    // Should be an odd number so it can have one center
    public const int worldSize = 5;
    private const int levelSize = 64;
    // Should we use random seed?
    private bool randomSeed = false;
    // Seed for world generation
    private string worldSeed = "tss";
    // A 2 dimentional map of string seeds
    private string[,] worldMap = new string[worldSize, worldSize];
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
        if(randomSeed) {
            worldSeed = Time.time.ToString();
        }
        pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());        
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
    public int WorldSize {
        get => worldSize;
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
        Debug.Log($"Current coordinates: {data.currentCoordinates[0]}, {data.currentCoordinates[1]}");
        Debug.Log($"Last coordinates: {data.lastCoordinates[0]}, {data.lastCoordinates[1]}");
        if(data != null) {
            world = new WorldData(data.worldData, data.currentCoordinates);
        }
        else {
            world = new WorldData(new string[worldSize, worldSize], new int[] { 0, 0 }) {
                lastCoordinates = new int[] { -1, -1 }
            };
        }
    }
    public void ChangeCurrentCoordinates(Vector2Int coordinates) {
        //ChangeLastCoordinates(new Vector2Int(world.currentCoordinates[0], world.currentCoordinates[1]));
        world.currentCoordinates = new int[] { coordinates.x, coordinates.y};
        GenerateCurrentLevels();
    }
    public void ChangeLastCoordinates(Vector2Int coordinates) {
        world.lastCoordinates = new int[] { coordinates.x, coordinates.y};
    }
    // Generate adjacent levels and unload farther ones
    private void GenerateCurrentLevels() {             
        for(int x = world.currentCoordinates[0] - 1; x <= world.currentCoordinates[0] + 1; x++) {
            for(int y = world.currentCoordinates[1] - 1; y <= world.currentCoordinates[1] + 1; y++) {                
                if(world.currentCoordinates[0] == world.lastCoordinates[0] && world.currentCoordinates[1] == world.lastCoordinates[1]) {
                    return;
                }
                if(!loadingPanel.gameObject.activeInHierarchy) {
                    loadingPanel.gameObject.SetActive(true);
                    loadingPanel.LoadingLevels();
                }                
                if(x == world.currentCoordinates[0] && y == world.currentCoordinates[1]) {
                    Debug.Log("Rescanning pathfinding...");
                    int levelSize = 64;
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
                if(world.worldData[x, y] == null) {
                    world.worldData[x, y] = pseudoRandomForWorld.Next().ToString();
                }
                worldMap[x, y] = world.worldData[x, y];
                // Create a level with object pooling
                GameObject levelClone = ObjectPooler.objectPooler.GetPooledObject(level.name);
                levelClone.transform.position = new Vector3(x * levelSize, y * levelSize, 0);
                levelClone.transform.rotation = Quaternion.identity;
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levels.Add(levelGen);
                currentRenderedLevels.Add(new Vector2Int(x, y));
                levelGen.layout = new LevelLayout(worldMap[x, y]) {
                    worldCoordinates = new Vector2Int(x, y),
                    worldSize = worldSize
                };
                levelClone.SetActive(true);
                /*
                Thread th = new Thread(() => levelGen.SetLayout(this));
                th.Start();*/                
                levelGen.SetLayout();
            }
        }
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
        }
    }
    private IEnumerator ScanPath() {
        yield return new WaitForSeconds(0.5f);        
        if(aStarPath) {
            aStarPath.Scan();
        }
        loadingPanel.FadeOut();
    }
}
