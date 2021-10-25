using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Animator enemyAnimator;

    private void Awake() {
        enemyAnimator = GetComponent<Animator>();
    }
}
