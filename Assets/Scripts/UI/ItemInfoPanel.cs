using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour {

    private Item item;

    private int currentInventoryIndex;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI[] currentItemOffenceStats;
    public TextMeshProUGUI[] currentItemDefenceStats;
    public TextMeshProUGUI[] equippedItemOffenceStats;
    public TextMeshProUGUI[] equippedItemDefenceStats;
    public TextMeshProUGUI currentItemDescription;
    public TextMeshProUGUI equippedItemDescription;

    public Image currentItemFirstImage;
    public Image currentItemSecondImage;
    public Image currentItemThirdImage;
    public Image equippedItemFirstImage;
    public Image equippedItemSecondImage;
    public Image equippedItemThirdImage;

    public Inventory inventory;

    public void SetItem(Item item) {
        this.item = item;
        SetStats();
        UpdateStats();
    }
    private void SetStats() {
        if(item != null) {
            itemName.text = item.itemName;
            itemName.color = ColorBySkillType.GetColorByRarity(item.rarity);
        }
    }
    private void UpdateStats() {
        currentItemDescription.text = LocalizationManager.localization.GetText(item.descriptionID);
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
            equippedItemDescription.text = LocalizationManager.localization.GetText(equippedItem.descriptionID);
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
            currentItemOffenceStats[0].text = w.fireDamage.ToString();
            currentItemOffenceStats[1].text = w.iceDamage.ToString();
            currentItemOffenceStats[2].text = w.airDamage.ToString();
            currentItemOffenceStats[3].text = w.earthDamage.ToString();
            currentItemOffenceStats[4].text = w.lightningDamage.ToString();
            currentItemOffenceStats[5].text = w.lightDamage.ToString();
            currentItemOffenceStats[6].text = w.darkDamage.ToString();
            currentItemOffenceStats[7].text = w.bashDamage.ToString();
            currentItemOffenceStats[8].text = w.pierceDamage.ToString();
            currentItemOffenceStats[9].text = w.slashDamage.ToString();
            currentItemOffenceStats[10].text = w.bleedDamage.ToString();
            currentItemOffenceStats[11].text = w.poisonDamage.ToString();
            currentItemOffenceStats[12].text = w.curseDamage.ToString();
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
            Weapon equippedWeapon = inventory.equipment[(int)w.slot] as Weapon;
            equippedItemOffenceStats[0].text = equippedWeapon.fireDamage.ToString();
            equippedItemOffenceStats[1].text = equippedWeapon.iceDamage.ToString();
            equippedItemOffenceStats[2].text = equippedWeapon.airDamage.ToString();
            equippedItemOffenceStats[3].text = equippedWeapon.earthDamage.ToString();
            equippedItemOffenceStats[4].text = equippedWeapon.lightningDamage.ToString();
            equippedItemOffenceStats[5].text = equippedWeapon.lightDamage.ToString();
            equippedItemOffenceStats[6].text = equippedWeapon.darkDamage.ToString();
            equippedItemOffenceStats[7].text = equippedWeapon.bashDamage.ToString();
            equippedItemOffenceStats[8].text = equippedWeapon.pierceDamage.ToString();
            equippedItemOffenceStats[9].text = equippedWeapon.slashDamage.ToString();
            equippedItemOffenceStats[10].text = equippedWeapon.bleedDamage.ToString();
            equippedItemOffenceStats[11].text = equippedWeapon.poisonDamage.ToString();
            equippedItemOffenceStats[12].text = equippedWeapon.curseDamage.ToString();
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
            currentItemDefenceStats[0].text = a.fireDefence.ToString();
            currentItemDefenceStats[1].text = a.iceDefence.ToString();
            currentItemDefenceStats[2].text = a.airDefence.ToString();
            currentItemDefenceStats[3].text = a.earthDefence.ToString();
            currentItemDefenceStats[4].text = a.lightningDefence.ToString();
            currentItemDefenceStats[5].text = a.lightDefence.ToString();
            currentItemDefenceStats[6].text = a.darkDefence.ToString();
            currentItemDefenceStats[7].text = a.bashDefence.ToString();
            currentItemDefenceStats[8].text = a.pierceDefence.ToString();
            currentItemDefenceStats[9].text = a.slashDefence.ToString();
            currentItemDefenceStats[10].text = a.bleedDefence.ToString();
            currentItemDefenceStats[11].text = a.poisonDefence.ToString();
            currentItemDefenceStats[12].text = a.curseDefence.ToString();
            Armor equippedArmor = inventory.equipment[(int)a.slot] as Armor;
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
            equippedItemDefenceStats[0].text = equippedArmor.fireDefence.ToString();
            equippedItemDefenceStats[1].text = equippedArmor.iceDefence.ToString();
            equippedItemDefenceStats[2].text = equippedArmor.airDefence.ToString();
            equippedItemDefenceStats[3].text = equippedArmor.earthDefence.ToString();
            equippedItemDefenceStats[4].text = equippedArmor.lightningDefence.ToString();
            equippedItemDefenceStats[5].text = equippedArmor.lightDefence.ToString();
            equippedItemDefenceStats[6].text = equippedArmor.darkDefence.ToString();
            equippedItemDefenceStats[7].text = equippedArmor.bashDefence.ToString();
            equippedItemDefenceStats[8].text = equippedArmor.pierceDefence.ToString();
            equippedItemDefenceStats[9].text = equippedArmor.slashDefence.ToString();
            equippedItemDefenceStats[10].text = equippedArmor.bleedDefence.ToString();
            equippedItemDefenceStats[11].text = equippedArmor.poisonDefence.ToString();
            equippedItemDefenceStats[12].text = equippedArmor.curseDefence.ToString();
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
            currentItemDefenceStats[0].text = s.fireDefence.ToString();
            currentItemDefenceStats[1].text = s.iceDefence.ToString();
            currentItemDefenceStats[2].text = s.airDefence.ToString();
            currentItemDefenceStats[3].text = s.earthDefence.ToString();
            currentItemDefenceStats[4].text = s.lightningDefence.ToString();
            currentItemDefenceStats[5].text = s.lightDefence.ToString();
            currentItemDefenceStats[6].text = s.darkDefence.ToString();
            currentItemDefenceStats[7].text = s.bashDefence.ToString();
            currentItemDefenceStats[8].text = s.pierceDefence.ToString();
            currentItemDefenceStats[9].text = s.slashDefence.ToString();
            currentItemDefenceStats[10].text = s.bleedDefence.ToString();
            currentItemDefenceStats[11].text = s.poisonDefence.ToString();
            currentItemDefenceStats[12].text = s.curseDefence.ToString();
            Shield equippedShield = inventory.equipment[(int)s.slot] as Shield;
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
            equippedItemDefenceStats[0].text = equippedShield.fireDefence.ToString();
            equippedItemDefenceStats[1].text = equippedShield.iceDefence.ToString();
            equippedItemDefenceStats[2].text = equippedShield.airDefence.ToString();
            equippedItemDefenceStats[3].text = equippedShield.earthDefence.ToString();
            equippedItemDefenceStats[4].text = equippedShield.lightningDefence.ToString();
            equippedItemDefenceStats[5].text = equippedShield.lightDefence.ToString();
            equippedItemDefenceStats[6].text = equippedShield.darkDefence.ToString();
            equippedItemDefenceStats[7].text = equippedShield.bashDefence.ToString();
            equippedItemDefenceStats[8].text = equippedShield.pierceDefence.ToString();
            equippedItemDefenceStats[9].text = equippedShield.slashDefence.ToString();
            equippedItemDefenceStats[10].text = equippedShield.bleedDefence.ToString();
            equippedItemDefenceStats[11].text = equippedShield.poisonDefence.ToString();
            equippedItemDefenceStats[12].text = equippedShield.curseDefence.ToString();
        }
        else if(item is Ring r) {
            currentItemOffenceStats[0].text = r.fireDamage.ToString();
            currentItemOffenceStats[1].text = r.iceDamage.ToString();
            currentItemOffenceStats[2].text = r.airDamage.ToString();
            currentItemOffenceStats[3].text = r.earthDamage.ToString();
            currentItemOffenceStats[4].text = r.lightningDamage.ToString();
            currentItemOffenceStats[5].text = r.lightDamage.ToString();
            currentItemOffenceStats[6].text = r.darkDamage.ToString();
            currentItemOffenceStats[7].text = r.bashDamage.ToString();
            currentItemOffenceStats[8].text = r.pierceDamage.ToString();
            currentItemOffenceStats[9].text = r.slashDamage.ToString();
            currentItemOffenceStats[10].text = r.bleedDamage.ToString();
            currentItemOffenceStats[11].text = r.poisonDamage.ToString();
            currentItemOffenceStats[12].text = r.curseDamage.ToString();
            currentItemDefenceStats[0].text = r.fireDefence.ToString();
            currentItemDefenceStats[1].text = r.iceDefence.ToString();
            currentItemDefenceStats[2].text = r.airDefence.ToString();
            currentItemDefenceStats[3].text = r.earthDefence.ToString();
            currentItemDefenceStats[4].text = r.lightningDefence.ToString();
            currentItemDefenceStats[5].text = r.lightDefence.ToString();
            currentItemDefenceStats[6].text = r.darkDefence.ToString();
            currentItemDefenceStats[7].text = r.bashDefence.ToString();
            currentItemDefenceStats[8].text = r.pierceDefence.ToString();
            currentItemDefenceStats[9].text = r.slashDefence.ToString();
            currentItemDefenceStats[10].text = r.bleedDefence.ToString();
            currentItemDefenceStats[11].text = r.poisonDefence.ToString();
            currentItemDefenceStats[12].text = r.curseDefence.ToString();
            Ring equippedRing = inventory.equipment[(int)r.slot] as Ring;
            equippedItemOffenceStats[0].text = equippedRing.fireDamage.ToString();
            equippedItemOffenceStats[1].text = equippedRing.iceDamage.ToString();
            equippedItemOffenceStats[2].text = equippedRing.airDamage.ToString();
            equippedItemOffenceStats[3].text = equippedRing.earthDamage.ToString();
            equippedItemOffenceStats[4].text = equippedRing.lightningDamage.ToString();
            equippedItemOffenceStats[5].text = equippedRing.lightDamage.ToString();
            equippedItemOffenceStats[6].text = equippedRing.darkDamage.ToString();
            equippedItemOffenceStats[7].text = equippedRing.bashDamage.ToString();
            equippedItemOffenceStats[8].text = equippedRing.pierceDamage.ToString();
            equippedItemOffenceStats[9].text = equippedRing.slashDamage.ToString();
            equippedItemOffenceStats[10].text = equippedRing.bleedDamage.ToString();
            equippedItemOffenceStats[11].text = equippedRing.poisonDamage.ToString();
            equippedItemOffenceStats[12].text = equippedRing.curseDamage.ToString();
            equippedItemDefenceStats[0].text = equippedRing.fireDefence.ToString();
            equippedItemDefenceStats[1].text = equippedRing.iceDefence.ToString();
            equippedItemDefenceStats[2].text = equippedRing.airDefence.ToString();
            equippedItemDefenceStats[3].text = equippedRing.earthDefence.ToString();
            equippedItemDefenceStats[4].text = equippedRing.lightningDefence.ToString();
            equippedItemDefenceStats[5].text = equippedRing.lightDefence.ToString();
            equippedItemDefenceStats[6].text = equippedRing.darkDefence.ToString();
            equippedItemDefenceStats[7].text = equippedRing.bashDefence.ToString();
            equippedItemDefenceStats[8].text = equippedRing.pierceDefence.ToString();
            equippedItemDefenceStats[9].text = equippedRing.slashDefence.ToString();
            equippedItemDefenceStats[10].text = equippedRing.bleedDefence.ToString();
            equippedItemDefenceStats[11].text = equippedRing.poisonDefence.ToString();
            equippedItemDefenceStats[12].text = equippedRing.curseDefence.ToString();
        }
        else {
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
        }
    }
}
