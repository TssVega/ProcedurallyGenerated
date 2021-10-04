using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Creator")]
public class ItemCreator : ScriptableObject {
    // Mine and weapon material colors
    public Color[] bladeMaterialColors;
    public Color[] otherMaterialColors;
    public Color[] paperColors;
    public Color[] textColors;
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
    // Dagger sprites
    public Sprite[] daggerHandles;
    public Sprite[] daggerGuards;
    public Sprite[] daggerBlades;
    // Tome sprites
    public Sprite tomeCover;
    public Sprite tomePaper;
    public Sprite tomeText;

    public MushroomDatabase mushroomDatabase;
    public ItemDatabase itemDatabase;
    public UniqueItemDatabase uniqueItemDatabase;

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

    private int uniqueRate;   // uniqueRate / 100 chance of getting a unique item
    private const int dieCapacity = 10;

    private System.Random pseudoRandom;

    // Create an item
    public Item CreateItem(string seed, int luck) {
        uniqueRate = luck;
        pseudoRandom = new System.Random(seed.GetHashCode());
        Item item;
        ItemMaterial mat = GetItemMaterial();
        if(CheckSpecialCase(seed) != null) {
            item = CheckSpecialCase(seed);
            return item;
        }
        int slotIndex = pseudoRandom.Next(0, 15);
        bool unique = pseudoRandom.Next(1, 101) <= uniqueRate;
        if(unique) {
            return CreateUniqueItem();
        }
        if(slotIndex > 5) {
            switch(slotIndex) {
                case 6:
                case 7:
                    return itemDatabase.coins[1];
                case 8:
                    return itemDatabase.commonBooks[pseudoRandom.Next(0, itemDatabase.commonBooks.Count)];
                case 9:
                    return itemDatabase.essentialBooks[pseudoRandom.Next(0, itemDatabase.essentialBooks.Count)];
                case 10:
                    return itemDatabase.coins[0];
                case 11:
                    return itemDatabase.grimoire[pseudoRandom.Next(0, itemDatabase.grimoire.Count)];
                case 12:
                    return itemDatabase.potions[pseudoRandom.Next(0, itemDatabase.potions.Count)];
                case 13:
                    return itemDatabase.physicalBooks[pseudoRandom.Next(0, itemDatabase.physicalBooks.Count)];
                case 14:
                    int rnd = pseudoRandom.Next(0, 6);
                    if(rnd == 0) {
                        return itemDatabase.tomes[pseudoRandom.Next(0, itemDatabase.tomes.Count)];
                    }
                    else if(rnd == 1) {
                        return itemDatabase.commonBooks[pseudoRandom.Next(0, itemDatabase.commonBooks.Count)];
                    }
                    else if(rnd == 2) {
                        return itemDatabase.essentialBooks[pseudoRandom.Next(0, itemDatabase.essentialBooks.Count)];
                    }
                    else if(rnd == 3) {
                        return itemDatabase.grimoire[pseudoRandom.Next(0, itemDatabase.grimoire.Count)];
                    }
                    else if(rnd == 4) {
                        return itemDatabase.physicalBooks[pseudoRandom.Next(0, itemDatabase.physicalBooks.Count)];
                    }
                    else {
                        return itemDatabase.essentialBooks[pseudoRandom.Next(0, itemDatabase.essentialBooks.Count)];
                    }
            }
        }
        // To ensure you don't get weapons and rings out of fabric
        while((int)mat > 15 && (slotIndex == 0 || slotIndex == 5 || slotIndex == 1)) {
            mat = GetItemMaterial();
        }
        switch(slotIndex) {
            case 0: {                
                Weapon w = CreateWeapon(mat);                
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
            default: {
                item = null;
                break;
            }      
                
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
    public Item CreateUniqueItem() {
        Item item = uniqueItemDatabase.uniqueItems[pseudoRandom.Next(0, uniqueItemDatabase.uniqueItems.Count)];
        string itemName = LocalizationManager.localization.GetText(item.seed);
        if(itemName != null) {
            item.itemName = itemName;
        }        
        return item;
    }
    // Create a weapon sprite
    public Weapon CreateWeapon(ItemMaterial mat) {
        int presetIndex = pseudoRandom.Next(0, 8);
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
            case 7:
                type = WeaponType.OneHanded;
                break;
        }
        Weapon weapon = CreateInstance<Weapon>();
        weapon.itemMaterial = mat;
        weapon.itemName += LocalizationManager.localization.GetText($"{mat}");
        weapon.preset = preset;
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
                    weapon.dismantleOutput = 2;
                    weapon.itemName += $" {LocalizationManager.localization.GetText("Sword")}";
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
                    weapon.dismantleOutput = 3;
                    weapon.itemName += $" {LocalizationManager.localization.GetText("Axe")}";
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
                    weapon.dismantleOutput = 3;
                    weapon.itemName += $" {LocalizationManager.localization.GetText("Hammer")}";
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
                    weapon.dismantleOutput = 2;
                    weapon.itemName += $" {LocalizationManager.localization.GetText("Spear")}";
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
                    weapon.dismantleOutput = 2;
                    weapon.itemName += $" {LocalizationManager.localization.GetText("Staff")}";
                }
                else if(preset == WeaponPreset.Tome) {
                    // Tome
                    Sprite cover = tomeCover;
                    Sprite paper = tomePaper;
                    Sprite scripture = tomeText;
                    Color coverColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
                    Color paperColor = paperColors[pseudoRandom.Next(0, paperColors.Length)];
                    Color scriptureColor = textColors[pseudoRandom.Next(0, textColors.Length)];
                    weapon.firstSprite = cover;
                    weapon.secondSprite = scripture;
                    weapon.thirdSprite = paper;
                    weapon.firstIcon = cover;
                    weapon.secondIcon = scripture;
                    weapon.thirdIcon = paper;
                    weapon.firstColor = coverColor;
                    weapon.secondColor = scriptureColor;
                    weapon.thirdColor = paperColor;
                    weapon.slot = EquipSlot.RightHand;
                    weapon.weaponType = WeaponType.OneHanded;
                    weapon.dismantleOutput = 2;
                    weapon.itemName = $"{LocalizationManager.localization.GetText("Tome")}";
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
                weapon.dismantleOutput = 2;
                weapon.itemName += $" {LocalizationManager.localization.GetText("Bow")}";
                break;
            }
            case WeaponType.Dagger: {
                // Swords
                Sprite handle = daggerHandles[pseudoRandom.Next(0, daggerHandles.Length)];
                Sprite guard = daggerGuards[pseudoRandom.Next(0, daggerGuards.Length)];
                Sprite blade = daggerBlades[pseudoRandom.Next(0, daggerBlades.Length)];
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
                weapon.weaponType = WeaponType.Dagger;
                weapon.dismantleOutput = 1;
                weapon.itemName += $" {LocalizationManager.localization.GetText("Dagger")}";
                /*
                weapon.firstSprite = null;
                weapon.secondSprite = null;
                weapon.thirdSprite = null;
                weapon.firstIcon = null;
                weapon.secondIcon = null;
                weapon.thirdIcon = null;
                weapon.firstColor = Color.clear;
                weapon.secondColor = Color.clear;
                weapon.thirdColor = Color.clear;
                weapon.dismantleOutput = 2;
                Debug.Log("This item is a dagger");*/
                // TODO: Fix daggers
                // weapon.itemName += $" {LocalizationManager.localization.GetText("Dagger")}";
                break;
            }
        }
        return weapon;
    }
    private Item CheckSpecialCase(string seed) {
        for(int i = 0; i < mushroomDatabase.mushrooms.Count; i++) {
            if(seed == mushroomDatabase.mushrooms[i].seed) {
                return mushroomDatabase.mushrooms[i];
            }
        }
        for(int i = 0; i < uniqueItemDatabase.uniqueItems.Count; i++) {
            if(seed == uniqueItemDatabase.uniqueItems[i].seed) {
                return uniqueItemDatabase.uniqueItems[i];
            }
        }
        for(int i = 0; i < itemDatabase.items.Count; i++) {
            if(seed == itemDatabase.items[i].seed) {
                return itemDatabase.items[i];
            }
        }
        return null;
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
            if(new[] { 5, 6, 7, 8 }.Contains(armorOverlayIndex)) {
                // Small shoulders
                //Debug.Log("Small shoulders");
                inGameArmor = chestArmorInGame[0];
            }
            else if(new[] { 0, 1, 2, 3, 4 }.Contains(armorOverlayIndex)) {
                // Big shoulders
                //Debug.Log("Big shoulders");
                // Represent shoulder guards in game
                int overlay;
                if(armorOverlayIndex == 0 || armorOverlayIndex == 1 || armorOverlayIndex == 2) {
                    overlay = 3;
                }
                else if(armorOverlayIndex == 3) {
                    overlay = 2;
                }
                else {
                    overlay = 0;
                }
                inGameArmor = chestArmorInGame[overlay];
            }
        }
        else if(leatherArmorIndices.Contains(armorBaseIndex)) {
            /*
            int leatherOverlayIndex = pseudoRandom.Next(0, 4);
            if(leatherOverlayIndex == 3) {
                leatherOverlayIndex = 0;
            }
            armorOverlay = chestArmorOverlays[leatherOverlayIndex];*/
            armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
            inGameArmor = chestArmorInGame[1];
        }
        else if(clothArmorIndices.Contains(armorBaseIndex)) {
            armorBack = chestArmorBacks[pseudoRandom.Next(0, chestArmorBacks.Length)];
            inGameArmor = chestArmorInGame[1];
        }
        Color baseColor = bladeMaterialColors[(int)mat];
        Color overlayColor = bladeMaterialColors[(int)mat];
        Color backColor = otherMaterialColors[pseudoRandom.Next(0, otherMaterialColors.Length)];
        // Sprite 
        Armor armor = CreateInstance<Armor>();
        armor.itemMaterial = mat;
        armor.dismantleOutput = 5;
        armor.itemName += $"{LocalizationManager.localization.GetText($"{armor.itemMaterial}")} {LocalizationManager.localization.GetText("Armor")}";
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
        helmet.itemMaterial = mat;
        helmet.dismantleOutput = 3;
        helmet.itemName += $"{LocalizationManager.localization.GetText($"{helmet.itemMaterial}")} {LocalizationManager.localization.GetText("Helmet")}";
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
        legging.itemMaterial = mat;
        legging.dismantleOutput = 4;
        legging.itemName += $"{LocalizationManager.localization.GetText($"{legging.itemMaterial}")} {LocalizationManager.localization.GetText("Leggings")}";
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
        shield.itemMaterial = mat;
        shield.dismantleOutput = 4;
        shield.itemName += $"{LocalizationManager.localization.GetText($"{shield.itemMaterial}")} {LocalizationManager.localization.GetText("Shield")}";
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
        ring.itemMaterial = mat;
        ring.dismantleOutput = 1;
        ring.itemName += $"{LocalizationManager.localization.GetText($"{ring.itemMaterial}")} {LocalizationManager.localization.GetText("Ring")}";
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
        // Wtf?
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
            switch(w.preset) {
                case WeaponPreset.Sword:
                    w.fireDamage *= 0.4f;
                    w.iceDamage *= 0.4f;
                    w.airDamage *= 0.4f;
                    w.earthDamage *= 0.4f;
                    w.lightningDamage *= 0.4f;
                    w.lightDamage *= 0.6f;
                    w.darkDamage *= 0.6f;
                    w.poisonDamage *= 0.5f;
                    w.curseDamage *= 0.4f;
                    break;
                case WeaponPreset.Axe:
                    w.bashDamage *= 0.6f;
                    w.pierceDamage *= 0.6f;
                    w.slashDamage *= 1.5f;
                    w.fireDamage *= 0.3f;
                    w.iceDamage *= 0.3f;
                    w.airDamage *= 0.3f;
                    w.earthDamage *= 0.3f;
                    w.lightningDamage *= 0.3f;
                    w.lightDamage *= 0.4f;
                    w.darkDamage *= 0.4f;
                    w.poisonDamage *= 0.4f;
                    w.bleedDamage *= 0.9f;
                    w.curseDamage *= 0.3f;
                    break;
                case WeaponPreset.Hammer:
                    w.bashDamage *= 1.9f;
                    w.pierceDamage *= 0.4f;
                    w.slashDamage *= 0.4f;
                    w.fireDamage *= 0.3f;
                    w.iceDamage *= 0.3f;
                    w.airDamage *= 0.3f;
                    w.earthDamage *= 0.3f;
                    w.lightningDamage *= 0.3f;
                    w.lightDamage *= 0.3f;
                    w.darkDamage *= 0.3f;
                    w.poisonDamage *= 0.2f;
                    w.bleedDamage *= 0.5f;
                    w.curseDamage *= 0.1f;
                    break;
                case WeaponPreset.Spear:
                    w.bashDamage *= 0.5f;
                    w.pierceDamage *= 1.6f;
                    w.slashDamage *= 0.5f;
                    w.fireDamage *= 0.3f;
                    w.iceDamage *= 0.3f;
                    w.airDamage *= 0.3f;
                    w.earthDamage *= 0.3f;
                    w.lightningDamage *= 0.3f;
                    w.lightDamage *= 0.4f;
                    w.darkDamage *= 0.4f;
                    w.poisonDamage *= 0.2f;
                    w.bleedDamage *= 0.5f;
                    w.curseDamage *= 0.3f;
                    break;
                case WeaponPreset.Bow:
                    w.bashDamage *= 0.1f;
                    w.pierceDamage *= 1.3f;
                    w.slashDamage *= 0.1f;
                    w.fireDamage *= 0.5f;
                    w.iceDamage *= 0.5f;
                    w.airDamage *= 0.5f;
                    w.earthDamage *= 0.5f;
                    w.lightningDamage *= 0.5f;
                    w.lightDamage *= 0.5f;
                    w.darkDamage *= 0.5f;
                    w.poisonDamage *= 0.5f;
                    w.bleedDamage *= 0.5f;
                    w.curseDamage *= 0.5f;
                    break;
                case WeaponPreset.Dagger:
                    w.bashDamage *= 0.1f;
                    w.pierceDamage *= 1.2f;
                    w.slashDamage *= 1.2f;
                    w.fireDamage *= 0.3f;
                    w.iceDamage *= 0.3f;
                    w.airDamage *= 0.3f;
                    w.earthDamage *= 0.3f;
                    w.lightningDamage *= 0.3f;
                    w.lightDamage *= 0.4f;
                    w.darkDamage *= 0.4f;
                    w.poisonDamage *= 1.1f;
                    w.bleedDamage *= 0.8f;
                    w.curseDamage *= 0.1f;
                    break;
                case WeaponPreset.Staff:
                    w.bashDamage *= 0.3f;
                    w.pierceDamage *= 0.2f;
                    w.slashDamage *= 0.2f;
                    w.fireDamage *= 1.2f;
                    w.iceDamage *= 1.2f;
                    w.airDamage *= 1.2f;
                    w.earthDamage *= 1.2f;
                    w.lightningDamage *= 0.5f;
                    w.lightDamage *= 0.6f;
                    w.darkDamage *= 0.6f;
                    w.poisonDamage *= 0.1f;
                    w.bleedDamage *= 0.1f;
                    w.curseDamage *= 0.8f;
                    break;
                case WeaponPreset.Tome:
                    w.bashDamage *= 0.1f;
                    w.pierceDamage *= 0.1f;
                    w.slashDamage *= 0.1f;
                    w.fireDamage *= 0.6f;
                    w.iceDamage *= 0.6f;
                    w.airDamage *= 0.6f;
                    w.earthDamage *= 0.6f;
                    w.lightningDamage *= 1.3f;
                    w.lightDamage *= 1.3f;
                    w.darkDamage *= 1.3f;
                    w.poisonDamage *= 0.1f;
                    w.bleedDamage *= 0.1f;
                    w.curseDamage *= 1.5f;
                    break;
            }
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
    Ruby, Emerald, Diamond, Musgravite, Taaffeite, Amber, Bone, Wool, Fur, Silk, Leather, Scale, Coal, Glass
}
