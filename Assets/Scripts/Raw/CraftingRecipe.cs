using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject {

    public int strengthRequirement;
    public int agilityRequirement;
    public int dexterityRequirement;
    public int intelligenceRequirement;
    public int faithRequirement;
    public int wisdomRequirement;
    public int vitalityRequirement;
    public int charismaRequirement;
    public Item output;
    public Item input0;
    public Item input1;
    public Item input2;
    public Item input3;
    public Item input4;
    public Item input5;
    public Item input6;
    public Item input7;
    public Item input8;
}
