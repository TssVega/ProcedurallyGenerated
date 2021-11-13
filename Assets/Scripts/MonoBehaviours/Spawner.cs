using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public static Spawner spawner;

    public bool spawn = true;

    private Biome currentBiome;

    public List<Enemy> rockyEntities;
    public List<GameObject> entities;

    private int density;

    private List<LevelGeneration> sideLevels;

    private Transform playerTransform;

    private float spawnCounter;
    private float vendorCounter;
    private float bossCounter;

    private const float spawnTime = 1f;
    private const int maxDensity = 10;
    private const float vendorTime = 300f;
    private const float vendorDisableDistance = 15f;
    private const float bossTime = 600f;

    private Vendor vendor;

    private Player player;

    private GameObject bossGate;

    private void Awake() {
        player = FindObjectOfType<Player>();
        spawnCounter = spawnTime;
        vendorCounter = vendorTime;
        bossCounter = bossTime;
        sideLevels = new List<LevelGeneration>();
        spawner = this;
        playerTransform = FindObjectOfType<Player>().transform;
    }
    private void Start() {
        density = 0;
    }
    private void Update() {
        if(bossCounter > 0f) {
            bossCounter -= Time.deltaTime;
        }
        if(spawnCounter > 0f) {
            spawnCounter -= Time.deltaTime;
        }
        if(vendorCounter > 0f) {
            vendorCounter -= Time.deltaTime;
        }
        if(bossCounter <= 0f && !player.bossAwake && bossGate && bossGate.gameObject.activeInHierarchy && Vector3.Distance(bossGate.transform.position, playerTransform.position) > vendorDisableDistance) {
            ClearBossGate();
            SpawnBossGate();
            bossCounter = bossTime;
        }
        else if(bossCounter <= 0f && !player.bossAwake && !bossGate) {
            SpawnBossGate();
            bossCounter = bossTime;
        }
        if(spawnCounter <= 0f && spawn && density < maxDensity) {
            spawnCounter = spawnTime;
            SpawnEntity();
        }
        if(vendorCounter <= vendorTime * 0.5f) {
            DisableVendor();
        }
        if(vendorCounter <= 0f/* && Vector3.Distance(vendor.transform.position, playerTransform.position) > vendorDisableDistance*/) {
            if(vendor && Vector3.Distance(vendor.transform.position, playerTransform.position) < vendorDisableDistance) {
                return;
            }
            SpawnVendor();
            vendorCounter = vendorTime;         
        }
    }
    private void SpawnBossGate() {
        LevelGeneration level = null;
        if(sideLevels.Count > 0) {
            level = sideLevels[Random.Range(0, sideLevels.Count)];
        }
        if(!level) {
            return;
        }
        GameObject bossGate = ObjectPooler.objectPooler.GetPooledObject("BossGate");
        bossGate.transform.position = level.GetRandomLocation();
        bossGate.transform.rotation = Quaternion.identity;
        this.bossGate = bossGate;
        bossGate.SetActive(true);
    }
    private void ClearBossGate() {
        if(bossGate) {
            bossGate.SetActive(false);
            bossGate = null;
        }        
    }
    private void SpawnVendor() {
        LevelGeneration level = null;
        if(sideLevels.Count > 0) {
            level = sideLevels[Random.Range(0, sideLevels.Count)];
        }
        if(!level) {
            return;
        }
        GameObject vendorObj = ObjectPooler.objectPooler.GetPooledObject("Vendor");
        vendorObj.transform.position = level.GetRandomLocation();
        vendorObj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 359f));
        vendor = vendorObj.GetComponent<Vendor>();
        vendorObj.SetActive(true);
    }
    private void DisableVendor() {
        if(vendor && vendor.gameObject.activeInHierarchy && Vector3.Distance(vendor.transform.position, playerTransform.position) > vendorDisableDistance) {
            vendor.gameObject.SetActive(false);
        }        
    }
    public void AddSideLevel(LevelGeneration sideLevel) {
        if(sideLevel.layout.worldCoordinates.x == 0 && sideLevel.layout.worldCoordinates.y == 0) {
            return;
        }
        sideLevels.Add(sideLevel);
    }
    public void RemoveSideLevel(LevelGeneration sideLevel) {
        if(!sideLevels.Contains(sideLevel)) {
            return;
        }
        sideLevels.Remove(sideLevel);
        for(int i = 0; i < entities.Count; ++i) {
            /*
            if(Vector3.Distance(playerTransform.position, entities[i].transform.position) > disableDistance) {
                entities[i].SetActive(false);
                RemoveEntity(entities[i]);
                --i;
                //continue;
            }*/
            if(entities[i].GetComponent<EnemyAI>().level == null || !entities[i].GetComponent<EnemyAI>().level.gameObject.activeInHierarchy) {
                entities[i].SetActive(false);
                RemoveEntity(entities[i]);
                --i;
            }
        }
    }
    public void ClearSideLevels() {
        sideLevels.Clear();
    }
    public void RemoveEntity(GameObject entity) {
        entities.Remove(entity);
        density--;
    }
    public void AddEntity(GameObject entity) {
        entities.Add(entity);
        density++;
        CheckEntities();
    }
    private void CheckEntities() {
        for(int i = 0; i < entities.Count; i++) {
            if(!entities[i].activeInHierarchy) {
                RemoveEntity(entities[i]);
                --i;
            }
        }
    }
    private void SpawnEntity() {
        LevelGeneration level = null;
        if(sideLevels.Count > 0) {
            level = sideLevels[Random.Range(0, sideLevels.Count)];
        }
        if(!level) {
            return;
        }
        currentBiome = (Biome)level.layout.biomeIndex - 1;
        switch(currentBiome) {            
            case Biome.Rock:                
                int diceResult = RollDice(4, 6);
                int index = 2;
                // Rock rabbits
                if(diceResult <= 6) {
                    index = 2;
                }
                // Birds
                else if(diceResult <= 11) {
                    index = 3;
                }
                // Rock bats
                else if(diceResult <= 14) {
                    index = 0;
                }
                // Nopes
                else if(diceResult < 16) {
                    index = 1;
                }
                GameObject entity = ObjectPooler.objectPooler.GetPooledObject(rockyEntities[index].name);
                entity.GetComponent<EnemyAI>().SetLevel(level);
                entity.transform.position = level.GetRandomLocation();
                entity.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 359f));                
                entity.SetActive(true);
                AddEntity(entity);
                break;
            case Biome.Volcanic:
                break;
            case Biome.Glacial:
                break;            
            case Biome.Crystal:
                break;
            default: break;
        }
    }
    private int RollDice(int diceCount, int diceMax) {
        int diceRollTotal = 0;
        // Roll diceCount d diceMax
        for(int i = 0; i < diceCount; i++) {
            int dieResult = Random.Range(1, diceMax + 1);
            diceRollTotal += dieResult;
        }
        return diceRollTotal;
    }
}

public enum Biome {
    Rock, Volcanic, Glacial, Crystal
}
