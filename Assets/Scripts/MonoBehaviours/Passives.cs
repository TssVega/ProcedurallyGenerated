using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour {

    public PassiveSkill toughSkin;
    public PassiveSkill lifeVamp;
    public PassiveSkill clarity;

    private float toughSkinCounter;
    private bool toughSkinOnline;
    private SkillUser skillUser;
    private StatusEffects status;

    private const float toughSkinCooldown = 10f;
    private const float lifeVampHeal = 8f;
    private const float clarityDamageBoostRate = 1.2f;

    private void Awake() {
        status = GetComponent<StatusEffects>();
        skillUser = GetComponent<SkillUser>();
        toughSkinCounter = 0f;
        toughSkinOnline = false;
    }
    private void OnEnable() {
        if(skillUser.acquiredSkills.Contains(toughSkin)) {
            toughSkinCounter = toughSkinCooldown;
            toughSkinOnline = true;
        }
        else {
            toughSkinOnline = false;
        }
    }
    private void Update() {
        if(skillUser.acquiredSkills.Contains(toughSkin) && toughSkinCounter > 0) {
            toughSkinCounter -= Time.deltaTime;
        }
        else if(skillUser.acquiredSkills.Contains(toughSkin)) {
            toughSkinOnline = true;
        }
    }
    public float OnHitTaken(float damage, AttackType type, Stats defender) {        
        float reducedDamage = damage;
        if(skillUser.acquiredSkills.Contains(toughSkin) && toughSkinOnline) {
            reducedDamage = damage / (defender.vitality * 0.2f);
            toughSkinOnline = false;
            toughSkinCounter = toughSkinCooldown;
        }
        else if(skillUser.acquiredSkills.Contains(toughSkin) && !toughSkinOnline) {
            toughSkinCounter = toughSkinCooldown;
        }
        return reducedDamage;
    }
    public float OnHit(float damage) {
        if(skillUser.acquiredSkills.Contains(clarity)) {
            damage *= clarityDamageBoostRate;
        }
        return damage;
    }
    public void OnEnemyKill() {
        if(skillUser.acquiredSkills.Contains(lifeVamp)) {
            status.Heal(lifeVampHeal);
        }        
    }
}
