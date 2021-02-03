using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Buff")]
public class BuffSkill : ActiveSkill {

    public BuffData buffData;

    protected override void Activate(Stats target) {
        // Heal target here
    }
}
