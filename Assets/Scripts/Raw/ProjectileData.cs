using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData {

    public int team;
    public bool arrowSkill;     // Arrow skills can only be used with bows
    [Range(0f, 1f)]
    public float lightIntensity = 0f;
    public float lightRadius = 1f;
    public Color lightColor;
    public float damageRate;
    public float rotationSpeed;
    public float projectileSpeed;
    public float radius;
    public float lifetime;
    public float stunDuration;
    public float immobilizeDuration;
    public bool penetrates;
    public int projectileCount = 1;
    public float angleDifference;
    public float timeDifference = 0.5f;
    public bool homing;
    [Range(-10f, 10f)]
    public float push;
    [Header("Stacks")]
    public int poisonStacks = 0;
    public float poisonDuration = 0f;
    public int bloodStacks = 0;
    public float bleedDuration = 0f;
    public int curseStacks = 0;
    public float curseDuration = 0f;
    public int fireStacks = 0;
    public float fireDuration = 0f;
    public int iceStacks = 0;
    public float iceDuration = 0f;
    public float iceDamageMultiplier = 1f;
    public int lightningStacks = 0;
    public float shockDuration = 0f;
    public int earthStacks = 0;
    public float earthenDuration = 0f;
    public float earthenHeal = 0f;
    public int lightStacks = 0;
    public float litDuration = 0f;
}
