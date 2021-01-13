using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;

public class LevelGeneration : MonoBehaviour {

    public bool generateDebugNumbersForWalls;
    // If the rooms should be connected
    public LevelLayout layout;
    // Tile database
    public TileDatabase tileDatabase;
    // Chest game object
    [Header("Tilemaps")]
    private Tilemap groundTilemap;
    private Tilemap tilemap;
    private Tilemap debugTilemap;
    private Tilemap plantTilemap;
    // A star path instance to scan walls
    private AstarPath path;
    // Two dimensional map array
    private int[,] map;
    private int[,] reservedForConnectionsMap;
    private int[,] plantMap;
    // Psuedo random number for generating chests and enemies
    System.Random pseudoRandomForLevel;
    System.Random pseudoRandomForGround;
    System.Random pseudoRandomForWalls;
    System.Random pseudoRandomForPlants;
    // A reference to world generation script
    private WorldGeneration worldGeneration;

    private void Awake() {
        worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    private void Start() {
        //path = transform.GetChild(0).GetComponent<AstarPath>();
        //SetLayout(layout);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            worldGeneration.ChangeCurrentCoordinates(layout.worldCoordinates);
        }
    }
    private IEnumerator ScanPath() {
        yield return new WaitForSeconds(0.2f);
        if(path) {
            path.Scan();
        }
    }
    public async Task SetLayoutAsync() {
        pseudoRandomForPlants = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForWalls = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForLevel = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForGround = new System.Random(this.layout.seed.GetHashCode());
        if(this.layout.generateWalls) {
            await Task.Run(() => GenerateMap());
        }
    }
    public void SetLayout(LevelLayout layout) {
        this.layout = layout;
        pseudoRandomForPlants = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForWalls = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForLevel = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForGround = new System.Random(this.layout.seed.GetHashCode());
        if(this.layout.generateWalls) {
            GenerateMap();
        }
    }
    public void SetLayout() {
        pseudoRandomForPlants = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForWalls = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForLevel = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForGround = new System.Random(this.layout.seed.GetHashCode());
        if(layout.generateWalls) {
            GenerateMap();
        }
    }
    public IEnumerator SetLayoutCoroutine() {
        pseudoRandomForPlants = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForWalls = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForLevel = new System.Random(this.layout.seed.GetHashCode());
        pseudoRandomForGround = new System.Random(this.layout.seed.GetHashCode());        
        if(layout.generateWalls) {
            GenerateMap();
        }
        yield return null;
    }
    // Generate random map
    private void GenerateMap() {
        if(!groundTilemap) {
            groundTilemap = GameObject.FindWithTag("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        }
        if(!tilemap) {
            tilemap = GameObject.FindWithTag("Grid").transform.GetChild(1).GetComponent<Tilemap>();
        }
        /*
        if(!plantTilemap) {
            plantTilemap = GameObject.FindWithTag("Grid").transform.GetChild(3).GetComponent<Tilemap>();
        }
        if(!debugTilemap) {
            debugTilemap = GameObject.FindWithTag("Grid").transform.GetChild(4).GetComponent<Tilemap>();
        }
        */
        map = new int[layout.width, layout.height];
        reservedForConnectionsMap = new int[layout.width, layout.height];
        if(layout.generatePlants) {
            plantMap = new int[layout.width, layout.height];
        }
        RandomFillMap();        
        for(int i = 0; i < layout.smoothLevel; i++) {
            SmoothMap();            
        }
        ProcessMap();
        //StartCoroutine(ProcessMapCoroutine());
        DrawMap();
        FillBackground();
        if(gameObject.activeInHierarchy) {
            //StartCoroutine(ScanPath());
        }        
    }
    private int ConvertTileIdToTilesetIndex(int id) {
        if(new int[] { 7, 15, 39, 47, 135, 143, 167, 175 }.Contains(id)) {
            return 0;
        }
        else if(new int[] { 62, 60, 30, 28, 156, 158, 188, 190 }.Contains(id)) {
            return 1;
        }
        else if(new int[] { 31, 95, 191, 159, 63 }.Contains(id)) {
            return 2;
        }
        else if(new int[] { 112, 114, 120, 122, 240, 242, 248, 250 }.Contains(id)) {
            return 3;
        }
        else if(new int[] { 119 }.Contains(id)) {
            return 4;
        }
        else if(new int[] { 124, 126, 252, 254 }.Contains(id)) {
            return 5;
        }
        else if(new int[] { 127 }.Contains(id)) {
            return 6;
        }
        else if(new int[] { 193, 195, 201, 203, 225, 227, 233, 235 }.Contains(id)) {
            return 7;
        }
        else if(new int[] { 207, 231, 199, 239 }.Contains(id)) {
            return 8;
        }
        else if(new int[] { 221, 215 }.Contains(id)) {
            return 9;
        }
        else if(new int[] { 223 }.Contains(id)) {
            return 10;
        }
        else if(new int[] { 241, 243, 249, 251 }.Contains(id)) {
            return 11;
        }
        else if(new int[] { 247 }.Contains(id)) {
            return 12;
        }
        else if(new int[] { 253 }.Contains(id)) {
            return 13;
        }
        else if(new int[] { 255 }.Contains(id)) {
            return 14;
        }
        else {
            Debug.Log($"Tile formation haven't been specified: {id}");
            return -1;
        }
    }
    public Vector3Int GetRandomLocation() {
        bool valid = false;
        Vector3Int location = Vector3Int.zero;
        // System.Random pseudoRandomForLevel = new System.Random(seed.GetHashCode());
        while(!valid) {
            location = new Vector3Int(pseudoRandomForLevel.Next(3, 60), pseudoRandomForLevel.Next(3, 60), 0);
            if(map[location.x, location.y] == 0) {
                valid = true;
            }
        }
        location = new Vector3Int(
                    (location.x - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (location.y - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
        return location;
    }
    private void FillBackground() {
        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (y - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
                if(tilemap.GetTile(tileCoordinate) != tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]) {
                    groundTilemap.SetTile(tileCoordinate, tileDatabase.groundTiles[pseudoRandomForGround.Next(0, 9)]);
                }
                else {
                    groundTilemap.SetTile(tileCoordinate, null);
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
        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
                // TODO: fix level connectivity
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
                if(new int[] { 30, 31, 32, 33 }.Contains(y) &&
                        new int[] { 0, 1, 2, 3 }.Contains(x)) {
                    if(layout.worldCoordinates.x == 0) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                else if(new int[] { 30, 31, 32, 33 }.Contains(y) &&
                        new int[] { 60, 61, 62, 63 }.Contains(x)) {
                    if(layout.worldCoordinates.x == layout.worldSize - 1) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                else if(new int[] { 30, 31, 32, 33 }.Contains(x) &&
                        new int[] { 0, 1, 2, 3 }.Contains(y)) {
                    if(layout.worldCoordinates.y == 0) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                else if(new int[] { 30, 31, 32, 33 }.Contains(x) &&
                        new int[] { 60, 61, 62, 63 }.Contains(y)) {
                    if(layout.worldCoordinates.y == layout.worldSize - 1) {
                        map[x, y] = 1;
                        reservedForConnectionsMap[x, y] = 0;
                    }
                    else {
                        map[x, y] = 0;
                        reservedForConnectionsMap[x, y] = 1;
                    }
                }
                else if(x < 5 || x > layout.width - 6 || y < 5 || y > layout.height - 6) {
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
        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
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
    private void DrawMap() {
        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
                Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (y - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
                if(map[x, y] == 0) {
                    // tilemap.SetTile(tileCoordinate, tiles[0]);
                    tilemap.SetTile(tileCoordinate, null);
                }
                else if(map[x, y] == 1) {
                    // tilemap.SetTile(tileCoordinate, wallTiles[15]);
                    // tilemap.SetTile(tileCoordinate, tileDatabase.jungleWalls[tileDatabase.jungleWalls.Count - 1]);
                    tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
                }
                else {
                    tilemap.SetTile(tileCoordinate, null);
                }
            }
        }
        if(layout.marchingSquares) {
            for(int i = 0; i < layout.addSideCount; i++) {
                AddSides();
            }
        }
    }
    // Add corner and side tiles
    private void AddSides() {
        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
                // Fill corners according to occupied spaces
                if(map[x, y] == 1 && GetSurroundingWallCount(x, y) < 8) {
                    Vector3Int tileCoordinate = new Vector3Int(
                    (x - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (y - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
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
                        // This block is to ignore out of index placements of tiles
                        // Do not touch!
                        if(y + 1 == 64 && x == 29) {
                            // 8
                            int id = 8;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y + 1 == 64 && x == 34) {
                            // 5
                            int id = 5;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y - 1 == -1 && x == 29) {
                            // 8
                            int id = 8;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(y - 1 == -1 && x == 34) {
                            // 5
                            int id = 5;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x + 1 == 64 && y == 29) {
                            // 2
                            int id = 2;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x + 1 == 64 && y == 34) {
                            // 11
                            int id = 11;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x - 1 == -1 && y == 29) {
                            // 2
                            int id = 2;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        else if(x - 1 == -1 && y == 34) {
                            // 11
                            int id = 11;
                            tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
                        }
                        continue;
                    }
                    if(x == 0 || x == layout.width - 1 || y == 0 || y == layout.height - 1) {
                        // tilemap.SetTile(tileCoordinate, wallTiles[15]);
                        tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
                        // Debug.Log("The tile " + x +", "+ y + " is on the side so it is setting to tile 15");
                    }
                    else {
                        int id = ConvertTileIdToTilesetIndex(wallSideId);
                        if(id < 0) {
                            map[x, y] = 0;
                            tilemap.SetTile(tileCoordinate, null);
                            continue;
                        }
                        if(generateDebugNumbersForWalls) {
                            //debugTilemap.SetTile(tileCoordinate, tileDatabase.debugNumbers[id]);
                        }
                        tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[id]);
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
                    Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (tile.tileY - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
                    map[tile.tileX, tile.tileY] = 0;
                    tilemap.SetTile(tileCoordinate, null);
                }
            }
        }
        // Clear unneccessary rooms
        foreach(List<Coordinate> groundRegion in groundRegions) {
            if(groundRegion.Count < layout.groundThresholdSize) {
                foreach(Coordinate tile in groundRegion) {
                    Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (tile.tileY - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);
                    map[tile.tileX, tile.tileY] = 1;
                    tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
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
        int[,] mapFlags = new int[layout.width, layout.height];
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
        int[,] mapFlags = new int[layout.width, layout.height];

        for(int x = 0; x < layout.width; x++) {
            for(int y = 0; y < layout.height; y++) {
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
        return x >= 0 && x < layout.width && y >= 0 && y < layout.height;
    }
    // Convert map coordinates to world coordinates
    private Vector3Int ConvertToWorldCoordinates(Coordinate tile) {
        return new Vector3Int(-layout.width / 2 + tile.tileX, -layout.height / 2 + tile.tileY, 0);
    }
    private Coordinate ConvertToCoordinates(Vector3Int pos) {
        return new Coordinate(pos.x + layout.width / 2, pos.y + layout.height / 2);
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