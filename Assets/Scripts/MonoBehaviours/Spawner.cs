using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public static Spawner spawner;

    private Biome currentBiome;

    public List<Enemy> rockyEntities;
    public List<GameObject> entities;

    private int density;

    private List<LevelGeneration> sideLevels;

    private Transform playerTransform;

    private const float disableDistance = 40f;

    private void Awake() {
        sideLevels = new List<LevelGeneration>();
        spawner = this;
        playerTransform = FindObjectOfType<Player>().transform;
    }
    private void Start() {
        density = 0;
        StartCoroutine(SpawnCounter());
    }
    public void AddSideLevel(LevelGeneration sideLevel) {
        sideLevels.Add(sideLevel);
    }
    public void RemoveSideLevel(LevelGeneration sideLevel) {
        sideLevels.Remove(sideLevel);
        for(int i = 0; i < entities.Count; i++) {
            /*
            if(Vector3.Distance(playerTransform.position, entities[i].transform.position) > disableDistance) {
                entities[i].SetActive(false);
                RemoveEntity(entities[i]);
                continue;
            }*/
            if(entities[i].GetComponent<EnemyAI>().level == null || !entities[i].GetComponent<EnemyAI>().level.gameObject.activeInHierarchy) {
                entities[i].SetActive(false);
                RemoveEntity(entities[i]);
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
    }
    private IEnumerator SpawnCounter() {
        while(true) {
            yield return new WaitForSeconds(1f);
            if(density <= 10) {
                SpawnEntity();
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
                int index = 0;
                if(diceResult <= 14) {
                    index = 0;
                }                
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
