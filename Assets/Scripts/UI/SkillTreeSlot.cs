using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeSlot : MonoBehaviour {

    public int slotIndex;

    private SkillTreeManager skillTreeManager;

    private void Awake() {
        skillTreeManager = FindObjectOfType<SkillTreeManager>();
    }
    public void OpenSlotInfo() {
        Debug.Log("Called");
        skillTreeManager.OpenSkillInfo(slotIndex);
    }
}
