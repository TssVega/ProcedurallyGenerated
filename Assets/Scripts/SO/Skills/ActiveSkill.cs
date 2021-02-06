using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill {

    public int manaCost;
    public float channelingTime;
    public float castTime;
    public float cooldown;

    public virtual void Launch(StatusEffects status) {
    
    }
    public virtual void Activate(StatusEffects target, Stats targetStats) {
    
    }
}
