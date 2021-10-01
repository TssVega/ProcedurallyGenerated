using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion")]
public class Potion : Item, IUsable {

    public PotionType potionType;
    public float power = 20f;
    public float time = 60f;

    private const float levonaEnergyRate = 0.4f;

    public void Consume(StatusEffects status) {
        switch(potionType) {
            case PotionType.Healing:
                status.Heal(power);
                break;
            case PotionType.Mana:
                status.GiveMana(power);
                break;
        }
        // Levona effect
        if(status.player && status.player.raceIndex == 0) {
            status.GiveEnergy(power * levonaEnergyRate);
        }
    }
}

public enum PotionType {
    Healing, Mana
}
