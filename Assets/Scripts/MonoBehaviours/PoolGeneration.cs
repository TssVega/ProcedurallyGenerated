using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGeneration : MonoBehaviour {

    private System.Random pseudoRandomForPools;
    private System.Random pseudoRandomForPortals;

    private LevelGeneration levelGeneration;

    public PoolData poolData;

    public Pool poolObject;

    public bool hasPool;
    public bool full;
    public bool healthPool; // True is health pool and false is mana pool
    public bool hasPortal;
    public bool usedPortal;

    private const int poolChance = 4;
    private const int portalChance = 2;

    private List<GameObject> pools;
    public List<GameObject> portals;

    private void Awake() {
        levelGeneration = GetComponent<LevelGeneration>();
        pools = new List<GameObject>();
        portals = new List<GameObject>();
    }
    public void GeneratePools(int slot, string seed) {
        pseudoRandomForPools = new System.Random(seed.GetHashCode());
        pseudoRandomForPortals = new System.Random(seed.GetHashCode());
        poolData = SaveSystem.LoadPools(slot, levelGeneration.layout.worldCoordinates);
        if(poolData != null) {
            hasPool = poolData.hasPool;
            full = poolData.full;
            healthPool = poolData.healthPool;
            hasPortal = poolData.hasPortal;
            usedPortal = poolData.usedPortal;
            // Get two next pseudo numbers if there are pool data
            // This is to prevent the pool to swapn in a different position when you come back to it
            pseudoRandomForPools.Next(0, 100);
            pseudoRandomForPools.Next(0, 2);
            bool x = levelGeneration.layout.worldCoordinates.x == 0;
            bool y = levelGeneration.layout.worldCoordinates.y == 0;
            if(x && y) {
                pseudoRandomForPortals.Next(0, 100);
            }
            pseudoRandomForPortals.Next(0, 100);            
        }
        else {
            hasPool = pseudoRandomForPools.Next(0, 100) < poolChance;
            full = hasPool;
            healthPool = pseudoRandomForPools.Next(0, 2) == 0;
            bool x = levelGeneration.layout.worldCoordinates.x == 0;
            bool y = levelGeneration.layout.worldCoordinates.y == 0;
            if(x && y) {
                pseudoRandomForPortals.Next(0, 100);
            }
            hasPortal = (x && y) || pseudoRandomForPortals.Next(0, 100) < portalChance;
            usedPortal = false;
        }
        if(hasPool) {
            Pool pool = ObjectPooler.objectPooler.GetPooledObject(poolObject.name).GetComponent<Pool>();            
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
            pool.SetPool(healthPool, full, hasPool, this);
        }
        if(hasPortal) {
            Portal portal = ObjectPooler.objectPooler.GetPooledObject("PortalInGame").GetComponent<Portal>();
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
            portal.transform.position = levelGeneration.GetPreciseLocation(randomLocation.x, randomLocation.y);
            portal.transform.rotation = Quaternion.identity;
            portals.Add(portal.gameObject);
            portal.gameObject.SetActive(true);
            portal.SetPortal(usedPortal, seed, this);
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
        portals.Clear();
    }
}
