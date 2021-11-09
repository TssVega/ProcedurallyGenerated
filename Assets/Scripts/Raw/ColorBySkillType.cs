using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorBySkillType {

    public static Color32 GetColorByType(AttackType attackType) {
        Color32 color;
        if(attackType == AttackType.Fire) {
            color = new Color32(255, 219, 0, 255);
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
            color = new Color32(190, 0, 220, 255);
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
            color = new Color32(110, 174, 120, 255);
        }
        else if(attackType == AttackType.Pierce) {
            color = new Color32(200, 200, 200, 255);
        }
        else if(attackType == AttackType.Slash) {
            color = new Color32(251, 50, 50, 255);
        }
        else {
            color = Color.white;
        }
        return color;
    }
    public static Color32 GetColorByStat() {
        return Color.white;
    }
    public static Color32 GetColorByRarity(Rarity rarity) {
        Color32 color;
        switch(rarity) {
            case Rarity.Common:
                color = Color.white;
                break;
            case Rarity.Rare:
                color = new Color32(0, 229, 6, 255);
                break;
            case Rarity.Epic:
                color = new Color32(202, 0, 229, 255);
                break;
            case Rarity.Legendary:
                color = new Color32(255, 131, 9, 255);
                break;
            default:
                color = Color.white;
                break;
        }
        return color;
    }
}

public enum Rarity {
    Common, Rare, Epic, Legendary
}
