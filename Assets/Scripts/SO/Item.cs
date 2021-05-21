using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Rules to follow when making an item
    
    *** Special effects are Enchantments
    * Weapons
    *   All weapons must have bash, pierce and slash values above 0
    *   Weapons may have at most two of the following
    *       Either a bleed, poison or curse damage
    *       One of elemental damage types
    *       One of main stat bonuses
    *   Weapons may or may not have special effects
    * Armors (Body Armor & Leg Armor)
    *   All armors must have basic resistance and basic armor values
    *   All resistance and armor values gain a bonus depending on this value
    *   All armor can have at most 3 bonus stats including
    *       Physical damage types
    *       Elemental damage types
    *       Damage over time types
    *   Armors may or may not have special effects
    * Rings
    *   Rings can have 5 bonus stats from any type
    *   Rings may or may not have special effects
    * Shields
    *   All shields must have all these types
    *       Parry duration above 0.2f
    *       Block duration above 0.5f
    *       Damage block percent above 0.9f
    *   Shields may only have one of the main stat bonuses
    *   Shields may or may not have special effects
    * Potions
    *   Potions can only have special effects
 */
[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject {

    public string seed;
    //public int itemSaveIndex;
    public string itemName;
    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite thirdSprite;
    public Sprite firstIcon;
    public Sprite secondIcon;
    public Sprite thirdIcon;
    public Color firstColor;
    public Color secondColor;
    public Color thirdColor;
    public EquipSlot slot;
    public Rarity rarity;
    public int diamondCost;
    public int goldCost;
    public int silverCost;
    public bool stackable;
    //public Enchantment enchantment;
    public int dismantleOutput;
    public GameObject particles;
    public bool hasTrail;
    public Vector2 trailPosition = new Vector2(0.4f, 0.4f);
    public Gradient weaponTrailGradient;
    public bool material = false;
    public ItemMaterial itemMaterial;

    [Header("Main Stats")]
    public int strength;
    public int agility;
    public int dexterity;
    public int intelligence;
    public int faith;
    public int wisdom;
    public int vitality;
    public int charisma;
    /*
    public virtual void Consume(StatusEffects user) {
    }*/
    public virtual void OnHitGiven() {
    }
    public virtual void OnHitTaken() {
    }
}
// Equip slots of items
public enum EquipSlot {
    RightHand, LeftHand, Head, Body, Legs, Finger, Consumable, Other
}

public enum AttackType {
    Slash, Pierce, Bash, Fire, Ice, Lightning, Air, Earth, Light, Dark, Poison, Bleed, Curse, None
}
