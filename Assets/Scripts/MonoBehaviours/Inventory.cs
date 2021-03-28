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

    public GameObject[] itemSlots;
    public GameObject[] equipmentSlots;
    public GameObject selectedItemSlot;
    private Image[] itemImages;
    private Image[] itemImagesSecondary;
    private Image[] itemImagesTertiary;
    private Image[] equipmentImages;
    private Image[] equipmentImagesSecondary;
    private Image[] equipmentImagesTertiary;
    private TextMeshProUGUI[] quantityTexts;

    private Player player;
    private Stats stats;

    private void Awake() {
        quantities = new int[70];
        player = FindObjectOfType<Player>();
        stats = GetComponent<Stats>();
        if(GetComponent<UICanvas>()) {
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
        if(GetComponent<UICanvas>()) {
            for(int i = 0; i < 40; i++) {
                Weapon weapon = player.itemCreator.CreateWeapon(i.ToString());
                AddToInventory(weapon);
            }
        }        
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
        }
        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentImages[i] = equipmentSlots[i].transform.GetChild(1).GetComponent<Image>();
            equipmentImagesSecondary[i] = equipmentSlots[i].transform.GetChild(2).GetComponent<Image>();
            equipmentImagesTertiary[i] = equipmentSlots[i].transform.GetChild(0).GetComponent<Image>();
        }
        for(int i = 0; i < quantities.Length; i++) {
            quantityTexts[i] = itemSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }
    }    
    public void UpdateSlot(int index) {
        if(inventory[index] != null) {
            itemImages[index].sprite = inventory[index].firstIcon;
            itemImages[index].color = inventory[index].firstColor;
            itemImagesSecondary[index].sprite = inventory[index].secondIcon;
            itemImagesSecondary[index].color = inventory[index].secondColor;
            itemImagesTertiary[index].sprite = inventory[index].thirdIcon;
            itemImagesTertiary[index].color = inventory[index].thirdColor;
            quantityTexts[index].text = quantities[index] > 1 ? quantities[index].ToString() : "";
        }
        else {
            itemImages[index].sprite = null;
            itemImages[index].color = Color.clear;
            itemImagesSecondary[index].sprite = null;
            itemImagesSecondary[index].color = Color.clear;
            itemImagesTertiary[index].sprite = null;
            itemImagesTertiary[index].color = Color.clear;
            quantityTexts[index].text = "";
        }
    }
    public void UpdateEquipmentSlot(int index) {
        if(equipment[index] != null) {
            equipmentImages[index].sprite = equipment[index].firstIcon;
            equipmentImages[index].color = equipment[index].firstColor;
            equipmentImagesSecondary[index].sprite = equipment[index].secondIcon;
            equipmentImagesSecondary[index].color = equipment[index].secondColor;
            equipmentImagesTertiary[index].sprite = equipment[index].thirdIcon;
            equipmentImagesTertiary[index].color = equipment[index].thirdColor;
        }
        else {
            equipmentImages[index].sprite = null;
            equipmentImages[index].color = Color.clear;
            equipmentImagesSecondary[index].sprite = null;
            equipmentImagesSecondary[index].color = Color.clear;
            equipmentImagesTertiary[index].sprite = null;
            equipmentImagesTertiary[index].color = Color.clear;
        }
    }
    public void SetInventoryImages() {
        for(int i = 0; i < inventorySize; i++) {
            if(inventory[i] != null) {
                itemImages[i].sprite = inventory[i].firstIcon;
                itemImages[i].color = inventory[i].firstColor;
                itemImagesSecondary[i].sprite = inventory[i].secondIcon;
                itemImagesSecondary[i].color = inventory[i].secondColor;
                itemImagesTertiary[i].sprite = inventory[i].thirdIcon;
                itemImagesTertiary[i].color = inventory[i].thirdColor;
                quantityTexts[i].text = quantities[i] > 1 ? quantities[i].ToString() : "";
            }
            else {
                itemImages[i].sprite = null;
                itemImages[i].color = Color.clear;
                itemImagesSecondary[i].sprite = null;
                itemImagesSecondary[i].color = Color.clear;
                itemImagesTertiary[i].sprite = null;
                itemImagesTertiary[i].color = Color.clear;
                quantityTexts[i].text = "";
            }
        }
        for(int i = 0; i < equipment.Count; i++) {
            if(equipment[i] != null) {
                equipmentImages[i].sprite = equipment[i].firstIcon;
                equipmentImages[i].color = equipment[i].firstColor;
                equipmentImagesSecondary[i].sprite = equipment[i].secondIcon;
                equipmentImagesSecondary[i].color = equipment[i].secondColor;
                equipmentImagesTertiary[i].sprite = equipment[i].thirdIcon;
                equipmentImagesTertiary[i].color = equipment[i].thirdColor;
            }
            else {
                equipmentImages[i].sprite = null;
                equipmentImages[i].color = Color.clear;
                equipmentImagesSecondary[i].sprite = null;
                equipmentImagesSecondary[i].color = Color.clear;
                equipmentImagesTertiary[i].sprite = null;
                equipmentImagesTertiary[i].color = Color.clear;
            }
        }
    }
    public void EquipItem(Item item) {
        // RightHand, LeftHand, Head, Body, Legs, Finger, Consumable
        /*if(equipment[(int)item.slot] != null) {
            AddToInventory(equipment[(int)item.slot]);
        }*/
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
            player.SetWeapon(w);
        }
        else if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                player.SetBodyArmor(item as Armor);
            }
            if(item.slot == EquipSlot.Head) {
                player.SetHelmet(item as Armor);
            }
        }
        else if(item is Shield) {
            Shield s = item as Shield;
            if(CanAddToInventory()) {
                UnequipItem((int)EquipSlot.LeftHand, GetEmptyInventorySlot());
            }
            else {
                return;
            }
            player.SetShield(s);
        }
        else if(item is Ring) {
            Debug.Log("Trying to equip ring");
        }
        equipment[(int)item.slot] = item;
        stats.OnItemEquip(item);
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
        if(item && !item.consumable && !equipment[(int)item.slot]) {
            inventory[slot] = null;
            quantities[slot]--;
            EquipItem(item);            
        }
        else if(item && !item.consumable && equipment[(int)item.slot]) {
            Item tempItem = equipment[(int)item.slot];
            //equipment[(int)item.slot] = null;
            inventory[slot] = null;
            quantities[slot]--;
            EquipItem(item);
            AddToInventory(tempItem);
            stats.OnItemUnequip(tempItem);
            //UnequipItem(slot, GetEmptyInventorySlot());
            UpdateSlot((int)tempItem.slot);
        }
    }
    public bool AddToInventory(Item item) {
        for(int i = 0; i < inventorySize; i++) {
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
    private bool CanAddToInventory() {
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
            quantities[toSlot]++;
            inventory[fromSlot] = null;
            quantities[fromSlot]--;
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
            equipment[fromSlot] = null;            
        }
        else {
            return;
        }
        UpdateEquipmentSlot(fromSlot);
        UpdateSlot(toSlot);
    }
    private void UpdateSpritesOnUnequip(Item item) {
        if(item is Weapon) {
            player.ClearWeapons();
        }
        else if(item is Armor) {
            if(item.slot == EquipSlot.Body) {
                player.ClearBodyArmor();
            }
            else if(item.slot == EquipSlot.Head) {
                player.ClearHelmet();
            }
        }
        else if(item is Shield) {
            player.ClearShield();
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
