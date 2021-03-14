using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData {

    public BuffType buffType;
    public float healRate;
    public float lifetime;
}

public enum BuffType {
    Heal
}
