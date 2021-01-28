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
    // Helmet sprites
    public Color[] helmetBaseColors;
    public Color helmetPropColor = Color.white;
    public Sprite[] helmetBases;
    public Sprite[] helmetProps;
    public Sprite[] helmetBasesInGame;
    public Sprite[] helmetPropsInGame;
    // Legging sprites
    public Color[] leggingBaseColors;
    public Color[] leggingPropColors;
    public Sprite[] leggingBases;
    public Sprite[] leggingProps;
    // Create an item
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
            Armor h = CreateHelmetSprite(seed);
            item = h;
        }
        else if(slotIndex == 3) {
            // Chest armor c
            Armor a = CreateChestArmorSprite(seed);
            item = a;
        }
        else if(slotIndex == 4) {
            // Leg armor l
            Armor l = CreateLeggingSprite(seed);
            item = l;
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
    // Create a weapon sprite
    public Weapon CreateWeaponSprite(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
        Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
        Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
        Color handleColor = weaponHandleColors[pseudoRandom.Next(0, weaponHandleColors.Length)];
        Color guardColor = weaponGuardColors[pseudoRandom.Next(0, weaponGuardColors.Length)];
        Color bladeColor = weaponBladeColors[pseudoRandom.Next(0, weaponBladeColors.Length)];
        Weapon weapon = CreateInstance<Weapon>();
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
    // Create a chest armor sprite
    // TODO: Match chest armor sprite and icons to be consistent
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
        Armor armor = CreateInstance<Armor>();
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
    // Create a helmet sprite
    // TODO: Match helmet sprite and icons to be consistent
    public Armor CreateHelmetSprite(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite helmetBase = helmetBases[pseudoRandom.Next(0, helmetBases.Length)];
        Sprite helmetProp = helmetProps[pseudoRandom.Next(0, helmetProps.Length)];
        Sprite helmetBaseInGame = helmetBasesInGame[pseudoRandom.Next(0, helmetBasesInGame.Length)];
        Sprite helmetPropInGame = helmetPropsInGame[pseudoRandom.Next(0, helmetPropsInGame.Length)];
        Color baseColor = helmetBaseColors[pseudoRandom.Next(0, helmetBaseColors.Length)];
        Color propColor = helmetPropColor;
        Armor helmet = CreateInstance<Armor>();
        helmet.firstIcon = helmetBase;
        helmet.secondIcon = helmetProp;
        helmet.firstSprite = helmetBaseInGame;
        helmet.secondSprite = helmetPropInGame;
        helmet.firstColor = baseColor;
        helmet.secondColor = propColor;
        helmet.slot = EquipSlot.Head;
        return helmet;
    }
    public Armor CreateLeggingSprite(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite leggingBase = leggingBases[pseudoRandom.Next(0, leggingBases.Length)];
        Sprite leggingProp = leggingProps[pseudoRandom.Next(0, leggingProps.Length)];
        Color baseColor = leggingBaseColors[pseudoRandom.Next(0, leggingBaseColors.Length)];
        Color propColor = leggingPropColors[pseudoRandom.Next(0, leggingPropColors.Length)];        
        Armor legging = CreateInstance<Armor>();
        legging.firstIcon = leggingBase;
        legging.secondIcon = leggingProp;
        legging.firstColor = baseColor;
        legging.secondColor = propColor;
        legging.slot = EquipSlot.Legs;
        return legging;
    }
}
