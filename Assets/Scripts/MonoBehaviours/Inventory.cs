using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public List<Item> equipment;
    public List<Item> inventory;
    private readonly int inventorySize = 70;

    public GameObject[] itemSlots;
    public GameObject[] equipmentSlots;
    private Image[] itemImages;
    private Image[] itemImagesSecondary;
    private Image[] itemImagesTertiary;
    private Image[] equipmentImages;
    private Image[] equipmentImagesSecondary;
    private Image[] equipmentImagesTertiary;

    private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();             
    }
    private void Start() {
        // Crate example weapon
        Weapon exampleItem = player.itemCreator.CreateWeaponSprite(Time.time.ToString());
        AddToInventory(exampleItem);
        EquipItemInSlot(0);
        player.SetWeapon(exampleItem);
        // Create example armor
        Armor testArmor = player.itemCreator.CreateChestArmorSprite(Time.time.ToString());
        AddToInventory(testArmor);
        EquipItemInSlot(0);
        player.SetBodyArmor(testArmor);
        // Create example helmet
        Armor testHelmet = player.itemCreator.CreateHelmetSprite(Time.time.ToString());
        AddToInventory(testHelmet);
        EquipItemInSlot(0);
        player.SetHelmet(testHelmet);
        // Create example legging
        Armor testLegging = player.itemCreator.CreateLeggingSprite(Time.time.ToString());
        AddToInventory(testLegging);
        EquipItemInSlot(0);
        // Create example shield
        Shield testShield = player.itemCreator.CreateShieldSprite(Time.time.ToString());
        AddToInventory(testShield);
        EquipItemInSlot(0);
        player.SetShield(testShield);
    }
    public void SetInventory() {
        itemImages = new Image[inventorySize];
        itemImagesSecondary = new Image[inventorySize];
        itemImagesTertiary = new Image[inventorySize];
        equipmentImages = new Image[inventorySize];
        equipmentImagesSecondary = new Image[inventorySize];
        equipmentImagesTertiary = new Image[inventorySize];

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
            }
            else {
                itemImages[i].sprite = null;
                itemImages[i].color = Color.clear;
                itemImagesSecondary[i].sprite = null;
                itemImagesSecondary[i].color = Color.clear;
                itemImagesTertiary[i].sprite = null;
                itemImagesTertiary[i].color = Color.clear;
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
        if(equipment[(int)item.slot] != null) {
            AddToInventory(equipment[(int)item.slot]);
        }
        equipment[(int)item.slot] = item;
    }
    public void EquipItemInSlot(int slot) {
        if(inventory[slot] != null && !inventory[slot].consumable) {
            Item item = inventory[slot];
            inventory[slot] = null;
            EquipItem(item);
        }
    }
    public bool AddToInventory(Item item) {
        for(int i = 0; i < inventorySize; i++) {
            if(inventory[i] != null) {
                continue;
            }
            else {
                inventory[i] = item;
                return true;
            }            
        }
        return false;
    }
}
