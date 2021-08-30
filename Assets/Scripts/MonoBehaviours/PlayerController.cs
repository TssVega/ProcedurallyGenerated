using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2D;
    private Vector2 joystickInput;
    public bool lockedOn;
    public Transform nearestEnemy;
    public FloatingJoystick joystick;
    private Stats playerStats;
    private StatusEffects statusEffects;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private const float autoLockSearchInterval = 0.5f;

    private FieldOfView fov;

    private bool autoLock;

    public LockUI lockUI;

    // Assign private values
    private void Awake() {
        /*
        player = FindObjectOfType<Player>();
        statusFx = GetComponent<StatusEffects>();
        lockedOn = false;
        if(FindObjectOfType<UICanvas>()) {
            joystick = FindObjectOfType<UICanvas>().playerUI.GetComponent<UIPlayerStatus>().joystick;
        }*/
        autoLock = PlayerPrefs.GetInt("lock") == 1;
        fov = GetComponent<FieldOfView>();
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        statusEffects = GetComponent<StatusEffects>();        
        rb2D = GetComponent<Rigidbody2D>();
        joystickInput = Vector2.zero;
    }
    private void OnEnable() {
        lockedOn = false;
        if(autoLock) {
            StartCoroutine(AutoLock());            
        }
        StartCoroutine(CheckLockUI());
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
    private IEnumerator AutoLock() {
        while(true) {
            yield return new WaitForSeconds(autoLockSearchInterval);
            if(fov.visibleTargets.Count > 0) {
                nearestEnemy = fov.GetClosestTarget();
                if(nearestEnemy) {
                    lockedOn = true;
                }
            }
            else {
                lockedOn = false;
            }
        }        
    }
    // Unlock if the player is locked and lock to the nearest enemy if there is one
    public void ToggleLock() {
        if(lockedOn) {
            lockedOn = false;
        }
        else {
            if(fov.visibleTargets.Count > 0) {
                nearestEnemy = fov.GetClosestTarget();
                if(nearestEnemy) {
                    lockedOn = true;
                }                    
            }
        }
    }
    private IEnumerator CheckLockUI() {
        while(true) {
            lockUI.SetUIVisibility(fov.visibleTargets.Count > 0);
            if(lockUI) {
                lockUI.CheckLock();
            }
            yield return new WaitForSeconds(1f);            
        }        
    }
    private void GetInput() {
        // Input
        if(joystick) {
            joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }        
        else {
            joystick = FindObjectOfType<FloatingJoystick>();
        }
        
        // Check if there is a nearest enemy
        if(!nearestEnemy || !nearestEnemy.gameObject.activeSelf) {
            lockedOn = false;            
        }
        
#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
#endif
    }
    // Move player and face the direction of movement
    private void PlayerMove() {
        if(!playerStats.living) {
            return;
        }
        if(!(statusEffects.stunned || statusEffects.immobilized || statusEffects.chanelling)) {
            // Look at the locked enemy
            if(lockedOn && nearestEnemy) {
                transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(
                            Vector3.up, nearestEnemy.transform.position - transform.position, Vector3.forward));
                rb2D.angularVelocity = 0;
            }
            // If there is an input            
            if(joystick && joystickInput.sqrMagnitude > 0.01f &&
                (!statusEffects.chanelling
                 && !statusEffects.stunned
                 && !statusEffects.immobilized)) {
                // Look at movement direction
                if(!lockedOn) {
                    transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(Vector3.up, joystickInput.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(joystickInput * playerStats.runSpeed);
            }
            else if(statusEffects.stunned || statusEffects.immobilized) {
                rb2D.velocity = Vector2.zero;
            }
            if(horizontalInput != 0 || verticalInput != 0 &&
                (!statusEffects.chanelling
                 && !statusEffects.stunned
                 && !statusEffects.immobilized)) {
                Vector3 movingDirection = new Vector3(horizontalInput, verticalInput, 0f);
                // Look at movement direction
                if(!lockedOn) {
                    transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(Vector3.up, movingDirection.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(movingDirection.normalized * playerStats.runSpeed);
            }
            else if(statusEffects.stunned || statusEffects.immobilized) {
                rb2D.velocity = Vector2.zero;
            }
        }
    }
}
// Movement status
public enum MovementState {
    Idle, Walking
}
