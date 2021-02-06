using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData {

    public GameObject attacker;
    public AttackType attackType;
    public float damageRate;
    public float rotationSpeed;
    public float projectileSpeed;
    public float lifetime;
}
