using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion")]
public class Potion : Item, IUsable {

    public PotionType potionType;
    public float power = 20f;
    public float time = 60f;

    public void Consume(StatusEffects status) {
        switch(potionType) {
            case PotionType.Healing:
                status.Heal(power);
                break;
            case PotionType.Mana:
                status.GiveMana(power);
                break;
        }        
    }
}

public enum PotionType {
    Healing, Mana
}
