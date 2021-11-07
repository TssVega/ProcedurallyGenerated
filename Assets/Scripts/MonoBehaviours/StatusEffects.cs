using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Stats))]
public class StatusEffects : MonoBehaviour {

    public Player player;
    private Player globalPlayer;
    //private Enemy enemy;
    private AIPath aiPath;
    private EnemyAI enemyAI;
    public DeathPanel deathPanel;
    public StatusUI statusUI;
    public HitWarning hitWarning;
    public Stats stats;
    public SkillUser skillUser;
    private Passives passives;
    private Rigidbody2D rb2d;
    //private StatusBar bar;
    //public StatusParticles statusParticles;
    // Coroutines
    private Coroutine poisonCounter;
    private Coroutine poisonStacksCounter;
    private Coroutine bleedCounter;
    private Coroutine bleedStacksCounter;
    private Coroutine curseCounter;
    private Coroutine curseStacksCounter;
    private Coroutine burnCounter;
    private Coroutine regenCounter;
    private Coroutine stunCounter;
    private Coroutine frostbiteCounter;
    private Coroutine chillCounter;
    private Coroutine lightningStacksCounter;
    private Coroutine iceStacksCounter;
    private Coroutine darkSigilCounter;
    private Coroutine timeBombCounter;
    private Coroutine airborneCounter;
    private Coroutine earthStacksCounter;
    private Coroutine immobilizeCounter;
    private Coroutine earthenCounter;
    private Coroutine lightCounter;
    private Coroutine speedUpCounter;
    private Coroutine chanellingCounter;
    private Coroutine fireStacksCounter;
    private Coroutine locatingTarget;
    private Coroutine blockingCoroutine;
    [Header("Status Effects")]
    public bool stunned = false;        // Prevents movement and attacking
    public bool frostbitten = false;    // Get more damage from ice or water when frostbitten
    public bool airborne = false;       // Pushes the gameObject
    public bool bleeding = false;       // Deals more damage over time
    public bool poisoned = false;       // Deals damage with periods getting shorter over time
    public bool cursed = false;         // Weakens
    public bool burning = false;        // Deals fire damage over time
    public bool regenerating = false;   // Regenerates health
    public bool chilled = false;        // Slows movement
    public bool darkSigil = false;      // Deals dark damage after darkSigil expires depending on damage taken
    public bool timeBombed = false;     // Deals damage after a few seconds
    public bool immobilized = false;    // Only prevents movement
    public bool earthed = false;        // Heals the damager and takes less lightning damage
    public bool lit = false;            // Takes pure light damage for the next light attack
    public bool cannotBeFrostbitten = false;
    public bool spedUp = false;
    public bool chanelling = false;
    public bool blocking = false;
    public bool parrying = false;
    public bool blessed = false;
    // Stacks
    [Header("Status Effect Counters")]
    private int lightningStacks = 0;
    private bool lightningStacksCounterRunning = false;
    private int iceStacks = 0;
    private bool iceStacksCounterRunning = false;
    private float darkSigilCharge = 0;
    private bool darkSigilRunning = false;
    private bool cannotBeDarkSigiled = false;
    private int earthStacks = 0;
    private bool earthStacksCounterRunning = false;
    private int lightStacks = 0;
    private bool lightStacksCounterRunning = false;
    private float frostbiteDamage = 0;
    private float originalSpeed = 0;    // To return to original value after slow effect ends    
    private float fireStacks = 0;
    private bool fireStacksCounterRunning = false;
    private bool poisonStacksCounterRunning = false;
    private int poisonStacks = 0;
    private bool bleedStacksCounterRunning = false;
    private int bleedStacks = 0;
    private bool curseStacksCounterRunning = false;
    private int curseStacks = 0;
    private bool blockingCoroutineRunning = false;
    //private bool locatingTargetRunning = false;
    // Constant values
    private const float lightningDamageForChilledMultiplier = 1.2f;
    private const float lightningDamageForEarthedMultiplier = 0.5f;
    private const int frostbiteCooldown = 16;
    private const int darkSigilCooldown = 15;
    // Game objects
    public Transform defaultShieldTransform;
    public Transform forwardShieldTransform;
    public Transform parryTransform;

    private GameObject stunParticles;

    private const float energyTime = 8f;
    private const float hungerSlowRate = 0.2f;
    private const float defaultParryStunDuration = 0.4f;

    private const float maximumDamageDistance = 10f;

    private WaitForSeconds energyTimeWait;
    private WaitForSeconds waitASec;

    public float blessRate = 1f;

    public GameObject lowEnergyParticles;
    public GameObject blessParticles;
    public GameObject burningParticles;
    public GameObject litParticles;
    public GameObject rootParticles;
    public GameObject bleedingParticles;
    public GameObject poisonedParticles;
    public GameObject cursedParticles;
    public GameObject frostbittenParticles;
    public GameObject chilledParticles;
    public GameObject darkSigilParticles;
    public GameObject spedUpParticles;
    public GameObject shockedParticles;

