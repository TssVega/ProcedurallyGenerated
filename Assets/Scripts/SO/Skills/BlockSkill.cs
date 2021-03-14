using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Block")]
public class BlockSkill : ActiveSkill {

    public BlockData blockData;

    public override void Launch(StatusEffects statusEffects) {
        if(blockData.blockType == BlockType.Block) {
            statusEffects.StartBlocking(blockData.duration);
        }
        else if(blockData.blockType == BlockType.Parry) {
            Debug.Log("Parrying");
            statusEffects.StartParrying(blockData.duration);
        }
    }
}
