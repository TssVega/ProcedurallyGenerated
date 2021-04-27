using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour {

    private readonly int worldSize = 128;
    private WorldGeneration worldGeneration;

    private bool mapSet = false;

    private void Awake() {
        worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    public void Set() {
        if(!mapSet) {
            mapSet = true;
            SetMap(worldGeneration.WorldMap);
        }        
    }
    private void SetMap(int[,] worldMap) {
        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                GameObject mapIcon = ObjectPooler.objectPooler.GetPooledObject("MapIcon");
                mapIcon.transform.SetParent(transform);
                mapIcon.SetActive(true);
                switch(worldMap[x, y]) {
                    case 0:
                        mapIcon.GetComponent<Image>().color = Color.black;
                        break;
                    case 1:
                        mapIcon.GetComponent<Image>().color = Color.white;
                        break;
                    case 2:
                        mapIcon.GetComponent<Image>().color = Color.red;
                        break;
                    case 3:
                        mapIcon.GetComponent<Image>().color = Color.blue;
                        break;
                    case 4:
                        mapIcon.GetComponent<Image>().color = Color.magenta;
                        break;
                }
            }
        }
    }
}
