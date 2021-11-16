using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculateDamage {

    public static float Calculate(float rawDamage, AttackType attackType, Stats attacker, Stats defender) {
        float damage = 0f;
        switch(attackType) {
            case AttackType.Bash:
                rawDamage = attacker != null ? rawDamage + attacker.strength * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.bashDefence);
                break;
            case AttackType.Pierce:
                rawDamage = attacker != null ? rawDamage + attacker.agility * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.pierceDefence);
                break;
            case AttackType.Slash:
                rawDamage = attacker != null ? rawDamage + attacker.dexterity * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.slashDefence);
                break;
            case AttackType.Fire:
                rawDamage = attacker != null ? rawDamage + attacker.intelligence * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.fireDefence);
                break;
            case AttackType.Ice:
                rawDamage = attacker != null ? rawDamage + attacker.intelligence * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.iceDefence);
                break;
            case AttackType.Lightning:
                rawDamage = attacker != null ? rawDamage + attacker.intelligence * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.lightningDefence);
                break;
            case AttackType.Air:
                rawDamage = attacker != null ? rawDamage + attacker.wisdom * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.airDefence);
                break;
            case AttackType.Earth:
                rawDamage = attacker != null ? rawDamage + attacker.wisdom * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.earthDefence);
                break;
            case AttackType.Light:
                rawDamage = attacker != null ? rawDamage + attacker.faith * 2 : rawDamage;
                if(defender.GetComponent<StatusEffects>().lit) {
                    damage = rawDamage;
                }
                else {
                    damage = CalculateDecreasedDamage(rawDamage, defender.lightDefence);
                }
                break;
            case AttackType.Dark:
                rawDamage = attacker != null ? rawDamage + attacker.faith * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.darkDefence);
                break;
            case AttackType.Poison:
                rawDamage = attacker != null ? rawDamage + attacker.charisma * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.poisonDefence);
                break;
            case AttackType.Bleed:
                rawDamage = attacker != null ? rawDamage + attacker.charisma * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.bleedDefence);
                break;
            case AttackType.Curse:
                rawDamage = attacker != null ? rawDamage + attacker.charisma * 2 : rawDamage;
                damage = CalculateDecreasedDamage(rawDamage, defender.curseDefence);
                break;
            case AttackType.None:
                break;
            default:
                Debug.LogError("No AttackType");
                break;
        }
        damage = attacker.status.blessed ? damage * attacker.status.blessRate : damage;
        return damage;
    }
    /*
    [UnityEditor.MenuItem("Tools/CalculateTestDamage")]
    private static void CalculateDecreasedDamage() {
        float damage = 7f;
        float tempDamage = damage;
        float armor = 30f;
        float damageReductionRate = (Mathf.Log10(armor) - 0.9f + armor * 0.02f) * 0.2f;
        damage *= 1 - damageReductionRate;
        damage = Mathf.Clamp(damage, 0, float.MaxValue);
        Debug.Log($"{tempDamage} damage against {armor} resistance deals {damage} damage");
    }
    */
    private static float CalculateDecreasedDamage(float damage, float armor) {
        float reducedDamage;
        if(armor >= 0f) {
            reducedDamage = damage * (100f / (100f + armor));
        }
        else {
            reducedDamage = damage * (2f - 100f / (100f - armor));
        }
        /*
        float damageReductionRate = (Mathf.Log10(armor) - 0.9f + armor * 0.02f) * 0.2f;
        damage *= 1f - damageReductionRate;*/
        damage = reducedDamage;
        damage = Mathf.Clamp(damage, 0f, float.MaxValue);
        return damage;        
    }
    /*
    public static AttackType CalculateAttackType(Weapon weapon) {
        switch() {
        }
        AttackType type = weapon.enchantments[0].type;
        return type;
    }
    public static float CalculateMagicDamage(Player player, Weapon weapon) {
        return 0;
    }
    */
    // Calculator when enemy is attacking to the player
    /*
    public static float Calculate(Enemy attacker, AttackType attackType, Player defender) {
        return 0f;
    }*/
}
