using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Skill Database")]
public class SkillDatabase : ScriptableObject {

    public List<Skill> skills;
}
