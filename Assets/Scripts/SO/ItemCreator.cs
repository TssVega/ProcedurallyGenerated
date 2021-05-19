using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Creator")]
public class ItemCreator : ScriptableObject {
    // Mine and weapon material colors
    public Color[] bladeMaterialColors;
    public Color[] otherMaterialColors;
    // Swords
    public Sprite[] weaponHandles;
    public Sprite[] weaponGuards;
    public Sprite[] weaponBlades;
    // Axes
    public Sprite[] axeHandles;
    public Sprite[] axeBlades;
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
    // Staff sprites
    public Sprite[] staffHandles;
    public Sprite[] staffProps;
    public Sprite[] staffHeads;

    public MushroomDatabase mushroomDatabase;
    public ItemDatabase itemDatabase;

    private readonly int[] metalArmorIndices = { 0, 3, 6, 8 };
    private readonly int[] leatherArmorIndices = { 1, 2, 5 };
    private readonly int[] clothArmorIndices = { 4, 7 };

    private System.Random pseudoRandom;

    // Create an item
    public Item CreateItem(string seed) {
        pseudoRandom = new System.Random(seed.GetHashCode());
        Item item;
        
        if(CheckSpecialCase(seed) != null) {
            item = CheckSpecialCase(seed);
            return item;
        }
        int slotIndex = pseudoRandom.Next(0, 6);
        switch (slotIndex)
        {
            case 0: {
                Weapon w = CreateWeapon(seed);
                item = w;
                break;
            }
            case 1: {
                Shield s = CreateShield(seed);
                item = s;
                break;
            }
            case 2: {
                // Head armor h
                Armor h = CreateHelmet(seed);
                item = h;
                break;
            }
            case 3: {
                // Chest armor c
                Armor a = CreateChestArmor(seed);
                item = a;
                break;
            }
            case 4: {
                // Leg armor l
                Armor l = CreateLegging(seed);
                item = l;
                break;
            }
            case 5: {
                Ring r = CreateRing(seed);
                item = r;
                break;
            }
            default:
                item = null;
                break;
        }
        if(item != null) {
            item.seed = seed;
        }
        return item;
    }
    // Create a weapon sprite
    public Weapon CreateWeapon(string seed) {
        int presetIndex = pseudoRandom.Next(0, 2);
        Debug.Log($"Seed: {seed} Hash code: {seed.GetHashCode()} Index: {presetIndex}");
        WeaponPreset preset = (WeaponPreset)presetIndex;
        WeaponType type = WeaponType.OneHanded;
        int estimatedPower = pseudoRandom.Next(0, 100);
        //int index = 0;  // Currently only short swords
        switch(presetIndex) {
            case 0:
                type = WeaponType.OneHanded;
                break;                
            case 1:
                type = WeaponType.OneHanded;
                break;
                /*
            case 2:
                type = WeaponType.Bow;
                break;
            case 3:
                type = WeaponType.Dagger;
                break;*/
        }
        Weapon weapon = CreateInstance<Weapon>();
        switch(type) {
            case WeaponType.OneHanded: {
                if(preset == WeaponPreset.Sword) {
                    // Swords
                    Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
                    Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
                    Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
                    Color bladeColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
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
                else if(preset == WeaponPreset.Axe) {
                    // Axes
                    Sprite handle = axeHandles[pseudoRandom.Next(0, axeHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = axeBlades[pseudoRandom.Next(0, axeBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = Color.clear;
                    Color bladeColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
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
                break;
            }
            case WeaponType.TwoHanded:
                // Staff
                Sprite staff = staffHandles[pseudoRandom.Next(0, staffHandles.Length)];
                Sprite prop = staffProps[pseudoRandom.Next(0, staffProps.Length)];
                Sprite head = staffHeads[pseudoRandom.Next(0, staffHeads.Length)];
                Color staffColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                Color propColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
                Color headColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
                weapon.firstSprite = staff;
                weapon.secondSprite = prop;
                weapon.thirdSprite = head;
                weapon.firstIcon = staff;
                weapon.secondIcon = prop;
                weapon.thirdIcon = head;
                weapon.firstColor = staffColor;
                weapon.secondColor = propColor;
                weapon.thirdColor = headColor;
                weapon.slot = EquipSlot.RightHand;
                weapon.weaponType = WeaponType.TwoHanded;
                break;
            case WeaponType.Bow: {
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
                break;
            }
        }
        return weapon;
    }
    private Item CheckSpecialCase(string seed) {
        Item item;
        switch(seed) {
            case "destroyingAngel":
                item = mushroomDatabase.mushrooms[0];
                break;
            case "truffle":
                item = mushroomDatabase.mushrooms[1];
                break;
            case "turkeyTail":
                item = mushroomDatabase.mushrooms[2];
                break;
            case "blackTrumpet":
                item = mushroomDatabase.mushrooms[3];
                break;
            case "chanterelle":
                item = mushroomDatabase.mushrooms[4];
                break;
            case "reishi":
                item = mushroomDatabase.mushrooms[5];
                break;
            case "matsutake":
                item = mushroomDatabase.mushrooms[6];
                break;
            case "puffball":
                item = mushroomDatabase.mushrooms[7];
                break;
            case "enoki":
                item = mushroomDatabase.mushrooms[8];
                break;
            case "porcini":
                item = mushroomDatabase.mushrooms[9];
                break;
            case "morel":
                item = mushroomDatabase.mushrooms[10];
                break;
            case "flyAgaric":
                item = mushroomDatabase.mushrooms[11];
                break;
            case "wood":
                item = itemDatabase.items[0];
                break;
            case "copper":
                item = itemDatabase.items[1];
                break;
            case "iron":
                item = itemDatabase.items[2];
                break;
            case "silver":
                item = itemDatabase.items[3];
                break;
            case "gold":
                item = itemDatabase.items[4];
                break;
            case "platinum":
                item = itemDatabase.items[5];
                break;
            case "titanium":
                item = itemDatabase.items[6];
                break;
            case "tungsten":
                item = itemDatabase.items[7];
                break;
            case "sapphire":
                item = itemDatabase.items[8];
                break;
            case "ruby":
                item = itemDatabase.items[9];
                break;
            case "emerald":
                item = itemDatabase.items[10];
                break;
            case "diamond":
                item = itemDatabase.items[11];
                break;
            case "musgravite":
                item = itemDatabase.items[12];
                break;
            case "taaffeite":
                item = itemDatabase.items[13];
                break;
            case "amber":
                item = itemDatabase.items[14];
                break;
            case "leather":
                item = itemDatabase.items[15];
                break;
            case "scales":
                item = itemDatabase.items[16];
                break;
            case "fur":
                item = itemDatabase.items[17];
                break;
            case "eye":
                item = itemDatabase.items[18];
                break;
            case "shell":
                item = itemDatabase.items[19];
                break;
            case "tongue":
                item = itemDatabase.items[20];
                break;
            case "feather":
                item = itemDatabase.items[21];
                break;
            case "web":
                item = itemDatabase.items[22];
                break;
            case "liver":
                item = itemDatabase.items[23];
                break;
            case "heart":
                item = itemDatabase.items[24];
                break;
            case "brain":
                item = itemDatabase.items[25];
                break;
            case "claw":
                item = itemDatabase.items[26];
                break;
            case "fang":
                item = itemDatabase.items[27];
                break;
            case "bone":
                item = itemDatabase.items[28];
                break;
            case "obsidian":
                item = itemDatabase.items[29];
                break;
            case "stone":
                item = itemDatabase.items[30];
                break;
            case "soul":
                item = itemDatabase.items[31];
                break;
            default:
                item = null;
                break; 
        }
        return item;
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
            if(new[] { 2, 9 }.Contains(armorOverlayIndex)) {
                // Small shoulders
                //Debug.Log("Small shoulders");
                inGameArmor = chestArmorInGame[1];
            }
            else if(new[] { 5, 6, 7, 8 }.Contains(armorOverlayIndex)) {
                // Big shoulders
                //Debug.Log("Big shoulders");
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
        shield.thirdColor = Color.clear;
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

public enum WeaponPreset {
    Sword, Axe, Hammer, Spear, Bow, Dagger, Staff, Tome
}
