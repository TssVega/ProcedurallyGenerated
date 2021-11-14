using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterPreset")]
public class CharacterPreset : ScriptableObject {

    public string classNameKey;
    public int strength = 10;
    public int agility = 10;
    public int dexterity = 10;
    public int intelligence = 10;
    public int faith = 10;
    public int wisdom = 10;
    public int vitality = 10;
    public int charisma = 10;
    public int total;

    private void OnValidate() {
        total = strength + agility + dexterity + intelligence + faith + wisdom + vitality + charisma;
    }
}
