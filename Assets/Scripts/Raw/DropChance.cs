using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropChance {

    public float dropRate = 0.1f;
    public int dropRange = 1;   // Drop count between Random.Range(1, dropRange + 1);
    public Item item;
}
