using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoPanel : MonoBehaviour {

    private Item item;

    public TextMeshProUGUI itemName;

    public void SetItem(Item item) {
        this.item = item;
        SetStats();
    }
    private void SetStats() {
        itemName.text = item.itemName;
        itemName.color = ColorBySkillType.GetColorByRarity(item.rarity);
    }
}
