using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion")]
public class Potion : Item, IUsable {

    public PotionType potionType;
    public float power = 20f;

    private const float levonaEnergyRate = 0.6f;

    public ItemDatabase itemDatabase;

    public void Consume(StatusEffects status) {
        switch(potionType) {
            case PotionType.Healing:
                status.Heal(status.player.npcBonuses[22] ? power * 1.5f : power);
                break;
            case PotionType.Mana:
                status.GiveMana(power);
                break;
        }
        // Add a vial to inventory after you consume a potion
        AudioSystem.audioManager.PlaySound("potionDrink", 0f);
        status.player.inventory.AddToInventory(itemDatabase.GetVial(), false);
        // Levona effect
        if(status.player && status.player.raceIndex == 0) {
            status.GiveEnergy(power * levonaEnergyRate);
        }
    }
}

public enum PotionType {
    Healing, Mana
}
