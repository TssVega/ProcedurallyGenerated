using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public SpriteRenderer skinColor;
    public SpriteRenderer hairColor;
    public SpriteRenderer hairStyle;

    [HideInInspector] public int skinColorIndex = 0;
    [HideInInspector] public int hairColorIndex = 0;
    [HideInInspector] public int hairStyleIndex = 0;

    public CharacterAppearance appearance;

    private void Start() {
        LoadPlayer();
    }
    public void SavePlayer() {
        SaveSystem.Save(this, 0);
    }
    public void LoadPlayer() {
        SaveData data = SaveSystem.Load(0);
        if(data != null) {
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;
            SetAppearance();
        }        
    }
    private void SetAppearance() {
        skinColor.color = appearance.skinColors[skinColorIndex];
        hairColor.color = appearance.hairColors[hairColorIndex];
        hairStyle.sprite = appearance.hairStyles[hairStyleIndex];
    }
}
