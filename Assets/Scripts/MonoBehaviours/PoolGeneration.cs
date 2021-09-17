using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGeneration : MonoBehaviour {

    private System.Random pseudoRandomForPools;

    private LevelGeneration levelGeneration;

    public PoolData poolData;

    public Pool poolObject;

    public bool hasPool;
    public bool full;
    public bool healthPool; // True is health pool and false is mana pool

    private const int poolChance = 100;

    private List<GameObject> pools;

    private void Awake() {
        levelGeneration = GetComponent<LevelGeneration>();
        pools = new List<GameObject>();
    }
    public void GeneratePools(int slot, string seed) {
        pseudoRandomForPools = new System.Random(seed.GetHashCode());
        poolData = SaveSystem.LoadPools(slot, levelGeneration.layout.worldCoordinates);
        if(poolData != null) {
            hasPool = poolData.hasPool;
            full = poolData.full;
            healthPool = poolData.healthPool;
        }
        else {
            hasPool = pseudoRandomForPools.Next(0, 100) < poolChance;
            full = hasPool;
            healthPool = pseudoRandomForPools.Next(0, 2) == 0;
        }
        if(hasPool) {
            Pool pool = ObjectPooler.objectPooler.GetPooledObject(poolObject.name).GetComponent<Pool>();
            pool.SetPool(healthPool, full, hasPool, this);
            Vector3Int randomLocation = Vector3Int.zero;
            bool valid = false;
            while(!valid) {
                // pseudoRandomForMushrooms.Next(3, levelGeneration.layout.levelSize - 4)
                randomLocation = new Vector3Int(pseudoRandomForPools.Next(3, levelGeneration.layout.levelSize - 4), pseudoRandomForPools.Next(3, levelGeneration.layout.levelSize - 4), 0);
                if(levelGeneration.CheckLocation(randomLocation.x, randomLocation.y) && !levelGeneration.occupiedCoordinates.Contains(randomLocation)) {
                    valid = true;
                }
            }
            levelGeneration.occupiedCoordinates.Add(randomLocation);
            pool.transform.position = levelGeneration.GetPreciseLocation(randomLocation.x, randomLocation.y);
            pool.transform.rotation = Quaternion.identity;
            pools.Add(pool.gameObject);
            pool.gameObject.SetActive(true);
        }
    }
    public void SavePools(int slot) {
        SaveSystem.SavePools(this, slot, levelGeneration.layout.worldCoordinates);
    }
    public void ClearPools() {
        SavePools(0);
        for(int i = 0; i < pools.Count; i++) {
            pools[i].SetActive(false);
        }
        pools.Clear();
        levelGeneration.occupiedCoordinates.Clear();
    }
}
