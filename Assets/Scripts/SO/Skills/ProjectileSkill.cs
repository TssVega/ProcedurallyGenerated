using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Projectile")]
public class ProjectileSkill : ActiveSkill {

    public ProjectileData projectileData;

    public override void Activate(StatusEffects targetStatus, Stats attackerStats) {
        
        if(targetStatus) {
            targetStatus.TakeDamage(
                projectileData.damageRate * DamageFromDamageType.GetDamage(projectileData.attackType, attackerStats),
                projectileData.attackType, this, attackerStats.status);
        }
    }
}
