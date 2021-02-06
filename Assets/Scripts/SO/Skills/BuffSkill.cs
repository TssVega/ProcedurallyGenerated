using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Buff")]
public class BuffSkill : ActiveSkill {

    public BuffData buffData;

    public override void Activate(StatusEffects target, Stats attackerStats) {
        // Heal target here
    }
}
