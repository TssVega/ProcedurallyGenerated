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
    // Chest Armor
    public Color[] chestArmorBaseColors;
    public Color[] chestArmorOverlayColors;
    public Color[] chestArmorBackColors;
    public Sprite[] chestArmorBases;
    public Sprite[] chestArmorOverlays;
    public Sprite[] chestArmorBacks;

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

    public Armor CreateArmorSprite() {
        Armor armor = CreateInstance("Armor") as Armor;
        return armor;
    }
}
