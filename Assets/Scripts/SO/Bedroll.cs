using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Bedroll")]
public class Bedroll : Item {

    public void Consume(Player player) {
        player.SavePlayer();
    }
}
