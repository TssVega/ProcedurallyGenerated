using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill, IUsable {

    public int manaCost;
    public bool focusedSkill;
    public float channelingTime;
    public float castTime;
    public float cooldown;
    public float aiRange;
    public string[] channelingParticleNames;
    public string[] particleNames;
    public string channelingAnimationName;
    public string castingAnimationName;
    public string idleAnimationName;

    
    public virtual void Launch(Stats stats) {
    
    }
    public virtual void Launch(StatusEffects targetStatus) {

    }
    public virtual void Launch(StatusEffects targetStatus, Stats stats) {

    }
    public virtual void Activate(StatusEffects target, Stats attackerStats) {
    
    }
}
