using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject {

    public List<Item> items;
    public List<Item> books;
    public List<Item> physicalBooks;
    public List<Item> grimoire;
    public List<Item> tomes;
    public List<Item> commonBooks;
    public List<Item> essentialBooks;
    public List<Item> potions;
    public List<Item> coins;
    [Header("Vendor Items")]
    public List<Item> vendorItems;

    public Item GetItemByMaterial(ItemMaterial mat) {
        if((int)mat < 15) {
            return items[(int)mat];
        }
        else if((int)mat == 15) {
            return items[28];
        }
        else if((int)mat == 16) {
            return items[22];
        }
        else if((int)mat == 17) {
            return items[17];
        }
        else if((int)mat == 18) {
            return items[22];
        }
        else if((int)mat == 19) {
            return items[15];
        }
        else if((int)mat == 20) {
            return items[16];
        }
        else if((int)mat == 21) {
            return items[32];
        }
        else if((int)mat == 22) {
            return items[104];
        }
        else {
            return null;
        }
    }
}

//Wood, Copper, Iron, Silver, Gold, Platinum, Titanium, Tungsten, Sapphire,
//Ruby, Emerald, Diamond, Musgravite, Taaffeite, Amber, Bone, Wool, Fur, Silk, Leather, Scale, Coal
