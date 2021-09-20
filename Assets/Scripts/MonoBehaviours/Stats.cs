using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int team;
    [Header("Status")]
    public float health;
    public float mana;
    public float energy;
    public float maxHealth;         // Max health saved in data before vitality and strength added
    public float maxMana;
    public float maxEnergy;
    public float runSpeed;
    public float trueMaxHealth;    // Max health after vitality and strength added
    public float trueMaxMana;
    public float trueMaxEnergy;
    [Header("Stats")]
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
    public int statPoints;
    [Header("Other Stats")]
    public float maxDamageTimesHealth;
    public float parryDuration;
    public float blockDuration;
    public float damageBlockRate;
    public int luck;
    [Header("Stack Counter Thresholds")]
    public int burningThreshold;
    public int earthingThreshold;
    public int frostbiteThreshold;
    public int shockThreshold;
    public int lightingThreshold;
    public int poisonThreshold;
    public int bleedThreshold;
    public int curseThreshold;

    public bool living;

    public StatusEffects status;
    private Enemy enemy;

    private void Awake() {
        //int skillCooldownsSize = FindObjectOfType<GameMaster>().talentDatabase.talents.Length;
        enemy = GetComponent<Enemy>();
        status = GetComponent<StatusEffects>();
    }
    private void OnEnable() {
        living = true;
        if(enemy) {
            trueMaxHealth = maxHealth + strength * 2 + vitality * 5;
            trueMaxMana = maxMana + dexterity * 2 + wisdom * 5 + intelligence * 2 + faith * 2;
            trueMaxEnergy = maxEnergy + vitality * 5 + strength * 2 + dexterity * 2 + agility * 2;
            health = trueMaxHealth;
            mana = trueMaxMana;
            energy = trueMaxEnergy;
        }
    }
    public void Die() {
        gameObject.SetActive(false);
        living = false;
    }
    public void OnItemEquip(Item item) {
        if(item is Weapon weap) {
            AddStats(weap);
        }
        if(item is Armor armor) {
            AddStats(armor);
        }
        if(item is Shield shi) {
            AddStats(shi);
        }
        if(item is Ring ring) {
            AddStats(ring);
        }
    }
    public void OnItemUnequip(Item item) {
        if(item is Weapon weap) {
            RemoveStats(weap);
        }
        if(item is Armor armor) {
            RemoveStats(armor);
        }
        if(item is Shield shi) {
            RemoveStats(shi);
        }
        if(item is Ring ring) {
            RemoveStats(ring);
        }
    }
    public void AddStats(Weapon weapon) {
        strength += weapon.strength;
        agility += weapon.agility;
        dexterity += weapon.dexterity;
        intelligence += weapon.intelligence;
        faith += weapon.faith;
        wisdom += weapon.wisdom;
        vitality += weapon.vitality;
        charisma += weapon.charisma;
        // Damages
        bashDamage += weapon.bashDamage;
        pierceDamage += weapon.pierceDamage;
        slashDamage += weapon.slashDamage;
        fireDamage += weapon.fireDamage;
        iceDamage += weapon.iceDamage;
        lightningDamage += weapon.lightningDamage;
        airDamage += weapon.airDamage;
        earthDamage += weapon.earthDamage;
        lightDamage += weapon.lightDamage;
        darkDamage += weapon.darkDamage;
        poisonDamage += weapon.poisonDamage;
        bleedDamage += weapon.bleedDamage;
        curseDamage += weapon.curseDamage;
        // Defences
        /*
        for(int i = 0; i < weapon.physicalDamageStats.Length; i++) {
            stats.physicalStats[i] += weapon.physicalDamageStats[i];
        }
        for(int i = 0; i < weapon.magicOffenceStats.Length; i++) {
            stats.magicOffenceStats[i] += weapon.magicOffenceStats[i];
        }
        for(int i = 0; i < weapon.damageOverTimeOffence.Length; i++) {
            stats.damageOverTimeStats[i] += weapon.damageOverTimeOffence[i];
        }
        for(int i = 0; i < weapon.mainStats.Length; i++) {
            stats.mainStats[i] += weapon.mainStats[i];
        }
        */
    }
    public void RemoveStats(Weapon weapon) {
        strength -= weapon.strength;
        agility -= weapon.agility;
        dexterity -= weapon.dexterity;
        intelligence -= weapon.intelligence;
        faith -= weapon.faith;
        wisdom -= weapon.wisdom;
        vitality -= weapon.vitality;
        charisma -= weapon.charisma;
        // Damages
        bashDamage -= weapon.bashDamage;
        pierceDamage -= weapon.pierceDamage;
        slashDamage -= weapon.slashDamage;
        fireDamage -= weapon.fireDamage;
        iceDamage -= weapon.iceDamage;
        lightningDamage -= weapon.lightningDamage;
        airDamage -= weapon.airDamage;
        earthDamage -= weapon.earthDamage;
        lightDamage -= weapon.lightDamage;
        darkDamage -= weapon.darkDamage;
        poisonDamage -= weapon.poisonDamage;
        bleedDamage -= weapon.bleedDamage;
        curseDamage -= weapon.curseDamage;
    }
    // Add armor stats to player on equip
    public void AddStats(Armor armor) {
        strength += armor.strength;
        agility += armor.agility;
        dexterity += armor.dexterity;
        intelligence += armor.intelligence;
        faith += armor.faith;
        wisdom += armor.wisdom;
        vitality += armor.vitality;
        charisma += armor.charisma;
        // Defences
        bashDefence += armor.bashDefence;
        pierceDefence += armor.pierceDefence;
        slashDefence += armor.slashDefence;
        fireDefence += armor.fireDefence;
        iceDefence += armor.iceDefence;
        lightningDefence += armor.lightningDefence;
        airDefence += armor.airDefence;
        earthDefence += armor.earthDefence;
        lightDefence += armor.lightDefence;
        darkDefence += armor.darkDefence;
        poisonDefence += armor.poisonDefence;
        bleedDefence += armor.bleedDefence;
        curseDefence += armor.curseDefence;
    }
    // Remove armor stats from player on unequip
    public void RemoveStats(Armor armor) {
        strength -= armor.strength;
        agility -= armor.agility;
        dexterity -= armor.dexterity;
        intelligence -= armor.intelligence;
        faith -= armor.faith;
        wisdom -= armor.wisdom;
        vitality -= armor.vitality;
        charisma -= armor.charisma;
        // Defences
        bashDefence -= armor.bashDefence;
        pierceDefence -= armor.pierceDefence;
        slashDefence -= armor.slashDefence;
        fireDefence -= armor.fireDefence;
        iceDefence -= armor.iceDefence;
        lightningDefence -= armor.lightningDefence;
        airDefence -= armor.airDefence;
        earthDefence -= armor.earthDefence;
        lightDefence -= armor.lightDefence;
        darkDefence -= armor.darkDefence;
        poisonDefence -= armor.poisonDefence;
        bleedDefence -= armor.bleedDefence;
        curseDefence -= armor.curseDefence;
    }
    // Add armor stats to player on equip
    public void AddStats(Shield shield) {
        parryDuration = shield.parryDuration;
        blockDuration = shield.blockDuration;
        damageBlockRate = shield.damageBlockRate;
        // Main
        strength += shield.strength;
        agility += shield.agility;
        dexterity += shield.dexterity;
        intelligence += shield.intelligence;
        faith += shield.faith;
        wisdom += shield.wisdom;
        vitality += shield.vitality;
        charisma += shield.charisma;
        // Defences
        bashDefence += shield.bashDefence;
        pierceDefence += shield.pierceDefence;
        slashDefence += shield.slashDefence;
        fireDefence += shield.fireDefence;
        iceDefence += shield.iceDefence;
        lightningDefence += shield.lightningDefence;
        airDefence += shield.airDefence;
        earthDefence += shield.earthDefence;
        lightDefence += shield.lightDefence;
        darkDefence += shield.darkDefence;
        poisonDefence += shield.poisonDefence;
        bleedDefence += shield.bleedDefence;
        curseDefence += shield.curseDefence;
    }
    // Remove armor stats from player on unequip
    public void RemoveStats(Shield shield) {
        parryDuration = 0.2f;
        blockDuration = 0.5f;
        damageBlockRate = 0.90f;
        // Main
        strength -= shield.strength;
        agility -= shield.agility;
        dexterity -= shield.dexterity;
        intelligence -= shield.intelligence;
        faith -= shield.faith;
        wisdom -= shield.wisdom;
        vitality -= shield.vitality;
        charisma -= shield.charisma;
        // Defences
        bashDefence -= shield.bashDefence;
        pierceDefence -= shield.pierceDefence;
        slashDefence -= shield.slashDefence;
        fireDefence -= shield.fireDefence;
        iceDefence -= shield.iceDefence;
        lightningDefence -= shield.lightningDefence;
        airDefence -= shield.airDefence;
        earthDefence -= shield.earthDefence;
        lightDefence -= shield.lightDefence;
        darkDefence -= shield.darkDefence;
        poisonDefence -= shield.poisonDefence;
        bleedDefence -= shield.bleedDefence;
        curseDefence -= shield.curseDefence;
    }
    public void AddStats(Ring ring) {
        // Main
        strength += ring.strength;
        agility += ring.agility;
        dexterity += ring.dexterity;
        intelligence += ring.intelligence;
        faith += ring.faith;
        wisdom += ring.wisdom;
        vitality += ring.vitality;
        charisma += ring.charisma;
        // Damages
        bashDamage += ring.bashDamage;
        pierceDamage += ring.pierceDamage;
        slashDamage += ring.slashDamage;
        fireDamage += ring.fireDamage;
        iceDamage += ring.iceDamage;
        lightningDamage += ring.lightningDamage;
        airDamage += ring.airDamage;
        earthDamage += ring.earthDamage;
        lightDamage += ring.lightDamage;
        darkDamage += ring.darkDamage;
        poisonDamage += ring.poisonDamage;
        bleedDamage += ring.bleedDamage;
        curseDamage += ring.curseDamage;
        // Defences
        bashDefence += ring.bashDefence;
        pierceDefence += ring.pierceDefence;
        slashDefence += ring.slashDefence;
        fireDefence += ring.fireDefence;
        iceDefence += ring.iceDefence;
        lightningDefence += ring.lightningDefence;
        airDefence += ring.airDefence;
        earthDefence += ring.earthDefence;
        lightDefence += ring.lightDefence;
        darkDefence += ring.darkDefence;
        poisonDefence += ring.poisonDefence;
        bleedDefence += ring.bleedDefence;
        curseDefence += ring.curseDefence;
    }
    // Remove armor stats from player on unequip
    public void RemoveStats(Ring ring) {
        strength -= ring.strength;
        agility -= ring.agility;
        dexterity -= ring.dexterity;
        intelligence -= ring.intelligence;
        faith -= ring.faith;
        wisdom -= ring.wisdom;
        vitality -= ring.vitality;
        charisma -= ring.charisma;
        // Damages
        bashDamage -= ring.bashDamage;
        pierceDamage -= ring.pierceDamage;
        slashDamage -= ring.slashDamage;
        fireDamage -= ring.fireDamage;
        iceDamage -= ring.iceDamage;
        lightningDamage -= ring.lightningDamage;
        airDamage -= ring.airDamage;
        earthDamage -= ring.earthDamage;
        lightDamage -= ring.lightDamage;
        darkDamage -= ring.darkDamage;
        poisonDamage -= ring.poisonDamage;
        bleedDamage -= ring.bleedDamage;
        curseDamage -= ring.curseDamage;
        // Defences
        bashDefence -= ring.bashDefence;
        pierceDefence -= ring.pierceDefence;
        slashDefence -= ring.slashDefence;
        fireDefence -= ring.fireDefence;
        iceDefence -= ring.iceDefence;
        lightningDefence -= ring.lightningDefence;
        airDefence -= ring.airDefence;
        earthDefence -= ring.earthDefence;
        lightDefence -= ring.lightDefence;
        darkDefence -= ring.darkDefence;
        poisonDefence -= ring.poisonDefence;
        bleedDefence -= ring.bleedDefence;
        curseDefence -= ring.curseDefence;
    }
}
