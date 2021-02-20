using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelativePosition {

    public static Relative GetRelativePosition(Transform attacker, Transform defender) {

        float dotProduct = Vector3.Dot(attacker.position, defender.position);
        return Relative.None;
    }
}

public enum Relative {
    None, FacingEachOther, Behind
}
