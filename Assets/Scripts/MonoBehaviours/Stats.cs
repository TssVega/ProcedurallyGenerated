using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    [Header("Status")]
    public float health;
    public float mana;
    public float energy;
    public float maxHealth;
    public float maxMana;
    public float maxEnergy;
    public float runSpeed;
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
    [Header("Stack Counter Thresholds")]
    public int burningThreshold;
    public int earthingThreshold;
    public int frostbiteThreshold;
    public int shockThreshold;
    public int lightingThreshold;
    public int poisonThreshold;
    public int bleedThreshold;
    public int curseThreshold;
    //[Header("Skills")]
    //public List<Skill> skills;
    

    private void Awake() {
        //int skillCooldownsSize = FindObjectOfType<GameMaster>().talentDatabase.talents.Length;
        //SkillCooldowns = new List<float>(skillCooldownsSize);
    }
    
}
