using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolData {

    public bool hasPool;
    public bool full;
    public bool healthPool;

    public PoolData(PoolGeneration pool) {
        this.hasPool = pool.hasPool;
        this.full = pool.full;
        this.healthPool = pool.healthPool;
    }
}
