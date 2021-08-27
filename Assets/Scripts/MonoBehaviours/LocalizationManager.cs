using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager localization;

    private Dictionary<string, string> texts;
    [SerializeField]
    private string DEFAULT_LANGUAGE = "English";
    private string currentLanguage;

    // Create delegate and events for use with LocalText.cs:
    public delegate void LanguageChangedEventHandler();
    public event LanguageChangedEventHandler LanguageChanged;

    public MushroomDatabase mushroomDatabase;
    public ItemDatabase itemDatabase;

    private void Awake() {
        localization = this;
        // Load user preferences, if any:
        if(PlayerPrefs.HasKey("LAST_LANGUAGE")) {
            string newLang = PlayerPrefs.GetString("LAST_LANGUAGE");
            try {
                SetLocalization(newLang);
            }
            catch(Exception e) {
                Debug.Log(e);
                Debug.Log("Trying Default Language: " + DEFAULT_LANGUAGE);
                SetLocalization(DEFAULT_LANGUAGE);
            }
        }
        else {
            // If not, we use defaults.
            SetLocalization(DEFAULT_LANGUAGE);
        }
    }
    private void Start() {
        if(mushroomDatabase) {
            for(int i = 0; i < mushroomDatabase.mushrooms.Count; i++) {
                mushroomDatabase.mushrooms[i].itemName = GetText(mushroomDatabase.mushrooms[i].seed);
            }
        }
        if(itemDatabase) {
            for(int i = 0; i < itemDatabase.items.Count; i++) {
                itemDatabase.items[i].itemName = GetText(itemDatabase.items[i].seed);
            }
        }
    }
    public string GetLanguage() {
        return currentLanguage;
    }
    /// <summary>
    /// Sets the current language used by getText() to the language specified.
    /// </summary>
    /// <param name="language">The language to change to.</param>
    public void SetLocalization(string language) {
        TextAsset textAsset = Resources.Load<TextAsset>("Locale_" + language);
        if(textAsset != null) {
            texts = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
            currentLanguage = language;
            OnLanguageChanged();
        }
        else {
            throw new Exception("Localization Error!: " + language + " does not have a .txt resource!");
        }
    }

    /// <summary>
    /// Get the text by the specified identifier.
    /// </summary>
    /// <param name="identifier">Identifier to search the current locale for.</param>
    /// <returns>The string associated with the identifier. If this doesn't exist, null.</returns>
    public string GetText(string identifier) {
        if(!texts.ContainsKey(identifier)) {
            Debug.Log("Localization Error!: " + identifier + " does not have an associated string!");
            return null;
        }
        return texts[identifier];
    }

    private void OnApplicationQuit() {
        PlayerPrefs.SetString("LAST_LANGUAGE", currentLanguage);
    }
    private void OnDisable() {
        PlayerPrefs.SetString("LAST_LANGUAGE", currentLanguage);
    }
    protected virtual void OnLanguageChanged() {
        if(LanguageChanged != null)
            LanguageChanged();
    }
}
