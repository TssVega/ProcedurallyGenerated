using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2D;
    private Vector2 joystickInput;
    public bool lockedOn;
    public Transform nearestEnemy;
    //public FloatingJoystick joystick;
    public DynamicJoystick joystick;
    private Stats playerStats;
    private StatusEffects statusEffects;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private const float autoLockSearchInterval = 0.5f;
    private const float footstepInterval = 0.4f;
    private const float footstepThreshold = 2.7f;
    private const float checkLockInterval = 1f;
    private const float agilityMultiplier = 0.01f;

    private WaitForSeconds autoLockSearchIntervalWait;
    private WaitForSeconds footstepIntervalWait;
    private WaitForSeconds checkLockIntervalWait;

    private FieldOfView fov;
    private Passives passives;
    private SkillUser skillUser;

    private bool autoLock;

    public bool makingNoise;

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
        skillUser = GetComponent<SkillUser>();
        passives = GetComponent<Passives>();
        autoLock = PlayerPrefs.GetInt("lock") == 1;
        fov = GetComponent<FieldOfView>();
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        statusEffects = GetComponent<StatusEffects>();        
        rb2D = GetComponent<Rigidbody2D>();
        joystickInput = Vector2.zero;
        autoLockSearchIntervalWait = new WaitForSeconds(autoLockSearchInterval);
        footstepIntervalWait = new WaitForSeconds(footstepInterval);
        checkLockIntervalWait = new WaitForSeconds(checkLockInterval);
    }
    private void OnEnable() {
        lockedOn = false;
        if(autoLock) {
            StartCoroutine(AutoLock());            
        }
        StartCoroutine(CheckLockUI());
        StartCoroutine(CheckFootsteps());
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
        for(; ; ) {
            yield return autoLockSearchIntervalWait;
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
        for(; ; ) {
            if(lockUI) {
                lockUI.SetUIVisibility(fov.visibleTargets.Count > 0);
            }            
            if(lockUI && LocalizationManager.localization) {
                lockUI.CheckLock();
            }
            yield return checkLockIntervalWait;            
        }        
    }
    private IEnumerator CheckFootsteps() {
        for(; ; ) {
            if(rb2D.velocity.magnitude >= footstepThreshold && !skillUser.acquiredSkills.Contains(passives.twinkleToes)) {
                AudioSystem.audioManager.PlaySound("footsteps", 0f);
                makingNoise = true;
            }
            else {
                makingNoise = false;
            }
            yield return footstepIntervalWait;
        }        
    }
    private void GetInput() {
        // Input
        if(joystick) {
            joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }        
        else {
            joystick = FindObjectOfType<DynamicJoystick>();
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
            // If there is an input            
            if(joystick && joystickInput.sqrMagnitude > 0.01f && !statusEffects.chanelling && !statusEffects.stunned && !statusEffects.immobilized && joystick.gameObject.activeInHierarchy) {
                // Look at the locked enemy
                if(lockedOn && nearestEnemy) {
                    transform.eulerAngles = new Vector3(
                            0, 0, Vector3.SignedAngle(
                                Vector3.up, nearestEnemy.transform.position - transform.position, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Look at movement direction
                if(!lockedOn) {
                    transform.eulerAngles = new Vector3(
                        0, 0, Vector3.SignedAngle(Vector3.up, joystickInput.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(joystickInput * playerStats.runSpeed * (1 + playerStats.agility * agilityMultiplier));
            }
            else if(statusEffects.stunned || statusEffects.immobilized) {
                rb2D.velocity = Vector2.zero;
            }
            else if(joystick && !joystick.gameObject.activeInHierarchy) {
                rb2D.velocity = Vector2.zero;
                joystick.OnPointerUp(null);
            }
            if(horizontalInput != 0 || verticalInput != 0) {
                Vector3 movingDirection = new Vector3(horizontalInput, verticalInput, 0f);
                // Look at movement direction
                if(!lockedOn && !statusEffects.chanelling && !statusEffects.stunned && !statusEffects.immobilized) {
                    transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, movingDirection.normalized, Vector3.forward));
                    rb2D.angularVelocity = 0;
                }
                // Move
                rb2D.AddForce(movingDirection.normalized * playerStats.runSpeed * (1 + playerStats.agility * agilityMultiplier));                
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
