﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PersistentData {

    public static int saveSlot = 1;

    public static List<string> fileNames;

    public static void GetFileNames() {
        fileNames = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles();
        foreach(FileInfo f in info) {
            AddFileName(f.ToString());
        }
    }
    public static string GetFileName(string key1, string key2, string key3) {
        string file = null;
        if(fileNames.Count < 1) {
            return null;
        }
        foreach(string f in fileNames) {
            if(f.Contains(key1) && f.Contains(key2) && f.Contains(key3)) {
                file = f;
            }
        }
        return file;
    }
    public static bool FileExists(string key1, string key2, string key3) {
        string file = null;
        foreach(string f in fileNames) {
            if(f.Contains(key1) && f.Contains(key2) && f.Contains(key3)) {
                file = f;
            }
        }
        return file != null;
    }
    // Update file names
    private static void AddFileName(string fileName) {
        fileNames.Add(fileName);
    }
    public static void DeleteFile(string key1, string key2, string key3) {
        string fileName = GetFileName(key1, key2, key3);
        File.Delete(fileName);
        Debug.LogWarning(fileName + " deleted");
    }
    public static List<string> GetAutosaveFiles() {
        List<string> autosaveFiles = new List<string>();
        GetFileNames();
        for(int i = 0; i < fileNames.Count; i++) {
            if(fileNames[i].Contains("Data0")) {
                autosaveFiles.Add(fileNames[i]);
            }
        }
        return autosaveFiles;
    }
    public static void ClearAutosaveFiles() {
        GetFileNames();
        for(int i = 0; i < fileNames.Count; i++) {
            if(fileNames[i].Contains("Data0")) {
                File.Delete(fileNames[i]);
            }
        }
    }
}
