using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour {

    private Item item;

    public SkillUser playerSkills;

    private bool isInventorySlot;
    private int currentIndex;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI[] currentItemOffenceStats;
    public TextMeshProUGUI[] currentItemDefenceStats;
    public TextMeshProUGUI[] equippedItemOffenceStats;
    public TextMeshProUGUI[] equippedItemDefenceStats;
    public TextMeshProUGUI currentItemDescription;
    public TextMeshProUGUI equippedItemDescription;
    public TextMeshProUGUI equippedItemText;
    public TextMeshProUGUI consumeEquipButtonText;
    public TextMeshProUGUI dismantleButtonText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI areYouSureText;
    public TextMeshProUGUI yesText;

    public GameObject consumeEquipButton;
    public GameObject dismantleButton;

    public TextMeshProUGUI quantityText;

    public Image currentItemFirstImage;
    public Image currentItemSecondImage;
    public Image currentItemThirdImage;
    public Image equippedItemFirstImage;
    public Image equippedItemSecondImage;
    public Image equippedItemThirdImage;

    public GameObject currentItemChart;
    public GameObject equippedItemChart;
    public GameObject dismantleConfirmationPanel;
    public GameObject totalChart;

    public Inventory inventory;

    public Image[] skillSlots;
    public Image[] secondaryImages;

    private const int skillSlotCount = 11;
    private const string Format = "0";

    public Sprite emptySlotSprite;

    public SkillUI skillUI;

    public TextMeshProUGUI[] quantityTexts;

    private void Awake() {
        equippedItemText.text = LocalizationManager.localization.GetText("equippedItem");
        areYouSureText.text = LocalizationManager.localization.GetText("dismantleConfirmation");
        yesText.text = LocalizationManager.localization.GetText("yes");
    }
    private void OnEnable() {
        dismantleConfirmationPanel.SetActive(false);
    }
    private void OnDisable() {
        totalChart.SetActive(true);
    }
    public void SetItem(Item item, int index, bool isInventorySlot) {
        this.item = item;
        currentIndex = index;
        this.isInventorySlot = isInventorySlot;
        SetStats();
        SetButtons();
        UpdateStats();
        Refresh();
    }
    private void SetStats() {
        if(item != null) {
            itemName.text = item.itemName;
            itemName.color = ColorBySkillType.GetColorByRarity(item.rarity);
        }
    }
    // Replace an IUsable in a slot with the current one
    public void ReplaceSkillSlot(int index) {
        for(int i = 0; i < skillSlotCount; i++) {
            if(playerSkills.currentSkills.Length <= i) {
                continue;
            }
            Mushroom mush = playerSkills.currentSkills[i] as Mushroom;
            if(mush == item) {
                playerSkills.currentSkills[i] = null;
                break;
            }
        }
        if(item is Mushroom m) {
            playerSkills.currentSkills[index] = m;
        }        
        skillUI.RefreshSkillSlots();
        Refresh();
    }
    // Refresh all the skill slots
    private void Refresh() {
        if(item.slot != EquipSlot.Consumable) {
            return;
        }
        // Set skill slots sprites
        for(int i = 0; i < skillSlotCount; i++) {
            skillSlots[i].gameObject.SetActive(true);
            if(i >= playerSkills.currentSkills.Length) {
                skillSlots[i].sprite = emptySlotSprite;
                skillSlots[i].color = Color.white;
                secondaryImages[i].sprite = null;
                secondaryImages[i].color = Color.clear;
                quantityTexts[i].text = "";
                continue;
            }
            ActiveSkill a = playerSkills.currentSkills[i] as ActiveSkill;
            if(a && a.skillIcon) {
                skillSlots[i].sprite = a.skillIcon;
                skillSlots[i].color = ColorBySkillType.GetColorByType(a.attackType);
                secondaryImages[i].sprite = null;
                secondaryImages[i].color = Color.clear;
                quantityTexts[i].text = "";
            }
            else if(playerSkills.currentSkills[i] is Mushroom m) {
                skillSlots[i].sprite = m.firstIcon;
                skillSlots[i].color = m.firstColor;
                secondaryImages[i].sprite = null;
                secondaryImages[i].color = Color.clear;
                quantityTexts[i].text = inventory.quantities[currentIndex].ToString();
            }
            /* Potions here
            else if(playerSkills.currentSkills[i] is Mushroom m) {
                skillSlots[i].sprite = m.firstIcon;
                skillSlots[i].color = m.firstColor;
                secondaryImages[i].sprite = null;
                secondaryImages[i].color = Color.white;
            }*/
            else {
                skillSlots[i].sprite = emptySlotSprite;
                skillSlots[i].color = Color.white;
                secondaryImages[i].sprite = null;
                secondaryImages[i].color = Color.clear;
                quantityTexts[i].text = "";
            }
        }
    }
    private void SetButtons() {
        if(isInventorySlot) {
            if(item.slot == EquipSlot.Consumable) {
                consumeEquipButton.SetActive(true);
                consumeEquipButtonText.text = LocalizationManager.localization.GetText("consume");
                dismantleButton.SetActive(false);
                for(int i = 0; i < skillSlots.Length; i++) {
                    skillSlots[i].gameObject.SetActive(true);
                }
                for(int i = 0; i < skillSlots.Length; i++) {
                    secondaryImages[i].gameObject.SetActive(true);
                }
                infoText.gameObject.SetActive(true);
                infoText.text = LocalizationManager.localization.GetText("skillInfoNotif");
            }
            else if(item.slot == EquipSlot.Other) {
                consumeEquipButton.SetActive(false);
                dismantleButton.SetActive(false);
                for(int i = 0; i < skillSlots.Length; i++) {
                    skillSlots[i].gameObject.SetActive(false);
                }
                for(int i = 0; i < skillSlots.Length; i++) {
                    secondaryImages[i].gameObject.SetActive(false);
                }
                infoText.gameObject.SetActive(false);
            }
            else {
                consumeEquipButton.SetActive(true);
                consumeEquipButtonText.text = LocalizationManager.localization.GetText("equip");
                dismantleButton.SetActive(true);
                dismantleButtonText.text = LocalizationManager.localization.GetText("dismantle");
                for(int i = 0; i < skillSlots.Length; i++) {
                    skillSlots[i].gameObject.SetActive(false);
                }
                for(int i = 0; i < skillSlots.Length; i++) {
                    secondaryImages[i].gameObject.SetActive(false);
                }
                infoText.gameObject.SetActive(false);
            }
        }
        else {
            consumeEquipButton.SetActive(true);
            dismantleButton.SetActive(false);
            consumeEquipButtonText.text = LocalizationManager.localization.GetText("unequip");
            for(int i = 0; i < skillSlots.Length; i++) {
                skillSlots[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < skillSlots.Length; i++) {
                secondaryImages[i].gameObject.SetActive(false);
            }
            infoText.gameObject.SetActive(false);
        }
    }
    public void ConsumeEquipButton() {
        if(isInventorySlot) {
            if(item.slot == EquipSlot.Consumable) {
                inventory.ConsumeInSlot(currentIndex);
                UpdateStats();
                if(inventory.quantities[currentIndex] < 1) {
                    gameObject.SetActive(false);
                }                
            }
            else if(item.slot != EquipSlot.Other) {
                inventory.EquipItemInSlot(currentIndex);
                UpdateStats();
                gameObject.SetActive(false);
            }
        }
        else {
            if(inventory.CanAddToInventory()) {
                int emptySlotIndex = -1;
                for(int i = 0; i < inventory.inventory.Count; i++) {
                    if(inventory.inventory[i] is null) {
                        emptySlotIndex = i;
                        break;
                    }
                }
                if(emptySlotIndex >= 0) {
                    inventory.UnequipItem(currentIndex, emptySlotIndex);
                    gameObject.SetActive(false);
                }                
            }            
        }
    }
    public void DismantleButton() {
        if(item.dismantleOutput > 0) {
            dismantleConfirmationPanel.SetActive(true);
        }        
    }
    public void Dismantle() {
        if(isInventorySlot) {
            inventory.DismantleInSlot(currentIndex);
            UpdateStats();
            gameObject.SetActive(false);
        }
    }
    private void UpdateStats() {
        currentItemDescription.text = LocalizationManager.localization.GetText($"{item.seed}Desc");
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
            quantityText.text = "";
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
            quantityText.text = "";
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
            quantityText.text = "";
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
            quantityText.text = "";
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
            quantityText.text = inventory.quantities[currentIndex].ToString();
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
