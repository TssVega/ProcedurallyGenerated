using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject {

    public List<Item> items;
}
