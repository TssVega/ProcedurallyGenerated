﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    private LocalizationManager localizationManager;
    private MainMenu mainMenu;
    public GameObject trActive;
    public GameObject ukActive;
    public TextMeshProUGUI languageText;

    public GameObject autoLockTick;
    public GameObject muteTick;
    public Slider fxSlider;
    public Slider musicSlider;

    private void Awake() {
        localizationManager = FindObjectOfType<LocalizationManager>();
        mainMenu = FindObjectOfType<MainMenu>();        
    }
    private void OnEnable() {
        LoadSoundOptions();
        LoadLockOptions();
        UpdateLanguageHighlighters(localizationManager.GetLanguage());
    }
    private void RefreshTexts() {
        languageText.text = localizationManager.GetText("language");
    }
    public void ChangeLanguage(string language) {
        localizationManager.SetLocalization(language);
        UpdateLanguageHighlighters(language);
    }
    private void UpdateLanguageHighlighters(string language) {
        mainMenu.RefreshTexts();
        RefreshTexts();
        if(language == "Turkish") {
            trActive.SetActive(true);
            ukActive.SetActive(false);
        }
        else if(language == "English") {
            trActive.SetActive(false);
            ukActive.SetActive(true);
        }
    }
    public void ChangeFXVolume() {
        UpdateFXVolume();
        AudioSystem.audioManager.PlaySound("menuButtonClick", fxSlider.value);
        PlayerPrefs.SetFloat("fx", fxSlider.value);
        PlayerPrefs.Save();
    }
    public void ChangeMusicVolume() {
        UpdateMusicVolume();
        AudioSystem.audioManager.PlaySound("menuButtonClick", musicSlider.value);
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.Save();
    }
    public void ToggleLock() {
        int lockValue = PlayerPrefs.GetInt("lock");

        if(!PlayerPrefs.HasKey("lock")) {
            lockValue = 1;
            PlayerPrefs.SetInt("lock", 1);
            PlayerPrefs.Save();
        }
        lockValue = lockValue == 1 ? 0 : 1;
        PlayerPrefs.SetInt("lock", lockValue);
        if(lockValue == 1) {
            autoLockTick.SetActive(true);
        }
        else {
            autoLockTick.SetActive(false);
        }
        PlayerPrefs.Save();
    }
    public void Mute() {
        AudioSystem.audioManager.mute = !AudioSystem.audioManager.mute;        
        if(AudioSystem.audioManager.mute) {
            muteTick.SetActive(true);
            PlayerPrefs.SetInt("mute", 1);
            for(int i = 0; i < AudioSystem.audioManager.sounds.Length; i++) {
                AudioSystem.audioManager.sounds[i].Stop();
            }            
        }
        else {
            AudioSystem.audioManager.PlaySound("menuButtonClick", fxSlider.value);
            muteTick.SetActive(false);
            PlayerPrefs.SetInt("mute", 0);
            AudioSystem.audioManager.PlaySound("menuTheme", fxSlider.value);
        }
        PlayerPrefs.Save();
    }
    private void LoadLockOptions() {
        int lockValue = PlayerPrefs.GetInt("lock");

        if(!PlayerPrefs.HasKey("lock")) {
            lockValue = 1;
        }
        if(lockValue == 1) {
            autoLockTick.SetActive(true);
        }
        else {
            autoLockTick.SetActive(false);
        }
    }
    public void LoadSoundOptions() {
        float fxValue = PlayerPrefs.GetFloat("fx");
        float musicValue = PlayerPrefs.GetFloat("music");
        int muteValue = PlayerPrefs.GetInt("mute");

        if(!PlayerPrefs.HasKey("fx")) {
            fxValue = 1f;
            PlayerPrefs.SetFloat("fx", 1f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("music")) {
            musicValue = 1f;
            PlayerPrefs.SetFloat("music", 1f);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("mute")) {
            muteValue = 0;
            PlayerPrefs.SetInt("mute", 0);
            PlayerPrefs.Save();
        }

        if(muteValue == 0) {
            AudioSystem.audioManager.mute = false;
            muteTick.SetActive(false);
        }
        else {
            AudioSystem.audioManager.mute = true;
            muteTick.SetActive(true);
            for(int i = 0; i < AudioSystem.audioManager.sounds.Length; i++) {
                AudioSystem.audioManager.sounds[i].Stop();
            }            
        }
        musicSlider.value = musicValue;
        fxSlider.value = fxValue;
        UpdateMusicVolume();
        UpdateFXVolume();
    }
    private void UpdateMusicVolume() {        
        AudioSystem.audioManager.sounds[0].volume = musicSlider.value;
        AudioSystem.audioManager.sounds[1].volume = musicSlider.value;
        AudioSystem.audioManager.sounds[0].ChangeVolume(0f);
        AudioSystem.audioManager.sounds[1].ChangeVolume(0f);
    }
    private void UpdateFXVolume() {        
        AudioSystem.audioManager.sounds[2].volume = fxSlider.value;        
        AudioSystem.audioManager.sounds[3].volume = fxSlider.value;
        AudioSystem.audioManager.sounds[2].ChangeVolume(0f);
        AudioSystem.audioManager.sounds[3].ChangeVolume(0f);
    }
}
