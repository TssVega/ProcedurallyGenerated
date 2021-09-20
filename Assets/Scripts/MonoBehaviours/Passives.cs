using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour {

    public PassiveSkill toughSkin;
    private float toughSkinCounter;
    private bool toughSkinOnline;
    private SkillUser skillUser;
    private const float toughSkinCooldown = 10f;

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
            reducedDamage = damage / (defender.vitality * 0.2f);
            toughSkinOnline = false;
            toughSkinCounter = toughSkinCooldown;
        }
        else if(skillUser.acquiredSkills.Contains(toughSkin) && !toughSkinOnline) {
            toughSkinCounter = toughSkinCooldown;
        }
        return reducedDamage;
    }
}
