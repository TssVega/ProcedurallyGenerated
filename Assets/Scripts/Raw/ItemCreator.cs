using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Creator")]
public class ItemCreator : ScriptableObject {

    public Color[] weaponHandleColors;
    public Color[] weaponGuardColors;
    public Color[] weaponBladeColors;
    public Sprite[] weaponHandles;
    public Sprite[] weaponGuards;
    public Sprite[] weaponBlades;

    public Weapon CreateWeaponSprite(string seed) {
        System.Random pseudoRandom= new System.Random(seed.GetHashCode());
        Sprite handle = weaponHandles[pseudoRandom.Next(0, weaponHandles.Length)];
        Sprite guard = weaponGuards[pseudoRandom.Next(0, weaponGuards.Length)];
        Sprite blade = weaponBlades[pseudoRandom.Next(0, weaponBlades.Length)];
        Color handleColor = weaponHandleColors[pseudoRandom.Next(0, weaponHandleColors.Length)];
        Color guardColor = weaponGuardColors[pseudoRandom.Next(0, weaponGuardColors.Length)];
        Color bladeColor = weaponBladeColors[pseudoRandom.Next(0, weaponBladeColors.Length)];
        Weapon weapon = CreateInstance("Weapon") as Weapon;
        weapon.weaponHandle = handle;
        weapon.weaponGuard = guard;
        weapon.weaponBlade = blade;
        weapon.weaponHandleColor = handleColor;
        weapon.weaponGuardColor = guardColor;
        weapon.weaponBladeColor = bladeColor;
        return weapon;
    }
}
