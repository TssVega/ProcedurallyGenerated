using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Mushroom")]
public class Mushroom : Item {

    public MushroomType mushroomType;
    [Range(1f, 50f)]
    public float mushroomPower;
    [Range(0f, 1f)] public float abundance;

    public void Consume(StatusEffects status) {
        if(mushroomType == MushroomType.Edible) {
            // heal over time and satisfy hunger here
            status.StartRegen(20f, mushroomPower * 0.05f);
        }
        else {
            // poison here
            //status.
        }
    }
}

public enum MushroomType {
    Edible, Poisonous
}
