using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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

    public TextMeshProUGUI lowText;
    public TextMeshProUGUI mediumText;
    public TextMeshProUGUI highText;

    public Color activeQualityColor;

    public UniversalRenderPipelineAsset urpAsset;

    private const int defaultQuality = 2;

    private void Awake() {
        localizationManager = FindObjectOfType<LocalizationManager>();
        mainMenu = FindObjectOfType<MainMenu>();        
    }
    private void OnEnable() {
        LoadSoundOptions();
        UpdateLockOptions();
        UpdateLanguageHighlighters(localizationManager.GetLanguage());
        UpdateQualitySettings();
    }
    private void RefreshTexts() {
        languageText.text = localizationManager.GetText("language");
    }
    public void ChangeLanguage(string language) {
        localizationManager.SetLocalization(language);
        UpdateLanguageHighlighters(language);
        AudioSystem.audioManager.PlaySound("menuButton", 0f);
    }
    // 0 is low, 1 is medium, 2 is high quality
    public void ChangeQuality(int quality) {
        PlayerPrefs.SetInt("quality", quality);
        PlayerPrefs.Save();
        AudioSystem.audioManager.PlaySound("menuButton", 0f);
        UpdateQualitySettings();
    }
    private void UpdateQualitySettings() {
        
        int quality = PlayerPrefs.HasKey("quality") ? PlayerPrefs.GetInt("quality") : defaultQuality;
        PlayerPrefs.SetInt("quality", quality);
        PlayerPrefs.Save();
        switch(quality) {
            case 0:
                lowText.color = activeQualityColor;
                mediumText.color = Color.white;
                highText.color = Color.white;
                urpAsset.renderScale = 0.33f;
                break;
            case 1:
                lowText.color = Color.white;
                mediumText.color = activeQualityColor;
                highText.color = Color.white;
                urpAsset.renderScale = 0.5f;
                break;
            case 2:
                lowText.color = Color.white;
                mediumText.color = Color.white;
                highText.color = activeQualityColor;
                urpAsset.renderScale = 1f;
                break;
            default:
                break;
        }
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
        //AudioSystem.audioManager.PlaySound("menuButton", 0f);
        PlayerPrefs.SetFloat("fx", fxSlider.value);
        PlayerPrefs.Save();
    }
    public void ChangeMusicVolume() {
        UpdateMusicVolume();
        //AudioSystem.audioManager.PlaySound("menuButton", 0f);
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.Save();
    }
    public void ToggleLock() {
        int lockValue = PlayerPrefs.HasKey("lock") ? PlayerPrefs.GetInt("lock") : 1;
        /*
        PlayerPrefs.SetInt("lock", lockValue);
        PlayerPrefs.Save();*/
        lockValue = lockValue == 1 ? 0 : 1;
        PlayerPrefs.SetInt("lock", lockValue);        
        PlayerPrefs.Save();
        UpdateLockOptions();
        AudioSystem.audioManager.PlaySound("menuButton", 0f);
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
            AudioSystem.audioManager.PlaySound("menuButton", fxSlider.value);
            muteTick.SetActive(false);
            PlayerPrefs.SetInt("mute", 0);
            AudioSystem.audioManager.PlaySound("menuTheme", fxSlider.value);
        }
        PlayerPrefs.Save();
        AudioSystem.audioManager.PlaySound("menuButton", 0f);
    }
    public void UpdateLockOptions() {
        int lockValue = PlayerPrefs.HasKey("lock") ? PlayerPrefs.GetInt("lock") : 1;
        PlayerPrefs.SetInt("lock", lockValue);
        PlayerPrefs.Save();
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
        // AudioSystem.audioManager.PlaySound("menuButton", 0f);
    }
    private void UpdateFXVolume() {
        for(int i = 2; i < AudioSystem.audioManager.sounds.Length; i++) {
            AudioSystem.audioManager.sounds[i].volume = fxSlider.value;
            AudioSystem.audioManager.sounds[i].ChangeVolume(0f);
        }
        AudioSystem.audioManager.PlaySound("menuButton", 0f);
    }
}
