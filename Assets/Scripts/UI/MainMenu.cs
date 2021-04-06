using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour {

    public LocalizationManager localizationManager;
    public TextMeshProUGUI playText;
    public TextMeshProUGUI loadText;
    public TextMeshProUGUI optionsText;

    public void OnEnable() {
        RefreshTexts();
    }
    public void RefreshTexts() {
        playText.text = localizationManager.GetText("play");
        loadText.text = localizationManager.GetText("load");
        optionsText.text = localizationManager.GetText("options");
    }
    public void StartNewGame(int slot) {
        PersistentData.saveSlot = slot;
        List<string> saveFiles = PersistentData.GetAllFilesWithKey($"Data{slot}", "", "");
        PersistentData.DeleteFiles(saveFiles);
        PersistentData.ClearAutosaveFiles();
        LoadSceneAsync("CharacterCreation");
    }
    public void LoadGame(int slot) {
        PersistentData.saveSlot = slot;
        PersistentData.GetFileNames();        
        if(!string.IsNullOrEmpty(PersistentData.GetFileName(slot.ToString(), "GameData", ""))) {
            PersistentData.ClearAutosaveFiles();
            LoadSceneAsync("Levels");
        }        
    }
    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
