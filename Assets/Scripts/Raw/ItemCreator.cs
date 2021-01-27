using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Creator")]
public class ItemCreator : ScriptableObject {
    // Weapons
    public Color[] weaponHandleColors;
    public Color[] weaponGuardColors;
    public Color[] weaponBladeColors;
    public Sprite[] weaponHandles;
    public Sprite[] weaponGuards;
    public Sprite[] weaponBlades;
    // UI view of chest armor
    public Color[] chestArmorBaseColors;
    public Color[] chestArmorOverlayColors;
    public Color[] chestArmorBackColors;
    public Sprite[] chestArmorBases;
    public Sprite[] chestArmorOverlays;
    public Sprite[] chestArmorBacks;
    // In game sprites of chest armor
    public Sprite[] chestArmorInGame;

    public Item CreateItem(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Item item;
        int slotIndex = pseudoRandom.Next(0, 6);
        if(slotIndex == 0) {
            Weapon w = CreateWeaponSprite(seed);
            item = w;
        }
        else if(slotIndex == 1) {
            // Shield s
            item = null;
        }
        else if(slotIndex == 2) {
            // Head armor h
            item = null;
        }
        else if(slotIndex == 3) {
            // Chest armor c
            Armor a = CreateChestArmorSprite(seed);
            item = null;
        }
        else if(slotIndex == 4) {
            // Leg armor l
            item = null;
        }
        else if(slotIndex == 5) {
            // Ring r
            item = null;
        }
        else {
            item = null;
        }
        return item;
    }

    public Weapon CreateWeaponSprite(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
        Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
        Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
        Color handleColor = weaponHandleColors[pseudoRandom.Next(0, weaponHandleColors.Length)];
        Color guardColor = weaponGuardColors[pseudoRandom.Next(0, weaponGuardColors.Length)];
        Color bladeColor = weaponBladeColors[pseudoRandom.Next(0, weaponBladeColors.Length)];
        Weapon weapon = CreateInstance("Weapon") as Weapon;
        weapon.firstSprite = handle;
        weapon.secondSprite = guard;
        weapon.thirdSprite = blade;
        weapon.firstIcon = handle;
        weapon.secondIcon = guard;
        weapon.thirdIcon = blade;
        weapon.firstColor = handleColor;
        weapon.secondColor = guardColor;
        weapon.thirdColor = bladeColor;
        weapon.slot = EquipSlot.RightHand;
        return weapon;
    }

    public Armor CreateChestArmorSprite(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite armorBase = chestArmorBases[pseudoRandom.Next(0, chestArmorBases.Length)];
        Sprite armorOverlay = chestArmorOverlays[pseudoRandom.Next(0, chestArmorOverlays.Length)];
        Sprite armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
        Sprite inGameArmor = chestArmorInGame[pseudoRandom.Next(0, chestArmorInGame.Length)];
        Color baseColor = chestArmorBaseColors[pseudoRandom.Next(0, chestArmorBaseColors.Length)];
        Color overlayColor = chestArmorOverlayColors[pseudoRandom.Next(0, chestArmorOverlayColors.Length)];
        Color backColor = chestArmorBackColors[pseudoRandom.Next(0, chestArmorBackColors.Length)];
        //Sprite 
        Armor armor = CreateInstance("Armor") as Armor;
        armor.firstIcon = armorBase;
        armor.secondIcon = armorOverlay;
        armor.thirdIcon = armorBack;
        armor.firstSprite = inGameArmor;
        armor.firstColor = baseColor;
        armor.secondColor = overlayColor;
        armor.thirdColor = backColor;
        armor.slot = EquipSlot.Body;
        return armor;
    }
}
