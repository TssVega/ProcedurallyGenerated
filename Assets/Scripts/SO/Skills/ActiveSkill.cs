using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill {

    public int manaCost;
    public float channelingTime;
    public float castTime;
    public float cooldown;

    protected virtual void Activate(Stats target) {
    
    }
}
