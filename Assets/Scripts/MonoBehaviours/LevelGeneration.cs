using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour {

    public bool generateDebugNumbersForWalls;
    // If the rooms should be connected
    public LevelLayout layout;
    // Tile database
    public TileDatabase tileDatabase;
    // Two dimensional map array
    private int[,] map;
    private int[,] reservedForConnectionsMap;
    // Psuedo random number for generating chests and enemies
    System.Random pseudoRandomForLevel;
    System.Random pseudoRandomForGround;
    System.Random pseudoRandomForTorches;
    System.Random pseudoRandomForWalls;
    System.Random pseudoRandomForPlants;
    // A reference to world generation script
    private WorldGeneration worldGeneration;
    private ChestGeneration chestGeneration;
    private MushroomGeneration mushroomGeneration;
    private PoolGeneration poolGeneration;

    private List<Vector3Int> wallCoordinates;
    private const int torchCount = 1;
    private const int crystalCount = 2;
    private const int stalagmiteCount = 3;

    private List<GameObject> torches;

    private Connections connections;

    public List<Vector3Int> occupiedCoordinates;

    public GameObject[] crystals;
    public GameObject[] stalagmites;

    private List<GameObject> activeCrystals;
    private List<GameObject> activeStalagmites;

    private void Awake() {
        worldGeneration = FindObjectOfType<WorldGeneration>();
        chestGeneration = GetComponent<ChestGeneration>();
        mushroomGeneration = GetComponent<MushroomGeneration>();
        poolGeneration = GetComponent<PoolGeneration>();
        torches = new List<GameObject>();
        occupiedCoordinates = new List<Vector3Int>();
        activeCrystals = new List<GameObject>();
        activeStalagmites = new List<GameObject>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if(collision.CompareTag("Player")) {
            worldGeneration.ChangeCurrentCoordinates(layout.worldCoordinates);
        }
        else if(enemy && enemy.moving) {
            enemy.level = this;
        }
    }    
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            worldGeneration.ChangeLastCoordinates(layout.worldCoordinates);
        }
    }
    /*
    private IEnumerator ScanPath() {
        yield return new WaitForSeconds(0.2f);
        if(path) {
            path.Scan();
        }
    }*/
    public void SetLayout(Connections connections) {
        this.connections = connections;
        pseudoRandomForPlants = new System.Random(layout.seed.GetHashCode());
        pseudoRandomForWalls = new System.Random(layout.seed.GetHashCode());
        pseudoRandomForLevel = new System.Random(layout.seed.GetHashCode());
        pseudoRandomForGround = new System.Random(layout.seed.GetHashCode());
        pseudoRandomForTorches = new System.Random(layout.seed.GetHashCode());
        if(layout.generateWalls) {
            GenerateMap();
        }
    }
    // Generate random map
    private void GenerateMap() {
        /*
        if(!plantTilemap) {
            plantTilemap = GameObject.FindWithTag("Grid").transform.GetChild(3).GetComponent<Tilemap>();
        }
        if(!debugTilemap) {
            debugTilemap = GameObject.FindWithTag("Grid").transform.GetChild(4).GetComponent<Tilemap>();
        }
        */
        map = new int[layout.levelSize, layout.levelSize];
        reservedForConnectionsMap = new int[layout.levelSize, layout.levelSize];
        wallCoordinates = new List<Vector3Int>();
        //await Task.Run(RandomFillMap);
        RandomFillMap();
        if(layout.worldCoordinates[0] == 0 && layout.worldCoordinates[1] == 0) {
            ClearStartPosition();
        }
        for(int i = 0; i < layout.smoothLevel; i++) {
            //await Task.Run(SmoothMap);       
            SmoothMap();
        }
        //await Task.Run(ProcessMap);
        ProcessMap();
        //StartCoroutine(DrawMapCoroutine());
        TileBase[] tileBase = DrawMap();    
        SetTiles(tileBase);
        if(layout.marchingSquares) {
            for(int i = 0; i < layout.addSideCount; i++) {
                AddSides();
            }
        }
        FillBackground();
        GenerateTorches();
        if(gameObject.activeInHierarchy) {
            //StartCoroutine(ScanPath());
        }
        chestGeneration.LoadChests(0, layout.seed);
        mushroomGeneration.GenerateMushrooms(0, layout.seed);
        GenerateCrystals();
        GenerateStalagmites();
        poolGeneration.GeneratePools(0, layout.seed);
        //mushroomGeneration.GenerateMushrooms(0, layout.seed);        
    }
    private void GenerateStalagmites() {
        // Only generate stalagmites on rocky caves
        if(layout.biomeIndex != 1) {
            return;
        }
        for(int i = 0; i < stalagmiteCount; i++) {
            GameObject stalagmite = ObjectPooler.objectPooler.GetPooledObject(stalagmites[pseudoRandomForPlants.Next(0, stalagmites.Length)].name);
            Vector3Int randomLocation = Vector3Int.zero;
            bool valid = false;
            while(!valid) {
                // pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4)
                randomLocation = new Vector3Int(pseudoRandomForPlants.Next(3, layout.levelSize - 4), pseudoRandomForPlants.Next(3, layout.levelSize - 4), 0);
                if(CheckLocation(randomLocation.x, randomLocation.y) && !occupiedCoordinates.Contains(randomLocation)) {
                    valid = true;
                }
            }
            occupiedCoordinates.Add(randomLocation);
            stalagmite.transform.position = GetPreciseLocation(randomLocation.x, randomLocation.y);
            stalagmite.transform.rotation = Quaternion.identity;
            stalagmite.SetActive(true);
            activeStalagmites.Add(stalagmite);
        }
    }
    private void ClearStalagmites() {
        for(int i = 0; i < activeStalagmites.Count; i++) {
            activeStalagmites[i].SetActive(false);
        }
        activeStalagmites.Clear();
    }
    private void GenerateCrystals() {
        for(int i = 0; i < crystalCount; i++) {
            int biomeIndex = layout.biomeIndex;
            int crystalIndex;
            if(biomeIndex == 0) {
                return;
            }
            else if(biomeIndex == 1) {
                crystalIndex = pseudoRandomForPlants.Next(1, 4);
            }
            else if(biomeIndex == 2) {
                crystalIndex = pseudoRandomForPlants.Next(3, 6);
            }
            else if(biomeIndex == 3) {
                crystalIndex = pseudoRandomForPlants.Next(0, 4);
                if(crystalIndex == 2) {
                    crystalIndex = 0;
                }
            }
            else {
                crystalIndex = pseudoRandomForPlants.Next(0, crystals.Length);
            }            
            GameObject crystal = ObjectPooler.objectPooler.GetPooledObject(crystals[crystalIndex].name);
            Vector3Int randomLocation = Vector3Int.zero;
            bool valid = false;
            while(!valid) {
                // pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4)
                randomLocation = new Vector3Int(pseudoRandomForPlants.Next(3, layout.levelSize - 4), pseudoRandomForPlants.Next(3, layout.levelSize - 4), 0);
                if(CheckLocation(randomLocation.x, randomLocation.y) && !occupiedCoordinates.Contains(randomLocation)) {
                    valid = true;
                }
            }
            occupiedCoordinates.Add(randomLocation);
            crystal.transform.position = GetPreciseLocation(randomLocation.x, randomLocation.y);
            crystal.transform.rotation = Quaternion.identity;
            crystal.SetActive(true);
            activeCrystals.Add(crystal);
        }        
    }
    private void ClearCrystals() {        
        for(int i = 0; i < activeCrystals.Count; i++) {
            activeCrystals[i].SetActive(false);
        }
        activeCrystals.Clear();
    }
    private void ClearStartPosition() {
        for(int x = layout.levelSize / 2 - 4; x < layout.levelSize / 2 + 4; x++) {
            for(int y = layout.levelSize / 2 - 4; y < layout.levelSize / 2 + 4; y++) {
                map[x, y] = 0;
            }
        }
    }
    private void GenerateTorches() {
        for(int i = 0; i < torchCount; i++) {
            Vector3Int torchPosition = GetRandomWallCoordinates();
            GameObject torchClone = ObjectPooler.objectPooler.GetPooledObject("CaveTorch");
            torchClone.transform.position = torchPosition;
            torchClone.transform.rotation = Quaternion.identity;
            torchClone.SetActive(true);
            torches.Add(torchClone);
        }
    }
    private void ClearTorches() {
        for(int i = 0; i < torches.Count; i++) {
            torches[i].SetActive(false);
        }
        torches.Clear();
    }
    private Vector3Int GetRandomWallCoordinates() {
        return wallCoordinates[pseudoRandomForTorches.Next(0, wallCoordinates.Count)];
    }
    public void UnloadLevel() {
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                //map[x, y] = 0;
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
                worldGeneration.tilemap.SetTile(tileCoordinate, null);
                worldGeneration.groundTilemap.SetTile(tileCoordinate, null);
            }
        }
        chestGeneration.ClearChests();
        mushroomGeneration.ClearMushrooms();
        ClearTorches();
        ClearCrystals();
        ClearStalagmites();
        poolGeneration.ClearPools();
        occupiedCoordinates.Clear();
        gameObject.SetActive(false);
    }    
    private static int ConvertTileIdToTilesetIndex(int id) {
        if(new[] { 7, 15, 39, 47, 135, 143, 167, 175 }.Contains(id)) {
            return 0;
        }
        else if(new[] { 62, 60, 30, 28, 156, 158, 188, 190 }.Contains(id)) {
            return 1;
        }
        else if(new[] { 31, 95, 191, 159, 63 }.Contains(id)) {
            return 2;
        }
        else if(new[] { 112, 114, 120, 122, 240, 242, 248, 250 }.Contains(id)) {
            return 3;
        }
        else if(new[] { 119 }.Contains(id)) {
            return 4;
        }
        else if(new[] { 124, 126, 252, 254 }.Contains(id)) {
            return 5;
        }
        else if(new[] { 127 }.Contains(id)) {
            return 6;
        }
        else if(new[] { 193, 195, 201, 203, 225, 227, 233, 235 }.Contains(id)) {
            return 7;
        }
        else if(new[] { 207, 231, 199, 239 }.Contains(id)) {
            return 8;
        }
        else if(new[] { 221, 215 }.Contains(id)) {
            return 9;
        }
        else if(new[] { 223 }.Contains(id)) {
            return 10;
        }
        else if(new[] { 241, 243, 249, 251 }.Contains(id)) {
            return 11;
        }
        else if(new[] { 247 }.Contains(id)) {
            return 12;
        }
        else if(new[] { 253 }.Contains(id)) {
            return 13;
        }
        else if(new[] { 255 }.Contains(id)) {
            return 14;
        }
        else {
            //Debug.Log($"Tile formation haven't been specified: {id}");
            return -1;
        }
    }
    // Returns the coordinates of a valid location in the level
    public Vector3Int GetRandomLocation() {
        bool valid = false;
        Vector3Int location = Vector3Int.zero;
        // System.Random pseudoRandomForLevel = new System.Random(seed.GetHashCode());
        while(!valid) {
            location = new Vector3Int(pseudoRandomForLevel.Next(3, layout.levelSize - 4), pseudoRandomForLevel.Next(3, layout.levelSize - 4), 0);
            if(map[location.x, location.y] == 0) {
                valid = true;
            }
        }
        location = GetPreciseLocation(location.x, location.y);
        //location = new Vector3Int(
        //           (location.x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
        //           (location.y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
        return location;
    }
    public bool CheckLocation(int x, int y) {
        return map[x, y] == 0;
    }
    public Vector3Int GetPreciseLocation(int x, int y) {
        Vector3Int tileCoordinate = new Vector3Int(
                        (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                        (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
        return tileCoordinate;
    }
    public bool IsValidLocation(Vector2Int coordinates) {
        bool valid = map[coordinates.x, coordinates.y] == 0;
        return valid;
    }
    // Fills the background of the level
    private void FillBackground() {
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
                if(worldGeneration.tilemap.GetTile(tileCoordinate) != tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]) {
                    
                    switch(worldGeneration.WorldMap[layout.worldCoordinates.x, layout.worldCoordinates.y]) {
                        case 1:
                            worldGeneration.groundTilemap.SetTile(tileCoordinate, tileDatabase.groundTiles[pseudoRandomForGround.Next(0, tileDatabase.groundTiles.Length)]);
                            break;
                        case 2:
                            worldGeneration.groundTilemap.SetTile(tileCoordinate, tileDatabase.volcanicTiles[pseudoRandomForGround.Next(0, tileDatabase.volcanicTiles.Length)]);
                            break;
                        case 3:
                            worldGeneration.groundTilemap.SetTile(tileCoordinate, tileDatabase.glacialTiles[pseudoRandomForGround.Next(0, tileDatabase.glacialTiles.Length)]);
                            break;
                        case 4:
                            worldGeneration.groundTilemap.SetTile(tileCoordinate, tileDatabase.crystalTiles[pseudoRandomForGround.Next(0, tileDatabase.crystalTiles.Length)]);
                            break;
                        default:
                            break;
                    }
                }
                else {
                    worldGeneration.groundTilemap.SetTile(tileCoordinate, null);
                }
            }
        }
    }
    // Fill the cells with random values determined by fillPercent value
    private void RandomFillMap() {
        if(layout.useRandomSeed) {
            layout.seed = Time.time.ToString();
        }
        // To get the same randomization with the same seed
        System.Random pseudoRandom = new System.Random(layout.seed.GetHashCode());
        // Fill the edges of the map with wall tiles
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                /*
                if(new int[] { 30, 31, 32, 33 }.Contains(x) && 
                    new int[] { 0, 1, 2, 3, 63, 62, 61, 60 }.Contains(y) ||
                        new int[] { 30, 31, 32, 33 }.Contains(y) && 
                        new int[] { 0, 1, 2, 3, 63, 62, 61, 60 }.Contains(x)) {
                    if(layout.worldCoordinates.x == 0) {
                    
                    }
                    map[x, y] = 0;
                    reservedForConnectionsMap[x, y] = 1;
                }*/
                // Left
                int middlePoint = layout.levelSize / 2;
                int topPoint = layout.levelSize;
                if(new[] { middlePoint - 2, middlePoint - 1, middlePoint, middlePoint + 1 }.Contains(y) &&
                        new[] { 0, 1, 2, 3 }.Contains(x)) {
                    if(layout.worldCoordinates.x == 0 && !connections.left) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(!connections.left) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(connections.left) {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                // Right
                else if(new[] { middlePoint - 2, middlePoint - 1, middlePoint, middlePoint + 1 }.Contains(y) &&
                        new[] { topPoint - 4, topPoint - 3, topPoint - 2, topPoint - 1 }.Contains(x)) {
                    if(layout.worldCoordinates.x == layout.worldSize - 1 && !connections.right) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(!connections.right) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(connections.right) {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                // Bottom
                else if(new[] { middlePoint - 2, middlePoint - 1, middlePoint, middlePoint + 1 }.Contains(x) &&
                        new[] { 0, 1, 2, 3 }.Contains(y)) {
                    if(layout.worldCoordinates.y == 0 && !connections.bottom) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(!connections.bottom) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(connections.bottom) {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                // Top
                else if(new[] { middlePoint - 2, middlePoint - 1, middlePoint, middlePoint + 1 }.Contains(x) &&
                        new[] { topPoint - 4, topPoint - 3, topPoint - 2, topPoint - 1 }.Contains(y)) {
                    if(layout.worldCoordinates.y == layout.worldSize - 1 && !connections.top) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(!connections.top) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else if(connections.top) {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                else if(x < 5 || x > layout.levelSize - 6 || y < 5 || y > layout.levelSize - 6) {
                    map[x, y] = 1;
                    reservedForConnectionsMap[x, y] = 0;
                }
                else {
                    map[x, y] = (pseudoRandom.Next(0, 100) < layout.fillPercent) ? 1 : 0;
                    reservedForConnectionsMap[x, y] = 0;
                }
            }
        }
    }
    // Smooth edges and get rid of noise
    private void SmoothMap() {
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if(reservedForConnectionsMap[x, y] == 1) {
                    continue;
                }
                if(neighbourWallTiles > 4) {
                    map[x, y] = 1;
                }
                else if(neighbourWallTiles < 4) {
                    map[x, y] = 0;
                }
            }
        }
    }
    // Draw map with sprites
    private TileBase[] DrawMap() {
        TileBase[] tiles = new TileBase[layout.levelSize * layout.levelSize];
        //int[] linearMap = new int[layout.levelSize * layout.levelSize];
        int index = 0;
        //Vector3Int bottomLeft = new Vector3Int((-layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize, (-layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
        //Vector3Int topRight = new Vector3Int((layout.levelSize-layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize, (layout.levelSize-layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
        //BoundsInt bounds = new BoundsInt(bottomLeft, topRight);
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                /*
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);*/
                //linearMap[index] = map[x, y];
                /*
                if(linearMap[index] == 0) {
                    tiles[index] = null;
                }
                else if(linearMap[index] == 1) {
                    tiles[index] = tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1];
                }
                else {
                    tiles[index] = null;
                }
                */
                if(map[x, y] == 0) {
                    tiles[index] = null;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, null);                    
                }
                else if(map[x, y] == 1) {
                    tiles[index] = tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1];
                    //worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);                    
                }
                else {
                    tiles[index] = null;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, null);                    
                }                
                index++;
            }
        }
        return tiles;
        //worldGeneration.tilemap.SetTilesBlock(bounds, tiles);
    }
    private void SetTiles(TileBase[] tiles) {
        /*
        var thread = new Thread(() => {
            // example: expensive computation of camera position

            Dispatcher.InvokeAsync(() => {
                // I'm in the main thread
                //Camera.main.transform.position = position;
                int index = 0;
                for(int x = 0; x < layout.levelSize; x++) {
                    for(int y = 0; y < layout.levelSize; y++) {
                        Vector3Int tileCoordinate = new Vector3Int(
                            (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                            (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);

                        worldGeneration.tilemap.SetTile(tileCoordinate, tiles[index]);
                        index++;
                    }
                }
            });
        });
        thread.Start();
        thread.Join();
        */        
        int index = 0;
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);

                worldGeneration.tilemap.SetTile(tileCoordinate, tiles[index]);
                index++;
            }
        }        
    }
    // Add corner and side tiles
    private void AddSides() {
        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                // Fill corners according to occupied spaces
                if(map[x, y] == 1 && GetSurroundingWallCount(x, y) < 8) {
                    Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (y - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);
                    int wallSideId = 0;
                    // Find other walls around the wall
                    try {
                        if(map[x, y + 1] == 1) {
                            wallSideId += 64;
                        }
                        if(map[x, y - 1] == 1) {
                            wallSideId += 4;
                        }
                        if(map[x - 1, y] == 1) {
                            wallSideId += 1;
                        }
                        if(map[x + 1, y] == 1) {
                            wallSideId += 16;
                        }
                        if(map[x - 1, y + 1] == 1) {
                            wallSideId += 128;
                        }
                        if(map[x + 1, y + 1] == 1) {
                            wallSideId += 32;
                        }
                        if(map[x + 1, y - 1] == 1) {
                            wallSideId += 8;
                        }
                        if(map[x - 1, y - 1] == 1) {
                            wallSideId += 2;
                        }
                    }
                    catch(IndexOutOfRangeException) {
                        if(y + 1 == layout.levelSize && x == layout.levelSize / 2 - 3) {
                            // 8
                            int id = 8;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y + 1 == layout.levelSize && x == layout.levelSize / 2 + 2) {
                            // 5
                            int id = 5;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y - 1 == -1 && x == layout.levelSize / 2 - 3) {
                            // 8
                            int id = 8;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y - 1 == -1 && x == layout.levelSize / 2 + 2) {
                            // 5
                            int id = 5;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x + 1 == layout.levelSize && y == layout.levelSize / 2 - 3) {
                            // 2
                            int id = 2;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x + 1 == layout.levelSize && y == layout.levelSize / 2 + 2) {
                            // 11
                            int id = 11;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x - 1 == -1 && y == layout.levelSize / 2 - 3) {
                            // 2
                            int id = 2;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x - 1 == -1 && y == layout.levelSize / 2 + 2) {
                            // 11
                            int id = 11;
                            worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        continue;
                    }
                    if(x == 0 || x == layout.levelSize - 1 || y == 0 || y == layout.levelSize - 1) {
                        // tilemap.SetTile(tileCoordinate, wallTiles[15]);
                        worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
                        // Debug.Log("The tile " + x +", "+ y + " is on the side so it is setting to tile 15");
                    }
                    else {
                        int id = ConvertTileIdToTilesetIndex(wallSideId);
                        if(id < 0) {
                            map[x, y] = 0;
                            worldGeneration.tilemap.SetTile(tileCoordinate, null);
                            continue;
                        }
                        else if(id != 14) {
                            wallCoordinates.Add(tileCoordinate);
                        }
                        if(generateDebugNumbersForWalls) {
                            //debugTilemap.SetTile(tileCoordinate, tileDatabase.debugNumbers[id]);
                        }
                        worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                    }
                }
            }
        }
    }
    // Get surrounding wall count of a cell
    private int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if(IsInMapRange(neighbourX, neighbourY)) {
                    if(neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    /*
    private int GetSurroundingGrassCount(int gridX, int gridY, int grassLevel) {
        int grassCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if(IsInMapRange(neighbourX, neighbourY)) {
                    if(neighbourX != gridX || neighbourY != gridY) {
                        int surroundingGrass = 0;
                        if(grassMap[neighbourX, neighbourY] == grassLevel) {
                            surroundingGrass++;
                        }
                        grassCount += surroundingGrass;
                    }
                }
            }
        }
        return grassCount;
    }*/
    // Process map
    private void ProcessMap() {
        List<List<Coordinate>> wallRegions = GetRegions(1);
        List<List<Coordinate>> groundRegions = GetRegions(0);
        List<Room> survivingRooms = new List<Room>();
        // Clear unneccessary walls
        foreach(List<Coordinate> wallRegion in wallRegions) {
            if(wallRegion.Count < layout.wallThresholdSize) {
                foreach(Coordinate tile in wallRegion) {
                    /*Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (tile.tileY - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);*/
                    map[tile.tileX, tile.tileY] = 0;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, null);
                }
            }
        }
        // Clear unneccessary rooms
        foreach(List<Coordinate> groundRegion in groundRegions) {
            if(groundRegion.Count < layout.groundThresholdSize) {
                foreach(Coordinate tile in groundRegion) {
                    /*Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.levelSize / 2) + layout.worldCoordinates.x * layout.levelSize,
                    (tile.tileY - layout.levelSize / 2) + layout.worldCoordinates.y * layout.levelSize, 0);*/
                    map[tile.tileX, tile.tileY] = 1;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
                }
            }
            else {
                survivingRooms.Add(new Room(groundRegion, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        if(layout.connectRooms) {
            ConnectClosestRooms(survivingRooms);
        }
    }
    // Search and connect the closest rooms
    private void ConnectClosestRooms(List<Room> allRooms, bool forceAccessToMainRoom = false) {
        // Room lists
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();
        // To compare distances
        float bestDistance = 0;
        Coordinate bestTileA = new Coordinate();
        Coordinate bestTileB = new Coordinate();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;
        //
        if(forceAccessToMainRoom) {
            foreach(Room room in allRooms) {
                if(room.isAccessibleFromMainRoom) {
                    roomListB.Add(room);
                }
                else {
                    roomListA.Add(room);
                }
            }
        }
        else {
            roomListA = allRooms;
            roomListB = allRooms;
        }
        // Look for closest distances between rooms
        foreach(Room roomA in roomListA) {
            if(!forceAccessToMainRoom) {
                possibleConnectionFound = false;
                if(roomA.connectedRooms.Count > 0) {
                    continue;
                }
            }
            foreach(Room roomB in roomListB) {
                if(roomA == roomB || roomA.IsConnected(roomB)) {
                    continue;
                }
                for(int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
                    for(int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
                        Coordinate tileA = roomA.edgeTiles[tileIndexA];
                        Coordinate tileB = roomB.edgeTiles[tileIndexB];

                        float distanceBetweenRooms = Mathf.Pow(tileA.tileX - tileB.tileX, 2)
                            + Mathf.Pow(tileA.tileY - tileB.tileY, 2);

                        if(distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
                            possibleConnectionFound = true;
                            bestDistance = distanceBetweenRooms;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if(possibleConnectionFound && !forceAccessToMainRoom) {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        if(possibleConnectionFound && forceAccessToMainRoom) {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }
        if(!forceAccessToMainRoom) {
            ConnectClosestRooms(allRooms, true);
        }
    }
    // Create a passage between two rooms
    private void CreatePassage(Room roomA, Room roomB, Coordinate tileA, Coordinate tileB) {
        Room.ConnectRooms(roomA, roomB);
        // Debug.DrawLine(ConvertToWorldCoordinates(tileA), ConvertToWorldCoordinates(tileB), Color.green, 100);

        List<Coordinate> line = GetLine(tileA, tileB);
        foreach(Coordinate c in line) {
            DrawCircle(c, layout.passageRadius);
        }
    }
    // Circle around the line
    private void DrawCircle(Coordinate center, int radius) {
        for(int x = -radius; x <= radius; x++) {
            for(int y = -radius; y <= radius; y++) {
                if(x * x + y * y <= radius * radius) {
                    int realX = center.tileX + x;
                    int realY = center.tileY + y;
                    if(IsInMapRange(realX, realY)) {
                        map[realX, realY] = 0;
                    }
                }
            }
        }
    }
    // Get the line from one room to another
    private List<Coordinate> GetLine(Coordinate from, Coordinate to) {
        List<Coordinate> line = new List<Coordinate>();
        // Line data
        int x = from.tileX;
        int y = from.tileY;
        // Change in variables
        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;
        // Signs
        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);
        // Absolutes
        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);
        // If the line is inverted
        if(longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);
            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }
        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++) {
            line.Add(new Coordinate(x, y));
            if(inverted) {
                y += step;
            }
            else {
                x += step;
            }
            gradientAccumulation += shortest;
            if(gradientAccumulation > longest) {
                if(inverted) {
                    x += gradientStep;
                }
                else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }
    // Get regions in the map
    private List<Coordinate> GetRegionTiles(int startX, int startY) {

        List<Coordinate> tiles = new List<Coordinate>();
        int[,] mapFlags = new int[layout.levelSize, layout.levelSize];
        int tileType = map[startX, startY];

        Queue<Coordinate> queue = new Queue<Coordinate>();
        queue.Enqueue(new Coordinate(startX, startY));
        mapFlags[startX, startY] = 1;

        while(queue.Count > 0) {
            Coordinate tile = queue.Dequeue();
            tiles.Add(tile);

            for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for(int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                    if(IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)) {
                        if(mapFlags[x, y] == 0 && map[x, y] == tileType) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coordinate(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }
    // Get all regions
    private List<List<Coordinate>> GetRegions(int tileType) {

        List<List<Coordinate>> regions = new List<List<Coordinate>>();
        int[,] mapFlags = new int[layout.levelSize, layout.levelSize];

        for(int x = 0; x < layout.levelSize; x++) {
            for(int y = 0; y < layout.levelSize; y++) {
                if(mapFlags[x, y] == 0 && map[x, y] == tileType) {
                    List<Coordinate> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach(Coordinate tile in newRegion) {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }
    // Check if the coordinate is in the range of the specified map range
    private bool IsInMapRange(int x, int y) {
        return x >= 0 && x < layout.levelSize && y >= 0 && y < layout.levelSize;
    }
    // Convert map coordinates to world coordinates
    private Vector3Int ConvertToWorldCoordinates(Coordinate tile) {
        return new Vector3Int(-layout.levelSize / 2 + tile.tileX, -layout.levelSize / 2 + tile.tileY, 0);
    }
    private Coordinate ConvertToCoordinates(Vector3Int pos) {
        return new Coordinate(pos.x + layout.levelSize / 2, pos.y + layout.levelSize / 2);
    }
    // Struct of a coordinate
    private struct Coordinate {
        public int tileX;
        public int tileY;

        public Coordinate(int x, int y) {
            tileX = x;
            tileY = y;
        }
    }
    // Room class that helps connecting rooms
    private class Room : IComparable<Room> {

        public List<Coordinate> tiles;
        public List<Coordinate> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;
        // Default constructor
        public Room() {

        }
        // Constructor
        public Room(List<Coordinate> roomTiles, int[,] map) {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();
            edgeTiles = new List<Coordinate>();

            foreach(Coordinate tile in tiles) {
                for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                    for(int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                        if(x == tile.tileX || y == tile.tileY) {
                            try {
                                if(map[x, y] == 1) {
                                    edgeTiles.Add(tile);
                                    break;
                                }
                            }
                            catch(IndexOutOfRangeException) {
                                continue;
                            }
                        }
                    }
                }
            }
        }
        public void SetAccesibleFromMainRoom() {
            if(!isAccessibleFromMainRoom) {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms) {
                    connectedRoom.SetAccesibleFromMainRoom();
                }
            }
        }
        // Add two rooms to their own connectedRooms list
        public static void ConnectRooms(Room roomA, Room roomB) {
            if(roomA.isAccessibleFromMainRoom) {
                roomB.SetAccesibleFromMainRoom();
            }
            else if(roomB.isAccessibleFromMainRoom) {
                roomA.SetAccesibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }
        // Check if a room is connected to another room
        public bool IsConnected(Room otherRoom) {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}

public struct Connections {
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;
}
