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
    // Hammers
    public Sprite[] hammerHandles;
    public Sprite[] hammerBlades;
    // Spears
    public Sprite[] spearHandles;
    public Sprite[] spearBlades;
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
    // Unique items
    public Sprite[] uniqueDaggers;
    public Sprite[] uniqueSwords;

    public MushroomDatabase mushroomDatabase;
    public ItemDatabase itemDatabase;

    private readonly int[] metalArmorIndices = { 0, 3, 6, 8 };
    private readonly int[] leatherArmorIndices = { 1, 2, 5 };
    private readonly int[] clothArmorIndices = { 4, 7 };

    private readonly int[][] diceRolls = new int[][] {
        new int[] { 1, 1, 1, 7, 6, 5, 9, 2, 3, 4, 0, 0, 0 },
        new int[] { 3, 5, 2, 1, 3, 6, 7, 7, 4, 1, 1, 0, 1 },
        new int[] { 6, 5, 6, 4, 5, 2, 3, 1, 2, 3, 4, 3, 2 },
        new int[] { 6, 7, 6, 2, 2, 1, 5, 1, 3, 2, 3, 1, 2 },
        new int[] { 7, 7, 7, 3, 4, 3, 2, 3, 1, 2, 5, 6, 1 },
        new int[] { 7, 5, 8, 1, 1, 2, 4, 2, 2, 1, 4, 2, 1 },
        new int[] { 6, 9, 6, 4, 4, 3, 2, 7, 3, 3, 4, 4, 2 },
        new int[] { 5, 6, 6, 3, 4, 3, 1, 0, 3, 2, 5, 3, 3 },
        new int[] { 9, 6, 6, 3, 3, 2, 3, 3, 2, 1, 3, 4, 3 },
        new int[] { 6, 5, 4, 2, 3, 4, 4, 1, 4, 4, 5, 5, 4 },
        new int[] { 6, 6, 9, 4, 4, 3, 2, 2, 1, 2, 2, 3, 2 },
        new int[] { 4, 4, 4, 6, 5, 4, 4, 5, 0, 6, 4, 4, 6 },
        new int[] { 8, 9, 9, 3, 4, 3, 2, 6, 2, 3, 6, 4, 3 },
        new int[] { 8, 9, 9, 4, 2, 3, 1, 3, 6, 5, 6, 1, 3 },
        new int[] { 10, 6, 7, 2, 1, 2, 6, 1, 4, 3, 4, 2, 4 },
        new int[] { 10, 7, 8, 6, 4, 1, 5, 7, 2, 1, 7, 1, 5 },
        new int[] { 4, 3, 2, 1, 9, 3, 4, 2, 3, 4, 1, 1, 2 },
        new int[] { 4, 5, 4, 7, 6, 3, 3, 2, 1, 1, 2, 2, 2 },
        new int[] { 4, 5, 3, 9, 1, 3, 2, 2, 3, 4, 4, 2, 3 },
        new int[] { 4, 6, 5, 8, 5, 4, 1, 4, 2, 3, 5, 2, 4 },
        new int[] { 5, 3, 4, 3, 3, 2, 9, 5, 3, 4, 1, 5, 2 },
        new int[] { 3, 4, 6, 2, 3, 3, 8, 2, 3, 3, 2, 4, 4 },
        new int[] { 6, 7, 5, 5, 5, 10, 6, 7, 6, 1, 4, 3, 4 },
        new int[] { 7, 8, 7, 3, 4, 10, 6, 6, 3, 1, 3, 4, 7 },
        new int[] { 4, 5, 4, 2, 2, 1, 2, 4, 1, 9, 2, 2, 3 },
        new int[] { 5, 4, 5, 2, 3, 4, 3, 3, 3, 9, 3, 2, 8 },
        new int[] { 3, 4, 3, 3, 3, 3, 3, 9, 3, 4, 3, 1, 2 },
        new int[] { 6, 3, 4, 1, 2, 3, 1, 9, 4, 3, 4, 3, 3 },
        new int[] { 2, 1, 2, 4, 3, 4, 4, 3, 9, 2, 1, 2, 1 },
        new int[] { 3, 4, 3, 4, 3, 4, 4, 3, 9, 1, 2, 1, 4 },
        new int[] { 2, 6, 7, 2, 3, 3, 6, 2, 3, 7, 5, 4, 9 },
        new int[] { 5, 4, 5, 4, 4, 3, 5, 4, 4, 6, 1, 1, 9 },
        new int[] { 1, 3, 2, 2, 4, 4, 8, 7, 1, 2, 9, 4, 2 },
        new int[] { 4, 5, 5, 3, 9, 5, 7, 4, 3, 3, 4, 4, 3 },
        new int[] { 3, 3, 2, 2, 4, 6, 6, 6, 6, 5, 5, 5, 6 },
        new int[] { 4, 4, 3, 1, 3, 7, 4, 2, 4, 3, 2, 9, 4 },
        new int[] { 6, 6, 6, 4, 4, 3, 5, 5, 5, 5, 3, 4, 4 }
    };

    private const int uniqueRate = 2;   // uniqueRate / 100 chance of getting a unique item
    private const int dieCapacity = 10;

    private System.Random pseudoRandom;

    // Create an item
    public Item CreateItem(string seed) {
        pseudoRandom = new System.Random(seed.GetHashCode());
        Item item;
        ItemMaterial mat = GetItemMaterial();
        if(CheckSpecialCase(seed) != null) {
            item = CheckSpecialCase(seed);
            return item;
        }
        int slotIndex = pseudoRandom.Next(0, 6);
        // To ensure you don't get weapons and rings out of fabric
        if((int)mat > 15 && slotIndex == 0) {
            slotIndex = pseudoRandom.Next(1, 5);
        }
        switch(slotIndex) {
            case 0: {
                bool unique = pseudoRandom.Next(1, 101) <= uniqueRate;
                Weapon w = unique ? CreateUniqueWeapon() : CreateWeapon(mat);
                item = w;
                break;
            }
            case 1: {
                Shield s = CreateShield(mat);
                item = s;
                break;
            }
            case 2: {
                // Head armor h
                Armor h = CreateHelmet(mat);
                item = h;
                break;
            }
            case 3: {
                // Chest armor c
                Armor a = CreateChestArmor(mat);
                item = a;
                break;
            }
            case 4: {
                // Leg armor l
                Armor l = CreateLegging(mat);
                item = l;
                break;
            }
            case 5: {
                Ring r = CreateRing(mat);
                item = r;
                break;
            }
            default:
                item = null;
                break;
        }
        if(item != null) {
            item.seed = seed;
            item.itemMaterial = mat;
            SetStats(item);
        }
        return item;
    }
    private ItemMaterial GetItemMaterial() {
        ItemMaterial newMaterial;
        int result = RollDice(3, 16);
        switch(result) {
            case 3:
                newMaterial = ItemMaterial.Diamond;
                break;
            case 4:
                newMaterial = ItemMaterial.Ruby;
                break;
            case 5:
                newMaterial = ItemMaterial.Musgravite;
                break;
            case 6:
                newMaterial = ItemMaterial.Amber;
                break;
            case 7:
                newMaterial = ItemMaterial.Sapphire;
                break;
            case 8:
                newMaterial = ItemMaterial.Tungsten;
                break;
            case 9:
                newMaterial = ItemMaterial.Titanium;
                break;
            case 10:
                newMaterial = ItemMaterial.Platinum;
                break;
            case 11:
                newMaterial = ItemMaterial.Scale;
                break;
            case 21:
            case 13:
            case 14:
                newMaterial = ItemMaterial.Fur;
                break;
            case 15:
            case 16:
            case 17:
                newMaterial = ItemMaterial.Silk;
                break;
            case 18:
            case 19:
            case 20:
                newMaterial = ItemMaterial.Leather;
                break;
            case 12:
                newMaterial = ItemMaterial.Gold;
                break;
            case 22:
                newMaterial = ItemMaterial.Silver;
                break;
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
                newMaterial = ItemMaterial.Iron;
                break;
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
                newMaterial = ItemMaterial.Copper;
                break;
            case 33:
            case 34:
            case 35:
            case 36:
            case 37:
            case 38:
            case 39:
            case 40:
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
            case 47:
                newMaterial = ItemMaterial.Wood;
                break;
            case 48:
            case 49:
            case 50:
            case 51:
                newMaterial = ItemMaterial.Wool;
                break;
            case 52:
                newMaterial = ItemMaterial.Taaffeite;
                break;
            case 53:
                newMaterial = ItemMaterial.Emerald;
                break;
            case 54:
                newMaterial = ItemMaterial.Bone;
                break;
            default:
                newMaterial = ItemMaterial.Iron;
                break;
        }
        return newMaterial;
    }
    public Weapon CreateUniqueWeapon() {
        int presetIndex = 0;    // Currently only swords
        WeaponPreset preset = (WeaponPreset)presetIndex;
        WeaponType type = WeaponType.OneHanded;
        Weapon weapon = CreateInstance<Weapon>();
        switch(type) {
            case WeaponType.OneHanded: {
                if(preset == WeaponPreset.Sword) {
                    Sprite swordSprite = uniqueSwords[pseudoRandom.Next(0, uniqueSwords.Length)];
                    // Swords
                    weapon.firstSprite = swordSprite;
                    weapon.secondSprite = null;
                    weapon.thirdSprite = null;
                    weapon.firstIcon = swordSprite;
                    weapon.secondIcon = null;
                    weapon.thirdIcon = null;
                    weapon.firstColor = Color.white;
                    weapon.secondColor = Color.clear;
                    weapon.thirdColor = Color.clear;
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
                else if(preset == WeaponPreset.Hammer) {
                    // Hammers
                    Sprite handle = hammerHandles[pseudoRandom.Next(0, hammerHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = hammerBlades[pseudoRandom.Next(0, hammerBlades.Length)];
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
                else if(preset == WeaponPreset.Spear) {
                    // Spears
                    Sprite handle = spearHandles[pseudoRandom.Next(0, spearHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = spearBlades[pseudoRandom.Next(0, spearBlades.Length)];
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
                break;
            }
            case WeaponType.Dagger: {

                break;
            }
        }
        return weapon;
    }
    // Create a weapon sprite
    public Weapon CreateWeapon(ItemMaterial mat) {
        int presetIndex = pseudoRandom.Next(0, 7);
        WeaponPreset preset = (WeaponPreset)presetIndex;
        WeaponType type = WeaponType.OneHanded;
        //int index = 0;  // Currently only short swords
        switch(presetIndex) {
            case 0:
                type = WeaponType.OneHanded;
                break;
            case 1:
                type = WeaponType.OneHanded;
                break;
            case 2:
                type = WeaponType.OneHanded;
                break;
            case 3:
                type = WeaponType.OneHanded;
                break;
            case 4:
                type = WeaponType.Bow;
                break;
            case 5:
                type = WeaponType.Dagger;
                break;
            case 6:
                type = WeaponType.OneHanded;
                break;
        }
        Weapon weapon = CreateInstance<Weapon>();
        weapon.itemName += $"{mat}";
        switch(type) {
            case WeaponType.OneHanded: {
                if(preset == WeaponPreset.Sword) {
                    // Swords
                    Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
                    Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
                    Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
                    Color bladeColor = bladeMaterialColors[(int)mat];
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
                    weapon.itemName += " Sword";
                }
                else if(preset == WeaponPreset.Axe) {
                    // Axes
                    Sprite handle = axeHandles[pseudoRandom.Next(0, axeHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = axeBlades[pseudoRandom.Next(0, axeBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = Color.clear;
                    Color bladeColor = bladeMaterialColors[(int)mat];
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
                    weapon.itemName += " Axe";
                }
                else if(preset == WeaponPreset.Hammer) {
                    // Hammers
                    Sprite handle = hammerHandles[pseudoRandom.Next(0, hammerHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = hammerBlades[pseudoRandom.Next(0, hammerBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = Color.clear;
                    Color bladeColor = bladeMaterialColors[(int)mat];
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
                    weapon.itemName += " Hammer";
                }
                else if(preset == WeaponPreset.Spear) {
                    // Spears
                    Sprite handle = spearHandles[pseudoRandom.Next(0, spearHandles.Length)];
                    Sprite guard = null;
                    Sprite blade = spearBlades[pseudoRandom.Next(0, spearBlades.Length)];
                    Color handleColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color guardColor = Color.clear;
                    Color bladeColor = bladeMaterialColors[(int)mat];
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
                    weapon.itemName += " Spear";
                }
                else if(preset == WeaponPreset.Staff) {
                    // Staff
                    Sprite staff = staffHandles[pseudoRandom.Next(0, staffHandles.Length)];
                    Sprite prop = staffProps[pseudoRandom.Next(0, staffProps.Length)];
                    Sprite head = staffHeads[pseudoRandom.Next(0, staffHeads.Length)];
                    Color staffColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color propColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
                    Color headColor = bladeMaterialColors[(int)mat];
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
                    weapon.weaponType = WeaponType.OneHanded;
                    weapon.itemName += " Staff";
                }
                break;
            }
            case WeaponType.Bow: {
                Sprite bowBase = bowBases[pseudoRandom.Next(0, bowBases.Length)];
                Color bowColor = bladeMaterialColors[(int)mat];
                weapon.firstSprite = bowBase;
                weapon.firstIcon = bowBase;
                weapon.firstColor = bowColor;
                weapon.slot = EquipSlot.RightHand;
                weapon.weaponType = WeaponType.Bow;
                weapon.itemName += " Bow";
                break;
            }
            case WeaponType.Dagger: {
                weapon.firstSprite = null;
                weapon.secondSprite = null;
                weapon.thirdSprite = null;
                weapon.firstIcon = null;
                weapon.secondIcon = null;
                weapon.thirdIcon = null;
                weapon.firstColor = Color.clear;
                weapon.secondColor = Color.clear;
                weapon.thirdColor = Color.clear;
                return null;
                //break;
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
    public Armor CreateChestArmor(ItemMaterial mat) {
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
        Color baseColor = bladeMaterialColors[(int)mat];
        Color overlayColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color backColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
        // Sprite 
        Armor armor = CreateInstance<Armor>();
        armor.itemName += $"{mat} Armor";
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
    public Armor CreateHelmet(ItemMaterial mat) {
        Sprite helmetBase = helmetBases[pseudoRandom.Next(0, helmetBases.Length)];
        int helmetPropIndex = pseudoRandom.Next(0, helmetProps.Length);
        Sprite helmetProp = helmetProps[helmetPropIndex];
        Sprite helmetBaseInGame = helmetBasesInGame[pseudoRandom.Next(0, helmetBasesInGame.Length)];
        Sprite helmetPropInGame = helmetPropsInGame[helmetPropIndex];
        Color baseColor = bladeMaterialColors[(int)mat];
        Color propColor = helmetPropColor;
        Armor helmet = CreateInstance<Armor>();
        helmet.itemName += $"{mat} Helmet";
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
    public Armor CreateLegging(ItemMaterial mat) {
        Sprite leggingBase = leggingBases[pseudoRandom.Next(0, leggingBases.Length)];
        Sprite leggingProp = leggingProps[pseudoRandom.Next(0, leggingProps.Length)];
        Color baseColor = bladeMaterialColors[(int)mat];
        Color propColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
        Armor legging = CreateInstance<Armor>();
        legging.itemName += $"{mat} Leggings";
        legging.firstIcon = leggingBase;
        legging.secondIcon = leggingProp;
        legging.firstColor = baseColor;
        legging.secondColor = propColor;
        legging.slot = EquipSlot.Legs;
        return legging;
    }
    public Shield CreateShield(ItemMaterial mat) {
        Sprite shieldBase = shieldBases[pseudoRandom.Next(0, shieldBases.Length)];
        Sprite shieldProp = shieldProps[pseudoRandom.Next(0, shieldProps.Length)];
        Sprite inGame = shieldInGame[pseudoRandom.Next(0, shieldInGame.Length)];
        Color baseColor = bladeMaterialColors[(int)mat];
        Color propColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
        Shield shield = CreateInstance<Shield>();
        shield.itemName += $"{mat} Shield";
        shield.firstIcon = shieldBase;
        shield.secondIcon = shieldProp;
        shield.firstSprite = inGame;
        shield.firstColor = baseColor;
        shield.secondColor = propColor;
        shield.thirdColor = Color.clear;
        shield.slot = EquipSlot.LeftHand;
        return shield;
    }
    public Ring CreateRing(ItemMaterial mat) {
        Sprite ringBase = ringBases[pseudoRandom.Next(0, ringBases.Length)];
        Sprite ringSocket = ringSockets[pseudoRandom.Next(0, ringSockets.Length)];
        Sprite ringJewel = ringJewels[pseudoRandom.Next(0, ringJewels.Length)];
        Color baseColor = bladeMaterialColors[pseudoRandom.Next(0, bladeMaterialColors.Length)];
        Color jewelColor = bladeMaterialColors[(int)mat];
        Ring ring = CreateInstance<Ring>();
        ring.itemName += $"{mat} Ring";
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
    private int RollDice(int diceCount, int diceMax) {
        int diceRollTotal = 0;
        // Roll 5d20
        for(int i = 0; i < diceCount; i++) {
            int dieResult = pseudoRandom.Next(1, diceMax + 1);
            diceRollTotal += dieResult;
        }
        return diceRollTotal;
    }
    private void SetStats(Item item) {
        int matIndex = (int)item.itemMaterial * 2;
        if(matIndex > 30) {
            int excess = matIndex % 30;
            excess /= 2;
            matIndex = 29 + excess;
        }
        int index = 0;
        if(item is Weapon w) {
            w.bashDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.pierceDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.slashDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.fireDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.iceDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.airDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.earthDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.lightningDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.lightDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.darkDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.bleedDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.poisonDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            w.curseDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
        }
        else if(item is Armor a) {
            a.bashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.pierceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.slashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.fireDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.iceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.airDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.earthDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.lightningDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.lightDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.darkDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.bleedDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.poisonDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            a.curseDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
        }
        else if(item is Shield s) {
            s.bashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.pierceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.slashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.fireDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.iceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.airDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.earthDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.lightningDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.lightDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.darkDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.bleedDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.poisonDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            s.curseDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
        }
        else if(item is Ring r) {
            r.bashDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.pierceDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.slashDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.fireDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.iceDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.airDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.earthDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.lightningDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.lightDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.darkDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.bleedDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.poisonDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            r.curseDamage = RollDice(diceRolls[matIndex][index++], dieCapacity);
            index = 0;
            r.bashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.pierceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.slashDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.fireDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.iceDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.airDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.earthDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.lightningDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.lightDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.darkDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.bleedDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.poisonDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
            r.curseDefence = RollDice(diceRolls[matIndex + 1][index++], dieCapacity);
        }
    }
}

public enum WeaponPreset {
    Sword, Axe, Hammer, Spear, Bow, Dagger, Staff, Tome
}

public enum ItemMaterial {
    Wood, Copper, Iron, Silver, Gold, Platinum, Titanium, Tungsten, Sapphire,
    Ruby, Emerald, Diamond, Musgravite, Taaffeite, Amber, Bone, Wool, Fur, Silk, Leather, Scale
}
