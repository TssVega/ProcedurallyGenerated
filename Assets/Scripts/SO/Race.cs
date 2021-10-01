using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Race")]
public class Race : ScriptableObject {

    public int index = 0;
    public string raceName;
    public Color skinColor;
}
// Races:
// Levona, Satian, Crevalonian, Othani, Pelthonese, Helgafelli, Yoseon, Qotush, Milona, Vilgerosi, Nastac, Havellian
