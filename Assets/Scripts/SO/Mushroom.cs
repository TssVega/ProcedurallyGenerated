using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Mushroom")]
public class Mushroom : Item, IUsable {

    public MushroomType mushroomType;
    [Range(1f, 50f)]
    public float mushroomPower;

    public void Consume(StatusEffects status) {
        if(mushroomType == MushroomType.Edible) {
            // Heal over time and satisfy hunger here
            status.StartRegen(20f, mushroomPower * 0.05f);
        }
        else {
            // Poison here
            status.AddPoisonStacks((int)mushroomPower);
        }
    }
}

public enum MushroomType {
    Edible, Poisonous
}
