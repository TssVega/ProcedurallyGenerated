using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour {

    public PassiveSkill toughSkin;
    private float toughSkinCounter;
    private readonly float toughSkinCooldown = 10f;

    private void Start() {
        if(toughSkin.acquired) {
            toughSkinCounter = toughSkinCooldown;
            toughSkin.online = true;
        }
    }
    private void Update() {
        if(toughSkin.acquired && toughSkinCounter > 0) {
            toughSkinCounter -= Time.deltaTime;
        }
        else if(toughSkin.acquired) {
            toughSkin.online = true;
        }
    }
    public float OnHitTaken(float damage, Stats attacker, Stats defender) {
        float reducedDamage = damage;
        if(toughSkin.acquired && toughSkin.online) {
            reducedDamage = damage / (defender.vitality * 0.7f);
            toughSkin.online = false;
            toughSkinCounter = toughSkinCooldown;
        }
        else if(toughSkin.acquired && !toughSkin.online) {
            toughSkinCounter = toughSkinCooldown;
        }
        return reducedDamage;
    }
}
