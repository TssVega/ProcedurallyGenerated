using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public int skinColorIndex;
    public int hairColorIndex;
    public int hairStyleIndex;

    public SaveData(Player data) {
        skinColorIndex = data.skinColorIndex;
        hairColorIndex = data.hairColorIndex;
        hairStyleIndex = data.hairStyleIndex;
    }
}
