using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculateDamage {

    public static float Calculate(float rawDamage, AttackType attackType, Stats defender) {
        float damage = 0f;
        switch(attackType) {
            case AttackType.Bash:
                damage = CalculateDecreasedDamage(rawDamage, defender.bashDefence);
                break;
            case AttackType.Pierce:
                damage = CalculateDecreasedDamage(rawDamage, defender.pierceDefence);
                break;
            case AttackType.Slash:
                damage = CalculateDecreasedDamage(rawDamage, defender.slashDefence);
                break;
            case AttackType.Fire:
                damage = CalculateDecreasedDamage(rawDamage, defender.fireDefence);
                break;
            case AttackType.Ice:
                damage = CalculateDecreasedDamage(rawDamage, defender.iceDefence);
                break;
            case AttackType.Lightning:
                damage = CalculateDecreasedDamage(rawDamage, defender.lightningDefence);
                break;
            case AttackType.Air:
                damage = CalculateDecreasedDamage(rawDamage, defender.airDefence);
                break;
            case AttackType.Earth:
                damage = CalculateDecreasedDamage(rawDamage, defender.earthDefence);
                break;
            case AttackType.Light:
                if(Random.Range(0, 10) > 5/*defender.GetComponent<StatusEffects>().lit*/) {
                    damage = rawDamage;
                }
                else {
                    damage = CalculateDecreasedDamage(rawDamage, defender.lightDefence);
                }
                break;
            case AttackType.Dark:
                damage = CalculateDecreasedDamage(rawDamage, defender.darkDefence);
                break;
            case AttackType.Poison:
                damage = CalculateDecreasedDamage(rawDamage, defender.poisonDefence);
                break;
            case AttackType.Bleed:
                damage = CalculateDecreasedDamage(rawDamage, defender.bleedDefence);
                break;
            case AttackType.Curse:
                damage = CalculateDecreasedDamage(rawDamage, defender.curseDefence);
                break;
            case AttackType.None:
                break;
            default:
                Debug.LogError("No AttackType found");
                break;
        }
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
        float damageReductionRate = (Mathf.Log10(armor) - 0.9f + armor * 0.02f) * 0.2f;
        damage *= 1f - damageReductionRate;
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
