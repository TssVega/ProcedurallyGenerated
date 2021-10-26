using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
//using UnityEngine.Experimental.Rendering.LWRP;

public class Inventory : MonoBehaviour {

    private const string Format = "0";

    public List<Item> equipment;
    public List<Item> inventory;
    public Item[] craftingSlots;
    public Item craft;
    public int[] quantities;
    public int[] lucks;
    public int[] equipmentLucks;
    private readonly int inventorySize = 70;
    private readonly int equipmentSize = 6;
    private const int gridSize = 9;

    public GameObject[] itemSlots;
    public GameObject[] equipmentSlots;
    public GameObject selectedItemSlot;
    public TextMeshProUGUI[] offenceStats;
    public TextMeshProUGUI[] defenceStats;
    private Image[] itemImages;
    private Image[] itemImagesSecondary;
    private Image[] itemImagesTertiary;
    private Image[] equipmentImages;
    private Image[] equipmentImagesSecondary;
    private Image[] equipmentImagesTertiary;
    private TextMeshProUGUI[] quantityTexts;

    private Image[] itemFrames;
    private Image[] equipmentFrames;

    private Player player;
    private Stats stats;

    public ItemDatabase itemDatabase;
    public CraftingDatabase craftingDatabase;

    private InventoryPanel inventoryPanel;
    private SkillUI skillUI;

    private Light2D playerLight;

