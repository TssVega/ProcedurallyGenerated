using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageFromDamageType {

    public static float GetDamage(AttackType type, Stats stats) {
        float damage = 0f;
        if(type == AttackType.Air) {
            damage = stats.airDamage;
        }
        if(type == AttackType.Bash) {
            damage = stats.bashDamage;
        }
        if(type == AttackType.Bleed) {
            damage = stats.bleedDamage;
        }
        if(type == AttackType.Curse) {
            damage = stats.curseDamage;
        }
        if(type == AttackType.Dark) {
            damage = stats.darkDamage;
        }
        if(type == AttackType.Earth) {
            damage = stats.earthDamage;
        }
        if(type == AttackType.Fire) {
            damage = stats.fireDamage;
        }
        if(type == AttackType.Ice) {
            damage = stats.iceDamage;
        }
        if(type == AttackType.Light) {
            damage = stats.lightDamage;
        }
        if(type == AttackType.Lightning) {
            damage = stats.lightningDamage;
        }
        if(type == AttackType.Pierce) {
            damage = stats.pierceDamage;
        }
        if(type == AttackType.Poison) {
            damage = stats.poisonDamage;
        }
        if(type == AttackType.Slash) {
            damage = stats.slashDamage;
        }
        return damage;
    }
    public static float GetDefence(AttackType type, Stats stats) {
        float defence = 0f;
        if(type == AttackType.Air) {
            defence = stats.airDefence;
        }
        if(type == AttackType.Bash) {
            defence = stats.bashDefence;
        }
        if(type == AttackType.Bleed) {
            defence = stats.bleedDefence;
        }
        if(type == AttackType.Curse) {
            defence = stats.curseDefence;
        }
        if(type == AttackType.Dark) {
            defence = stats.darkDefence;
        }
        if(type == AttackType.Earth) {
            defence = stats.earthDefence;
        }
        if(type == AttackType.Fire) {
            defence = stats.fireDefence;
        }
        if(type == AttackType.Ice) {
            defence = stats.iceDefence;
        }
        if(type == AttackType.Light) {
            defence = stats.lightDefence;
        }
        if(type == AttackType.Lightning) {
            defence = stats.lightningDefence;
        }
        if(type == AttackType.Pierce) {
            defence = stats.pierceDefence;
        }
        if(type == AttackType.Poison) {
            defence = stats.poisonDefence;
        }
        if(type == AttackType.Slash) {
            defence = stats.slashDefence;
        }
        return defence;
    }
}
