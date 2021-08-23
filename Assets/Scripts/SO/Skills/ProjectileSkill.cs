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
                targetStatus.StartStun(projectileData.immobilizeDuration);
            }
        }
    }
}
