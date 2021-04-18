using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Creator")]
public class ItemCreator : ScriptableObject {
    // Mine and weapon material colors
    public Color[] bladeMaterialColors;
    public Color[] otherMaterialColors;
    // Weapons
    public Sprite[] weaponHandles;
    public Sprite[] weaponGuards;
    public Sprite[] weaponBlades;
    // UI view of chest armor
    public Sprite[] chestArmorBases;
    public Sprite[] chestArmorOverlays;
    public Sprite[] chestArmorBacks;
    // In game sprites of chest armor
    public Sprite[] chestArmorInGame;
    // Helmet sprites
    public Color helmetPropColor = Color.white;
    public Sprite[] helmetBases;
    public Sprite[] helmetProps;
    public Sprite[] helmetBasesInGame;
    public Sprite[] helmetPropsInGame;
    // Legging sprites
    public Sprite[] leggingBases;
    public Sprite[] leggingProps;
    // Shield sprites
    public Sprite[] shieldBases;
    public Sprite[] shieldProps;
    public Sprite[] shieldInGame;
    // Ring sprites
    public Sprite[] ringBases;
    public Sprite[] ringSockets;
    public Sprite[] ringJewels;
    // Bow sprites
    public Color[] bowColors;
    public Sprite[] bowBases;

    private readonly int[] metalArmorIndices = new int[] { 0, 3, 6, 8 };
    private readonly int[] leatherArmorIndices = new int[] { 1, 2, 5 };
    private readonly int[] clothArmorIndices = new int[] { 4, 7 };
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
        if(item) {
            item.seed = seed;
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
            Color handleColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
            Color guardColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
            Color bladeColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
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
    public Armor CreateChestArmor(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        int armorBaseIndex = pseudoRandom.Next(0, chestArmorBases.Length);
        Sprite armorBase = chestArmorBases[armorBaseIndex];
        Sprite armorOverlay = null;
        Sprite armorBack = null;
        Sprite inGameArmor = null;
        if(metalArmorIndices.Contains(armorBaseIndex)) {
            int armorOverlayIndex = pseudoRandom.Next(0, chestArmorOverlays.Length);
            armorOverlay = chestArmorOverlays[armorOverlayIndex];
            armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
            if(new int[] { 2, 9 }.Contains(armorOverlayIndex)) {
                // Small shoulders
                Debug.Log("Small shoulders");
                inGameArmor = chestArmorInGame[1];
            }
            else if(new int[] { 5, 6, 7, 8 }.Contains(armorOverlayIndex)) {
                // Big shoulders
                Debug.Log("Big shoulders");
                int overlay = pseudoRandom.Next(0, 4);
                if(overlay == 1) {
                    overlay = 0;
                }
                inGameArmor = chestArmorInGame[overlay];
            }
        }
        else if(leatherArmorIndices.Contains(armorBaseIndex)) {
            int leatherOverlayIndex = pseudoRandom.Next(0, 4);
            if(leatherOverlayIndex == 3) {
                leatherOverlayIndex = 0;
            }
            armorOverlay = chestArmorOverlays[leatherOverlayIndex];
            armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
            inGameArmor = chestArmorInGame[1];
        }
        else if(clothArmorIndices.Contains(armorBaseIndex)) {
            armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
        }
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color overlayColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color backColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
        // Sprite 
        Armor armor = CreateInstance<Armor>();
        armor.firstIcon = armorBase;
        if(armorOverlay) {
            armor.secondIcon = armorOverlay;
            armor.secondColor = overlayColor;
        }
        else {
            armor.secondColor = Color.clear;
        }
        if(armorBack) {
            armor.thirdIcon = armorBack;
            armor.thirdColor = backColor;
        }
        else {
            armor.thirdColor = Color.clear;
        }
        armor.firstSprite = inGameArmor;
        armor.firstColor = baseColor;
        
        
        armor.slot = EquipSlot.Body;
        return armor;
    }
    // Create a helmet sprite
    // TODO: Match helmet sprite and icons to be consistent
    public Armor CreateHelmet(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite helmetBase = helmetBases[pseudoRandom.Next(0, helmetBases.Length)];
        int helmetPropIndex = pseudoRandom.Next(0, helmetProps.Length);
        Sprite helmetProp = helmetProps[helmetPropIndex];
        Sprite helmetBaseInGame = helmetBasesInGame[pseudoRandom.Next(0, helmetBasesInGame.Length)];
        Sprite helmetPropInGame = helmetPropsInGame[helmetPropIndex];
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color propColor = helmetPropColor;
        Armor helmet = CreateInstance<Armor>();
        helmet.firstIcon = helmetBase;
        helmet.firstColor = baseColor;
        if(helmetProp) {
            helmet.secondIcon = helmetProp;
            helmet.secondColor = propColor;
            helmet.secondSprite = helmetPropInGame;
        }        
        helmet.firstSprite = helmetBaseInGame;
         
        helmet.slot = EquipSlot.Head;
        return helmet;
    }
    public Armor CreateLegging(string seed) {
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        Sprite leggingBase = leggingBases[pseudoRandom.Next(0, leggingBases.Length)];
        Sprite leggingProp = leggingProps[pseudoRandom.Next(0, leggingProps.Length)];
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color propColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];        
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
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color propColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
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
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color jewelColor = bladeMaterialColors[pseudoRandom.Next(8, bladeMaterialColors.Length)];
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
