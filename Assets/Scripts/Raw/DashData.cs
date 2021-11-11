using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashData {

    public float dashMultiplier = 1f;
    // Warning! Do not add sideways dashes. It conflicts with A* pathfinding
}
