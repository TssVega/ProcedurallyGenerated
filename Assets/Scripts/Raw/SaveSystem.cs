﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    public static void Save(Player data, int slot) {
        // Formatter to convert game data to binary
        BinaryFormatter bin = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData" + slot.ToString() + ".tss";
        using(FileStream stream = new FileStream(path, FileMode.Create)) {
            SaveData _data = new SaveData(data);
            bin.Serialize(stream, _data);
        }
        // FileStream stream = new FileStream(path, FileMode.Create);
        // Take the data and write it to the file
        // SaveData _data = new SaveData(data);
        // bin.Serialize(stream, _data);
        // Close the stream
        Debug.LogWarning(data + " saved to slot " + slot + " successfully at " + Application.persistentDataPath);
        // stream.Close();
    }
    // Load from a slot
    public static SaveData Load(int slot) {
        string path = Application.persistentDataPath + "/GameData" + slot.ToString() + ".tss";
        if(File.Exists(path)) {
            Debug.Log("Deserializing file");
            BinaryFormatter bin = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open)) {
                SaveData data = bin.Deserialize(stream) as SaveData;
                Debug.Log("Deserialized file");
                return data;
            }
            // FileStream stream = new FileStream(path, FileMode.Open);
            // SaveData data = bin.Deserialize(stream) as SaveData;
            // Debug.Log("Deserialized file");
            // stream.Close();
            // return data;
        }
        else {
            Debug.LogWarning("No file found in path");
            return null;
        }
    }
}