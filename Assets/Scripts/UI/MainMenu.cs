using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void StartNewGame() {
        LoadSceneAsync("CharacterCreation");
    }

    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
