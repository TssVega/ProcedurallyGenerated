using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Stats stats;

    public SpriteRenderer skinColor;
    public SpriteRenderer hairColor;
    public SpriteRenderer hairStyle;

    [HideInInspector] public int skinColorIndex = 0;
    [HideInInspector] public int hairColorIndex = 0;
    [HideInInspector] public int hairStyleIndex = 0;

    public CharacterAppearance appearance;

    private void Awake() {
        stats = GetComponent<Stats>();
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
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;
            // Status
            stats.health = data.health;
            stats.mana = data.mana;
            stats.energy = data.energy;
            stats.maxHealth = data.maxHealth;
            stats.maxMana = data.maxMana;
            stats.maxEnergy = data.maxEnergy;
            stats.runSpeed = data.runSpeed;
            // Main
            stats.strength = data.strength;
            stats.agility = data.agility;
            stats.dexterity = data.dexterity;
            stats.intelligence = data.intelligence;
            stats.faith = data.faith;
            stats.wisdom = data.wisdom;
            stats.vitality = data.vitality;
            stats.charisma = data.charisma;
            // Damages
            stats.bashDamage = data.bashDamage;
            stats.pierceDamage = data.pierceDamage;
            stats.slashDamage = data.slashDamage;
            stats.fireDamage = data.fireDamage;
            stats.iceDamage = data.iceDamage;
            stats.lightningDamage = data.lightningDamage;
            stats.airDamage = data.airDamage;
            stats.earthDamage = data.earthDamage;
            stats.lightDamage = data.lightDamage;
            stats.darkDamage = data.darkDamage;
            stats.poisonDamage = data.poisonDamage;
            stats.bleedDamage = data.bleedDamage;
            stats.curseDamage = data.curseDamage;
            // Defences
            stats.bashDefence = data.bashDefence;
            stats.pierceDefence = data.pierceDefence;
            stats.slashDefence = data.slashDefence;
            stats.fireDefence = data.fireDefence;
            stats.iceDefence = data.iceDefence;
            stats.lightningDefence = data.lightningDefence;
            stats.airDefence = data.airDefence;
            stats.earthDefence = data.earthDefence;
            stats.lightDefence = data.lightDefence;
            stats.darkDefence = data.darkDefence;
            stats.poisonDefence = data.poisonDefence;
            stats.bleedDefence = data.bleedDefence;
            stats.curseDefence = data.curseDefence;
            // Other
            stats.statPoints = data.statPoints;
            stats.maxDamageTimesHealth = data.maxDamageTimesHealth;
            stats.parryDuration = data.parryDuration;
            stats.blockDuration = data.blockDuration;
            stats.damageBlockRate = data.damageBlockRate;
            // Thresholds
            stats.burningThreshold = data.burningThreshold;
            stats.earthingThreshold = data.earthingThreshold;
            stats.frostbiteThreshold = data.frostbiteThreshold;
            stats.shockThreshold = data.shockThreshold;
            stats.lightingThreshold = data.lightingThreshold;
            stats.poisonThreshold = data.poisonThreshold;
            stats.bleedThreshold = data.bleedThreshold;
            stats.curseThreshold = data.curseThreshold;
        }        
    }
    private void SetAppearance() {
        skinColor.color = appearance.skinColors[skinColorIndex];
        hairColor.color = appearance.hairColors[hairColorIndex];
        hairStyle.sprite = appearance.hairStyles[hairStyleIndex];
    }
}
