using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile")]
public class ProjectileSkill : ActiveSkill {

    public ProjectileData projectileData;

    public override void Activate(StatusEffects targetStatus, Stats attackerStats) {
        
        if(targetStatus) {
            targetStatus.TakeDamage(
                projectileData.damageRate * DamageFromDamageType.GetDamage(attackType, attackerStats),
                attackType, this, attackerStats.status);
            if(projectileData.stunDuration > 0) {
                targetStatus.StartStun(projectileData.stunDuration);
            }
            if(projectileData.immobilizeDuration > 0) {
                targetStatus.StartImmobilize(projectileData.immobilizeDuration);
            }
            if(projectileData.push != 0f) {
                targetStatus.StartAirPush(projectileData.push, attackerStats.transform);
            }
            if(projectileData.poisonStacks > 0) {
                targetStatus.AddPoisonStacks(projectileData.poisonStacks, attackerStats.poisonDamage, projectileData.poisonDuration, this, attackerStats.status);
            }
            if(projectileData.bloodStacks > 0) {
                targetStatus.AddBleedStacks(projectileData.bloodStacks, attackerStats.bleedDamage, projectileData.bleedDuration, this, attackerStats.status);
            }
            if(projectileData.curseStacks > 0) {
                targetStatus.AddCurseStacks(projectileData.curseStacks, attackerStats.curseDamage, projectileData.curseDuration, this, attackerStats.status);
            }
            if(projectileData.fireStacks > 0) {
                targetStatus.AddFireStacks(projectileData.fireStacks, attackerStats.fireDamage, projectileData.fireDuration, this, attackerStats.status);
            }
            if(projectileData.iceStacks > 0) {
                targetStatus.AddIceStacks(projectileData.iceStacks, projectileData.iceDuration, projectileData.iceDamageMultiplier);
            }
            if(projectileData.lightningStacks > 0) {
                targetStatus.AddLightningStacks(projectileData.lightningStacks, attackerStats.lightningDamage, projectileData.shockDuration, this, attackerStats.status);
            }
            if(projectileData.earthStacks > 0) {
                targetStatus.AddEarthStacks(projectileData.earthStacks, projectileData.earthenHeal, projectileData.earthenDuration, attackerStats.gameObject);
            }
            if(projectileData.lightStacks > 0) {
                targetStatus.AddLightStacks(projectileData.lightStacks, projectileData.litDuration);
            }
        }
    }
}
