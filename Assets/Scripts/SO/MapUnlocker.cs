using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/MapUnlocker")]
public class MapUnlocker : Item {

    public void Consume(Player player) {
        player.mapUnlocked = true;
    }
}
