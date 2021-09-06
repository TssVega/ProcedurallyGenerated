using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe {

    public int strengthRequirement;
    public int agilityRequirement;
    public int dexterityRequirement;
    public int intelligenceRequirement;
    public int faithRequirement;
    public int wisdomRequirement;
    public int vitalityRequirement;
    public int charismaRequirement;
    public List<CraftingIngredient> recipe;
    public Item output;
}
