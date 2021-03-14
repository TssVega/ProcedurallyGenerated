using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour {

    public PassiveSkill toughSkin;
    private float toughSkinCounter;
    private bool toughSkinOnline;
    private SkillUser skillUser;
    private readonly float toughSkinCooldown = 10f;

    private void Awake() {
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
            reducedDamage = damage / (defender.vitality * 0.3f);
            toughSkinOnline = false;
            toughSkinCounter = toughSkinCooldown;
            Debug.Log("Tough skin activated");
        }
        else if(skillUser.acquiredSkills.Contains(toughSkin) && !toughSkinOnline) {
            toughSkinCounter = toughSkinCooldown;
        }
        return reducedDamage;
    }
}
