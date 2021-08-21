using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using System;

public class CharacterCreation : MonoBehaviour {

    public Image skinImage;
    public Image hairImage;

    public CharacterAppearance appearance;

    public TextMeshProUGUI skinColorText;
    public TextMeshProUGUI hairColorText;
    public TextMeshProUGUI hairStyleText;
    public TextMeshProUGUI confirmText;

    private Color currentSkinColor;
    private Color currentHairColor;
    private Sprite currentHairStyle;

    private int currentSkinColorIndex = 0;
    private int currentHairColorIndex = 0;
    private int currentHairStyleIndex = 0;

    private Player player;
    private LocalizationManager localizationManager;

    private WorldData world;

    private bool newGameButtonPressed = false;

    private readonly int worldSize = 128;

    private void Awake() {
        localizationManager = FindObjectOfType<LocalizationManager>();
        player = FindObjectOfType<Player>();
        currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        currentHairColor = appearance.hairColors[currentHairColorIndex];
        currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        skinImage.color = currentSkinColor;
        hairImage.color = currentHairColor;
        hairImage.sprite = currentHairStyle;
    }
    private void Start() {
        RefreshTexts();
        Debug.Log(localizationManager.GetLanguage());
    }
    public async void CompleteCharacterCreation() {
        if(!newGameButtonPressed) {
            newGameButtonPressed = true;
            await SetNewGameData();
            player.SavePlayer();
            LoadSceneAsync("Levels");
        }        
    }
    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void ChangeSkinColor(bool increment) {
        if(increment) {
            currentSkinColorIndex++;
            if(currentSkinColorIndex > appearance.skinColors.Length - 1) {
                currentSkinColorIndex = 0;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        else {
            currentSkinColorIndex--;
            if(currentSkinColorIndex < 0) {
                currentSkinColorIndex = appearance.skinColors.Length - 1;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        skinImage.color = currentSkinColor;
        player.skinColorIndex = currentSkinColorIndex;
    }
    public void ChangeHairColor(bool increment) {
        if(increment) {
            currentHairColorIndex++;
            if(currentHairColorIndex > appearance.hairColors.Length - 1) {
                currentHairColorIndex = 0;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        else {
            currentHairColorIndex--;
            if(currentHairColorIndex < 0) {
                currentHairColorIndex = appearance.hairColors.Length - 1;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        hairImage.color = currentHairColor;
        player.hairColorIndex = currentHairColorIndex;
    }
    public void ChangeHairStyle(bool increment) {
        if(increment) {
            currentHairStyleIndex++;
            if(currentHairStyleIndex > appearance.hairStyles.Length - 1) {
                currentHairStyleIndex = 0;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        else {
            currentHairStyleIndex--;
            if(currentHairStyleIndex < 0) {
                currentHairStyleIndex = appearance.hairStyles.Length - 1;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        hairImage.sprite = currentHairStyle;
        player.hairStyleIndex = currentHairStyleIndex;
    }
    public void RefreshTexts() {
        skinColorText.text = localizationManager.GetText("skinColor");
        hairColorText.text = localizationManager.GetText("hairColor");
        hairStyleText.text = localizationManager.GetText("hairStyle");
        confirmText.text = localizationManager.GetText("complete");
    }
    private async Task SetNewGameData() {
        // World data
        world = new WorldData(new string[worldSize, worldSize], new[] { 0, 0 }, new int[worldSize, worldSize], UnityEngine.Random.Range(0, 99999999).ToString(), new int[worldSize, worldSize]);
        await GenerateNewWorld();
        SaveSystem.SaveWorld(world, PersistentData.saveSlot);
        if(!player || !player.skinColor || !player.hairColor || !player.hairStyle) {
            return;
        }
        // Appearance
        player.skinColor.color = currentSkinColor;
        player.hairColor.color = currentHairColor;
        player.hairStyle.sprite = currentHairStyle;
        player.skinColorIndex = currentSkinColorIndex;
        player.hairColorIndex = currentHairColorIndex;
        player.hairStyleIndex = currentHairStyleIndex;
        // Inventory and equipment
        for(int i = 0; i < player.inventory.InventorySize; i++) {
            player.inventory.inventory[i] = null;
        }
        for(int i = 0; i < player.inventory.EquipmentSize; i++) {
            player.inventory.equipment[i] = null;
        }
        // Transform
        player.transform.position = new Vector3(5, 0, 0);
        player.transform.rotation = Quaternion.identity;
        // Status
        player.stats.health = 100f;
        player.stats.mana = 100f;
        player.stats.energy = 100f;
        player.stats.maxHealth = 100f;
        player.stats.maxMana = 100f;
        player.stats.maxEnergy = 100f;
        // Main
        player.stats.strength = 10;
        player.stats.agility = 10;
        player.stats.dexterity = 10;
        player.stats.intelligence = 10;
        player.stats.faith = 10;
        player.stats.wisdom = 10;
        player.stats.vitality = 10;
        player.stats.charisma = 10;
        // Damages
        player.stats.bashDamage = 10f;
        player.stats.pierceDamage = 10f;
        player.stats.slashDamage = 10f;
        player.stats.fireDamage = 10f;
        player.stats.iceDamage = 10f;
        player.stats.lightningDamage = 10f;
        player.stats.airDamage = 10f;
        player.stats.earthDamage = 10f;
        player.stats.lightDamage = 10f;
        player.stats.darkDamage = 10f;
        player.stats.poisonDamage = 10f;
        player.stats.bleedDamage = 10f;
        player.stats.curseDamage = 10f;
        // Defences
        player.stats.bashDefence = 10f;
        player.stats.pierceDefence = 10f;
        player.stats.slashDefence = 10f;
        player.stats.fireDefence = 10f;
        player.stats.iceDefence = 10f;
        player.stats.lightningDefence = 10f;
        player.stats.airDefence = 10f;
        player.stats.earthDefence = 10f;
        player.stats.lightDefence = 10f;
        player.stats.darkDefence = 10f;
        player.stats.poisonDefence = 10f;
        player.stats.bleedDefence = 10f;
        player.stats.curseDefence = 10f;
        // Stat points
        player.stats.statPoints = 10;
        // Skills
        // Beginner skills
        player.skillUser.acquiredSkills = new List<Skill>();
        for(int i = 0; i < 6; i++) {
            player.skillUser.acquiredSkills.Add(player.skillUser.skillDatabase.skills[i]);
        }
        player.skillUser.currentSkills = new List<ActiveSkill>();
        for(int i = 0; i < 11; i++) {
            if(i < 6) {
                player.skillUser.currentSkills.Add(player.skillUser.skillDatabase.skills[i] as ActiveSkill);
            }
            else {
                player.skillUser.currentSkills.Add(null);
            }
        }
    }
    private async Task GenerateNewWorld() {
        int smoothLevel = 4;
        // Generate world map data
        await Task.Run(() => RandomFillWorld());
        for(int i = 0; i < smoothLevel; i++) {
            await Task.Run(() => SmoothMap());
        }
        for(int i = 0; i < 6; i++) {
            for(int j = 0; j < 6; j++) {
                world.worldMap[i, j] = 1;
            }
        }        
        await Task.Run(() => ProcessMap());        
    }
    private void RandomFillWorld() {
        world.worldMap = new int[worldSize, worldSize];
        // To get the same randomization with the same seed
        System.Random pseudoRandom = new System.Random(world.seed.GetHashCode());
        // Fill the edges of the map with wall tiles
        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                world.worldMap[x, y] = Mathf.Clamp(pseudoRandom.Next(-2, 5), 0, 4);
            }
        }
    }
    // Smooth edges and get rid of noise
    private void SmoothMap() {

        int eliminationThreshold = 3;

        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                int neighbourWallTiles = GetSurroundingBiomeCount(x, y);
                if(neighbourWallTiles > eliminationThreshold) {
                    world.worldMap[x, y] = GetSurroundingBiomeType(x, y);
                    // Get surrounding wall type here
                }
                else if(neighbourWallTiles < eliminationThreshold) {
                    world.worldMap[x, y] = 0;
                }
            }
        }
    }
    private int GetSurroundingBiomeCount(int gridX, int gridY) {
        int biomeCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if(IsInMapRange(neighbourX, neighbourY)) {
                    if(neighbourX != gridX || neighbourY != gridY) {                        
                        biomeCount += Mathf.Clamp(world.worldMap[neighbourX, neighbourY], 0, 1);
                        //biomeCount++;
                    }
                }
                else {
                    biomeCount++;
                }
            }
        }
        return biomeCount;
    }
    private int GetSurroundingBiomeType(int gridX, int gridY) {
        int biomeType = 0;
        int[] biomeTypeCounters = new int[5];
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if(IsInMapRange(neighbourX, neighbourY)) {
                    if(neighbourX != gridX || neighbourY != gridY && world.worldMap[neighbourX, neighbourY] != 0) {
                        biomeTypeCounters[world.worldMap[neighbourX, neighbourY]]++;
                    }
                }
            }
        }
        int smallest = 0;
        for(int i = 0; i < biomeTypeCounters.Length; i++) {
            if(biomeTypeCounters[i] > smallest) {
                biomeType = i;
                smallest = biomeTypeCounters[i];
            }
        }
        return biomeType;
    }
    private bool IsInMapRange(int x, int y) {
        return x >= 0 && x < worldSize && y >= 0 && y < worldSize;
    }
    // Process map
    private void ProcessMap() {
        List<List<Coordinate>> wallRegions = GetRegions(0);
        List<List<Coordinate>> groundRegions = GetRegions(1);
        List<Room> survivingRooms = new List<Room>();
        // Clear unneccessary walls
        foreach(List<Coordinate> wallRegion in wallRegions) {
            if(wallRegion.Count < 24) {
                foreach(Coordinate tile in wallRegion) {
                    /*Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (tile.tileY - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);*/
                    world.worldMap[tile.tileX, tile.tileY] = 1;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, null);
                }
            }
        }
        // Clear unneccessary rooms
        foreach(List<Coordinate> groundRegion in groundRegions) {
            if(groundRegion.Count < 24) {
                foreach(Coordinate tile in groundRegion) {
                    /*Vector3Int tileCoordinate = new Vector3Int(
                    (tile.tileX - layout.width / 2) + layout.worldCoordinates.x * layout.width,
                    (tile.tileY - layout.height / 2) + layout.worldCoordinates.y * layout.height, 0);*/
                    world.worldMap[tile.tileX, tile.tileY] = 0;
                    //worldGeneration.tilemap.SetTile(tileCoordinate, tileDatabase.wallTiles[tileDatabase.wallTiles.Length - 1]);
                }
            }
            else {
                survivingRooms.Add(new Room(groundRegion, world.worldMap));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);
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

        int passageRadius = 2;

        List<Coordinate> line = GetLine(tileA, tileB);
        foreach(Coordinate c in line) {
            DrawCircle(c, passageRadius);
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
                        world.worldMap[realX, realY] = 1;
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
        int[,] mapFlags = new int[worldSize, worldSize];
        int tileType = world.worldMap[startX, startY];

        Queue<Coordinate> queue = new Queue<Coordinate>();
        queue.Enqueue(new Coordinate(startX, startY));
        mapFlags[startX, startY] = 1;

        while(queue.Count > 0) {
            Coordinate tile = queue.Dequeue();
            tiles.Add(tile);

            for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for(int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                    if(IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)) {
                        if(mapFlags[x, y] == 0 && world.worldMap[x, y] == tileType) {
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
        int[,] mapFlags = new int[worldSize, worldSize];

        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                if(mapFlags[x, y] == 0 && world.worldMap[x, y] >= tileType) {
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
                                if(map[x, y] == 0) {
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
