using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class Weapon : Item {

    public WeaponType weaponType;
    public WeaponPreset preset;
    // Damages
    public float bashDamage;
    public float pierceDamage;
    public float slashDamage;
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;
    public float airDamage;
    public float earthDamage;
    public float lightDamage;
    public float darkDamage;
    public float poisonDamage;
    public float bleedDamage;
    public float curseDamage;
}

public enum WeaponType {
    OneHanded, TwoHanded, Bow, Dagger
}
