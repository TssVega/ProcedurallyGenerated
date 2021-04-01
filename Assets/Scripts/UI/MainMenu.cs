using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject startNewGamePanel;
    public GameObject loadGamePanel;

    public void StartNewGame(int slot) {
        PersistentData.saveSlot = slot;
        List<string> saveFiles = PersistentData.GetAllFilesWithKey($"Data{slot}", "", "");
        PersistentData.DeleteFiles(saveFiles);
        LoadSceneAsync("CharacterCreation");
    }
    public void LoadGame(int slot) {
        PersistentData.saveSlot = slot;
        PersistentData.GetFileNames();
        if(!string.IsNullOrEmpty(PersistentData.GetFileName(slot.ToString(), "GameData", ""))) {
            LoadSceneAsync("Levels");
        }        
    }
    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
