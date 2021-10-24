using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Block")]
public class BlockSkill : ActiveSkill {

    public BlockData blockData;

    public override void Launch(StatusEffects statusEffects) {
        if(blockData.blockType == BlockType.Block) {
            statusEffects.StartBlocking(castTime + (statusEffects.stats.dexterity * 0.003f) + (statusEffects.stats.agility * 0.003f));
        }
        else if(blockData.blockType == BlockType.Parry) {
            statusEffects.StartParrying(castTime + (statusEffects.stats.dexterity * 0.003f) + (statusEffects.stats.agility * 0.003f));
        }
    }
}
