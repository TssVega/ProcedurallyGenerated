using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Buff")]
public class BuffSkill : ActiveSkill {

    public BuffData buffData;

    private const float healMultiplier = 2f;

    public override void Launch(StatusEffects status, Stats stats) {
        if(buffData.buffType == BuffType.Heal) {
            status.Heal(stats.faith * buffData.healRate * healMultiplier);
        }
        else if(buffData.buffType == BuffType.Shield) {
            status.StartShield(buffData.damageReductionRate, buffData.lifetime);
        }
        else if(buffData.buffType == BuffType.RestoreLostHealth) {
            status.Heal(status.GetRecentlyTakenDamage() * buffData.restoreRate);
        }
        else if(buffData.buffType == BuffType.HunterAid) {
            status.Heal(stats.dexterity * buffData.healRate * healMultiplier);
        }
    }
    public override void Activate(StatusEffects target, Stats attackerStats) {
        // Heal target here
    }
}
