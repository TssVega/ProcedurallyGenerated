using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Appearance")]
public class CharacterAppearance : ScriptableObject {

    public Sprite[] hairStyles;
    public Color[] skinColors;
    public Color[] hairColors;
}
