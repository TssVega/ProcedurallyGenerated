using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Meat")]
public class Meat : Item, IUsable {

    public bool cooked;
    public float power = 20f;

    private const float timePeriod = 20f;

    private const float vilgerosiBless = 1.1f;
    private const float vilgerosiBlessDuration = 20f;

    public ItemDatabase itemDatabase;

    public void Consume(StatusEffects status) {
        status.StartRegen(status.player.npcBonuses[41] ? timePeriod * 2f : timePeriod, cooked ? power / timePeriod : power / 3f / timePeriod, false);
        status.GiveEnergy(power);
        // Vilgerosi bless
        if(cooked && status.player.raceIndex == 9) {
            status.StartBless(vilgerosiBless, vilgerosiBlessDuration);
        }
        AudioSystem.audioManager.PlaySound("meatEat", 0f);
    }
}
