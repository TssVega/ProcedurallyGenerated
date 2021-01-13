using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    /*
    private Vector2 movementVector;

    private void Start() {
        movementVector = Vector2.zero;
    }

    private void Update() {
        movementVector = Vector2.zero;
        if(Input.GetKey(KeyCode.LeftArrow)) {
            movementVector += Vector2.left;
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            movementVector += Vector2.right;
        }
        if(Input.GetKey(KeyCode.UpArrow)) {
            movementVector += Vector2.up;
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            movementVector += Vector2.down;
        }
        transform.Translate(movementVector.normalized * Time.deltaTime * 8f);
    }
    */
    private float walkSpeed;
    private Rigidbody2D rb2D;
    private Vector2 joystickInput;
    //private Joystick joystick;
    public bool lockedOn;
    //public Enemy nearestEnemy;
    //private Enemy[] nearbyEnemies;
    //private Player player;
    //private StatusEffects statusFx;
#if UNITY_EDITOR
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
#endif
    // Assign private values
    private void Start() {
        /*
        player = FindObjectOfType<Player>();
        statusFx = GetComponent<StatusEffects>();
        lockedOn = false;
        if(FindObjectOfType<UICanvas>()) {
            joystick = FindObjectOfType<UICanvas>().playerUI.GetComponent<UIPlayerStatus>().joystick;
        }*/
        walkSpeed = 16f;
        lockedOn = false;
        rb2D = GetComponent<Rigidbody2D>();
        //joystickInput = Vector2.zero;
    }
    // Get input
    private void Update() {
        GetInput();
    }
    // Move
    private void FixedUpdate() {
        PlayerMove();
        /*
        if(joystick.Horizontal != 0 || joystick.Vertical != 0) {
            PlayerMove();
        }
        */
    }
    /*
    // Unlock if the player is locked and lock to the nearest enemy if there is one
    public void ToggleLock() {
        if(lockedOn) {
            lockedOn = false;
        }
        else {
            float shortestLength = 1000;
            nearbyEnemies = FindObjectsOfType<Enemy>();
            if(nearbyEnemies.Length > 0) {
                for(int i = 0; i < nearbyEnemies.Length; i++) {
                    if(shortestLength > (nearbyEnemies[i].transform.position - transform.position).sqrMagnitude) {
                        shortestLength = (nearbyEnemies[i].transform.position - transform.position).sqrMagnitude;
                        nearestEnemy = nearbyEnemies[i];
                    }
                }
                if(nearestEnemy)
                    lockedOn = true;
            }
        }
    }*/
    private void GetInput() {
        // Input
        //if(joystick) {
        //    joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        //}
        /*
        else {
            joystick = FindObjectOfType<Joystick>();
        }
        // Check if there is a nearest enemy
        if(!nearestEnemy || !nearestEnemy.gameObject.activeSelf) {
            lockedOn = false;
            LockUI lockUI = FindObjectOfType<LockUI>();
            if(lockUI) {
                lockUI.CheckLock();
            }
        }*/
#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif
    }
    // Move player and face the direction of movement
    private void PlayerMove() {
        if(/*!(statusFx.stunned || statusFx.immobilized || statusFx.chanelling)*/true) {
            // Look at the locked enemy
            /*if(lockedOn && nearestEnemy) {
                transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(
                            Vector3.up, nearestEnemy.transform.position - transform.position, Vector3.forward));
                rb2D.angularVelocity = 0;
            }*/
            // If there is an input
            /*
            if(joystick && joystickInput.sqrMagnitude > 0.1f) {
                // Look at movement direction
                if(!lockedOn) {
                    transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(Vector3.up, joystickInput.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(joystickInput.normalized * walkSpeed);
            }*/
            if(horizontalInput != 0 || verticalInput != 0) {
                Vector3 movingDirection = new Vector3(horizontalInput, verticalInput, 0f);
                // Look at movement direction
                if(!lockedOn) {
                    transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(Vector3.up, movingDirection.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(movingDirection.normalized * walkSpeed);
            }
        }
    }
}
// Movement status
public enum MovementState {
    Idle, Walking
}
