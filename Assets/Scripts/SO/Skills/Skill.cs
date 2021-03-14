using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject{

    public int skillIndex;
    public string skillName;
    [TextArea(3, 20)]public string skillDescription;
    public Sprite skillIcon;
    public bool interruptable;
}
