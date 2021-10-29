using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/SkillBookUnlocker")]
public class SkillBookUnlocker : Item {

    public void Consume(Player player) {
        player.skillBookUnlocked = true;
    }
}
