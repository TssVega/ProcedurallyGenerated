﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int saveSlot;

    public Stats stats;

    public SpriteRenderer skinColor;
    public SpriteRenderer hairColor;
    public SpriteRenderer hairStyle;

    [HideInInspector] public int skinColorIndex = 0;
    [HideInInspector] public int hairColorIndex = 0;
    [HideInInspector] public int hairStyleIndex = 0;

    public CharacterAppearance appearance;
    public ItemCreator itemCreator;
    // Sprites for weapons
    public SpriteRenderer weaponHandle;
    public SpriteRenderer weaponGuard;
    public SpriteRenderer weaponBlade;
    // Sprites for body armor
    public SpriteRenderer bodyArmor;
    // Sprites for helmet
    public SpriteRenderer helmetBase;
    public SpriteRenderer helmetProp;
    // Sprite for shield
    public SpriteRenderer shield;
    // Sprites for bows
    public SpriteRenderer bowFirst;
    public SpriteRenderer bowSecond;
    public SpriteRenderer bowThird;
    // World generation
    private WorldGeneration worldGeneration;
    // BodyPart[] bodyParts;
    public GameObject releasedBowString;
    public GameObject[] tenseBowStrings;

    private void Awake() {
        worldGeneration = FindObjectOfType<WorldGeneration>();
        stats = GetComponent<Stats>();
    }
    private void Start() {
        LoadPlayer();
        releasedBowString.SetActive(false);
        for(int i = 0; i < tenseBowStrings.Length; i++) {
            tenseBowStrings[i].SetActive(false);
        }
        //SetWeapon(itemCreator.CreateWeaponSprite("tss"));
    }
    /*
    private void Update() { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            SetWeapon(itemCreator.CreateWeaponSprite(Time.time.ToString()));
        }
    }*/
    public void CheckWeapon() {
        /*
        if(bodyParts[0].item && bodyParts[0].item.hasTrail) {
            //weaponTrail.ActivateTrail(bodyParts[0].item);
        }
        else {
            //weaponTrail.StopTrail();
        }*/
    }
    public void SavePlayer() {
        SaveSystem.Save(this, PersistentData.saveSlot);
        if(worldGeneration) {
            worldGeneration.SaveWorldData();
        }
    }
    public void LoadPlayer() {
        SaveData data = SaveSystem.Load(PersistentData.saveSlot);
        if(data != null) {
            saveSlot = data.saveSlot;
            Vector3 pos = new Vector3(data.position[0], data.position[1], data.position[2]);
            transform.position = pos;
            Quaternion quat = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
            transform.rotation = quat;
            // Appearance
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;            
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;
            SetAppearance();
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
    public void SetWeapon(Weapon weapon) {
        ClearWeapons();
        if(weapon.weaponType == WeaponType.OneHanded) {
            weaponHandle.sprite = weapon.firstSprite;
            weaponGuard.sprite = weapon.secondSprite;
            weaponBlade.sprite = weapon.thirdSprite;
            weaponHandle.color = weapon.firstColor;
            weaponGuard.color = weapon.secondColor;
            weaponBlade.color = weapon.thirdColor;
        }
        else if(weapon.weaponType == WeaponType.TwoHanded) {
        
        }
        else if(weapon.weaponType == WeaponType.Bow) {
            bowFirst.sprite = weapon.firstSprite;
            bowSecond.sprite = weapon.secondSprite;
            bowThird.sprite = weapon.thirdSprite;
            bowFirst.color = weapon.firstColor;
            bowSecond.color = weapon.secondColor;
            bowThird.color = weapon.thirdColor;
            for(int i = 0; i < tenseBowStrings.Length; i++) {
                tenseBowStrings[i].SetActive(true);
            }

        }
        else if(weapon.weaponType == WeaponType.Dagger) {
            
        }
    }
    public void ClearWeapons() {
        weaponHandle.sprite = null;
        weaponGuard.sprite = null;
        weaponBlade.sprite = null;
        weaponHandle.color = Color.clear;
        weaponGuard.color = Color.clear;
        weaponBlade.color = Color.clear;
        bowFirst.sprite = null;
        bowSecond.sprite = null;
        bowThird.sprite = null;
        bowFirst.color = Color.clear;
        bowSecond.color = Color.clear;
        bowThird.color = Color.clear;
        releasedBowString.SetActive(false);
        for(int i = 0; i < tenseBowStrings.Length; i++) {
            tenseBowStrings[i].SetActive(false);
        }
    }
    public void SetBodyArmor(Armor armor) {
        bodyArmor.sprite = armor.firstSprite;
        bodyArmor.color = armor.firstColor;
    }
    public void ClearBodyArmor() {
        bodyArmor.sprite = null;
        bodyArmor.color = Color.clear;
    }
    public void SetHelmet(Armor helmet) {
        helmetBase.sprite = helmet.firstSprite;
        helmetBase.color = helmet.firstColor;
        helmetProp.sprite = helmet.secondSprite;
        helmetProp.color = helmet.secondColor;
    }
    public void ClearHelmet() {
        helmetBase.sprite = null;
        helmetBase.color = Color.clear;
        helmetProp.sprite = null;
        helmetProp.color = Color.clear;
    }
    public void SetShield(Shield shield) {
        this.shield.sprite = shield.firstSprite;
        this.shield.color = shield.firstColor;
    }
    public void ClearShield() {
        this.shield.sprite = null;
        this.shield.color = Color.clear;
    }
}
