using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Area Skill")]
public class AreaSkill : ActiveSkill {

    public float damageRate;
    public float duration;
    public GameObject hitbox;
    public bool warn = false;
    public float stunDuration = 0f;
    public float immobilizeDuration = 0f;
    [Tooltip("Negative values pull and positive values push back")]
    [Range(-10f, 10f)]
    public float push = 0f;
    [Header("Stacks")]
    public int poisonStacks = 0;
    public float poisonDuration = 0f;
    public int bloodStacks = 0;
    public float bleedDuration = 0f;
    public int curseStacks = 0;
    public float curseDuration = 0f;
    public int fireStacks = 0;
    public float fireDuration = 0f;
    public int iceStacks = 0;
    public float iceDuration = 0f;
    public float iceDamageMultiplier = 1f;
    public int lightningStacks = 0;
    public float shockDuration = 0f;
    public int earthStacks = 0;
    public float earthenDuration = 0f;
    public float earthenHeal = 0f;
    public int lightStacks = 0;
    public float litDuration = 0f;

    public override void Activate(StatusEffects targetStatus, Stats attackerStats) {
        /*GameObject clone = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.SetActive(true);*/
        targetStatus.TakeDamage(DamageFromDamageType.GetDamage(attackType, attackerStats)
            * damageRate, attackType, this, attackerStats.status);
        if(stunDuration > 0) {
            targetStatus.StartStun(stunDuration);
        }
        if(immobilizeDuration > 0) {
            targetStatus.StartImmobilize(immobilizeDuration);
        }
        if(push != 0f) {
            targetStatus.StartAirPush(push, attackerStats.transform);
        }        
        if(poisonStacks > 0) {
            targetStatus.AddPoisonStacks(poisonStacks, attackerStats.poisonDamage, poisonDuration, this, attackerStats.status);
        }
        if(bloodStacks > 0) {
            targetStatus.AddBleedStacks(bloodStacks, attackerStats.bleedDamage, bleedDuration, this, attackerStats.status);
        }
        if(curseStacks > 0) {
            targetStatus.AddCurseStacks(curseStacks, attackerStats.curseDamage, curseDuration, this, attackerStats.status);
        }
        if(fireStacks > 0) {
            targetStatus.AddFireStacks(fireStacks, attackerStats.fireDamage, fireDuration, this, attackerStats.status);
        }
        if(iceStacks > 0) {
            targetStatus.AddIceStacks(iceStacks, iceDuration, iceDamageMultiplier);
        }
        if(lightningStacks > 0) {
            targetStatus.AddLightningStacks(lightningStacks, attackerStats.lightningDamage, shockDuration, this, attackerStats.status);
        }
        if(earthStacks > 0) {
            targetStatus.AddEarthStacks(earthStacks, earthenHeal, earthenDuration, attackerStats.gameObject);
        }
        if(lightStacks > 0) {
            targetStatus.AddLightStacks(lightStacks, litDuration);
        }
    }
    public override void Launch(Stats attackerStats) {
        GameObject hitboxObject = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        hitboxObject.transform.position = attackerStats.transform.position;
        hitboxObject.transform.rotation = attackerStats.transform.rotation;
        Hitbox h = hitboxObject.GetComponent<Hitbox>();
        h.SetTeam(attackerStats.team);
        h.SetSkill(this);
        h.SetAttackerStats(attackerStats);
        hitboxObject.SetActive(true);
    }
    /*
    private IEnumerator Persistence() {
    
    }*/
}
