using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingPanel : MonoBehaviour {

    public Color activeTabPanelColor;
    public Color inactiveTabPanelColor;

    public CraftingDatabase craftingDatabase;

    public ItemImages[] inputImages;

    public ItemImages outputImages;

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
}
