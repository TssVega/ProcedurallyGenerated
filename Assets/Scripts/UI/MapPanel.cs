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

    public GameObject portalIcon;

    private List<GameObject> portalIcons;

    private void Awake() {
        //mapTexture = mapImage.mainTexture;
        portalIcons = new List<GameObject>();
        worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    private void OnEnable() {
        SetPortalIcons();
    }
    private void OnDisable() {
        ClearPortalIcons();
    }
    public void SetCursor(Vector2Int coordinates) {
        Vector3 screenPosition = new Vector3(coordinates.x * 2 - 143, coordinates.y * 2 - 143, 0);
        cursor.rectTransform.anchoredPosition = screenPosition;
    }
    public void Set() {
        if(!mapSet) {
            mapSet = true;
            SetMap(worldGeneration.WorldMap);            
        }
    }
    public void SetPortalIcons() {
        for(int x = 0; x < worldGeneration.PortalData.GetUpperBound(0); x++) {
            for(int y = 0; y < worldGeneration.PortalData.GetUpperBound(1); y++) {
                if(worldGeneration.PortalData[x, y]) {
                    Vector3 screenPosition = new Vector3(x * 2 - 143, y * 2 - 143, 0);
                    GameObject portalIcon = ObjectPooler.objectPooler.GetPooledObject("PortalIcon");
                    portalIcon.transform.SetParent(transform);
                    RectTransform rt = portalIcon.GetComponent<RectTransform>();
                    rt.anchoredPosition = screenPosition;
                    rt.localScale = Vector3.one;
                    portalIcons.Add(portalIcon);
                    portalIcon.GetComponent<PortalTeleport>().SetPortal(new Vector2Int(x, y), this);
                    portalIcon.SetActive(true);
                }
            }
        }
    }
    public void ClearPortalIcons() {
        for(int i = 0; i < portalIcons.Count; i++) {
            portalIcons[i].SetActive(false);
        }
        portalIcons.Clear();
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
