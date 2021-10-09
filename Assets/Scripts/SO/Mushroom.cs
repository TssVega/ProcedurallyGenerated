using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Mushroom")]
public class Mushroom : Item, IUsable {

    public MushroomType mushroomType;
    [Range(1f, 50f)]
    public float mushroomPower;

    private const float timePeriod = 20f;

    public void Consume(StatusEffects status) {
        // Yoseon fervor
        if(status.player && status.player.raceIndex == 6) {
            const float yoseonFervorRate = 1.15f;
            const float yoseonFervorDuration = 10f;
            status.StartSpeedUp(yoseonFervorRate, yoseonFervorDuration);
        }
        if(mushroomType == MushroomType.Edible) {
            // Heal over time and satisfy hunger here
            status.StartRegen(timePeriod, mushroomPower / timePeriod);
            status.GiveEnergy(mushroomPower);
        }
        else {
            // Poison here
            status.AddPoisonStacks((int)mushroomPower);
        }
        AudioSystem.audioManager.PlaySound("mushroomEat", 0f);
    }
}

public enum MushroomType {
    Edible, Poisonous
}
