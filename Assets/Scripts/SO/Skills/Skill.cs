using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject{

    public int skillIndex = -1;
    public string skillName;
    public Sprite skillIcon;
    public bool interruptable;
    public AttackType attackType;
    public int skillPointsNeeded;
    public int[] prerequisites;     // Prerequisites to unlock this skill in index form of required skills
}
