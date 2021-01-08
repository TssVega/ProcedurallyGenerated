using UnityEngine;

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
    // Current coordinates
    private Vector2Int currentCoordinates = new Vector2Int(1, 1);
    // So each world can be same with same seed
    private System.Random pseudoRandomForWorld;

    public GameObject level;

    private void Start() {
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

    private void GenerateCurrentLevels() {
        for(int x = currentCoordinates.x - 1; x <= currentCoordinates.x + 1; x++) {
            for(int y = currentCoordinates.y - 1; y <= currentCoordinates.y + 1; y++) {
                if(x < 0 || y < 0) {
                    continue;
                }
                Debug.Log($"{x}, {y}");
                worldMap[x, y] = pseudoRandomForWorld.Next().ToString();
                GameObject levelClone = Instantiate(level, new Vector3(x * levelSize, y * levelSize, 0), Quaternion.identity);
                LevelGeneration levelGen = levelClone.GetComponent<LevelGeneration>();
                levelGen.layout = new LevelLayout(worldMap[x, y]) {
                    worldCoordinates = new Vector2Int(x, y),
                    worldSize = worldSize
                };
                levelGen.SetLayout();
            }
        }
    }
}
