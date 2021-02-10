using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData {

    public BlockType blockType;
    public float duration;
}

public enum BlockType {
    Block, Parry
}
