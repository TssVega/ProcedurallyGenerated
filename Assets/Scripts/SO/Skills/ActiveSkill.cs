using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill {

    public int manaCost;
    public bool focusedSkill;
    public float channelingTime;
    public float castTime;
    public float cooldown;
    public string[] channelingParticleNames;
    public string[] castingParticleNames;
    public string[] particleNames;
    public string channelingAnimationName;
    public string castingAnimationName;

    public virtual void Launch(StatusEffects status) {
    
    }
    public virtual void Activate(StatusEffects target, Stats targetStats) {
    
    }
}
