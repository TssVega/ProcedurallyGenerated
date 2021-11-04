using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemInfo : MonoBehaviour {

    private Item item;
    private int index;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI currentItemDescription;
    public TextMeshProUGUI equippedItemText;
    public TextMeshProUGUI equippedItemDescription;
    public TextMeshProUGUI[] currentItemOffenceStats;
    public TextMeshProUGUI[] currentItemDefenceStats;
    public TextMeshProUGUI[] equippedItemOffenceStats;
    public TextMeshProUGUI[] equippedItemDefenceStats;

    public Image currentItemFirstImage;
    public Image currentItemSecondImage;
    public Image currentItemThirdImage;
    public Image equippedItemFirstImage;
    public Image equippedItemSecondImage;
    public Image equippedItemThirdImage;

    public Image coinImage;

    public Sprite goldImage;
    public Sprite silverImage;

    public GameObject currentItemChart;
    public GameObject equippedItemChart;
    public GameObject totalChart;

    private const string Format = "0";

    private Inventory inventory;

    public VendorUI vendorUI;

    public void SetItem(Item item, int index) {
        inventory = FindObjectOfType<Player>().inventory;
        this.item = item;
        this.index = index;
        if(item == null) {
            return;
        }
        SetStats();
        SetSellButton();
        UpdateStats();
    }
    public void Buy() {
        vendorUI.BuyItem(index);
    }
    private void SetStats() {
        itemName.text = item.itemName;
        itemName.color = ColorBySkillType.GetColorByRarity(item.rarity);
    }
    private void SetSellButton() {
        bool costsGold = item.goldCost > 0;
        costText.text = costsGold ? (item.goldCost * vendorUI.priceMultiplier).ToString() : (item.silverCost * vendorUI.priceMultiplier).ToString();
        coinImage.sprite = costsGold ? goldImage : silverImage;
    }
    private string GT(string str) {
        return LocalizationManager.localization.GetText(str);
    }
    private void UpdateStats() {
        currentItemDescription.text = LocalizationManager.localization.GetText($"{item.seed}Desc");
        if(item.strength > 0) {
            currentItemDescription.text += $"+{item.strength} {GT("strength")} ";
        }
        if(item.agility > 0) {
            currentItemDescription.text += $"+{item.agility} {GT("agility")} ";
        }
        if(item.dexterity > 0) {
            currentItemDescription.text += $"+{item.dexterity} {GT("dexterity")} ";
        }
        if(item.intelligence > 0) {
            currentItemDescription.text += $"+{item.intelligence} {GT("intelligence")} ";
        }
        if(item.faith > 0) {
            currentItemDescription.text += $"+{item.faith} {GT("faith")} ";
        }
        if(item.wisdom > 0) {
            currentItemDescription.text += $"+{item.wisdom} {GT("wisdom")} ";
        }
        if(item.vitality > 0) {
            currentItemDescription.text += $"+{item.vitality} {GT("vitality")} ";
        }
        if(item.charisma > 0) {
            currentItemDescription.text += $"+{item.charisma} {GT("charisma")} ";
        }
        Item equippedItem = null;
        if((int)item.slot < 6) {
            equippedItem = inventory.equipment[(int)item.slot];
        }
        if(item && item.firstIcon) {
            currentItemFirstImage.sprite = item.firstIcon;
            currentItemFirstImage.color = item.firstColor;
        }
        else {
            currentItemFirstImage.color = Color.clear;
        }
        if(item && item.secondIcon) {
            currentItemSecondImage.sprite = item.secondIcon;
            currentItemSecondImage.color = item.secondColor;
        }
        else {
            currentItemSecondImage.color = Color.clear;
        }
        if(item && item.thirdIcon) {
            currentItemThirdImage.sprite = item.thirdIcon;
            currentItemThirdImage.color = item.thirdColor;
        }
        else {
            currentItemThirdImage.color = Color.clear;
        }
        if(equippedItem) {
            equippedItemDescription.text = LocalizationManager.localization.GetText($"{equippedItem.seed}Desc");
            if(equippedItem && equippedItem.firstIcon) {
                equippedItemFirstImage.sprite = equippedItem.firstIcon;
                equippedItemFirstImage.color = equippedItem.firstColor;
            }
            else {
                equippedItemFirstImage.color = Color.clear;
            }
            if(equippedItem && equippedItem.secondIcon) {
                equippedItemSecondImage.sprite = equippedItem.secondIcon;
                equippedItemSecondImage.color = equippedItem.secondColor;
            }
            else {
                equippedItemSecondImage.color = Color.clear;
            }
            if(equippedItem && equippedItem.thirdIcon) {
                equippedItemThirdImage.sprite = equippedItem.thirdIcon;
                equippedItemThirdImage.color = equippedItem.thirdColor;
            }
            else {
                equippedItemThirdImage.color = Color.clear;
            }
        }
        else {
            equippedItemFirstImage.color = Color.clear;
            equippedItemSecondImage.color = Color.clear;
            equippedItemThirdImage.color = Color.clear;
            equippedItemDescription.text = "";
        }
        if(item is Weapon w) {
            currentItemOffenceStats[0].text = w.fireDamage.ToString(Format);
            currentItemOffenceStats[1].text = w.iceDamage.ToString(Format);
            currentItemOffenceStats[2].text = w.airDamage.ToString(Format);
            currentItemOffenceStats[3].text = w.earthDamage.ToString(Format);
            currentItemOffenceStats[4].text = w.lightningDamage.ToString(Format);
            currentItemOffenceStats[5].text = w.lightDamage.ToString(Format);
            currentItemOffenceStats[6].text = w.darkDamage.ToString(Format);
            currentItemOffenceStats[7].text = w.bashDamage.ToString(Format);
            currentItemOffenceStats[8].text = w.pierceDamage.ToString(Format);
            currentItemOffenceStats[9].text = w.slashDamage.ToString(Format);
            currentItemOffenceStats[10].text = w.bleedDamage.ToString(Format);
            currentItemOffenceStats[11].text = w.poisonDamage.ToString(Format);
            currentItemOffenceStats[12].text = w.curseDamage.ToString(Format);
            currentItemDefenceStats[0].text = "0";
            currentItemDefenceStats[1].text = "0";
            currentItemDefenceStats[2].text = "0";
            currentItemDefenceStats[3].text = "0";
            currentItemDefenceStats[4].text = "0";
            currentItemDefenceStats[5].text = "0";
            currentItemDefenceStats[6].text = "0";
            currentItemDefenceStats[7].text = "0";
            currentItemDefenceStats[8].text = "0";
            currentItemDefenceStats[9].text = "0";
            currentItemDefenceStats[10].text = "0";
            currentItemDefenceStats[11].text = "0";
            currentItemDefenceStats[12].text = "0";
            currentItemChart.SetActive(true);
            totalChart.SetActive(true);
            Weapon equippedWeapon = inventory.equipment[(int)w.slot] as Weapon;
            if(equippedWeapon) {
                equippedItemOffenceStats[0].text = equippedWeapon.fireDamage.ToString(Format);
                equippedItemOffenceStats[1].text = equippedWeapon.iceDamage.ToString(Format);
                equippedItemOffenceStats[2].text = equippedWeapon.airDamage.ToString(Format);
                equippedItemOffenceStats[3].text = equippedWeapon.earthDamage.ToString(Format);
                equippedItemOffenceStats[4].text = equippedWeapon.lightningDamage.ToString(Format);
                equippedItemOffenceStats[5].text = equippedWeapon.lightDamage.ToString(Format);
                equippedItemOffenceStats[6].text = equippedWeapon.darkDamage.ToString(Format);
                equippedItemOffenceStats[7].text = equippedWeapon.bashDamage.ToString(Format);
                equippedItemOffenceStats[8].text = equippedWeapon.pierceDamage.ToString(Format);
                equippedItemOffenceStats[9].text = equippedWeapon.slashDamage.ToString(Format);
                equippedItemOffenceStats[10].text = equippedWeapon.bleedDamage.ToString(Format);
                equippedItemOffenceStats[11].text = equippedWeapon.poisonDamage.ToString(Format);
                equippedItemOffenceStats[12].text = equippedWeapon.curseDamage.ToString(Format);
                equippedItemDefenceStats[0].text = "0";
                equippedItemDefenceStats[1].text = "0";
                equippedItemDefenceStats[2].text = "0";
                equippedItemDefenceStats[3].text = "0";
                equippedItemDefenceStats[4].text = "0";
                equippedItemDefenceStats[5].text = "0";
                equippedItemDefenceStats[6].text = "0";
                equippedItemDefenceStats[7].text = "0";
                equippedItemDefenceStats[8].text = "0";
                equippedItemDefenceStats[9].text = "0";
                equippedItemDefenceStats[10].text = "0";
                equippedItemDefenceStats[11].text = "0";
                equippedItemDefenceStats[12].text = "0";
                equippedItemChart.SetActive(true);
                equippedItemText.gameObject.SetActive(true);
            }
            else {
                for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                    equippedItemOffenceStats[i].text = "0";
                    equippedItemDefenceStats[i].text = "0";
                }
                equippedItemChart.SetActive(false);
                equippedItemText.gameObject.SetActive(false);
            }
        }
        else if(item is Armor a) {
            currentItemOffenceStats[0].text = "0";
            currentItemOffenceStats[1].text = "0";
            currentItemOffenceStats[2].text = "0";
            currentItemOffenceStats[3].text = "0";
            currentItemOffenceStats[4].text = "0";
            currentItemOffenceStats[5].text = "0";
            currentItemOffenceStats[6].text = "0";
            currentItemOffenceStats[7].text = "0";
            currentItemOffenceStats[8].text = "0";
            currentItemOffenceStats[9].text = "0";
            currentItemOffenceStats[10].text = "0";
            currentItemOffenceStats[11].text = "0";
            currentItemOffenceStats[12].text = "0";
            currentItemDefenceStats[0].text = a.fireDefence.ToString(Format);
            currentItemDefenceStats[1].text = a.iceDefence.ToString(Format);
            currentItemDefenceStats[2].text = a.airDefence.ToString(Format);
            currentItemDefenceStats[3].text = a.earthDefence.ToString(Format);
            currentItemDefenceStats[4].text = a.lightningDefence.ToString(Format);
            currentItemDefenceStats[5].text = a.lightDefence.ToString(Format);
            currentItemDefenceStats[6].text = a.darkDefence.ToString(Format);
            currentItemDefenceStats[7].text = a.bashDefence.ToString(Format);
            currentItemDefenceStats[8].text = a.pierceDefence.ToString(Format);
            currentItemDefenceStats[9].text = a.slashDefence.ToString(Format);
            currentItemDefenceStats[10].text = a.bleedDefence.ToString(Format);
            currentItemDefenceStats[11].text = a.poisonDefence.ToString(Format);
            currentItemDefenceStats[12].text = a.curseDefence.ToString(Format);
            currentItemChart.SetActive(true);
            totalChart.SetActive(true);
            Armor equippedArmor = inventory.equipment[(int)a.slot] as Armor;
            if(equippedArmor) {
                equippedItemOffenceStats[0].text = "0";
                equippedItemOffenceStats[1].text = "0";
                equippedItemOffenceStats[2].text = "0";
                equippedItemOffenceStats[3].text = "0";
                equippedItemOffenceStats[4].text = "0";
                equippedItemOffenceStats[5].text = "0";
                equippedItemOffenceStats[6].text = "0";
                equippedItemOffenceStats[7].text = "0";
                equippedItemOffenceStats[8].text = "0";
                equippedItemOffenceStats[9].text = "0";
                equippedItemOffenceStats[10].text = "0";
                equippedItemOffenceStats[11].text = "0";
                equippedItemOffenceStats[12].text = "0";
                equippedItemDefenceStats[0].text = equippedArmor.fireDefence.ToString(Format);
                equippedItemDefenceStats[1].text = equippedArmor.iceDefence.ToString(Format);
                equippedItemDefenceStats[2].text = equippedArmor.airDefence.ToString(Format);
                equippedItemDefenceStats[3].text = equippedArmor.earthDefence.ToString(Format);
                equippedItemDefenceStats[4].text = equippedArmor.lightningDefence.ToString(Format);
                equippedItemDefenceStats[5].text = equippedArmor.lightDefence.ToString(Format);
                equippedItemDefenceStats[6].text = equippedArmor.darkDefence.ToString(Format);
                equippedItemDefenceStats[7].text = equippedArmor.bashDefence.ToString(Format);
                equippedItemDefenceStats[8].text = equippedArmor.pierceDefence.ToString(Format);
                equippedItemDefenceStats[9].text = equippedArmor.slashDefence.ToString(Format);
                equippedItemDefenceStats[10].text = equippedArmor.bleedDefence.ToString(Format);
                equippedItemDefenceStats[11].text = equippedArmor.poisonDefence.ToString(Format);
                equippedItemDefenceStats[12].text = equippedArmor.curseDefence.ToString(Format);
                equippedItemChart.SetActive(true);
                equippedItemText.gameObject.SetActive(true);
            }
            else {
                for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                    equippedItemOffenceStats[i].text = "0";
                    equippedItemDefenceStats[i].text = "0";
                }
                equippedItemChart.SetActive(false);
                equippedItemText.gameObject.SetActive(false);
            }
        }
        else if(item is Shield s) {
            currentItemOffenceStats[0].text = "0";
            currentItemOffenceStats[1].text = "0";
            currentItemOffenceStats[2].text = "0";
            currentItemOffenceStats[3].text = "0";
            currentItemOffenceStats[4].text = "0";
            currentItemOffenceStats[5].text = "0";
            currentItemOffenceStats[6].text = "0";
            currentItemOffenceStats[7].text = "0";
            currentItemOffenceStats[8].text = "0";
            currentItemOffenceStats[9].text = "0";
            currentItemOffenceStats[10].text = "0";
            currentItemOffenceStats[11].text = "0";
            currentItemOffenceStats[12].text = "0";
            currentItemDefenceStats[0].text = s.fireDefence.ToString(Format);
            currentItemDefenceStats[1].text = s.iceDefence.ToString(Format);
            currentItemDefenceStats[2].text = s.airDefence.ToString(Format);
            currentItemDefenceStats[3].text = s.earthDefence.ToString(Format);
            currentItemDefenceStats[4].text = s.lightningDefence.ToString(Format);
            currentItemDefenceStats[5].text = s.lightDefence.ToString(Format);
            currentItemDefenceStats[6].text = s.darkDefence.ToString(Format);
            currentItemDefenceStats[7].text = s.bashDefence.ToString(Format);
            currentItemDefenceStats[8].text = s.pierceDefence.ToString(Format);
            currentItemDefenceStats[9].text = s.slashDefence.ToString(Format);
            currentItemDefenceStats[10].text = s.bleedDefence.ToString(Format);
            currentItemDefenceStats[11].text = s.poisonDefence.ToString(Format);
            currentItemDefenceStats[12].text = s.curseDefence.ToString(Format);
            currentItemChart.SetActive(true);
            totalChart.SetActive(true);
            Shield equippedShield = inventory.equipment[(int)s.slot] as Shield;
            if(equippedShield) {
                equippedItemOffenceStats[0].text = "0";
                equippedItemOffenceStats[1].text = "0";
                equippedItemOffenceStats[2].text = "0";
                equippedItemOffenceStats[3].text = "0";
                equippedItemOffenceStats[4].text = "0";
                equippedItemOffenceStats[5].text = "0";
                equippedItemOffenceStats[6].text = "0";
                equippedItemOffenceStats[7].text = "0";
                equippedItemOffenceStats[8].text = "0";
                equippedItemOffenceStats[9].text = "0";
                equippedItemOffenceStats[10].text = "0";
                equippedItemOffenceStats[11].text = "0";
                equippedItemOffenceStats[12].text = "0";
                equippedItemDefenceStats[0].text = equippedShield.fireDefence.ToString(Format);
                equippedItemDefenceStats[1].text = equippedShield.iceDefence.ToString(Format);
                equippedItemDefenceStats[2].text = equippedShield.airDefence.ToString(Format);
                equippedItemDefenceStats[3].text = equippedShield.earthDefence.ToString(Format);
                equippedItemDefenceStats[4].text = equippedShield.lightningDefence.ToString(Format);
                equippedItemDefenceStats[5].text = equippedShield.lightDefence.ToString(Format);
                equippedItemDefenceStats[6].text = equippedShield.darkDefence.ToString(Format);
                equippedItemDefenceStats[7].text = equippedShield.bashDefence.ToString(Format);
                equippedItemDefenceStats[8].text = equippedShield.pierceDefence.ToString(Format);
                equippedItemDefenceStats[9].text = equippedShield.slashDefence.ToString(Format);
                equippedItemDefenceStats[10].text = equippedShield.bleedDefence.ToString(Format);
                equippedItemDefenceStats[11].text = equippedShield.poisonDefence.ToString(Format);
                equippedItemDefenceStats[12].text = equippedShield.curseDefence.ToString(Format);
                equippedItemChart.SetActive(true);
                equippedItemText.gameObject.SetActive(true);
            }
            else {
                for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                    equippedItemOffenceStats[i].text = "0";
                    equippedItemDefenceStats[i].text = "0";
                }
                equippedItemChart.SetActive(false);
                equippedItemText.gameObject.SetActive(false);
            }
        }
        else if(item is Ring r) {
            currentItemOffenceStats[0].text = r.fireDamage.ToString(Format);
            currentItemOffenceStats[1].text = r.iceDamage.ToString(Format);
            currentItemOffenceStats[2].text = r.airDamage.ToString(Format);
            currentItemOffenceStats[3].text = r.earthDamage.ToString(Format);
            currentItemOffenceStats[4].text = r.lightningDamage.ToString(Format);
            currentItemOffenceStats[5].text = r.lightDamage.ToString(Format);
            currentItemOffenceStats[6].text = r.darkDamage.ToString(Format);
            currentItemOffenceStats[7].text = r.bashDamage.ToString(Format);
            currentItemOffenceStats[8].text = r.pierceDamage.ToString(Format);
            currentItemOffenceStats[9].text = r.slashDamage.ToString(Format);
            currentItemOffenceStats[10].text = r.bleedDamage.ToString(Format);
            currentItemOffenceStats[11].text = r.poisonDamage.ToString(Format);
            currentItemOffenceStats[12].text = r.curseDamage.ToString(Format);
            currentItemDefenceStats[0].text = r.fireDefence.ToString(Format);
            currentItemDefenceStats[1].text = r.iceDefence.ToString(Format);
            currentItemDefenceStats[2].text = r.airDefence.ToString(Format);
            currentItemDefenceStats[3].text = r.earthDefence.ToString(Format);
            currentItemDefenceStats[4].text = r.lightningDefence.ToString(Format);
            currentItemDefenceStats[5].text = r.lightDefence.ToString(Format);
            currentItemDefenceStats[6].text = r.darkDefence.ToString(Format);
            currentItemDefenceStats[7].text = r.bashDefence.ToString(Format);
            currentItemDefenceStats[8].text = r.pierceDefence.ToString(Format);
            currentItemDefenceStats[9].text = r.slashDefence.ToString(Format);
            currentItemDefenceStats[10].text = r.bleedDefence.ToString(Format);
            currentItemDefenceStats[11].text = r.poisonDefence.ToString(Format);
            currentItemDefenceStats[12].text = r.curseDefence.ToString(Format);
            currentItemChart.SetActive(true);
            totalChart.SetActive(true);
            Ring equippedRing = inventory.equipment[(int)r.slot] as Ring;
            if(equippedRing) {
                equippedItemOffenceStats[0].text = equippedRing.fireDamage.ToString(Format);
                equippedItemOffenceStats[1].text = equippedRing.iceDamage.ToString(Format);
                equippedItemOffenceStats[2].text = equippedRing.airDamage.ToString(Format);
                equippedItemOffenceStats[3].text = equippedRing.earthDamage.ToString(Format);
                equippedItemOffenceStats[4].text = equippedRing.lightningDamage.ToString(Format);
                equippedItemOffenceStats[5].text = equippedRing.lightDamage.ToString(Format);
                equippedItemOffenceStats[6].text = equippedRing.darkDamage.ToString(Format);
                equippedItemOffenceStats[7].text = equippedRing.bashDamage.ToString(Format);
                equippedItemOffenceStats[8].text = equippedRing.pierceDamage.ToString(Format);
                equippedItemOffenceStats[9].text = equippedRing.slashDamage.ToString(Format);
                equippedItemOffenceStats[10].text = equippedRing.bleedDamage.ToString(Format);
                equippedItemOffenceStats[11].text = equippedRing.poisonDamage.ToString(Format);
                equippedItemOffenceStats[12].text = equippedRing.curseDamage.ToString(Format);
                equippedItemDefenceStats[0].text = equippedRing.fireDefence.ToString(Format);
                equippedItemDefenceStats[1].text = equippedRing.iceDefence.ToString(Format);
                equippedItemDefenceStats[2].text = equippedRing.airDefence.ToString(Format);
                equippedItemDefenceStats[3].text = equippedRing.earthDefence.ToString(Format);
                equippedItemDefenceStats[4].text = equippedRing.lightningDefence.ToString(Format);
                equippedItemDefenceStats[5].text = equippedRing.lightDefence.ToString(Format);
                equippedItemDefenceStats[6].text = equippedRing.darkDefence.ToString(Format);
                equippedItemDefenceStats[7].text = equippedRing.bashDefence.ToString(Format);
                equippedItemDefenceStats[8].text = equippedRing.pierceDefence.ToString(Format);
                equippedItemDefenceStats[9].text = equippedRing.slashDefence.ToString(Format);
                equippedItemDefenceStats[10].text = equippedRing.bleedDefence.ToString(Format);
                equippedItemDefenceStats[11].text = equippedRing.poisonDefence.ToString(Format);
                equippedItemDefenceStats[12].text = equippedRing.curseDefence.ToString(Format);
                equippedItemChart.SetActive(true);
                equippedItemText.gameObject.SetActive(true);
            }
            else {
                for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                    equippedItemOffenceStats[i].text = "0";
                    equippedItemDefenceStats[i].text = "0";
                }
                equippedItemChart.SetActive(false);
                equippedItemText.gameObject.SetActive(true);
            }
        }
        else {
            for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                currentItemOffenceStats[i].text = "0";
                currentItemDefenceStats[i].text = "0";
            }
            for(int i = 0; i < equippedItemOffenceStats.Length; i++) {
                equippedItemOffenceStats[i].text = "0";
                equippedItemDefenceStats[i].text = "0";
            }
            currentItemChart.SetActive(false);
            equippedItemChart.SetActive(false);
            totalChart.SetActive(false);
            equippedItemText.gameObject.SetActive(false);
        }
    }
}
