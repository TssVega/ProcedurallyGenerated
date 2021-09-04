using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private ProjectileSkill projectileSkill;
    private ProjectileData projectileData;
    private Rigidbody2D rb2d;
    private Vector3 projectileVector;
    private float countdown;
    private Stats attackerStats;
    private Transform target;
    private CircleCollider2D circleCollider2D;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void Update() {
        countdown -= Time.deltaTime;
        if(countdown <= 0f) {
            EndProjectile();
        }
    }
    private void FixedUpdate() {
        if(projectileData.homing && target) {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -rotateAmount * projectileData.rotationSpeed;
            rb2d.velocity = transform.up * projectileData.projectileSpeed;
        }
        else {
            gameObject.transform.position += projectileVector * projectileData.projectileSpeed * Time.fixedDeltaTime;
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Stats>() && projectileData.team != collision.GetComponent<Stats>().team  ) {
            projectileSkill.Activate(collision.GetComponent<StatusEffects>(), attackerStats);
            if(!projectileData.penetrates) {
                EndProjectile();
            }
        }        
    }
    private void EndProjectile() {
        for(int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).GetComponent<ParticleSystem>()) {
                transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }            
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.DetachChildren();
        target = null;
        gameObject.SetActive(false);
    }
    public void SetProjectile(ProjectileSkill skill, Stats attackerStats) {
        projectileSkill = skill;
        projectileData = skill.projectileData;        
        this.attackerStats = attackerStats;
        circleCollider2D.radius = projectileData.radius;
    }
    public void StartProjectile(Transform target) {
        if(target) {
            this.target = target;
        }
    }
    public void StartProjectile(Vector2 movementVector) {
        countdown = projectileData.lifetime;
        projectileVector = movementVector;             
    }
}
