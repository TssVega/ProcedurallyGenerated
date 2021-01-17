using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour {

    public Image skinImage;
    public Image hairImage;

    public CharacterAppearance appearance;

    private Color currentSkinColor;
    private Color currentHairColor;
    private Sprite currentHairStyle;

    private int currentSkinColorIndex = 0;
    private int currentHairColorIndex = 0;
    private int currentHairStyleIndex = 0;

    private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();
        currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        currentHairColor = appearance.hairColors[currentHairColorIndex];
        currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        skinImage.color = currentSkinColor;
        hairImage.color = currentHairColor;
        hairImage.sprite = currentHairStyle;
    }
    public void CompleteCharacterCreation() {
        player.SavePlayer();
        LoadSceneAsync("Levels");
    }
    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void ChangeSkinColor(bool increment) {
        if(increment) {
            currentSkinColorIndex++;
            if(currentSkinColorIndex > appearance.skinColors.Length - 1) {
                currentSkinColorIndex = 0;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        else {
            currentSkinColorIndex--;
            if(currentSkinColorIndex < 0) {
                currentSkinColorIndex = appearance.skinColors.Length - 1;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        skinImage.color = currentSkinColor;
        player.skinColorIndex = currentSkinColorIndex;
    }
    public void ChangeHairColor(bool increment) {
        if(increment) {
            currentHairColorIndex++;
            if(currentHairColorIndex > appearance.hairColors.Length - 1) {
                currentHairColorIndex = 0;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        else {
            currentHairColorIndex--;
            if(currentHairColorIndex < 0) {
                currentHairColorIndex = appearance.hairColors.Length - 1;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        hairImage.color = currentHairColor;
        player.hairColorIndex = currentHairColorIndex;
    }
    public void ChangeHairStyle(bool increment) {
        if(increment) {
            currentHairStyleIndex++;
            if(currentHairStyleIndex > appearance.hairStyles.Length - 1) {
                currentHairStyleIndex = 0;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        else {
            currentHairStyleIndex--;
            if(currentHairStyleIndex < 0) {
                currentHairStyleIndex = appearance.hairStyles.Length - 1;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        hairImage.sprite = currentHairStyle;
        player.hairStyleIndex = currentHairStyleIndex;
    }
}
