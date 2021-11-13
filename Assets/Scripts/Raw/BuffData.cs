using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData {

    public BuffType buffType;
    [Header("Heal")]
    public float healRate;
    public float lifetime;
    [Header("Shield")]
    public float damageReductionRate = 0.9f;
    [Header("Restore lost health")]
    public float restoreRate = 0.5f;
}

public enum BuffType {
    Heal, Shield, RestoreLostHealth, HunterAid
}