    private void Awake() {
        if(craftingDatabase) {
            craftingDatabase.SortAllRecipes();
        }        
        skillUI = FindObjectOfType<SkillUI>();
        inventoryPanel = FindObjectOfType<InventoryPanel>();
        quantities = new int[InventorySize];
        lucks = new int[InventorySize];
        equipmentLucks = new int[EquipmentSize];
        craftingSlots = new Item[gridSize];
        player = FindObjectOfType<Player>();
        playerLight = GetComponent<Light2D>();
        stats = player.stats;
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Levels")) {
            for(int i = 0; i < itemSlots.Length; i++) {
                itemSlots[i].GetComponent<ItemSlot>().slotIndex = i;
            }
            for(int i = 0; i < equipmentSlots.Length; i++) {
                equipmentSlots[i].GetComponent<EquipmentSlot>().slotIndex = i;
            }
            for(int i = 0; i < quantities.Length; i++) {
                quantities[i] = 0;
            }
            for(int i = 0; i < lucks.Length; i++) {
                lucks[i] = 0;
            }
            for(int i = 0; i < equipmentLucks.Length; i++) {
                equipmentLucks[i] = 0;
            }
        }        
    }
    private void Start() {
        UpdateStats();
        CheckLights(null);
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Levels") {
            for(int i = 0; i < craftingSlots.Length; i++) {
                UpdateCraftingSlot(i);
            }
            UpdateCraftingResult();
        }        
        /*
        // Torch
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Levels") {
            for(int i = 0; i < 1; i++) {
                Item ex = itemDatabase.items[89];
                AddToInventory(ex, false);
            }
        }*/
        // Create example weapon
        /*
        Weapon exampleItem = player.itemCreator.CreateWeapon("testWeapon");
        AddToInventory(exampleItem);
        //player.SetWeapon(exampleItem);
        // Create example armor
        Armor testArmor = player.itemCreator.CreateChestArmor("testArmor");
        AddToInventory(testArmor);
        //player.SetBodyArmor(testArmor);
        // Create example helmet
        Armor testHelmet = player.itemCreator.CreateHelmet("testHelmet");
        AddToInventory(testHelmet);
        //player.SetHelmet(testHelmet);
        // Create example legging
        Armor testLegging = player.itemCreator.CreateLegging("testLegging");
        AddToInventory(testLegging);
        // Create example shield
        Shield testShield = player.itemCreator.CreateShield("testShield");
        AddToInventory(testShield);
        //player.SetShield(testShield);
        // Create example ring
        Ring testRing = player.itemCreator.CreateRing("testRing");
        AddToInventory(testRing);
        // Create test item
        Weapon testWeapon = player.itemCreator.CreateWeapon("testItem");
        AddToInventory(testWeapon);
        // Another test item
        Armor testing = player.itemCreator.CreateChestArmor("chest");
        // Test shield
        Shield teshShield = player.itemCreator.CreateShield("testShield");*/
        //AddToInventory(teshShield);
        //AddToInventory(testing);      
    }
    public int InventorySize {
        get => inventorySize;
    }
    public int EquipmentSize {
        get => equipmentSize;
    }
    public void ClearCrafting() {
        if(craft) {
            AddToInventory(craft, false);
            craft = null;
            UpdateCraftingResult();
            for(int i = 0; i < craftingSlots.Length; i++) {
                if(craftingSlots[i]) {
                    craftingSlots[i] = null;
                    UpdateCraftingSlot(i);
                }
            }
            return;
        }
        for(int i = 0; i < craftingSlots.Length; i++) {
            if(craftingSlots[i]) {
                AddToInventory(craftingSlots[i], false);
                craftingSlots[i] = null;
                UpdateCraftingSlot(i);
            }            
        }                
    }
    public void ConsumeCraftingMaterials() {
        for(int i = 0; i < craftingSlots.Length; i++) {
            if(craftingSlots[i]) {
                craftingSlots[i] = null;
                UpdateCraftingSlot(i);
            }
        }
        UpdateCraftingResult();
    }
    public void CheckCrafting() {
        for(int i = 0; i < craftingDatabase.allRecipes.Count; i++) {
            int checker = 0;
            if(craftingSlots[0] == craftingDatabase.allRecipes[i].input0) {
                checker++;
            }
            if(craftingSlots[1] == craftingDatabase.allRecipes[i].input1) {
                checker++;
            }
            if(craftingSlots[2] == craftingDatabase.allRecipes[i].input2) {
                checker++;
            }
            if(craftingSlots[3] == craftingDatabase.allRecipes[i].input3) {
                checker++;
            }
            if(craftingSlots[4] == craftingDatabase.allRecipes[i].input4) {
                checker++;
            }
            if(craftingSlots[5] == craftingDatabase.allRecipes[i].input5) {
                checker++;
            }
            if(craftingSlots[6] == craftingDatabase.allRecipes[i].input6) {
                checker++;
            }
            if(craftingSlots[7] == craftingDatabase.allRecipes[i].input7) {
                checker++;
            }
            if(craftingSlots[8] == craftingDatabase.allRecipes[i].input8) {
                checker++;
            }
            if(craftingDatabase.allRecipes[i].strengthRequirement > 0 && stats.strength >= craftingDatabase.allRecipes[i].strengthRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].agilityRequirement > 0 && stats.agility >= craftingDatabase.allRecipes[i].agilityRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].dexterityRequirement > 0 && stats.dexterity >= craftingDatabase.allRecipes[i].dexterityRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].intelligenceRequirement > 0 && stats.intelligence >= craftingDatabase.allRecipes[i].intelligenceRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].faithRequirement > 0 && stats.faith >= craftingDatabase.allRecipes[i].faithRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].wisdomRequirement > 0 && stats.wisdom >= craftingDatabase.allRecipes[i].wisdomRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].vitalityRequirement > 0 && stats.vitality >= craftingDatabase.allRecipes[i].vitalityRequirement) {
                checker++;
            }
            else if(craftingDatabase.allRecipes[i].charismaRequirement > 0 && stats.charisma >= craftingDatabase.allRecipes[i].charismaRequirement) {
                checker++;
            }
            else if(player.raceIndex == 4) {
                if(craftingDatabase.allRecipes[i].strengthRequirement > 0 && stats.strength >= craftingDatabase.allRecipes[i].strengthRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].agilityRequirement > 0 && stats.agility >= craftingDatabase.allRecipes[i].agilityRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].dexterityRequirement > 0 && stats.dexterity >= craftingDatabase.allRecipes[i].dexterityRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].intelligenceRequirement > 0 && stats.intelligence >= craftingDatabase.allRecipes[i].intelligenceRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].faithRequirement > 0 && stats.faith >= craftingDatabase.allRecipes[i].faithRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].wisdomRequirement > 0 && stats.wisdom >= craftingDatabase.allRecipes[i].wisdomRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].vitalityRequirement > 0 && stats.vitality >= craftingDatabase.allRecipes[i].vitalityRequirement - 3) {
                    checker++;
                }
                else if(craftingDatabase.allRecipes[i].charismaRequirement > 0 && stats.charisma >= craftingDatabase.allRecipes[i].charismaRequirement - 3) {
                    checker++;
                }
            }
            if(craftingDatabase.allRecipes[i].strengthRequirement == 0
                && craftingDatabase.allRecipes[i].agilityRequirement == 0
                && craftingDatabase.allRecipes[i].dexterityRequirement == 0
                && craftingDatabase.allRecipes[i].intelligenceRequirement == 0
                && craftingDatabase.allRecipes[i].faithRequirement == 0
                && craftingDatabase.allRecipes[i].wisdomRequirement == 0
                && craftingDatabase.allRecipes[i].vitalityRequirement == 0
                && craftingDatabase.allRecipes[i].charismaRequirement == 0) {
                checker++;
            }
            //Debug.Log(checker);
            if(checker >= 10) {
                craft = craftingDatabase.allRecipes[i].output;
                UpdateCraftingResult();
                break;
            }
            else {
                craft = null;
                UpdateCraftingResult();                
            }
        }
        
    }
    public Item GetItem(int index) {
        return craftingSlots[index];
    }
    public void SetItem(Item item, int index) {
        craftingSlots[index] = item;
        UpdateCraftingSlot(index);
        CheckCrafting();
    }
    public void RemoveItem(int index) {
        SetItem(null, index);
        UpdateCraftingSlot(index);
        CheckCrafting();
    }
    public void UpdateCraftingSlot(int index) {
        if(craftingSlots[index] != null) {
            inventoryPanel.PutItem(craftingSlots[index], index);
        }
        else {
            inventoryPanel.ClearItem(index);
        }
    }
    public void UpdateCraftingResult() {
        if(craft != null) {
            inventoryPanel.SetCraft(craft);
        }
        else {
            inventoryPanel.ClearCraft();
        }
    }
    public void CheckLights(Item item) {
        if(!player) {
            return;
        }
        if(!item) {
            for(int i = 0; i < equipment.Count; i++) {
                if(equipment[i] && equipment[i].alterLight) {
                    item = equipment[i];
                    break;
                }
            }
        }
        // Qotush sight
        if(player.raceIndex == 7) {
            const float qotushOuter = 7f;
            const float qotushInner = 6f;
            playerLight.pointLightOuterRadius = qotushOuter;
            playerLight.pointLightInnerRadius = qotushInner;
        }
        else if(item && item.alterLight) {
            playerLight.pointLightInnerRadius = item.innerRadius;
            playerLight.pointLightOuterRadius = item.outerRadius;
            playerLight.intensity = item.intensity;
        }
        else {
            const float defaultOuter = 6f;
            const float defaultInner = 1f;
            playerLight.pointLightOuterRadius = defaultOuter;
            playerLight.pointLightInnerRadius = defaultInner;
        }
    }
    public void UpdateStats() {
        if(offenceStats.Length <= 0 || offenceStats[0] == null) {
            return;
        }
        offenceStats[0].text = stats.fireDamage.ToString(Format);
        offenceStats[1].text = stats.iceDamage.ToString(Format);
        offenceStats[2].text = stats.airDamage.ToString(Format);
        offenceStats[3].text = stats.earthDamage.ToString(Format);
        offenceStats[4].text = stats.lightningDamage.ToString(Format);
        offenceStats[5].text = stats.lightDamage.ToString(Format);
        offenceStats[6].text = stats.darkDamage.ToString(Format);
        offenceStats[7].text = stats.bashDamage.ToString(Format);
        offenceStats[8].text = stats.pierceDamage.ToString(Format);
        offenceStats[9].text = stats.slashDamage.ToString(Format);
        offenceStats[10].text = stats.bleedDamage.ToString(Format);
        offenceStats[11].text = stats.poisonDamage.ToString(Format);
        offenceStats[12].text = stats.curseDamage.ToString(Format);
        defenceStats[0].text = stats.fireDefence.ToString(Format);
        defenceStats[1].text = stats.iceDefence.ToString(Format);
        defenceStats[2].text = stats.airDefence.ToString(Format);
        defenceStats[3].text = stats.earthDefence.ToString(Format);
        defenceStats[4].text = stats.lightningDefence.ToString(Format);
        defenceStats[5].text = stats.lightDefence.ToString(Format);
        defenceStats[6].text = stats.darkDefence.ToString(Format);
        defenceStats[7].text = stats.bashDefence.ToString(Format);
        defenceStats[8].text = stats.pierceDefence.ToString(Format);
        defenceStats[9].text = stats.slashDefence.ToString(Format);
        defenceStats[10].text = stats.bleedDefence.ToString(Format);
        defenceStats[11].text = stats.poisonDefence.ToString(Format);
        defenceStats[12].text = stats.curseDefence.ToString(Format);
        inventoryPanel.UpdateTexts(stats);
    }
    public void SetInventory() {
        itemImages = new Image[inventorySize];
        itemImagesSecondary = new Image[inventorySize];
        itemImagesTertiary = new Image[inventorySize];
        equipmentImages = new Image[inventorySize];
        equipmentImagesSecondary = new Image[inventorySize];
        equipmentImagesTertiary = new Image[inventorySize];
        quantityTexts = new TextMeshProUGUI[inventorySize];

        for(int i = 0; i < itemSlots.Length; i++) {
            itemImages[i] = itemSlots[i].transform.GetChild(1).GetComponent<Image>();
            itemImagesSecondary[i] = itemSlots[i].transform.GetChild(2).GetComponent<Image>();
            itemImagesTertiary[i] = itemSlots[i].transform.GetChild(0).GetComponent<Image>();
            itemImages[i].color = Color.clear;
            itemImagesSecondary[i].color = Color.clear;
            itemImagesTertiary[i].color = Color.clear;
        }
        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentImages[i] = equipmentSlots[i].transform.GetChild(1).GetComponent<Image>();
            equipmentImagesSecondary[i] = equipmentSlots[i].transform.GetChild(2).GetComponent<Image>();
            equipmentImagesTertiary[i] = equipmentSlots[i].transform.GetChild(0).GetComponent<Image>();
            equipmentImages[i].color = Color.clear;
            equipmentImagesSecondary[i].color = Color.clear;
            equipmentImagesTertiary[i].color = Color.clear;
        }
        for(int i = 0; i < quantities.Length; i++) {
            quantityTexts[i] = itemSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            quantityTexts[i].text = "";
        }
        itemFrames = new Image[inventory.Count];
        for(int i = 0; i < itemFrames.Length; i++) {
            itemFrames[i] = itemSlots[i].transform.GetChild(4).GetComponent<Image>();
        }
        equipmentFrames = new Image[equipment.Count];
        for(int i = 0; i < equipmentFrames.Length; i++) {
            equipmentFrames[i] = equipmentSlots[i].transform.GetChild(3).GetComponent<Image>();
        }        
        //UpdateStats();
    }    
    public void UpdateSlot(int index) {
        if(inventory[index] != null) {
            itemImages[index].sprite = inventory[index].firstIcon ? inventory[index].firstIcon : null;
            itemImages[index].color = inventory[index].firstColor != null ? inventory[index].firstColor : Color.clear;
            itemImagesSecondary[index].sprite = inventory[index].secondIcon ? inventory[index].secondIcon : null;
            itemImagesSecondary[index].color = inventory[index].secondColor != null ? inventory[index].secondColor : Color.clear;
            itemImagesTertiary[index].sprite = inventory[index].thirdIcon ? inventory[index].thirdIcon : null;
            itemImagesTertiary[index].color = inventory[index].thirdColor != null ? inventory[index].thirdColor : Color.clear;
            quantityTexts[index].text = quantities[index] > 1 ? quantities[index].ToString() : "";
            itemFrames[index].color = ColorBySkillType.GetColorByRarity(inventory[index].rarity);
        }
        else {
            itemImages[index].sprite = null;
            itemImages[index].color = Color.clear;
            itemImagesSecondary[index].sprite = null;
            itemImagesSecondary[index].color = Color.clear;
            itemImagesTertiary[index].sprite = null;
            itemImagesTertiary[index].color = Color.clear;
            quantityTexts[index].text = "";
            itemFrames[index].color = Color.clear;
        }
    }
    public void UpdateEquipmentSlot(int index) {
        if(equipment[index] != null) {
            equipmentImages[index].sprite = equipment[index].firstIcon ? equipment[index].firstIcon : null;
            equipmentImages[index].color = equipment[index].firstColor != null ? equipment[index].firstColor : Color.clear;
            equipmentImagesSecondary[index].sprite = equipment[index].secondIcon ? equipment[index].secondIcon : null;
            equipmentImagesSecondary[index].color = equipment[index].secondColor != null ? equipment[index].secondColor : Color.clear;
            equipmentImagesTertiary[index].sprite = equipment[index].thirdIcon ? equipment[index].thirdIcon : null;
            equipmentImagesTertiary[index].color = equipment[index].thirdColor != null ? equipment[index].thirdColor : Color.clear;
            equipmentFrames[index].color = ColorBySkillType.GetColorByRarity(equipment[index].rarity);
        }
        else {
            equipmentImages[index].sprite = null;
            equipmentImages[index].color = Color.clear;
            equipmentImagesSecondary[index].sprite = null;
            equipmentImagesSecondary[index].color = Color.clear;
            equipmentImagesTertiary[index].sprite = null;
            equipmentImagesTertiary[index].color = Color.clear;
            equipmentFrames[index].color = Color.clear;
        }
    }
    public void SetInventoryImages() {
        for(int i = 0; i < inventorySize; i++) {
            if(inventory[i] != null) {
                itemImages[i].sprite = inventory[i].firstIcon ? inventory[i].firstIcon : null;
                itemImages[i].color = inventory[i].firstColor != null ? inventory[i].firstColor : Color.clear;
                itemImagesSecondary[i].sprite = inventory[i].secondIcon ? inventory[i].secondIcon : null;
                itemImagesSecondary[i].color = inventory[i].secondColor != null ? inventory[i].secondColor : Color.clear;
                itemImagesTertiary[i].sprite = inventory[i].thirdIcon ? inventory[i].thirdIcon : null;
                itemImagesTertiary[i].color = inventory[i].thirdColor != null ? inventory[i].thirdColor : Color.clear;
                quantityTexts[i].text = quantities[i] > 1 ? quantities[i].ToString() : "";
                itemFrames[i].color = ColorBySkillType.GetColorByRarity(inventory[i].rarity);
            }
            else {
                itemImages[i].sprite = null;
                itemImages[i].color = Color.clear;
                itemImagesSecondary[i].sprite = null;
                itemImagesSecondary[i].color = Color.clear;
                itemImagesTertiary[i].sprite = null;
                itemImagesTertiary[i].color = Color.clear;
                quantityTexts[i].text = "";
                itemFrames[i].color = Color.clear;
            }
        }
        for(int i = 0; i < equipment.Count; i++) {
            if(equipment[i] != null) {
                equipmentImages[i].sprite = equipment[i].firstIcon ? equipment[i].firstIcon : null;
                equipmentImages[i].color = equipment[i].firstColor != null ? equipment[i].firstColor : Color.clear;
                equipmentImagesSecondary[i].sprite = equipment[i].secondIcon ? equipment[i].secondIcon : null;
                equipmentImagesSecondary[i].color = equipment[i].secondColor != null ? equipment[i].secondColor : Color.clear;
                equipmentImagesTertiary[i].sprite = equipment[i].thirdIcon ? equipment[i].thirdIcon : null;
                equipmentImagesTertiary[i].color = equipment[i].thirdColor != null ? equipment[i].thirdColor : Color.clear;
                equipmentFrames[i].color = ColorBySkillType.GetColorByRarity(equipment[i].rarity);
            }
            else {
                equipmentImages[i].sprite = null;
                equipmentImages[i].color = Color.clear;
                equipmentImagesSecondary[i].sprite = null;
                equipmentImagesSecondary[i].color = Color.clear;
                equipmentImagesTertiary[i].sprite = null;
                equipmentImagesTertiary[i].color = Color.clear;
                equipmentFrames[i].color = Color.clear;
            }
        }
    }
    public bool CanConsumeItem(IUsable usable) {
        Item usableItem = usable as Item;
        for(int i = 0; i < inventory.Count; i++) {
            if(usableItem == inventory[i]) {
                return true;
            }
        }
        return false;
    }
    public int GetItemCount(IUsable usable) {
        Item usableItem = usable as Item;
        for(int i = 0; i < inventory.Count; i++) {
            if(usableItem == inventory[i]) {
                return quantities[i];
            }
        }
        return 0;
    }
    public void ConsumeItem(IUsable usable) {
        Item usableItem = usable as Item;
        for(int i = 0; i < inventory.Count; i++) {
            if(usableItem == inventory[i]) {
                ConsumeInSlot(i);                
                return;
            }
        }
    }
    public void ConsumeInSlot(int slotIndex) {
        if(!inventory[slotIndex] || quantities[slotIndex] < 1) {
            return;
        }
        quantities[slotIndex]--;
        if(inventory[slotIndex] is Mushroom m) {
            m.Consume(stats.status);
        }
        else if(inventory[slotIndex] is Book b) {
            b.Consume(stats.status);
            CheckCrafting();
        }
        else if(inventory[slotIndex] is Potion p) {
            p.Consume(stats.status);
        }
        else if(inventory[slotIndex] is Meat meat) {
            meat.Consume(stats.status);
        }
        if(quantities[slotIndex] < 1) {
            inventory[slotIndex] = null;
            lucks[slotIndex] = 0;
        }
        UpdateSlot(slotIndex);
        UpdateStats();
    }
    public void DismantleInSlot(int slotIndex) {
        if(!inventory[slotIndex]) {
            return;
        }
        if(!CanAddToInventory()) {
            return;
        }
        for(int i = 0; i < inventory[slotIndex].dismantleOutput; i++) {
            AddToInventory(itemDatabase.GetItemByMaterial(inventory[slotIndex].itemMaterial), true);
        }
        inventory[slotIndex] = null;
        quantities[slotIndex]--;
        lucks[slotIndex] = 0;
        UpdateSlot(slotIndex);
    }
    public void EquipItem(Item item) {
        // RightHand, LeftHand, Head, Body, Legs, Finger, Consumable
        /*if(equipment[(int)item.slot] != null) {
            AddToInventory(equipment[(int)item.slot]);
        }*/
        if(!item) {
            return;
        }
        if(item.slot == EquipSlot.Consumable) {
            return;
        }
        if(item is Weapon) {
            Weapon w = item as Weapon;
            if(w.weaponType != WeaponType.OneHanded) {
                if(CanAddToInventory()) {
                    UnequipItem((int)EquipSlot.LeftHand, GetEmptyInventorySlot());
                }
                else {
                    return;
                }
            }
            player.SetItem(w);
        }
        else if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                player.SetItem(item);
            }
            if(item.slot == EquipSlot.Head) {
                player.SetItem(item);
            }
        }
        else if(item is Shield) {
            Shield s = item as Shield;
            Weapon w = equipment[(int)EquipSlot.RightHand] as Weapon;
            if(w != null && CanAddToInventory() && (w.weaponType == WeaponType.TwoHanded || w.weaponType == WeaponType.Dagger)) {
                UnequipItem((int)EquipSlot.RightHand, GetEmptyInventorySlot());
            }
            else if(equipment[(int)EquipSlot.LeftHand] && CanAddToInventory()) {
                UnequipItem((int)EquipSlot.LeftHand, GetEmptyInventorySlot());
            }
            player.SetItem(s);
        }
        else if(item is Ring) {
            // Debug.Log("Trying to equip ring");
        }
        equipment[(int)item.slot] = item;
        equipmentLucks[(int)item.slot] = item.luck;
        AudioSystem.audioManager.PlaySound("itemEquip", 0f);
        stats.OnItemEquip(item);
        if(item.alterLight) {
            CheckLights(item);
        }
        UpdateStats();
        UpdateEquipmentSlot((int)item.slot);
    }
    public void SellItem(int slot) {
        if(inventory[slot] == null) {
            return;
        }
        if(!CanAddToInventory()) {
            return;
        }
        for(int i = 0; i < inventory[slot].goldCost; i++) {
            AddToInventory(itemDatabase.items[87], false);
        }
        for(int i = 0; i < inventory[slot].silverCost; i++) {
            AddToInventory(itemDatabase.items[88], false);
        }
        TakeItemFromSlot(slot);
        AudioSystem.audioManager.PlaySound("sell", 0f);
    }
    public void EquipItemInSlot(int slot) {
        Item item;
        if(inventory[slot] != null) {
            item = inventory[slot];
        }
        else {
            return;
        }
        if(!CanAddToInventory()) {
            return;
        }
        if(item && item.slot != EquipSlot.Consumable && !equipment[(int)item.slot]) {
            inventory[slot] = null;
            quantities[slot]--;
            lucks[slot] = 0;
            EquipItem(item);
            UpdateSlot(slot);
        }
        else if(item && item.slot != EquipSlot.Consumable && equipment[(int)item.slot]) {
            Item tempItem = equipment[(int)item.slot];
            //equipment[(int)item.slot] = null;
            inventory[slot] = null;
            quantities[slot]--;
            lucks[slot] = 0;
            player.ClearItem(tempItem);
            EquipItem(item);
            AddToInventory(tempItem, false);
            stats.OnItemUnequip(tempItem);
            UpdateStats();
            //UnequipItem(slot, GetEmptyInventorySlot());
            UpdateSlot((int)tempItem.slot);
            UpdateSlot(slot);
        }
    }
    public bool AddToInventory(Item item, bool playSound) {
        if(!item) {
            return false;
        }
        bool exists = false;
        if(item.stackable) {
            for(int i = 0; i < inventorySize; i++) {
                if(inventory[i] == item) {
                    exists = true;
                    break;
                }
            }
            if(exists) {
                for(int i = 0; i < inventorySize; i++) {
                    if(inventory[i] == item) {
                        quantities[i]++;
                        lucks[i] = inventory[i].luck;
                        UpdateSlot(i);
                        skillUI.UpdateQuantities();
                        if(playSound) {
                            AudioSystem.audioManager.PlaySound("pickItem", 0f);
                        }
                        return true;
                    }
                }
            }
            else {
                for(int i = 0; i < inventorySize; i++) {
                    if(inventory[i] == null) {
                        inventory[i] = item;
                        quantities[i]++;
                        lucks[i] = inventory[i].luck;
                        UpdateSlot(i);
                        skillUI.UpdateQuantities();
                        if(playSound) {
                            AudioSystem.audioManager.PlaySound("pickItem", 0f);
                        }
                        return true;                        
                    }
                }
            }
            return false;
        }
        for(int i = 0; i < inventorySize; i++) {
            /*
            if(item.stackable && inventory[i] == item) {
                inventory[i] = item;
                quantities[i]++;
                UpdateSlot(i);
                return true;
            }
            else if(item.stackable ) {
                continue;
            }*/
            if(inventory[i] != null) {
                continue;
            }
            else {
                inventory[i] = item;
                quantities[i]++;
                lucks[i] = inventory[i].luck;
                UpdateSlot(i);
                skillUI.UpdateQuantities();
                AudioSystem.audioManager.PlaySound("pickItem", 0f);
                return true;
            }            
        }
        return false;
    }
    private int GetEmptyInventorySlot() {
        for(int i = 0; i < inventorySize; i++) {
            if(inventory[i] != null) {
                continue;
            }
            else {
                return i;
            }
        }
        return -1;
    }
    public bool CanAddToInventory() {
        for(int i = 0; i < inventorySize; i++) {
            if(inventory[i] != null) {
                continue;
            }
            else {
                return true;
            }
        }
        return false;
    }
    public void TakeItemFromSlot(int slot) {
        if(!inventory[slot]) {
            return;
        }
        if(quantities[slot] < 1) {
            return;
        }
        --quantities[slot];        
        if(quantities[slot] < 1) {
            inventory[slot] = null;
            lucks[slot] = 0;
        }
        UpdateSlot(slot);        
    }
    public void MoveItem(int fromSlot, int toSlot) {
        if(!inventory[fromSlot]) {
            return;
        }
        if(!inventory[toSlot]) {
            inventory[toSlot] = inventory[fromSlot];
            //quantities[toSlot]++;
            inventory[fromSlot] = null;
            // Swap quantities
            int tempQuantity = quantities[toSlot];
            quantities[toSlot] = quantities[fromSlot];
            quantities[fromSlot] = tempQuantity;
            int tempLuck = lucks[toSlot];
            lucks[toSlot] = lucks[fromSlot];
            lucks[fromSlot] = tempLuck;
        }
        else {
            Item tempItem = inventory[toSlot];
            inventory[toSlot] = inventory[fromSlot];
            inventory[fromSlot] = tempItem;
            // Swap quantities
            int tempQuantity = quantities[toSlot];
            quantities[toSlot] = quantities[fromSlot];
            quantities[fromSlot] = tempQuantity;
            int tempLuck = lucks[toSlot];
            lucks[toSlot] = lucks[fromSlot];
            lucks[fromSlot] = tempLuck;
        }
        UpdateSlot(fromSlot);
        UpdateSlot(toSlot);
    }
    public void UnequipItem(int fromSlot, int toSlot) {
        if(!equipment[fromSlot]) {
            return;
        }
        if(!inventory[toSlot]) {
            inventory[toSlot] = equipment[fromSlot];            
            quantities[toSlot]++;
            lucks[toSlot] = inventory[toSlot].luck;
            UpdateSpritesOnUnequip(equipment[fromSlot]);
            stats.OnItemUnequip(equipment[fromSlot]);
            UpdateStats();
            equipment[fromSlot] = null;
            equipmentLucks[fromSlot] = 0;
        }
        else {
            return;
        }
        UpdateEquipmentSlot(fromSlot);
        UpdateSlot(toSlot);
        CheckLights(null);
    }
    private void UpdateSpritesOnUnequip(Item item) {
        if(item is Weapon) {
            player.ClearItem(item);
        }
        else if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                player.ClearItem(item);
            }
            else if(item.slot == EquipSlot.Head) {
                player.ClearItem(item);
            }
        }
        else if(item is Shield) {
            player.ClearItem(item);
        }
    }
    public void SetVisibilityOfCraftingSlot(bool visible, int index) {
        if(visible) {
            if(craftingSlots[index]) {
                inventoryPanel.PutItem(craftingSlots[index], index);
            }
        }
        else {
            if(craftingSlots[index]) {
                inventoryPanel.ClearItem(index);
            }
        }
    }
    public void SetVisibilityOfCraft(bool visible) {
        if(visible) {
            if(craft) {
                inventoryPanel.SetCraft(craft);
            }
        }
        else {
            if(craft) {
                inventoryPanel.ClearCraft();
            }
        }
    }
    public void SetVisibilityOfInventorySlot(bool visible, int index) {
        if(visible) {
            if(inventory[index]) {
                itemImages[index].sprite = inventory[index].firstIcon;
                itemImages[index].color = inventory[index].firstColor;
                itemImagesSecondary[index].sprite = inventory[index].secondIcon;
                itemImagesSecondary[index].color = inventory[index].secondColor;
                itemImagesTertiary[index].sprite = inventory[index].thirdIcon;
                itemImagesTertiary[index].color = inventory[index].thirdColor;
                quantityTexts[index].text = quantities[index] > 1 ? quantities[index].ToString() : "";
            }            
        }
        else {
            if(inventory[index]) {
                itemImages[index].sprite = null;
                itemImages[index].color = Color.clear;
                itemImagesSecondary[index].sprite = null;
                itemImagesSecondary[index].color = Color.clear;
                itemImagesTertiary[index].sprite = null;
                itemImagesTertiary[index].color = Color.clear;
                quantityTexts[index].text = "";
            }            
        }
    }
    public void SetVisibilityOfEquipmentSlot(bool visible, int index) {
        if(visible) {
            if(equipment[index]) {
                equipmentImages[index].sprite = equipment[index].firstIcon;
                equipmentImages[index].color = equipment[index].firstColor;
                equipmentImagesSecondary[index].sprite = equipment[index].secondIcon;
                equipmentImagesSecondary[index].color = equipment[index].secondColor;
                equipmentImagesTertiary[index].sprite = equipment[index].thirdIcon;
                equipmentImagesTertiary[index].color = equipment[index].thirdColor;
            }            
        }
        else {
            if(equipment[index]) {
                equipmentImages[index].sprite = null;
                equipmentImages[index].color = Color.clear;
                equipmentImagesSecondary[index].sprite = null;
                equipmentImagesSecondary[index].color = Color.clear;
                equipmentImagesTertiary[index].sprite = null;
                equipmentImagesTertiary[index].color = Color.clear;
            }            
        }
    }
}
