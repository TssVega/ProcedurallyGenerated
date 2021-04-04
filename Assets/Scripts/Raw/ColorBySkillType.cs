﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorBySkillType {

    public static Color32 GetColorByType(AttackType attackType) {
        Color32 color;
        if(attackType == AttackType.Fire) {
            color = new Color32(197, 25, 0, 255);
        }
        else if(attackType == AttackType.Ice) {
            color = new Color32(0, 198, 255, 255);
        }
        else if(attackType == AttackType.Lightning) {
            color = new Color32(231, 221, 0, 255);
        }
        else if(attackType == AttackType.Air) {
            color = new Color32(248, 248, 248, 255);
        }
        else if(attackType == AttackType.Earth) {
            color = new Color32(18, 123, 0, 255);
        }
        else if(attackType == AttackType.Light) {
            color = new Color32(255, 255, 165, 255);
        }
        else if(attackType == AttackType.Dark) {
            color = new Color32(97, 0, 116, 255);
        }
        else if(attackType == AttackType.Bleed) {
            color = new Color32(229, 0, 0, 255);
        }
        else if(attackType == AttackType.Poison) {
            color = new Color32(23, 229, 34, 255);
        }
        else if(attackType == AttackType.Curse) {
            color = new Color32(169, 108, 165, 255);
        }
        else if(attackType == AttackType.Bash) {
            color = new Color32(55, 87, 60, 255);
        }
        else if(attackType == AttackType.Pierce) {
            color = new Color32(222, 222, 222, 255);
        }
        else if(attackType == AttackType.Slash) {
            color = new Color32(251, 89, 89, 255);
        }
        else {
            color = Color.white;
        }
        return color;
    }
    public static Color32 GetColorByStat() {
        return Color.white;
    }
}