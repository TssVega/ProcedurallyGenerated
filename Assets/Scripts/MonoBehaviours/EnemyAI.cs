using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Animator enemyAnimator;
    private SkillUser skillUser;
    private FieldOfView fov;
    private Stats stats;
    private StatusEffects status;

    private Rigidbody2D rb2D;

    public float roamTime = 3f;
    public float timeBetweenSkills = 0.5f;
    public float rotationTime = 1.5f;
    [HideInInspector] public float speed = 3f;

    public bool moving = false;
    private bool roaming = false;
    private bool rotating = false;
    public bool attacked = false;
    public bool hostility = false;          // Will it attack enemies on sight?
    public bool hostile = false;            // Current hostility status
    public bool willDefendItself = true;    // Will this entity defend itself if it gets attacked?
    public bool hasEars = true;             // If this enemy is hostile and it hears footsteps, will it attack the target?
    public bool isBoss = false;

    public string walkAnimationName;
    public string idleAnimationName;

    public LevelGeneration level;

    private WaitForSeconds rotationTimeWait;

    private void Awake() {
        status = GetComponent<StatusEffects>();
        rb2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        fov = GetComponent<FieldOfView>();
        skillUser = GetComponent<SkillUser>();
        enemyAnimator = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        rotationTimeWait = new WaitForSeconds(rotationTime);
    }
    private void OnEnable() {
        moving = false;
        roaming = false;
        aiPath.canMove = true;
        speed = stats.runSpeed;
        aiPath.maxSpeed = speed;
        destinationSetter.target = null;
        hostile = hostility;
        attacked = false;
        aiPath.enableRotation = true;
        rotating = false;
        if(gameObject.activeInHierarchy && gameObject.activeSelf) {
            StartCoroutine(Roam());
            StartCoroutine(RotateTowardsPlayer());
        }        
    }
    private void OnDisable() {
        //aiPath.destination = transform.position;
        StopAllCoroutines();        
    }
    private void Update() {
        if(!stats.living || rotating) {
            aiPath.canMove = false;
        }
        if(!rotating && !status.stunned && !status.immobilized && !status.chanelling) {
            aiPath.canMove = true;
            //transform.LookAt(destinationSetter.target, Vector3.forward);
            Vector3 lookingDirection = aiPath.desiredVelocity;
            if(lookingDirection.magnitude > 0.1f) {
                transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, lookingDirection.normalized, Vector3.forward));
                rb2D.angularVelocity = 0;
            }
            
        }
        if(roaming && !aiPath.reachedDestination) {
            moving = true;
        }
        else if(destinationSetter.target && !aiPath.reachedDestination) {
            moving = true;
        }
        else {
            moving = false;
        }
        if(aiPath.canMove && moving) {
            if(!string.IsNullOrEmpty(walkAnimationName)) {
                enemyAnimator.SetTrigger(walkAnimationName);
            }
        }
        else {
            if(!string.IsNullOrEmpty(idleAnimationName)) {
                enemyAnimator.SetTrigger(idleAnimationName);
            }            
        }        
    }
    private IEnumerator RotateTowardsPlayer() {

        float progress = 0f;
        float speed = aiPath.rotationSpeed / 360f;

        while(progress < 1f && destinationSetter.target && !stats.status.stunned && !stats.status.chanelling && !stats.status.immobilized) {
            Vector3 targetRotation = new Vector3(
            0, 0, Vector3.SignedAngle(
                Vector3.up, destinationSetter.target.transform.position - transform.position, Vector3.forward));
            Quaternion lookOnLook = Quaternion.Euler(targetRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);
            rotating = true;
            progress += Time.deltaTime * speed;
            if(progress <= 1f) {
                yield return null;
            }
        }
        rotating = false;
        yield return rotationTimeWait;
        if(stats.living && gameObject.activeInHierarchy) {
            StartCoroutine(RotateTowardsPlayer());
        }        
    }
    private IEnumerator Roam() {
        yield return null;
        if(!destinationSetter.target) {
            roaming = true;
            if(level && level.isActiveAndEnabled) {
                aiPath.destination = GetRoamingPosition();
            }
            else if(!isBoss) {
                gameObject.SetActive(false);
            }
        }
        else {
            roaming = false;
        }
        yield return new WaitForSeconds(Random.Range(roamTime, roamTime * 2));
        if(stats.living && gameObject.activeInHierarchy) {
            StartCoroutine(Roam());
        }        
    }
    public void SetLevel(LevelGeneration level) {
        this.level = level;
    }
    public void Enrage() {
        if(willDefendItself && hostile) {
            aiPath.enableRotation = false;
            StartCoroutine(UseRandomSkill());
        }
        else {
            Flee();
        }
    }
    private void Flee() {
        // Escape from the attacker
        StopCoroutine(Roam());
        StartCoroutine(Roam());        
    }
    private IEnumerator UseRandomSkill() {
        yield return new WaitForSeconds(Random.Range(timeBetweenSkills, timeBetweenSkills * 2f));
        if(stats.living && gameObject.activeInHierarchy) {
            List<int> availableIndices = new List<int>();
            
            for(int i = 0; i < skillUser.currentSkills.Length; i++) {
                ActiveSkill a = skillUser.currentSkills[i] as ActiveSkill;
                if(a && a.aiRange > fov.GetDistanceToEnemy() && skillUser.skillCooldowns[i] <= 0f) {
                    availableIndices.Add(i);
                }
            }
            if(availableIndices.Count > 0) {
                ActiveSkill activeSkill = skillUser.currentSkills[availableIndices[Random.Range(0, availableIndices.Count)]] as ActiveSkill;
                skillUser.UseSkill(activeSkill);
            }
            if(destinationSetter.target != null) {
                StartCoroutine(UseRandomSkill());
            }
        }              
    }
    private Vector3 GetRoamingPosition() {
        if(level) {
            return level.GetRandomLocation();
        }
        return transform.position;
    }
}
