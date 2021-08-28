using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Brackeys' sound system
public class AudioSystem : MonoBehaviour {

    public static AudioSystem audioManager;
    public bool mute;
    public Sound[] sounds;

    public OptionsUI optionsUI;

    private void Awake() {
        mute = false;
        audioManager = this;
        for(int i = 0; i < sounds.Length; i++) {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].audioName);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
        if(optionsUI) {
            optionsUI.LoadSoundOptions();
        }        
    }
    private void Start() {        
        PlaySongs();
    }
    private void PlaySongs() {
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            PlaySound("menuTheme");
        }
        else {
            PlaySound("theCubeOfEverything");
        }
    }
    public void PlaySound(string _name) {
        if(!mute) {
            for(int i = 0; i < sounds.Length; i++) {
                if(sounds[i].audioName == _name) {
                    sounds[i].Play();
                }
            }
        }
    }
    public void Mute() {
        mute = !mute;
    }
    public void StopSound(string _name) {
        for(int i = 0; i < sounds.Length; i++) {
            if(sounds[i].audioName == _name) {
                sounds[i].Stop();
            }
        }
    }
}