    private float DistanceToPlayer {
        get {
            return player ? 0f : Vector3.Distance(globalPlayer.transform.position, transform.position);
        }
    }
    private void OnEnable() {
        globalPlayer = FindObjectOfType<Player>();
        aiPath = GetComponent<AIPath>();
        enemyAI = GetComponent<EnemyAI>();
        player = GetComponent<Player>();
        //enemy = GetComponent<Enemy>();
        stats = GetComponent<Stats>();
        passives = GetComponent<Passives>();
        rb2d = GetComponent<Rigidbody2D>();
        skillUser = GetComponent<SkillUser>();
        waitASec = new WaitForSeconds(1f);
        //if(enemy) {
        //    stats.walkSpeed = enemy.enemy.speed;
        //}
        //bar = GetComponent<StatusBar>();
        originalSpeed = stats.runSpeed;
        if(enemyAI) {
            enemyAI.speed = originalSpeed;
        }
        DefaultStatusValues();
        if(statusUI) {
            statusUI.UpdateHealth(stats.health / stats.trueMaxHealth, stats.health);
            statusUI.UpdateMana(stats.mana / stats.trueMaxMana, stats.mana);
            statusUI.UpdateEnergy(stats.energy / stats.trueMaxEnergy, stats.energy);
        }
        if(player && statusUI) {
            energyTimeWait = new WaitForSeconds(energyTime);
            StartCoroutine(EnergyCounter());
        }
    }
    // Stop all effect counters and reset values on disable
    private void OnDisable() {
        //statusParticles.StopAllParticles();
        StopAllCoroutines();
    }
    // Reset status values
    private void DefaultStatusValues() {
        //if(enemy && enemy.enemyDetection) {
        //    enemy.enemy.speed = originalSpeed;
        //    enemy.enemyDetection.aiPath.maxSpeed = originalSpeed;
        //}
        lightningStacks = 0;
        lightningStacksCounterRunning = false;
        iceStacks = 0;
        iceStacksCounterRunning = false;
        darkSigilCharge = 0;
        darkSigilRunning = false;
        cannotBeDarkSigiled = false;
        earthStacks = 0;
        earthStacksCounterRunning = false;
        lightStacks = 0;
        lightStacksCounterRunning = false;
        fireStacks = 0;
        fireStacksCounterRunning = false;
        poisonStacks = 0;
        poisonStacksCounterRunning = false;
        bleedStacks = 0;
        bleedStacksCounterRunning = false;
        curseStacks = 0;
        curseStacksCounterRunning = false;
        blockingCoroutineRunning = false;
        //locatingTargetRunning = false;
        stunned = false;
        frostbitten = false;
        airborne = false;
        bleeding = false;
        poisoned = false;
        cursed = false;
        burning = false;
        regenerating = false;
        chilled = false;
        darkSigil = false;
        timeBombed = false;
        immobilized = false;
        earthed = false;
        lit = false;
        cannotBeFrostbitten = false;
        chanelling = false;
        blocking = false;
        parrying = false;
        blessed = false;
        blessRate = 1f;
        if(stunParticles && stunParticles.activeInHierarchy) {
            stunParticles.SetActive(false);
        }
    }
    private IEnumerator EnergyCounter() {
        for(; ; ) {
            yield return energyTimeWait;
            DecreaseEnergy(1f);
            CheckEnergy();
        }
    }
    private void CheckEnergy() {
        if(!player) {
            return;
        }
        if(stats.energy >= 20f) {
            lowEnergyParticles.SetActive(false);
            return;
        }
        if(stats.energy < 20f) {
            StartChill(energyTime, hungerSlowRate);
            lowEnergyParticles.SetActive(true);
            AudioSystem.audioManager.PlaySound("hungry", 0f);
        }
        if(stats.energy < 1f) {
            TakeDamage(10f, AttackType.Bleed, null, null);
        }
    }
    public void Heal(float amount) {
        stats.health += Mathf.Clamp(amount, 0, stats.trueMaxHealth - stats.health);
        //if(player && player.playerStatus) {
        //    player.playerStatus.OnStatusChange(PlayerStatus.Health);
        //}
        if(statusUI) {
            statusUI.UpdateHealth(stats.health / stats.trueMaxHealth, stats.health);
            statusUI.UpdateEnergy(stats.energy / stats.trueMaxEnergy, stats.energy);
        }
        InstantiateParticles("HealedParticles");
        AudioSystem.audioManager.PlaySound("heal", DistanceToPlayer);
    }    
    public void UseMana(float amount) {
        float finalAmount = Mathf.Clamp(amount, 0, stats.trueMaxMana);
        stats.mana -= finalAmount;
        //if(player) {
        //    player.PlayerConsumeMana(amount);
        //}
        // Satian healing
        if(finalAmount > 0f && player && player.raceIndex == 1) {
            const float satianHealRate = 0.2f;
            Heal(finalAmount * satianHealRate);
        }
        if(statusUI) {
            statusUI.UpdateMana(stats.mana / stats.trueMaxMana, stats.mana);
        }
    }
    public void GiveMana(float amount) {
        stats.mana += Mathf.Clamp(amount, 0, stats.trueMaxMana - stats.mana);
        if(statusUI) {
            statusUI.UpdateMana(stats.mana / stats.trueMaxMana, stats.mana);
        }
        InstantiateParticles("GainedManaParticles");
        AudioSystem.audioManager.PlaySound("manaGain", DistanceToPlayer);
    }
    public bool CanUseMana(float amount) {
        return stats.mana >= amount;
    }
    public void DecreaseEnergy(float amount) {
        stats.energy -= Mathf.Clamp(amount, 0, stats.trueMaxEnergy);
        if(statusUI) {
            statusUI.UpdateEnergy(stats.energy / stats.trueMaxEnergy, stats.energy);
        }
    }
    public void GiveEnergy(float amount) {
        stats.energy += Mathf.Clamp(amount, 0, stats.trueMaxEnergy - stats.energy);
        if(statusUI) {
            statusUI.UpdateEnergy(stats.energy / stats.trueMaxEnergy, stats.energy);
        }
        CheckEnergy();
        InstantiateParticles("GainedEnergyParticles");
        AudioSystem.audioManager.PlaySound("energyGain", DistanceToPlayer);
    }
    private void TakeMushroomDamage(float amount) {
        float damage = 0f;
        AttackType attackType = AttackType.Poison;
        if(!stats.living) {
            return;
        }
        if(stats) {
            damage = CalculateDamage.Calculate(amount, attackType, null, stats);
        }
        InstantiateDamageTakenParticles("PoisonDamageTaken");
        damage = Mathf.Clamp(damage, 0f, stats.maxDamageTimesHealth * stats.trueMaxHealth);
        stats.health -= damage;
        GenerateBloodParticles();
        if(statusUI) {
            statusUI.UpdateHealth(stats.health / stats.trueMaxHealth, stats.health);
        }
        if(hitWarning) {
            hitWarning.Hit();
        }
        if(stats.health <= 0f) {
            Die(false);
        }
    }
    public void TakeDamage(float amount, AttackType attackType, Skill skill, StatusEffects attacker) {
        float damage = 0f;
        if(!stats.living) {
            return;
        }
        if(stats && attacker) {
            damage = CalculateDamage.Calculate(amount, attackType, attacker.stats, stats);
        }
        else if(stats) {
            damage = amount;
        }        
        if(passives) {
            damage = passives.OnHitTaken(damage, attackType, stats);
        }
        if(parrying && attacker && skill && skill.parryable) {
            damage = 0f;
            float defaultStunDuration = stats.vitality * 0.05f + defaultParryStunDuration;
            attacker.StartStun(!player && globalPlayer.npcBonuses[24] ? 1.5f * defaultStunDuration : defaultStunDuration);
            AudioSystem.audioManager.PlaySound("parry", DistanceToPlayer);
        }
        else if(blocking && attacker && skill && skill.parryable) {
            damage *= 1f - stats.damageBlockRate;
            AudioSystem.audioManager.PlaySound("block", DistanceToPlayer);
        }
        else {
            // If this target is dark sigiled, add sigil charges
            if(darkSigilRunning && attackType == AttackType.Dark) {
                AddDarkSigilCharge(damage);
            }
            // Activate frostbite damage and end frostbite counter
            if(frostbitten && attackType == AttackType.Ice) {
                damage += GetFrostbiteDamage();
                StopFrostbite();
                SetFrostbiteDamage(0f);
            }
            // If this gameObject is earthed, it takes half damage from lightning attacks
            if(earthed && attackType == AttackType.Lightning) {
                damage *= lightningDamageForEarthedMultiplier;
            }
            // Get more damage from lightning when chilled
            if(chilled && attackType == AttackType.Lightning) {
                damage *= lightningDamageForChilledMultiplier;
            }
            // If the target is enlightened, the next light attack will ignore all armor
            if(lit && attackType == AttackType.Light) {
                StopEnlighten();
            }
            // Fire attacks end the chill effect
            if(chilled && attackType == AttackType.Fire) {
                StopChill();
            }
            // Ice attacks end burning effect
            if(burning && attackType == AttackType.Ice) {
                StopBurn();
            }
        }
        switch(attackType) {
            case AttackType.Slash:
                InstantiateDamageTakenParticles("SlashDamageTaken");
                AudioSystem.audioManager.PlaySound("slashDamage", DistanceToPlayer);
                break;
            case AttackType.Bash:
                InstantiateDamageTakenParticles("BashDamageTaken");
                AudioSystem.audioManager.PlaySound("bashDamage", DistanceToPlayer);
                break;
            case AttackType.Pierce:
                InstantiateDamageTakenParticles("PierceDamageTaken");
                AudioSystem.audioManager.PlaySound("pierceDamage", DistanceToPlayer);
                break;
            case AttackType.Fire:
                InstantiateDamageTakenParticles("FireDamageTaken");
                AudioSystem.audioManager.PlaySound("fireDamage", DistanceToPlayer);
                break;
            case AttackType.Ice:
                InstantiateDamageTakenParticles("IceDamageTaken");
                AudioSystem.audioManager.PlaySound("waterDamage", DistanceToPlayer);
                break;
            case AttackType.Air:
                InstantiateDamageTakenParticles("AirDamageTaken");
                AudioSystem.audioManager.PlaySound("airDamage", DistanceToPlayer);
                break;
            case AttackType.Earth:
                InstantiateDamageTakenParticles("EarthDamageTaken");
                AudioSystem.audioManager.PlaySound("earthDamage", DistanceToPlayer);
                break;
            case AttackType.Lightning:
                InstantiateDamageTakenParticles("LightningDamageTaken");
                AudioSystem.audioManager.PlaySound("lightningDamage", DistanceToPlayer);
                break;
            case AttackType.Light:
                InstantiateDamageTakenParticles("LightDamageTaken");
                AudioSystem.audioManager.PlaySound("lightDamage", DistanceToPlayer);
                break;
            case AttackType.Dark:
                InstantiateDamageTakenParticles("DarkDamageTaken");
                AudioSystem.audioManager.PlaySound("darkDamage", DistanceToPlayer);
                break;
            case AttackType.Poison:
                InstantiateDamageTakenParticles("PoisonDamageTaken");
                AudioSystem.audioManager.PlaySound("poisonDamage", DistanceToPlayer);
                break;
            case AttackType.Bleed:
                InstantiateDamageTakenParticles("BloodDamageTaken");
                AudioSystem.audioManager.PlaySound("bleedDamage", DistanceToPlayer);
                break;
            case AttackType.Curse:
                InstantiateDamageTakenParticles("CurseDamageTaken");
                AudioSystem.audioManager.PlaySound("curseDamage", DistanceToPlayer);
                break;
            default:
                break;
        }
        if(attacker.player && globalPlayer.npcBonuses[25]) {
            attacker.GiveMana(0.5f);
        }
        if(attacker.player && globalPlayer.npcBonuses[31] && skill is AreaSkill) {
            damage *= 1.15f;
        }
        if(attacker.player && globalPlayer.npcBonuses[32] && skill is ProjectileSkill) {
            damage *= 1.2f;
        }
        if(attacker.player && globalPlayer.npcBonuses[37] && skill is ProjectileSkill) {
            damage *= 0.5f;
        }
        if(attacker.player && globalPlayer.npcBonuses[37] && skill is AreaSkill) {
            damage *= 1.33f;
        }
        if(attacker.player && globalPlayer.npcBonuses[38] && skill is ProjectileSkill) {
            damage *= 1.4f;
        }
        if(attacker.player && globalPlayer.npcBonuses[38] && skill is AreaSkill) {
            damage *= 0.33f;
        }
        if(attacker.player && globalPlayer.npcBonuses[39]) {
            float extraDamage = 1f;
            // Increases extra damage by up to 0.1f depending on how far the player is
            extraDamage += Mathf.Clamp(Vector3.Distance(attacker.transform.position, transform.position) / (maximumDamageDistance * 5f), 0f, 0.2f);
            damage *= extraDamage;
        }
        if(attacker.player && globalPlayer.npcBonuses[40]) {
            float extraDamage = 1f;
            // Increases extra damage by up to 0.1f depending on how far the player is
            if(Vector3.Distance(attacker.transform.position, transform.position) < 3f) {
                extraDamage += 0.2f;
            }
            damage *= extraDamage;
        }
        bool backstabbedByNastac = false;
        if(RelativePosition.GetRelativePosition(attacker.transform, transform) == Relative.Behind) {
            if(attacker.player && attacker.player.raceIndex == 10) {
                backstabbedByNastac = true;
            }
            AudioSystem.audioManager.PlaySound("backstab", DistanceToPlayer);
            damage *= stats.backstabMultiplier;
        }
        // Take damage
        damage = Mathf.Clamp(damage, 0f, stats.maxDamageTimesHealth * stats.trueMaxHealth);
        stats.health -= damage;
        GenerateBloodParticles();
        if(enemyAI && enemyAI.willDefendItself) {
            if(!stunned && !chanelling && !immobilized && !enemyAI.attacked) {
                enemyAI.transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, attacker.transform.position - transform.position, Vector3.forward));
            }
            enemyAI.hostile = true;
            enemyAI.attacked = true;
        }        
        if(hitWarning) {
            hitWarning.Hit();
        }
        if(stats.health <= 0f) {
            if(player && player.npcBonuses[28] && attackType == AttackType.Poison) {
                stats.health = 1f;
            }
            else if(player && player.npcBonuses[29] && attackType == AttackType.Bleed) {
                stats.health = 1f;
            }
            else if(player && player.npcBonuses[30] && attackType == AttackType.Curse) {
                stats.health = 1f;
            }
            else {
                Die(backstabbedByNastac);
            }
        }        
        // Havellian clarity
        if(player && player.raceIndex == 11) {
            const float havellianManaRate = 0.2f;
            GiveMana(damage * havellianManaRate);
        }
        if(statusUI) {
            statusUI.UpdateHealth(stats.health / stats.trueMaxHealth, stats.health);
        }
        //statusParticles.StartHitParticles();
        /*if(enemy && enemy.gameObject.activeInHierarchy) {
            if(!locatingTargetRunning) {
                locatingTarget = StartCoroutine(LocateTargetWhenHit());
            }
        }*/
        //if(player && player.playerStatus) {
        //    player.playerStatus.OnStatusChange(PlayerStatus.Health);
        //}
        //if(bar) {
        //   bar.UpdateStatus();
        //}
        /*if(enemy && stats.health <= 0f) {
            stats.health = 0;
            enemy.EnemyDie();
        }*/
    }
    private void GenerateBloodParticles() {
        GameObject blood = ObjectPooler.objectPooler.GetPooledObject("BloodTrails");
        blood.transform.position = transform.position;
        blood.transform.rotation = Quaternion.identity;
        blood.SetActive(true);
    }
    private void Die(bool backstabbedByNastac) {
        if(player && deathPanel) {
            deathPanel.gameObject.SetActive(true);
            deathPanel.DeathPanelInit();
        }
        if(!player && globalPlayer.npcBonuses[27]) {
            globalPlayer.stats.status.GiveMana(10f);
        }
        if(enemyAI) {
            Spawner.spawner.RemoveEntity(gameObject);
        }
        AudioSystem.audioManager.PlaySound("death", DistanceToPlayer);
        stats.Die(backstabbedByNastac);
    }
    private void InstantiateDamageTakenParticles(string name) {
        GameObject damageTakenParticles = ObjectPooler.objectPooler.GetPooledObject(name);
        damageTakenParticles.transform.position = transform.position;
        damageTakenParticles.transform.rotation = Quaternion.identity;
        damageTakenParticles.SetActive(true);
    }
    private void InstantiateParticles(string name) {
        GameObject part = ObjectPooler.objectPooler.GetPooledObject(name);
        part.transform.position = transform.position;
        part.transform.rotation = Quaternion.identity;
        // part.transform.parent = transform;
        part.transform.SetParent(transform);
        part.SetActive(true);
    }
    /*
    private IEnumerator LocateTargetWhenHit() {
        locatingTargetRunning = true;
        if(FindObjectOfType<Player>()) {
            Debug.Log("Locating player");
            enemy.enemyDetection.aiPath.destination = FindObjectOfType<Player>().transform.position;
            enemy.enemyDetection.enemyFound = true;
            enemy.vision.recentlyHit = true;
            enemy.enemyDetection.CheckFollowStatus();
        }
        yield return new WaitForSeconds(2f);
        locatingTargetRunning = false;
        enemy.enemyDetection.enemyFound = false;
        enemy.vision.recentlyHit = false;
        enemy.enemyDetection.CheckFollowStatus();
    }*/
    private void InterruptEnemyAI() {
        if(aiPath) {
            aiPath.canMove = false;
            aiPath.destination = aiPath.transform.position;
        }
    }
    private void CommenceEnemyAI() {
        if(aiPath) {
            aiPath.canMove = true;
        }
    }
    // Bless provides damage *= extraDamageRate extra damage
    public void StartBless(float extraDamageRate, float duration) {
        if(!blessParticles) {
            return;
        }
        StopCoroutine(Bless(extraDamageRate, duration));
        StartCoroutine(Bless(extraDamageRate, player && player.npcBonuses[42] ? duration * 1.5f : duration));
    }
    private IEnumerator Bless(float extraDamageRate, float duration) {
        blessRate = extraDamageRate;
        blessParticles.GetComponent<Particles>().duration = duration;
        blessParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("blessed", DistanceToPlayer);
        yield return new WaitForSeconds(duration);
        blessRate = 1f;
        if(blessParticles) {
            blessParticles.SetActive(false);
        }
    }
    public void StartChanelling(float duration) {
        if(gameObject.activeInHierarchy) {
            chanellingCounter = StartCoroutine(Chanelling(duration));
        }
    }
    public void StopChanelling() {
        if(chanelling) {
            StopCoroutine(chanellingCounter);
        }
        chanelling = false;
    }
    private IEnumerator Chanelling(float duration) {
        chanelling = true;
        yield return new WaitForSeconds(duration);
        chanelling = false;
    }
    // Add lightningStacks
    public void AddLightningStacks(int amount, float damage, float duration, Skill skill, StatusEffects attacker) {
        lightningStacks += amount;
        CheckLightningStacks(damage, duration, skill, attacker);
    }
    // Check lightningStacks
    public void CheckLightningStacks(float damage, float duration, Skill skill, StatusEffects attacker) {
        if(lightningStacks >= stats.shockThreshold) {
            lightningStacks = 0;
            StartStun(duration);
            // Particles
            shockedParticles = ObjectPooler.objectPooler.GetPooledObject("ShockedParticles");
            shockedParticles.transform.position = transform.position;
            shockedParticles.transform.rotation = Quaternion.identity;
            shockedParticles.transform.SetParent(transform);
            shockedParticles.GetComponent<Particles>().duration = duration;
            shockedParticles.SetActive(true);
            // Sound
            AudioSystem.audioManager.PlaySound("shocked", DistanceToPlayer);
            //statusParticles.StartShockedParticles(duration);
            TakeDamage(damage, AttackType.Lightning, skill, attacker);
        }
        else if(lightningStacks > 0) {
            if(!lightningStacksCounterRunning && gameObject.activeInHierarchy) {
                lightningStacksCounter = StartCoroutine(DecreaseLightningStacks());
            }
        }
    }
    // Gradually decrease lightningStack count over time
    private IEnumerator DecreaseLightningStacks() {
        lightningStacks--;
        if(!lightningStacksCounterRunning) {
            lightningStacksCounterRunning = true;
        }
        yield return waitASec;
        if(lightningStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseLightningStacks());
        }
        else {
            lightningStacksCounterRunning = false;
        }
    }
    // Add lightningStacks
    public void AddIceStacks(int amount, float duration, float damageAmplifier) {
        iceStacks += amount;
        CheckIceStacks(duration, damageAmplifier);
    }
    // Check lightningStacks
    public void CheckIceStacks(float duration, float damageAmplifier) {
        if(iceStacks >= stats.frostbiteThreshold) {
            iceStacks = 0;            
            StartFrostbite(duration, damageAmplifier);
        }
        else if(iceStacks > 0) {
            if(!iceStacksCounterRunning && gameObject.activeInHierarchy) {
                iceStacksCounter = StartCoroutine(DecreaseIceStacks());
            }
        }
    }
    // Gradually decrease lightningStack count over time
    private IEnumerator DecreaseIceStacks() {
        iceStacks--;
        if(!iceStacksCounterRunning) {
            iceStacksCounterRunning = true;
        }
        yield return waitASec;
        if(iceStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseIceStacks());
        }
        else {
            iceStacksCounterRunning = false;
        }
    }
    // Start regeneration coroutine
    public void StartRegen(float duration, float healAmountPerTick, bool mushroom) {
        if(gameObject.activeInHierarchy) {
            if(regenerating) {
                StopRegen();
            }
            regenCounter = StartCoroutine(Regeneration(duration, healAmountPerTick, mushroom));
        }
    }
    // Stop regeneration
    public void StopRegen() {
        StopCoroutine(regenCounter);
        regenerating = false;
    }
    // Damage over time coroutines
    public IEnumerator Regeneration(float duration, float healAmountPerTick, bool mushroom) {
        float period = 1f;
        bool done = false;
        regenerating = true;
        while(!done) {
            yield return new WaitForSeconds(period);
            Heal(healAmountPerTick);
            if(!player && globalPlayer.npcBonuses[23] && mushroom) {
                GiveMana(healAmountPerTick);
            }
            duration -= period;
            if(duration <= 0) {
                done = true;
            }
        }
        regenerating = false;
    }
    public void AddFireStacks(int amount, float damage, float duration, Skill skill, StatusEffects attacker) {
        fireStacks += amount;
        CheckFireStacks(duration, damage, skill, attacker);
    }
    // Check earthStacks
    public void CheckFireStacks(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(fireStacks >= stats.burningThreshold) {
            fireStacks = 0;            
            StartBurn(duration, damage, skill, attacker);
            AudioSystem.audioManager.PlaySound("burning", DistanceToPlayer);
        }
        else if(fireStacks > 0) {
            if(!fireStacksCounterRunning && gameObject.activeInHierarchy) {
                fireStacksCounter = StartCoroutine(DecreaseFireStacks());
            }
        }
    }
    // Gradually decrease earthStacks count over time
    private IEnumerator DecreaseFireStacks() {
        fireStacks--;
        if(!fireStacksCounterRunning) {
            fireStacksCounterRunning = true;
        }
        yield return waitASec;
        if(fireStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseFireStacks());
        }
        else {
            fireStacksCounterRunning = false;
        }
    }
    private void StartBurn(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(burning) {
                StopBurn();
            }
            //statusParticles.StartBurningParticles(duration);
            burnCounter = StartCoroutine(Burn(duration, damage, skill, attacker));
        }
    }
    public void StopBurn() {
        if(burnCounter != null) {
            //statusParticles.StopBurningParticles();
            StopCoroutine(burnCounter);
            burning = false;
            if(burningParticles) {
                burningParticles.SetActive(false);
            }
        }
    }
    public IEnumerator Burn(float duration, float damage, Skill skill, StatusEffects attacker) {
        float period = 2f;
        bool done = false;
        burning = true;
        burningParticles = ObjectPooler.objectPooler.GetPooledObject("BurningParticles");
        burningParticles.transform.position = transform.position;
        burningParticles.transform.rotation = Quaternion.identity;
        burningParticles.transform.SetParent(transform);
        burningParticles.GetComponent<Particles>().duration = duration;
        burningParticles.SetActive(true);
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage, AttackType.Fire, skill, attacker);
            duration -= period;
            if(duration <= 0f) {
                done = true;
            }
        }
        //statusParticles.StopBurningParticles();
        burning = false;
    }
    public void AddPoisonStacks(int amount) {
        poisonStacks += amount;
        CheckPoisonStacks(amount, amount, null, null);
    }
    public void AddPoisonStacks(int amount, float damage, float duration, Skill skill, StatusEffects attacker) {
        poisonStacks += amount;
        CheckPoisonStacks(duration, damage, skill, attacker);
    }
    // Check stacks
    public void CheckPoisonStacks(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(poisonStacks >= stats.poisonThreshold) {
            poisonStacks = 0;
            float poisonDamageInterval = 1f;
            // Decrease poison damage interval by 1 second per tick
            StartPoison(duration, damage, poisonDamageInterval, skill, attacker);
        }
        else if(poisonStacks > 0) {
            if(!poisonStacksCounterRunning && gameObject.activeInHierarchy) {
                poisonStacksCounter = StartCoroutine(DecreasePoisonStacks());
            }
        }
    }
    // Gradually decrease stack count over time
    private IEnumerator DecreasePoisonStacks() {
        poisonStacks--;
        if(!poisonStacksCounterRunning) {
            poisonStacksCounterRunning = true;
        }
        yield return waitASec;
        if(poisonStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreasePoisonStacks());
        }
        else {
            poisonStacksCounterRunning = false;
        }
    }
    /*
    public void StartPoison(float mushroomPower) {
        if(gameObject.activeInHierarchy) {
            if(poisoned) {
                StopPoison();
            }
            poisonCounter = StartCoroutine(Poison(mushroomPower, mushroomPower, 1f, null, null));
        }
    }*/
    private void StartPoison(float duration, float damage, float periodTimeDecrease, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(poisoned) {
                StopPoison();
            }
            poisonCounter = StartCoroutine(Poison(duration, damage, periodTimeDecrease, skill, attacker));
        }
    }
    public void StopPoison() {
        if(poisonCounter != null) {
            StopCoroutine(poisonCounter);
            poisoned = false;
            if(poisonedParticles) {
                poisonedParticles.SetActive(false);
            }
        }        
    }
    private IEnumerator Poison(float duration, float damage, float periodTimeDecrease, Skill skill, StatusEffects attacker) {
        float period = 1f;
        bool done = false;
        poisoned = true;
        poisonedParticles = ObjectPooler.objectPooler.GetPooledObject("PoisonedParticles");
        poisonedParticles.transform.position = transform.position;
        poisonedParticles.transform.rotation = Quaternion.identity;
        poisonedParticles.transform.SetParent(transform);
        poisonedParticles.GetComponent<Particles>().duration = duration;
        poisonedParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("poisoned", DistanceToPlayer);
        while(!done) {
            yield return new WaitForSeconds(period);
            if(skill && attacker) {
                TakeDamage(damage / duration, AttackType.Poison, skill, attacker);
            }
            else {
                TakeMushroomDamage(damage);
            }
            duration -= period;
            period = period <= 1f ? 1f : period -= periodTimeDecrease;
            if(duration <= 0) {
                done = true;
            }
        }
        poisoned = false;
    }
    public void AddBleedStacks(int amount, float damage, float duration, Skill skill, StatusEffects attacker) {
        bleedStacks += amount;
        CheckBleedStacks(duration, damage, skill, attacker);
    }
    // Check stacks
    public void CheckBleedStacks(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(bleedStacks >= stats.bleedThreshold) {
            bleedStacks = 0;
            float bleedDamageIncrease = damage * 0.2f;
            // Increase bleed damage by damage / 5 per tick
            StartBleed(duration, damage, bleedDamageIncrease, skill, attacker);
        }
        else if(bleedStacks > 0) {
            if(!bleedStacksCounterRunning && gameObject.activeInHierarchy) {
                bleedStacksCounter = StartCoroutine(DecreaseBleedStacks());
            }
        }
    }
    // Gradually decrease stack count over time
    private IEnumerator DecreaseBleedStacks() {
        bleedStacks--;
        if(!bleedStacksCounterRunning) {
            bleedStacksCounterRunning = true;
        }
        yield return waitASec;
        if(bleedStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseBleedStacks());
        }
        else {
            bleedStacksCounterRunning = false;
        }
    }
    private void StartBleed(float duration, float damage, float damageIncrease, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(bleeding) {
                StopBleed();
            }
            bleedCounter = StartCoroutine(Bleed(duration, damage, damageIncrease, skill, attacker));
        }
    }
    public void StopBleed() {
        if(bleedCounter != null) {
            StopCoroutine(bleedCounter);
            bleeding = false;
            if(bleedingParticles) {
                bleedingParticles.SetActive(false);
            }
        }        
    }
    public IEnumerator Bleed(float duration, float damage, float damageIncrease, Skill skill, StatusEffects attacker) {
        float period = 0.5f;
        bool done = false;
        bleeding = true;
        bleedingParticles = ObjectPooler.objectPooler.GetPooledObject("BleedingParticles");
        bleedingParticles.transform.position = transform.position;
        bleedingParticles.transform.rotation = Quaternion.identity;
        bleedingParticles.transform.SetParent(transform);
        bleedingParticles.GetComponent<Particles>().duration = duration;
        bleedingParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("bleeding", DistanceToPlayer);
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage / duration, AttackType.Bleed, skill, attacker);
            duration -= period;
            damage += damageIncrease;
            if(duration <= 0f) {
                done = true;
            }
        }
        bleeding = false;
    }
    public void AddCurseStacks(int amount, float damage, float duration, Skill skill, StatusEffects attacker) {
        curseStacks += amount;
        CheckCurseStacks(duration, damage, skill, attacker);
    }
    // Check stacks
    public void CheckCurseStacks(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(curseStacks >= stats.curseThreshold) {
            curseStacks = 0;
            // Increase bleed damage by damage / 4 per tick
            StartCurse(duration, damage, skill, attacker);
        }
        else if(curseStacks > 0) {
            if(!curseStacksCounterRunning && gameObject.activeInHierarchy) {
                curseStacksCounter = StartCoroutine(DecreaseCurseStacks());
            }
        }
    }
    // Gradually decrease stack count over time
    private IEnumerator DecreaseCurseStacks() {
        curseStacks--;
        if(!curseStacksCounterRunning) {
            curseStacksCounterRunning = true;
        }
        yield return waitASec;
        if(curseStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseCurseStacks());
        }
        else {
            curseStacksCounterRunning = false;
        }
    }
    public void StartCurse(float duration, float damage, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(cursed) {
                StopCurse();
            }
            curseCounter = StartCoroutine(Curse(duration, damage, skill, attacker));
        }
    }
    public void StopCurse() {
        if(curseCounter != null) {
            StopCoroutine(curseCounter);
            cursed = false;
            if(cursedParticles) {
                cursedParticles.SetActive(false);
            }
        }        
    }
    public IEnumerator Curse(float duration, float damage, Skill skill, StatusEffects attacker) {
        float period = 2f;
        bool done = false;
        cursed = true;
        cursedParticles = ObjectPooler.objectPooler.GetPooledObject("CursedParticles");
        cursedParticles.transform.position = transform.position;
        cursedParticles.transform.rotation = Quaternion.identity;
        cursedParticles.transform.SetParent(transform);
        cursedParticles.GetComponent<Particles>().duration = duration;
        cursedParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("cursed", DistanceToPlayer);
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage / duration, AttackType.Curse, skill, attacker);
            duration -= period;
            if(duration <= 0f) {
                done = true;
            }
        }
        cursed = false;
    }
    public void StartStun(float duration) {
        if(gameObject.activeInHierarchy) {
            if(stunned) {
                StopStun();
            }
            stunCounter = StartCoroutine(Stun(player && player.npcBonuses[33] ? duration * 0.75f : duration));
        }
    }
    public void StopStun() {
        stunned = false;
        if(stunParticles) {
            stunParticles.SetActive(false);
        }
        StopCoroutine(stunCounter);
        CommenceEnemyAI();
    }
    // Makes target unable to attack and move for a couple of seconds
    private IEnumerator Stun(float duration) {
        // Stun here
        stunned = true;
        stunParticles = ObjectPooler.objectPooler.GetPooledObject("StunParticles");
        stunParticles.GetComponent<Particles>().duration = duration;
        stunParticles.transform.parent = transform;
        stunParticles.transform.position = transform.position;
        stunParticles.transform.rotation = transform.rotation;
        stunParticles.SetActive(true);
        InterruptEnemyAI();
        AudioSystem.audioManager.PlaySound("stunned", DistanceToPlayer);
        /*
        if(enemy)
            ToggleCrowdControl();*/
        yield return new WaitForSeconds(duration);
        // Return to normal here
        stunned = false;
        CommenceEnemyAI();
        /*if(enemy)
            ToggleCrowdControl();*/
    }
    public void StartImmobilize(float duration) {
        if(gameObject.activeInHierarchy) {
            if(immobilized) {
                StopImmobilize();
            }
            immobilizeCounter = StartCoroutine(Immobilize(duration));
        }
    }
    public void StopImmobilize() {
        StopCoroutine(immobilizeCounter);
        immobilized = false;
        CommenceEnemyAI();
    }
    private IEnumerator Immobilize(float duration) {
        immobilized = true;
        InterruptEnemyAI();
        /*if(enemy)
            ToggleCrowdControl();*/
        yield return new WaitForSeconds(duration);
        immobilized = false;
        CommenceEnemyAI();
        /*if(enemy)
            ToggleCrowdControl();*/
    }
    public void StartFrostbite(float duration, float damageAmplifier) {
        if(gameObject.activeInHierarchy) {
            if(!cannotBeFrostbitten) {
                if(frostbitten) {
                    StopFrostbite();
                }
                frostbiteCounter = StartCoroutine(Frostbite(duration, damageAmplifier));
            }
        }
    }
    public void StopFrostbite() {
        if(frostbiteCounter != null) {
            frostbitten = false;
            StopCoroutine(frostbiteCounter);
            if(frostbittenParticles) {
                frostbittenParticles.SetActive(false);
            }
        }        
    }
    // Frostbite effect stops burn
    private IEnumerator Frostbite(float duration, float amplifiedDamage) {
        frostbitten = true;
        if(burning) {
            StopBurn();
        }
        // Particles
        frostbittenParticles = ObjectPooler.objectPooler.GetPooledObject("FrostbiteParticles");
        frostbittenParticles.transform.position = transform.position;
        frostbittenParticles.transform.rotation = Quaternion.identity;
        frostbittenParticles.transform.SetParent(transform);
        frostbittenParticles.GetComponent<Particles>().duration = duration;
        frostbittenParticles.SetActive(true);
        /*if(enemy) {
            ToggleCrowdControl();
        }*/
        AudioSystem.audioManager.PlaySound("frostbitten", DistanceToPlayer);
        StartStun(duration);
        SetFrostbiteDamage(amplifiedDamage);
        StartCoroutine(FrostbiteCooldown(frostbiteCooldown));
        yield return new WaitForSeconds(duration);
        frostbitten = false;
        /*if(enemy) {
            ToggleCrowdControl();
        }*/
    }
    private IEnumerator FrostbiteCooldown(float duration) {
        cannotBeFrostbitten = true;
        yield return new WaitForSeconds(duration);
        cannotBeFrostbitten = false;
    }
    private float GetFrostbiteDamage() {
        return frostbiteDamage;
    }
    private void SetFrostbiteDamage(float damage) {
        frostbiteDamage = damage;
    }
    public void StartChill(float duration, float slowRate) {
        // Helgafelli vigor
        if(player.raceIndex == 5) {
            return;
        }
        if(gameObject.activeInHierarchy) {
            if(chilled) {
                StopChill();
            }
            chillCounter = StartCoroutine(Chill(duration, slowRate));
        }
    }
    public void StopChill() {
        if(chillCounter != null) {
            chilled = false;
            stats.runSpeed = originalSpeed;
            StopCoroutine(chillCounter);
            if(aiPath) {
                aiPath.maxSpeed = aiPath.GetComponent<EnemyAI>().speed;
            }
            if(chilledParticles) {
                chilledParticles.SetActive(false);
            }
        }        
    }
    // Chill effect stops burn
    private IEnumerator Chill(float duration, float slowRate) {
        chilled = true;
        if(spedUp) {
            StopSpeedUp();
        }
        if(burning) {
            StopBurn();
        }
        stats.runSpeed *= 1f - slowRate;
        if(aiPath) {
            aiPath.maxSpeed *= 1f - slowRate;
        }
        chilledParticles = ObjectPooler.objectPooler.GetPooledObject("ChilledParticles");
        chilledParticles.transform.position = transform.position;
        chilledParticles.transform.rotation = Quaternion.identity;
        chilledParticles.transform.SetParent(transform);
        chilledParticles.GetComponent<Particles>().duration = duration;
        chilledParticles.SetActive(true);
        /*
        if(enemy)
            enemy.enemyDetection.aiPath.maxSpeed *= 1 - slowRate;
        else if(player)
            GetComponent<PlayerMovement>().walkSpeed *= 1 - slowRate;*/
        yield return new WaitForSeconds(duration);
        chilled = false;
        stats.runSpeed = originalSpeed;
        if(aiPath) {
            aiPath.maxSpeed = aiPath.GetComponent<EnemyAI>().speed;
        }
        /*
        if(enemy)
            enemy.enemyDetection.aiPath.maxSpeed = originalSpeed;
        else if(player)
            GetComponent<PlayerMovement>().walkSpeed = originalSpeed;*/
    }
    public void StartDarkSigil(float duration, float damageRate, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(!darkSigilRunning && !cannotBeDarkSigiled) {
                StartCoroutine(DarkSigilCooldown(darkSigilCooldown));
                darkSigilCounter = StartCoroutine(DarkSigil(duration, damageRate, skill, attacker));
            }
        }
    }
    public void EndDarkSigil() {
        if(darkSigilCounter != null) {
            darkSigilRunning = false;
            darkSigilCharge = 0;
            StopCoroutine(darkSigilCounter);
            if(darkSigilParticles) {
                darkSigilParticles.SetActive(false);
            }
        }        
    }
    // Dark sigil deals dark damage when time ends depending on the target's 
    // Dark damage taken during DarkSigil effect
    private IEnumerator DarkSigil(float duration, float damageRate, Skill skill, StatusEffects attacker) {
        darkSigil = true;
        if(!darkSigilRunning) {
            darkSigilRunning = true;
        }
        darkSigilParticles = ObjectPooler.objectPooler.GetPooledObject("DarkSigilParticles");
        darkSigilParticles.transform.position = transform.position;
        darkSigilParticles.transform.rotation = Quaternion.identity;
        darkSigilParticles.transform.SetParent(transform);
        darkSigilParticles.GetComponent<Particles>().duration = duration;
        darkSigilParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("darkSigilActivate", DistanceToPlayer);
        yield return new WaitForSeconds(duration);
        TakeDamage(darkSigilCharge * damageRate, AttackType.Dark, skill, attacker);
        AudioSystem.audioManager.PlaySound("darkSigilHit", DistanceToPlayer);
        darkSigilCharge = 0;
        darkSigil = false;
        darkSigilRunning = false;
    }
    // Count damage taken during dark sigil effect
    private void AddDarkSigilCharge(float amount) {
        darkSigilCharge += amount;
    }
    // Track dark sigilability
    private IEnumerator DarkSigilCooldown(float cooldown) {
        cannotBeDarkSigiled = true;
        yield return new WaitForSeconds(cooldown);
        cannotBeDarkSigiled = false;
    }
    public void StartAirPush(float pushDistance, Transform attackerPosition) {
        if(gameObject.activeInHierarchy)
            airborneCounter = StartCoroutine(AirPush(pushDistance, attackerPosition));
    }
    public void EndAirPush() {
        StopCoroutine(airborneCounter);
        immobilized = false;
    }
    private IEnumerator AirPush(float pushDistance, Transform attackerPosition) {
        /*
        float currentTime = 0;
        const float speedMultiplier = 4;
        Vector2 startPos = transform.position;
        Vector2 pushVector = new Vector2(transform.position.x - attackerPosition.position.x,
            transform.position.y - attackerPosition.position.y).normalized * pushDistance;
        Vector2 endPos = startPos + pushVector;
        immobilized = true;
        while(currentTime < pushDistance / speedMultiplier) {
            transform.position = Vector3.Lerp(startPos, endPos, currentTime / pushDistance * speedMultiplier);
            currentTime += Time.deltaTime;
            yield return null;
        }
        immobilized = false;*/
        float multiplier = 100f;
        immobilized = true;
        rb2d.AddForce((transform.position - attackerPosition.position).normalized * pushDistance * multiplier);
        yield return new WaitForSeconds(pushDistance * 0.5f);
        immobilized = false;
    }
    public void StartTimeBomb(float seconds, float damage, AttackType attackType, Skill skill, StatusEffects attacker) {
        if(gameObject.activeInHierarchy) {
            if(timeBombed) {
                EndTimeBomb();
            }
            timeBombCounter = StartCoroutine(TimeBomb(seconds, damage, attackType, skill, attacker));
        }
    }
    public void EndTimeBomb() {
        StopCoroutine(timeBombCounter);
        timeBombed = false;
    }
    public IEnumerator TimeBomb(float seconds, float damage, AttackType attackType, Skill skill, StatusEffects attacker) {
        timeBombed = true;
        yield return new WaitForSeconds(seconds);
        TakeDamage(damage, attackType, skill, attacker);
        timeBombed = false;
    }
    public void AddEarthStacks(int amount, float heal, float duration, GameObject damager) {
        earthStacks += amount;
        CheckEarthStacks(heal, duration, damager);
    }
    // Check earthStacks
    public void CheckEarthStacks(float heal, float duration, GameObject damager) {
        if(earthStacks >= stats.earthingThreshold) {
            earthStacks = 0;
            StartStun(duration);
            StartEarthen(duration);
            if(damager.GetComponent<Player>()) {
                damager.GetComponent<StatusEffects>().Heal(heal);
            }
        }
        else if(earthStacks > 0) {
            if(!earthStacksCounterRunning && gameObject.activeInHierarchy) {
                earthStacksCounter = StartCoroutine(DecreaseEarthStacks());
            }
        }
    }
    // Gradually decrease earthStacks count over time
    private IEnumerator DecreaseEarthStacks() {
        earthStacks--;
        if(!earthStacksCounterRunning) {
            earthStacksCounterRunning = true;
        }
        yield return waitASec;
        if(earthStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseEarthStacks());
        }
        else {
            earthStacksCounterRunning = false;
        }
    }
    public void StartEarthen(float duration) {
        if(gameObject.activeInHierarchy) {
            if(earthed) {
                StopEarthen();
            }
            earthenCounter = StartCoroutine(Earthen(duration));
        }
    }
    public void StopEarthen() {
        if(earthenCounter != null) {
            StopCoroutine(earthenCounter);
            earthed = false;
            if(rootParticles) {
                rootParticles.SetActive(false);
            }
        }        
    }
    private IEnumerator Earthen(float duration) {
        earthed = true;
        rootParticles = ObjectPooler.objectPooler.GetPooledObject("RootParticles");
        rootParticles.transform.position = transform.position;
        rootParticles.transform.rotation = Quaternion.identity;
        rootParticles.transform.SetParent(transform);
        rootParticles.GetComponent<Particles>().duration = duration;
        rootParticles.SetActive(true);
        AudioSystem.audioManager.PlaySound("rooted", DistanceToPlayer);
        yield return new WaitForSeconds(duration);
        earthed = false;
    }
    public void AddLightStacks(int amount, float duration) {
        lightStacks += amount;
        CheckLightStacks(duration);
    }
    // Check earthStacks
    public void CheckLightStacks(float duration) {
        if(lightStacks >= stats.lightingThreshold) {
            lightStacks = 0;
            StartEnlighten(duration);
            AudioSystem.audioManager.PlaySound("lit", DistanceToPlayer);
        }
        else if(lightStacks > 0) {
            if(!lightStacksCounterRunning && gameObject.activeInHierarchy) {
                lightCounter = StartCoroutine(DecreaseLightStacks());
            }
        }
    }
    // Gradually decrease earthStacks count over time
    private IEnumerator DecreaseLightStacks() {
        lightStacks--;
        if(!lightStacksCounterRunning) {
            lightStacksCounterRunning = true;
        }
        yield return waitASec;
        if(lightStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseLightStacks());
        }
        else {
            lightStacksCounterRunning = false;
        }
    }
    public void StartEnlighten(float duration) {
        if(gameObject.activeInHierarchy) {
            if(lit) {
                StopEnlighten();
            }
            lightCounter = StartCoroutine(Enlighten(duration));
        }
    }
    public void StopEnlighten() {
        if(lightCounter != null) {
            StopCoroutine(lightCounter);
            lit = false;
            if(litParticles) {
                litParticles.SetActive(false);
            }            
        }        
    }
    private IEnumerator Enlighten(float duration) {
        lit = true;
        litParticles = ObjectPooler.objectPooler.GetPooledObject("LitParticles");
        litParticles.transform.position = transform.position;
        litParticles.transform.rotation = Quaternion.identity;
        litParticles.transform.SetParent(transform);
        litParticles.GetComponent<Particles>().duration = duration;
        litParticles.SetActive(true);
        yield return new WaitForSeconds(duration);
        lit = false;
    }
    /// <summary>
    /// Increases targets speed by speedRate * currentSpeed.
    /// Speed rate example: 1.1f speedRate will increase your speed by %10
    /// </summary>
    /// <param name="speedRate"></param>
    /// <param name="duration"></param>
    public void StartSpeedUp(float speedRate, float duration) {
        if(gameObject.activeInHierarchy) {
            if(spedUp) {
                StopSpeedUp();
            }
            speedUpCounter = StartCoroutine(SpeedUp(speedRate, player && player.npcBonuses[43] ? duration * 1.5f : duration));
        }
    }
    public void StopSpeedUp() {
        if(speedUpCounter != null) {
            StopCoroutine(speedUpCounter);
            stats.runSpeed = originalSpeed;
            spedUp = false;
            if(spedUpParticles) {
                spedUpParticles.SetActive(false);
            }            
        }        
    }
    private IEnumerator SpeedUp(float speedRate, float duration) {
        if(chilled) {
            StopChill();
        }
        spedUpParticles = ObjectPooler.objectPooler.GetPooledObject("SpedUpParticles");
        spedUpParticles.transform.position = transform.position;
        spedUpParticles.transform.rotation = Quaternion.identity;
        spedUpParticles.transform.SetParent(transform);
        spedUpParticles.GetComponent<Particles>().duration = duration;
        spedUpParticles.SetActive(true);
        stats.runSpeed *= speedRate;
        spedUp = true;
        yield return new WaitForSeconds(duration);
        spedUp = false;
        stats.runSpeed = originalSpeed;
    }
    public void StartBlocking(float duration) {
        if(gameObject.activeInHierarchy) {
            if(blockingCoroutineRunning) {
                StopBlocking();
                StopCoroutine(blockingCoroutine);
            }
            blockingCoroutine = StartCoroutine(Blocking(duration));
        }
    }
    private IEnumerator Blocking(float duration) {
        blockingCoroutineRunning = true;
        blocking = true;
        defaultShieldTransform.position = forwardShieldTransform.position;
        defaultShieldTransform.rotation = forwardShieldTransform.rotation;
        yield return new WaitForSeconds(duration);
        defaultShieldTransform.localPosition = Vector3.zero;
        defaultShieldTransform.localRotation = Quaternion.identity;
        blocking = false;
        blockingCoroutineRunning = false;
    }
    private void StopBlocking() {
        defaultShieldTransform.localPosition = Vector3.zero;
        defaultShieldTransform.localRotation = Quaternion.identity;
        blocking = false;
        blockingCoroutineRunning = false;
    }
    public void StartParrying(float duration) {
        if(gameObject.activeInHierarchy) {
            blockingCoroutine = StartCoroutine(Parry(duration));
        }
    }
    private IEnumerator Parry(float duration) {
        parrying = true;
        defaultShieldTransform.position = parryTransform.position;
        defaultShieldTransform.rotation = parryTransform.rotation;
        yield return new WaitForSeconds(duration);
        defaultShieldTransform.localPosition = Vector3.zero;
        defaultShieldTransform.localRotation = Quaternion.identity;
        parrying = false;
    }
}
