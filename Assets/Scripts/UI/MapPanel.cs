using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class MapPanel : MonoBehaviour {

    private const int worldSize = 128;
    private WorldGeneration worldGeneration;

    private bool mapSet = false;
    private Color32 backgroundColor = new Color32(219, 176, 107, 255);
    public Texture2D mapTexture;

    public Color rockyColor;
    public Color volcanicColor;
    public Color glacialColor;
    public Color crystalColor;
    public Color coveredColor;

    public Image cursor;

    private void Awake() {
        //mapTexture = mapImage.mainTexture;
        worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    public void SetCursor(Vector2Int coordinates) {
        Vector3 screenPosition = new Vector3(coordinates.x * 2 - 127, coordinates.y * 2 - 127, 0);
        cursor.rectTransform.anchoredPosition = screenPosition;
    }
    public void Set() {
        if(!mapSet) {
            mapSet = true;
            SetMap(worldGeneration.WorldMap);
        }        
    }
    public void NewExploration(Vector2Int coordinates, int[,] worldMap) {
        switch(worldMap[coordinates.x, coordinates.y]) {
            case 0:
                mapTexture.SetPixel(coordinates.x, coordinates.y, backgroundColor);
                break;
            case 1:
                mapTexture.SetPixel(coordinates.x, coordinates.y, rockyColor);
                break;
            case 2:
                mapTexture.SetPixel(coordinates.x, coordinates.y, volcanicColor);
                break;
            case 3:
                mapTexture.SetPixel(coordinates.x, coordinates.y, glacialColor);
                break;
            case 4:
                mapTexture.SetPixel(coordinates.x, coordinates.y, crystalColor);
                break;
        }
        mapTexture.Apply();
    }
    private void SetMap(int[,] worldMap) {
        for(int x = 0; x < worldSize; x++) {
            for(int y = 0; y < worldSize; y++) {
                if(worldGeneration.ExplorationData[x, y] != 1) {
                    mapTexture.SetPixel(x, y, coveredColor);
                    continue;
                }
                switch(worldMap[x, y]) {
                    case 0:
                        mapTexture.SetPixel(x, y, backgroundColor);
                        break;
                    case 1:
                        mapTexture.SetPixel(x, y, rockyColor);
                        break;
                    case 2:
                        mapTexture.SetPixel(x, y, volcanicColor);
                        break;
                    case 3:
                        mapTexture.SetPixel(x, y, glacialColor);
                        break;
                    case 4:
                        mapTexture.SetPixel(x, y, crystalColor);
                        break;
                }
            }
        }
        mapTexture.Apply();
    }
}
