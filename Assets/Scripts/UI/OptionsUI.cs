using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsUI : MonoBehaviour {

    private LocalizationManager localizationManager;
    private MainMenu mainMenu;
    public GameObject trActive;
    public GameObject ukActive;
    public TextMeshProUGUI languageText;

    private void Awake() {
        localizationManager = FindObjectOfType<LocalizationManager>();
        mainMenu = FindObjectOfType<MainMenu>();
    }
    private void OnEnable() {
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
}
