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
    // Shield sprites
    public Color[] shieldBaseColors;
    public Color[] shieldPropColors;
    public Sprite[] shieldBases;
    public Sprite[] shieldProps;
    public Sprite[] shieldInGame;
    // Ring sprites
    public Color[] ringBaseColors;
    public Color[] ringJewelColors;
    public Sprite[] ringBases;
    public Sprite[] ringSockets;
    public Sprite[] ringJewels;
    // Bow sprites
    public Color[] bowColors;
    public Sprite[] bowBases;
    // Create an item
    public Item CreateItem(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Item item;
        int slotIndex = pseudoRandom.Next(0, 6);
        if(slotIndex == 0) {
            Weapon w = CreateWeapon(seed);
            item = w;
        }
        else if(slotIndex == 1) {
            Shield s = CreateShield(seed);
            item = s;
        }
        else if(slotIndex == 2) {
            // Head armor h
            Armor h = CreateHelmet(seed);
            item = h;
        }
        else if(slotIndex == 3) {
            // Chest armor c
            Armor a = CreateChestArmor(seed);
            item = a;
        }
        else if(slotIndex == 4) {
            // Leg armor l
            Armor l = CreateLegging(seed);
            item = l;
        }
        else if(slotIndex == 5) {
            Ring r = CreateRing(seed);
            item = r;
        }
        else {
            item = null;
        }
        return item;
    }
    // Create a weapon sprite
    public Weapon CreateWeapon(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        WeaponType type = WeaponType.OneHanded;
        int estimatedPower = pseudoRandom.Next(0, 100);
        //int index = pseudoRandom.Next(0, 4);
        int index = 0;  // To try out bows
        if(index == 0) {
            type = WeaponType.OneHanded;
        }
        else if(index == 1) {
            type = WeaponType.TwoHanded;
        }
        else if(index == 2) {
            type = WeaponType.Bow;
        }
        else if(index == 3) {
            type = WeaponType.Dagger;
        }
        Weapon weapon = CreateInstance<Weapon>();
        if(type == WeaponType.OneHanded) {
            Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
            Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
            Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
            Color handleColor = weaponHandleColors[pseudoRandom.Next(0, weaponHandleColors.Length)];
            Color guardColor = weaponGuardColors[pseudoRandom.Next(0, weaponGuardColors.Length)];
            Color bladeColor = weaponBladeColors[pseudoRandom.Next(0, weaponBladeColors.Length)];
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
            weapon.weaponType = WeaponType.OneHanded;
        }
        else if(type == WeaponType.TwoHanded) {
            weapon.slot = EquipSlot.RightHand;
        }
        else if(type == WeaponType.Bow) {
            Sprite bowBase = bowBases[pseudoRandom.Next(0, bowBases.Length)];
            Color bowColor = bowColors[pseudoRandom.Next(0, bowColors.Length)];
            weapon.firstSprite = bowBase;
            weapon.firstIcon = bowBase;
            weapon.firstColor = bowColor;
            weapon.slot = EquipSlot.RightHand;
            weapon.weaponType = WeaponType.Bow;
            weapon.bashDamage = pseudoRandom.Next(0, estimatedPower);
            weapon.pierceDamage = pseudoRandom.Next(0, estimatedPower);
            weapon.slashDamage = pseudoRandom.Next(0, estimatedPower);
        }
        return weapon;
    }
    // Create a chest armor sprite
    // TODO: Match chest armor sprite and icons to be consistent
    public Armor CreateChestArmor(string seed) {
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
    public Armor CreateHelmet(string seed) {
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
    public Armor CreateLegging(string seed) {
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
    public Shield CreateShield(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite shieldBase = shieldBases[pseudoRandom.Next(0, shieldBases.Length)];
        Sprite shieldProp = shieldProps[pseudoRandom.Next(0, shieldProps.Length)];
        Sprite inGame = shieldInGame[pseudoRandom.Next(0, shieldInGame.Length)];
        Color baseColor = shieldBaseColors[pseudoRandom.Next(0, shieldPropColors.Length)];
        Color propColor = shieldPropColors[pseudoRandom.Next(0, shieldPropColors.Length)];
        Shield shield = CreateInstance<Shield>();
        shield.firstIcon = shieldBase;
        shield.secondIcon = shieldProp;
        shield.firstSprite = inGame;
        shield.firstColor = baseColor;
        shield.secondColor = propColor;
        shield.slot = EquipSlot.LeftHand;
        return shield;
    }
    public Ring CreateRing(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite ringBase = ringBases[pseudoRandom.Next(0, ringBases.Length)];
        Sprite ringSocket = ringSockets[pseudoRandom.Next(0, ringSockets.Length)];
        Sprite ringJewel = ringJewels[pseudoRandom.Next(0, ringJewels.Length)];
        Color baseColor = ringBaseColors[pseudoRandom.Next(0, ringBaseColors.Length)];
        Color jewelColor = ringJewelColors[pseudoRandom.Next(0, ringJewelColors.Length)];
        Ring ring = CreateInstance<Ring>();
        ring.firstIcon = ringBase;
        ring.firstColor = baseColor;
        if(ringJewel != null) {
            ring.secondIcon = ringSocket;
            ring.thirdIcon = ringJewel;
            ring.secondColor = baseColor;
            ring.thirdColor = jewelColor;
        }              
        ring.slot = EquipSlot.Finger;
        return ring;
    }
}
