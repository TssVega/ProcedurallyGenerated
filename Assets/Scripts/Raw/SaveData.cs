using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    // Save slot
    public int saveSlot;
    // Race
    public int race;
    // Inventory buttons
    public bool skillBookUnlocked;
    public bool mapUnlocked;
    // Transform
    public float[] position;
    public float[] rotation;
    // Appearance
    public int skinColorIndex;
    public int hairColorIndex;
    public int hairStyleIndex;
    // Status
    public float health;
    public float mana;
    public float energy;
    public float maxHealth;
    public float maxMana;
    public float maxEnergy;
    public float runSpeed;
    // Main
    public int strength;
    public int agility;
    public int dexterity;
    public int intelligence;
    public int faith;
    public int wisdom;
    public int vitality;
    public int charisma;
    // Damages
    public float bashDamage;
    public float pierceDamage;
    public float slashDamage;
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    public float airDamage;
    public float earthDamage;
    public float lightDamage;
    public float darkDamage;
    public float poisonDamage;
    public float bleedDamage;
    public float curseDamage;
    // Defences
    public float bashDefence;
    public float pierceDefence;
    public float slashDefence;
    public float fireDefence;
    public float iceDefence;
    public float lightningDefence;
    public float airDefence;
    public float earthDefence;
    public float lightDefence;
    public float darkDefence;
    public float poisonDefence;
    public float bleedDefence;
    public float curseDefence;
    // Other
    public int statPoints;
    public float maxDamageTimesHealth;
    public float parryDuration;
    public float blockDuration;
    public float damageBlockRate;
    public int luck;
    // Thresholds
    public int burningThreshold;
    public int earthingThreshold;
    public int frostbiteThreshold;
    public int shockThreshold;
    public int lightingThreshold;
    public int poisonThreshold;
    public int bleedThreshold;
    public int curseThreshold;
    // Inventory and equipment
    public string[] inventory;
    public string[] equipment;
    public int[] inventoryQuantities;
    // Skills
    public int[] acquiredSkills;
    public int[] currentSkills;
    // Consumable items in skill slots
    public string[] consumableItems;

    public SaveData(Player data) {
        saveSlot = data.saveSlot;
        // Race
        race = data.raceIndex;
        // Inventory buttons
        skillBookUnlocked = data.skillBookUnlocked;
        mapUnlocked = data.mapUnlocked;
        // Inventory and equipment
        inventory = new string[data.inventory.InventorySize];
        for(int i = 0; i < data.inventory.InventorySize; i++) {
            if(data.inventory.inventory[i]) {
                inventory[i] = data.inventory.inventory[i].seed;
            }
            else {
                inventory[i] = null;
            }
        }
        equipment = new string[data.inventory.EquipmentSize];
        for(int i = 0; i < data.inventory.EquipmentSize; i++) {
            if(data.inventory.equipment[i]) {
                equipment[i] = data.inventory.equipment[i].seed;
            }
            else {
                equipment[i] = null;
            }
        }
        inventoryQuantities = new int[data.inventory.InventorySize];
        for(int i = 0; i < data.inventory.InventorySize; i++) {
            inventoryQuantities[i] = data.inventory.quantities[i];
        }
        // Position and world data
        position = new float[3];
        position[0] = data.transform.position.x;
        position[1] = data.transform.position.y;
        position[2] = data.transform.position.z;
        rotation = new float[4];
        rotation[0] = data.transform.rotation.x;
        rotation[1] = data.transform.rotation.y;
        rotation[2] = data.transform.rotation.z;
        rotation[3] = data.transform.rotation.w;
        // Appearance
        hairColorIndex = data.hairColorIndex;
        hairStyleIndex = data.hairStyleIndex;
        // Status
        health = data.stats.health;
        mana = data.stats.mana;
        energy = data.stats.energy;
        maxHealth = data.stats.maxHealth;
        maxMana = data.stats.maxMana;
        maxEnergy = data.stats.maxEnergy;
        runSpeed = data.stats.runSpeed;
        // Main
        strength = data.stats.strength;
        agility = data.stats.agility;
        dexterity = data.stats.dexterity;
        intelligence = data.stats.intelligence;
        faith = data.stats.faith;
        wisdom = data.stats.wisdom;
        vitality = data.stats.vitality;
        charisma = data.stats.charisma;
        // Damages
        bashDamage = data.stats.bashDamage;
        pierceDamage = data.stats.pierceDamage;
        slashDamage = data.stats.slashDamage;
        fireDamage = data.stats.fireDamage;
        iceDamage = data.stats.iceDamage;
        lightningDamage = data.stats.lightningDamage;
        airDamage = data.stats.airDamage;
        earthDamage = data.stats.earthDamage;
        lightDamage = data.stats.lightDamage;
        darkDamage = data.stats.darkDamage;
        poisonDamage = data.stats.poisonDamage;
        bleedDamage = data.stats.bleedDamage;
        curseDamage = data.stats.curseDamage;
        // Defences
        bashDefence = data.stats.bashDefence;
        pierceDefence = data.stats.pierceDefence;
        slashDefence = data.stats.slashDefence;
        fireDefence = data.stats.fireDefence;
        iceDefence = data.stats.iceDefence;
        lightningDefence = data.stats.lightningDefence;
        airDefence = data.stats.airDefence;
        earthDefence = data.stats.earthDefence;
        lightDefence = data.stats.lightDefence;
        darkDefence = data.stats.darkDefence;
        poisonDefence = data.stats.poisonDefence;
        bleedDefence = data.stats.bleedDefence;
        curseDefence = data.stats.curseDefence;
        // Other
        statPoints = data.stats.statPoints;
        maxDamageTimesHealth = data.stats.maxDamageTimesHealth;
        parryDuration = data.stats.parryDuration;
        blockDuration = data.stats.blockDuration;
        damageBlockRate = data.stats.damageBlockRate;
        luck = data.stats.luck;
        // Thresholds
        burningThreshold = data.stats.burningThreshold;
        earthingThreshold = data.stats.earthingThreshold;
        frostbiteThreshold = data.stats.frostbiteThreshold;
        shockThreshold = data.stats.shockThreshold;
        lightingThreshold = data.stats.lightingThreshold;
        poisonThreshold = data.stats.poisonThreshold;
        bleedThreshold = data.stats.bleedThreshold;
        curseThreshold = data.stats.curseThreshold;
        // Skills
        acquiredSkills = new int[data.skillUser.acquiredSkills.Count];
        for(int i = 0; i < data.skillUser.acquiredSkills.Count; i++) {
            if(data.skillUser.acquiredSkills[i]) {
                acquiredSkills[i] = data.skillUser.acquiredSkills[i].skillIndex;
            }
            else {
                acquiredSkills[i] = -1;
            }
        }
        currentSkills = new int[11];
        consumableItems = new string[11];
        for(int i = 0; i < 11; i++) {
            if(i >= data.skillUser.currentSkills.Length) {
                break;
            }
            if(data.skillUser.currentSkills[i] != null && data.skillUser.currentSkills[i] is ActiveSkill a) {
                // ActiveSkill a = data.skillUser.currentSkills[i] as ActiveSkill;
                currentSkills[i] = a.skillIndex;
            }
            else {
                currentSkills[i] = -1;
            }
            if(data.skillUser.currentSkills[i] != null && data.skillUser.currentSkills[i] is Mushroom m) {
                consumableItems[i] = m.seed;
            }
        }
    }
}
