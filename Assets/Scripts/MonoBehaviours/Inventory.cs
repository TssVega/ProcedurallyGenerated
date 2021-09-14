using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {

    public List<Item> equipment;
    public List<Item> inventory;
    public int[] quantities;
    private readonly int inventorySize = 70;
    private readonly int equipmentSize = 6;

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

    private void Awake() {
        quantities = new int[70];
        player = FindObjectOfType<Player>();
        stats = GetComponent<Stats>();
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
        }        
    }
    private void Start() {
        UpdateStats();
        // Crate example weapon
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
    public void UpdateStats() {
        if(offenceStats.Length <= 0 || offenceStats[0] == null) {
            return;
        }
        offenceStats[0].text = stats.fireDamage.ToString();
        offenceStats[1].text = stats.iceDamage.ToString();
        offenceStats[2].text = stats.airDamage.ToString();
        offenceStats[3].text = stats.earthDamage.ToString();
        offenceStats[4].text = stats.lightningDamage.ToString();
        offenceStats[5].text = stats.lightDamage.ToString();
        offenceStats[6].text = stats.darkDamage.ToString();
        offenceStats[7].text = stats.bashDamage.ToString();
        offenceStats[8].text = stats.pierceDamage.ToString();
        offenceStats[9].text = stats.slashDamage.ToString();
        offenceStats[10].text = stats.bleedDamage.ToString();
        offenceStats[11].text = stats.poisonDamage.ToString();
        offenceStats[12].text = stats.curseDamage.ToString();
        defenceStats[0].text = stats.fireDefence.ToString();
        defenceStats[1].text = stats.iceDefence.ToString();
        defenceStats[2].text = stats.airDefence.ToString();
        defenceStats[3].text = stats.earthDefence.ToString();
        defenceStats[4].text = stats.lightningDefence.ToString();
        defenceStats[5].text = stats.lightDefence.ToString();
        defenceStats[6].text = stats.darkDefence.ToString();
        defenceStats[7].text = stats.bashDefence.ToString();
        defenceStats[8].text = stats.pierceDefence.ToString();
        defenceStats[9].text = stats.slashDefence.ToString();
        defenceStats[10].text = stats.bleedDefence.ToString();
        defenceStats[11].text = stats.poisonDefence.ToString();
        defenceStats[12].text = stats.curseDefence.ToString();
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
        if(quantities[slotIndex] < 1) {
            inventory[slotIndex] = null;
        }
        UpdateSlot(slotIndex);
    }
    public void DismantleInSlot(int slotIndex) {
        if(!inventory[slotIndex]) {
            return;
        }
        if(!CanAddToInventory()) {
            return;
        }
        for(int i = 0; i < inventory[slotIndex].dismantleOutput; i++) {
            AddToInventory(itemDatabase.GetItemByMaterial(inventory[slotIndex].itemMaterial));
        }
        inventory[slotIndex] = null;
        quantities[slotIndex]--;
        UpdateSlot(slotIndex);
    }
    public void EquipItem(Item item) {
        // RightHand, LeftHand, Head, Body, Legs, Finger, Consumable
        /*if(equipment[(int)item.slot] != null) {
            AddToInventory(equipment[(int)item.slot]);
        }*/
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
            if(w != null && CanAddToInventory() && w.weaponType == WeaponType.TwoHanded) {
                UnequipItem((int)EquipSlot.RightHand, GetEmptyInventorySlot());
            }
            player.SetItem(s);
        }
        else if(item is Ring) {
            // Debug.Log("Trying to equip ring");
        }
        equipment[(int)item.slot] = item;
        stats.OnItemEquip(item);
        UpdateStats();
        UpdateEquipmentSlot((int)item.slot);
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
            EquipItem(item);            
        }
        else if(item && item.slot != EquipSlot.Consumable && equipment[(int)item.slot]) {
            Item tempItem = equipment[(int)item.slot];
            //equipment[(int)item.slot] = null;
            inventory[slot] = null;
            quantities[slot]--;
            EquipItem(item);
            AddToInventory(tempItem);
            stats.OnItemUnequip(tempItem);
            UpdateStats();
            //UnequipItem(slot, GetEmptyInventorySlot());
            UpdateSlot((int)tempItem.slot);
        }
    }
    public bool AddToInventory(Item item) {
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
                        UpdateSlot(i);
                        return true;
                    }
                }
            }
            else {
                for(int i = 0; i < inventorySize; i++) {
                    if(inventory[i] == null) {
                        inventory[i] = item;
                        quantities[i]++;
                        UpdateSlot(i);
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
                UpdateSlot(i);
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
        }
        else {
            Item tempItem = inventory[toSlot];
            inventory[toSlot] = inventory[fromSlot];
            inventory[fromSlot] = tempItem;
            // Swap quantities
            int tempQuantity = quantities[toSlot];
            quantities[toSlot] = quantities[fromSlot];
            quantities[fromSlot] = tempQuantity;
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
            UpdateSpritesOnUnequip(equipment[fromSlot]);
            stats.OnItemUnequip(equipment[fromSlot]);
            UpdateStats();
            equipment[fromSlot] = null;
            Debug.Log("Unequipping");
        }
        else {
            return;
        }
        UpdateEquipmentSlot(fromSlot);
        UpdateSlot(toSlot);
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
