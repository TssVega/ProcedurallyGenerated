using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public SkillDatabase skillDatabase;

    private void Awake() {
        //Application.targetFrameRate = 60;
        skillDatabase.SetIndices();
    }
}
