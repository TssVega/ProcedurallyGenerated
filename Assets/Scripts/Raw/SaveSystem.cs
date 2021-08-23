using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    public static void Save(Player data, int slot) {
        // Formatter to convert game data to binary
        BinaryFormatter bin = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/GameData{slot}.tss";
        using(FileStream stream = new FileStream(path, FileMode.Create)) {
            SaveData _data = new SaveData(data);
            bin.Serialize(stream, _data);
        }
        // FileStream stream = new FileStream(path, FileMode.Create);
        // Take the data and write it to the file
        // SaveData _data = new SaveData(data);
        // bin.Serialize(stream, _data);
        // Close the stream
        // stream.Close();
    }
    // Load from a slot
    public static SaveData Load(int slot) {
        string path = $"{Application.persistentDataPath}/GameData{slot}.tss";
        if(File.Exists(path)) {
            BinaryFormatter bin = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open)) {
                SaveData data = bin.Deserialize(stream) as SaveData;
                return data;
            }
            // FileStream stream = new FileStream(path, FileMode.Open);
            // SaveData data = bin.Deserialize(stream) as SaveData;
            // Debug.Log("Deserialized file");
            // stream.Close();
            // return data;
        }
        else {
            return null;
        }
    }
    public static void SaveWorld(WorldData data, int slot) {
        // Formatter to convert game data to binary
        BinaryFormatter bin = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/WorldData{slot}.tss";
        using(FileStream stream = new FileStream(path, FileMode.Create)) {
            WorldData _data = new WorldData(data.worldData, data.currentCoordinates, data.worldMap, data.seed, data.explorationData);
            bin.Serialize(stream, _data);
        }
        // FileStream stream = new FileStream(path, FileMode.Create);
        // Take the data and write it to the file
        // SaveData _data = new SaveData(data);
        // bin.Serialize(stream, _data);
        // Close the stream
        // stream.Close();
    }
    // Load from a slot
    public static WorldData LoadWorld(int slot) {
        string path = $"{Application.persistentDataPath}/WorldData{slot}.tss";
        if(File.Exists(path)) {
            BinaryFormatter bin = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open)) {
                WorldData data = bin.Deserialize(stream) as WorldData;
                return data;
            }
            // FileStream stream = new FileStream(path, FileMode.Open);
            // SaveData data = bin.Deserialize(stream) as SaveData;
            // Debug.Log("Deserialized file");
            // stream.Close();
            // return data;
        }
        else {
            return null;
        }
    }
    public static void SaveChests(ChestGeneration data, int slot, Vector2Int coordinates) {
        // Formatter to convert game data to binary
        BinaryFormatter bin = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/ChestData{slot}_{coordinates.x}x{coordinates.y}y.tss";
        using(FileStream stream = new FileStream(path, FileMode.Create)) {
            ChestData _data = new ChestData(data);
            bin.Serialize(stream, _data);
        }
        // FileStream stream = new FileStream(path, FileMode.Create);
        // Take the data and write it to the file
        // SaveData _data = new SaveData(data);
        // bin.Serialize(stream, _data);
        // Close the stream
        //Debug.Log("Chest data " + data + " saved to slot " + slot + " successfully at " + Application.persistentDataPath);
        // stream.Close();
    }
    // Load from a slot
    public static ChestData LoadChests(int slot, Vector2Int coordinates) {
        string path = $"{Application.persistentDataPath}/ChestData{slot}_{coordinates.x}x{coordinates.y}y.tss";
        if(File.Exists(path)) {
            BinaryFormatter bin = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open)) {
                ChestData data = bin.Deserialize(stream) as ChestData;
                return data;
            }
            // FileStream stream = new FileStream(path, FileMode.Open);
            // SaveData data = bin.Deserialize(stream) as SaveData;
            // Debug.Log("Deserialized file");
            // stream.Close();
            // return data;
        }
        else {
            return null;
        }
    }
    public static void SaveMushrooms(MushroomGeneration data, int slot, Vector2Int coordinates) {
        // Formatter to convert game data to binary
        BinaryFormatter bin = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/MushroomData{slot}_{coordinates.x}x{coordinates.y}y.tss";
        using(FileStream stream = new FileStream(path, FileMode.Create)) {
            MushroomData _data = new MushroomData(data);
            bin.Serialize(stream, _data);
        }
        // FileStream stream = new FileStream(path, FileMode.Create);
        // Take the data and write it to the file
        // SaveData _data = new SaveData(data);
        // bin.Serialize(stream, _data);
        // Close the stream
        //Debug.Log("Mushroom data " + data + " saved to slot " + slot + " successfully at " + Application.persistentDataPath);
        // stream.Close();
    }
    public static MushroomData LoadMushrooms(int slot, Vector2Int coordinates) {
        string path = $"{Application.persistentDataPath}/MushroomData{slot}_{coordinates.x}x{coordinates.y}y.tss";
        if(File.Exists(path)) {
            BinaryFormatter bin = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open)) {
                MushroomData data = bin.Deserialize(stream) as MushroomData;
                return data;
            }
            // FileStream stream = new FileStream(path, FileMode.Open);
            // SaveData data = bin.Deserialize(stream) as SaveData;
            // Debug.Log("Deserialized file");
            // stream.Close();
            // return data;
        }
        else {
            return null;
        }
    }
}
