﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour {

    public int saveSlot;

    public Stats stats;
    public Inventory inventory;
    public SkillUser skillUser;

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

    public Transform testTarget;

    private List<IInteractable> interactionList;

    private UICanvas uiCanvas;

    public bool skillBookUnlocked = false;
    public bool mapUnlocked = false;

    public string[] consumableItems;

    public MushroomDatabase mushroomDatabase;

    private StatusUI statusUI;

    private void Awake() {
        consumableItems = new string[11];
        worldGeneration = FindObjectOfType<WorldGeneration>();
        stats = GetComponent<Stats>();
        skillUser = GetComponent<SkillUser>();
        uiCanvas = FindObjectOfType<UICanvas>();
        inventory = GetComponent<Inventory>();
        statusUI = FindObjectOfType<StatusUI>();
    }
    private void Start() {
        interactionList = new List<IInteractable>();
        ClearWeapons();
        LoadPlayer();
        //SetWeapon(itemCreator.CreateWeaponSprite("tss"));
    }
    /*
    private void Update() {
        
        if(RelativePosition.GetRelativePosition(transform, testTarget) == Relative.Behind) {
            Debug.Log("You are looking behind the enemy");
        }
        else if(RelativePosition.GetRelativePosition(transform, testTarget) == Relative.None) {
            Debug.Log("You are looking away");
        }
        
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
        if(!PersistentData.CanSave()) {
            Debug.LogWarning($"Cannot save right now. Currently working threads: {PersistentData.ThreadCount}");
            return;
        }
        SaveSystem.Save(this, PersistentData.saveSlot);
        if(worldGeneration) {
            worldGeneration.SaveWorldData();
        }
        // Save chests here
        ChestGeneration[] chestGenerators = FindObjectsOfType<ChestGeneration>();
        MushroomGeneration[] mushroomGenerators = FindObjectsOfType<MushroomGeneration>();
        PoolGeneration[] poolGenerators = FindObjectsOfType<PoolGeneration>();
        for(int i = 0; i < chestGenerators.Length; i++) {
            chestGenerators[i].SaveChests(0);
        }
        for(int i = 0; i < mushroomGenerators.Length; i++) {
            mushroomGenerators[i].SaveMushrooms(0);
        }
        for(int i = 0; i < poolGenerators.Length; i++) {
            poolGenerators[i].SavePools(0);
        }
        ReplaceAutosaveFilesWithSlotSpecificOnes();
        Debug.Log($"Saved successfully");
        /*
        for(int i = 0; i < chestGen.Length; i++) {
            chestGen[i].SaveChests(PersistentData.saveSlot);
        }*/
    }
    public void ReplaceAutosaveFilesWithSlotSpecificOnes() {
        List<string> autosaveFiles = PersistentData.GetAutosaveFiles();
        if(autosaveFiles.Count < 1) {
            return;
        }
        for(int i = 0; i < autosaveFiles.Count; i++) {
            if(File.Exists(autosaveFiles[i].Replace("Data0", $"Data{PersistentData.saveSlot}"))) {
                File.Delete(autosaveFiles[i].Replace("Data0", $"Data{PersistentData.saveSlot}"));
            }
            File.Copy(autosaveFiles[i], autosaveFiles[i].Replace("Data0", $"Data{PersistentData.saveSlot}"));
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
            // Inventory buttons
            skillBookUnlocked = data.skillBookUnlocked;
            mapUnlocked = data.mapUnlocked;
            // Inventory and equipment
            for(int i = 0; i < data.inventory.Length; i++) {
                if(data.inventory[i] != null) {
                    inventory.inventory[i] = itemCreator.CreateItem(data.inventory[i], stats.luck);
                }
                else {
                    inventory.inventory[i] = null;
                }
            }
            for(int i = 0; i < data.equipment.Length; i++) {
                if(data.equipment[i] != null) {
                    inventory.equipment[i] = itemCreator.CreateItem(data.equipment[i], stats.luck);
                    SetItem(inventory.equipment[i]);
                }
                else {
                    inventory.equipment[i] = null;
                }
            }            
            for(int i = 0; i < data.inventoryQuantities.Length; i++) {
                inventory.quantities[i] = data.inventoryQuantities[i];
            }
            // Appearance
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;            
            skinColorIndex = data.skinColorIndex;
            hairColorIndex = data.hairColorIndex;
            hairStyleIndex = data.hairStyleIndex;
            SetAppearance();
            // Status
            stats.maxHealth = data.maxHealth;
            stats.maxMana = data.maxMana;
            stats.maxEnergy = data.maxEnergy;
            stats.trueMaxHealth = stats.maxHealth + stats.strength * 2 + stats.vitality * 5;
            stats.trueMaxMana = stats.maxMana + stats.dexterity * 2 + stats.wisdom * 5 + stats.intelligence * 2 + stats.faith * 2;
            stats.trueMaxEnergy = stats.maxEnergy + stats.vitality * 5 + stats.strength * 2 + stats.dexterity * 2 + stats.agility * 2;
            stats.health = data.health;
            stats.mana = data.mana;
            stats.energy = data.energy;            
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
            stats.luck = data.luck;
            // Thresholds
            stats.burningThreshold = data.burningThreshold;
            stats.earthingThreshold = data.earthingThreshold;
            stats.frostbiteThreshold = data.frostbiteThreshold;
            stats.shockThreshold = data.shockThreshold;
            stats.lightingThreshold = data.lightingThreshold;
            stats.poisonThreshold = data.poisonThreshold;
            stats.bleedThreshold = data.bleedThreshold;
            stats.curseThreshold = data.curseThreshold;
            // Skills
            skillUser.acquiredSkills = new List<Skill>();
            for(int i = 0; i < data.acquiredSkills.Length; i++) {
                if(data.acquiredSkills[i] >= 0) {
                    //Debug.Log($"i = {i}");
                    //skillUser.acquiredSkills[i] = skillUser.skillDatabase.skills[data.acquiredSkills[i]];
                    skillUser.acquiredSkills.Add(skillUser.skillDatabase.skills[data.acquiredSkills[i]]);
                }
                else {
                    skillUser.acquiredSkills[i] = null;
                }
            }
            const int maxCurrentSkillCount = 11;
            for(int i = 0; i < maxCurrentSkillCount; i++) {
                if(data.currentSkills[i] >= 0) {
                    skillUser.currentSkills[i] = skillUser.skillDatabase.skills[data.currentSkills[i]] as ActiveSkill;
                }
                else {
                    skillUser.currentSkills[i] = null;
                }
            }
            for(int i = 0; i < data.consumableItems.Length; i++) {
                if(!string.IsNullOrEmpty(data.consumableItems[i])) {
                    int index = -1;
                    for(int j = 0; j < mushroomDatabase.mushrooms.Count; j++) {
                        if(mushroomDatabase.mushrooms[j].seed == data.consumableItems[i]) {
                            index = j;
                            break;
                        }
                    }
                    if(index >= 0) {
                        skillUser.currentSkills[i] = mushroomDatabase.mushrooms[index];
                    }
                }
            }
        }
        inventory.UpdateStats();
        if(statusUI) {
            statusUI.UpdateHealth(stats.health / stats.trueMaxHealth, stats.health);
            statusUI.UpdateMana(stats.mana / stats.trueMaxMana, stats.mana);
            statusUI.UpdateEnergy(stats.energy / stats.trueMaxEnergy, stats.energy);
        }        
    }
    private void SetAppearance() {
        skinColor.color = appearance.skinColors[skinColorIndex];
        hairColor.color = appearance.hairColors[hairColorIndex];
        hairStyle.sprite = appearance.hairStyles[hairStyleIndex];
    }
    public void SetWeapon(Weapon weapon) {
        ClearWeapons();
        if(!weapon) {            
            return;
        }        
        if(weapon.weaponType == WeaponType.OneHanded) {
            weaponHandle.sprite = weapon.firstSprite;
            weaponGuard.sprite = weapon.secondSprite;
            weaponBlade.sprite = weapon.thirdSprite;
            weaponHandle.color = weapon.firstColor;
            weaponGuard.color = weapon.secondColor;
            weaponBlade.color = weapon.thirdColor;
        }
        else if(weapon.weaponType == WeaponType.TwoHanded) {
            weaponHandle.sprite = weapon.firstSprite;
            weaponGuard.sprite = weapon.secondSprite;
            weaponBlade.sprite = weapon.thirdSprite;
            weaponHandle.color = weapon.firstColor;
            weaponGuard.color = weapon.secondColor;
            weaponBlade.color = weapon.thirdColor;
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
    public void SetItem(Item item) {
        if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                SetBodyArmor(item as Armor);
            }
            else if(item.slot == EquipSlot.Head) {
                SetHelmet(item as Armor);
            }
        }
        if(item is Shield) {
            SetShield(item as Shield);
        }
        if(item is Weapon) {
            SetWeapon(item as Weapon);
        }
    }
    public void ClearItem(Item item) {
        if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                ClearBodyArmor();
            }
            else if(item.slot == EquipSlot.Head) {
                ClearHelmet();
            }
        }
        if(item is Shield) {
            ClearShield();
        }
        if(item is Weapon) {
            ClearWeapons();
        }
    }
    private void SetBodyArmor(Armor armor) {
        bodyArmor.sprite = armor.firstSprite;
        bodyArmor.color = armor.secondColor;
    }
    private void ClearBodyArmor() {
        bodyArmor.sprite = null;
        bodyArmor.color = Color.clear;
    }
    private void SetHelmet(Armor helmet) {
        helmetBase.sprite = helmet.firstSprite;
        helmetBase.color = helmet.firstColor;
        helmetProp.sprite = helmet.secondSprite;
        helmetProp.color = helmet.secondColor;
    }
    private void ClearHelmet() {
        helmetBase.sprite = null;
        helmetBase.color = Color.clear;
        helmetProp.sprite = null;
        helmetProp.color = Color.clear;
    }
    private void SetShield(Shield shield) {
        this.shield.sprite = shield.firstSprite;
        this.shield.color = shield.firstColor;
    }
    private void ClearShield() {
        this.shield.sprite = null;
        this.shield.color = Color.clear;
    }
    public void SetInteraction(IInteractable interactable) {
        /*interaction = interactable;
        uiCanvas.ChangeInteractButton(interactable.UISprite);*/
        interactionList.Add(interactable);
        if(interactionList.Count > 0) {
            uiCanvas.ChangeInteractButton(interactionList[interactionList.Count - 1].UISprite);
        }
        else {
            uiCanvas.ClearInteractButton();
        }
    }
    public void ClearInteraction(IInteractable interactable) {
        /*
        interaction = null;
        if(uiCanvas) {
            uiCanvas.ClearInteractButton();
        }        */
        interactionList.Remove(interactable);
        if(interactionList.Count > 0) {
            uiCanvas.ChangeInteractButton(interactionList[interactionList.Count - 1].UISprite);
        }
        else {
            uiCanvas.ClearInteractButton();
        }
    }
    public void Interact() {
        /*
        if(interaction != null) {
            interaction.Interact();
        }*/
        if(interactionList.Count > 0) {
            interactionList[interactionList.Count - 1].Interact();
        }
    }
}

public enum Race {
    Levona, Satian, Crevalonian, Othani, Pelthonese, Helgafelli, Yoseon, Qotush, Milona, Vilgerosi, Nastac, Havellian
}
