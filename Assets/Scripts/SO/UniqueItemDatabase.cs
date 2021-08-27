using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/UniqueItemDatabase")]
public class UniqueItemDatabase : ScriptableObject {

    public List<Item> uniqueItems;
}
