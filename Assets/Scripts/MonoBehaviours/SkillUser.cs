using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser : MonoBehaviour {

    private Stats stats;

    private void Awake() {
        stats = GetComponent<Stats>();
    }

    public void UseSkill(ActiveSkill skill) {
        if(skill.cooldownCounter > 0f) {
            return;
        }
        if(skill.manaCost > stats.mana) {
            return;
        }
    }
}
