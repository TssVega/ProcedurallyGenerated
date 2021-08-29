using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public static Spawner spawner;

    public List<Enemy> entities;

    private void Awake() {
        spawner = this;
    }
}
