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
        else {
            LoadSoundOptions();
        }
    }
    private void Start() {        
        PlaySongs();
    }
    private void PlaySongs() {
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            PlaySound("menuTheme", 0f);
        }
        else {
            PlaySound("theCubeOfEverything", 0f);
        }
    }
    public void PlaySound(string _name, float distance) {
        if(!mute) {
            for(int i = 0; i < sounds.Length; i++) {
                if(sounds[i].audioName == _name) {
                    sounds[i].Play(distance);
                    return;
                }
            }
            Debug.LogError($"{_name} cannot be found on audioSystem");
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
    public void LoadSoundOptions() {
        float fxValue = PlayerPrefs.GetFloat("fx");
        float musicValue = PlayerPrefs.GetFloat("music");
        int muteValue = PlayerPrefs.GetInt("mute");

        if(!PlayerPrefs.HasKey("fx")) {
            fxValue = 1f;
        }
        if(!PlayerPrefs.HasKey("music")) {
            musicValue = 1f;
        }
        if(!PlayerPrefs.HasKey("mute")) {
            muteValue = 0;
        }

        if(muteValue == 0) {
            mute = false;
        }
        else {
            mute = true;
            for(int i = 0; i < sounds.Length; i++) {
                sounds[i].Stop();
            }
        }
        sounds[0].volume = musicValue;
        sounds[1].volume = musicValue;
        sounds[0].ChangeVolume(0f);
        sounds[1].ChangeVolume(0f);
        sounds[2].volume = fxValue;
        sounds[3].volume = fxValue;
        sounds[2].ChangeVolume(0f);
        sounds[3].ChangeVolume(0f);
        for(int i = 4; i < sounds.Length; i++) {
            sounds[i].volume = fxValue;
        }
    }
}
