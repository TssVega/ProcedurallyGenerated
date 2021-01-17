using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Tilemaps;

public class WorldGeneration : MonoBehaviour {
    // Should be an odd number so it can have one center
    private const int worldSize = 5;
    private const int levelSize = 64;
    // Should we use random seed?
    private bool randomSeed = false;
    // Seed for world generation
    private string worldSeed = "tss";
    // A 2 dimentional map of string seeds
    private string[,] worldMap = new string[worldSize, worldSize];
    private Vector2Int lastCoordinates = new Vector2Int(-1, -1);
    // Current coordinates
    private Vector2Int currentCoordinates = new Vector2Int(0, 0);
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

    private void Awake() {
        player = FindObjectOfType<Player>();
        levels = new List<LevelGeneration>();
        currentRenderedLevels = new List<Vector2Int>();
        aStarPath = FindObjectOfType<AstarPath>();
        loadingPanel = FindObjectOfType<UICanvas>().loadingPanel.GetComponent<LoadingPanel>();
    }
    private void Start() {
        player.LoadPlayer();
        if(randomSeed) {
            worldSeed = Time.time.ToString();
        }
        pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());
        if(!groundTilemap) {
            groundTilemap = GameObject.FindWithTag("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        }
        if(!tilemap) {
            tilemap = GameObject.FindWithTag("Grid").transform.GetChild(1).GetComponent<Tilemap>();
        }
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
    /*
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            worldSeed = Time.time.ToString();
            pseudoRandomForWorld = new System.Random(worldSeed.GetHashCode());
            StartCoroutine(GenerateCurrentLevels());
        }
    }*/
    public void ChangeCurrentCoordinates(Vector2Int coordinates) {
        currentCoordinates = coordinates;
        GenerateCurrentLevels();
    }
    public void ChangeLastCoordinates(Vector2Int coordinates) {
        lastCoordinates = coordinates;
    }
    // Generate adjacent levels and unload farther ones
    private void GenerateCurrentLevels() {
             
        for(int x = currentCoordinates.x - 1; x <= currentCoordinates.x + 1; x++) {
            for(int y = currentCoordinates.y - 1; y <= currentCoordinates.y + 1; y++) {
                if(currentCoordinates == lastCoordinates) {
                    return;
                }
                if(!loadingPanel.gameObject.activeInHierarchy) {
                    loadingPanel.gameObject.SetActive(true);
                    loadingPanel.LoadingLevels();
                }
                if(x == currentCoordinates.x && y == currentCoordinates.y) {
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
                worldMap[x, y] = pseudoRandomForWorld.Next().ToString();
                //GameObject levelClone = Instantiate(level, new Vector3(x * levelSize, y * levelSize, 0), Quaternion.identity);
                GameObject levelClone = ObjectPooler.objectPooler.GetPooledObject(level.name);
                levelClone.transform.position = new Vector3(x * levelSize, y * levelSize, 0);
                levelClone.transform.rotation = Quaternion.identity;
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levels.Add(levelGen);
                levelGen.layout = new LevelLayout(worldMap[x, y]) {
                    worldCoordinates = new Vector2Int(x, y),
                    worldSize = worldSize
                };
                levelClone.SetActive(true);
                /*
                Thread th = new Thread(() => levelGen.SetLayout(this));
                th.Start();*/
                currentRenderedLevels.Add(new Vector2Int(x, y));
                levelGen.SetLayout();                
            }
        }
        // Unload far away levels
        for(int i = 0; i < levels.Count; i++) {
            if(levels[i].layout.worldCoordinates.x < currentCoordinates.x - 1 ||
                levels[i].layout.worldCoordinates.x > currentCoordinates.x + 1 ||
                levels[i].layout.worldCoordinates.y < currentCoordinates.y - 1 ||
                levels[i].layout.worldCoordinates.y > currentCoordinates.y + 1) {
                levels[i].UnloadLevel();
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
