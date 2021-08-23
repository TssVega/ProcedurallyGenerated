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

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        countdown -= Time.deltaTime;
        if(countdown <= 0f) {
            EndProjectile();
        }
    }
    private void FixedUpdate() {
        gameObject.transform.position += projectileVector * projectileData.projectileSpeed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Stats>() && projectileData.team != collision.GetComponent<Stats>().team  ) {
            projectileSkill.Activate(collision.GetComponent<StatusEffects>(), attackerStats);
            if(!projectileData.penetrates) {
                gameObject.SetActive(false);
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
        gameObject.SetActive(false);
    }
    public void SetProjectile(ProjectileSkill skill, Stats attackerStats) {
        projectileSkill = skill;
        projectileData = skill.projectileData;
        countdown = projectileData.lifetime;
        this.attackerStats = attackerStats;
    }
    public void StartProjectile(Transform target) {
        if(target) {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -rotateAmount * projectileData.rotationSpeed;
            rb2d.velocity = transform.up * projectileData.projectileSpeed;
        }
    }
    public void StartProjectile(Vector2 movementVector) {        
        projectileVector = movementVector;
    }
}
