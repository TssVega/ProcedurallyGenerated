using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData {

    public int team;
    public bool arrowSkill;     // Arrow skills can only be used with bows
    public float damageRate;
    public float rotationSpeed;
    public float projectileSpeed;
    public float hitTime;
    public float lifetime;
    public float stunDuration;
    public float immobilizeDuration;
    public bool penetrates;
    public int projectileCount = 1;
    public float angleDifference;
    public float timeDifference = 0.5f;
    public bool homing;
}
