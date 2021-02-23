using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelativePosition {

    public static Relative GetRelativePosition(Transform attacker, Transform defender) {

        Vector3 direction = attacker.position - defender.position;
        direction.Normalize();
        float dotProduct = Vector3.Dot(direction, defender.up);
        if(dotProduct < -0.7) {
            return Relative.Behind;
        }
        else if(dotProduct > 0.5) {
            return Relative.FacingEachOther;
        }
        return Relative.None;
    }
}

public enum Relative {
    None, FacingEachOther, Behind
}
