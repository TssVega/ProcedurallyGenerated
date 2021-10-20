using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingPanel : MonoBehaviour {

    public Color activeTabPanelColor;
    public Color inactiveTabPanelColor;

    public CraftingDatabase craftingDatabase;

    public Image[] inputFirstImages;
    public Image[] inputSecondImages;
    public Image[] inputThirdImages;

    public Image outputFirstImage;
    public Image outputSecondImage;
    public Image outputThirdImage;

    private Inventory inventory;

    // private int page;
    private void Awake() {
        inventory = FindObjectOfType<Inventory>();
    }
    public void ChangeTab(int index) {
        switch(index) {
            case 0:
                // page = 0;

                break;
        }
    }
    private void PutItem(Item item, int x, int y) {
        
    }
}
