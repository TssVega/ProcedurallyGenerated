﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData {

    public int team;
    public AttackType attackType;
    public bool arrowSkill;     // Arrow skills can only be used with bows
    public float damageRate;
    public float rotationSpeed;
    public float projectileSpeed;
    public float hitTime;
    public float lifetime;
}
