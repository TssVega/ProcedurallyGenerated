using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill {

    public int manaCost;
    public float cooldown;
    public float cooldownCounter;

    protected virtual void Activate() {
    
    }
}
