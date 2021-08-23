using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Skill Database")]
public class SkillDatabase : ScriptableObject {

    public List<Skill> skills;
    public List<Skill> enemySkills;

    public void SetIndices() {
        for(int i = 0; i < skills.Count; i++) {
            skills[i].skillIndex = i;
            
        }
        for(int i = 0; i < enemySkills.Count; i++) {
            enemySkills[i].skillIndex = i;
        }
    }
}
