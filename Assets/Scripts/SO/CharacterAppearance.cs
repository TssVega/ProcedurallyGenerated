using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Appearance")]
public class CharacterAppearance : ScriptableObject {

    public Sprite[] hairStyles;
    public Race[] races;
    public CharacterPreset[] presets;
    public Color[] hairColors;
    public Sprite defaultShoulders;
}
