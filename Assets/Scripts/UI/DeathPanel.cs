using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour {

    public void LoadLastSave() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
