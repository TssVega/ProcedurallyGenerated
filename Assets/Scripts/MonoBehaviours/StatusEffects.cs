using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class StatusEffects : MonoBehaviour {

    private Player player;
    //private Enemy enemy;
    private Stats stats;
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
    private bool locatingTargetRunning = false;
    // Constant values
    private const float lightningDamageForChilledMultiplier = 1.2f;
    private const float lightningDamageForEarthedMultiplier = 0.5f;
    private const int frostbiteCooldown = 16;
    private const int darkSigilCooldown = 15;

    private void OnEnable() {
        player = GetComponent<Player>();
        //enemy = GetComponent<Enemy>();
        stats = GetComponent<Stats>();
        //if(enemy) {
        //    stats.walkSpeed = enemy.enemy.speed;
        //}
        //bar = GetComponent<StatusBar>();
        originalSpeed = stats.runSpeed;
        DefaultStatusValues();
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
        locatingTargetRunning = false;
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
    }
    public void Heal(float amount) {
        stats.health += Mathf.Clamp(amount, 0, stats.maxHealth - stats.health);
        //if(player && player.playerStatus) {
        //    player.playerStatus.OnStatusChange(PlayerStatus.Health);
        //}
    }
    public void UseMana(float amount) {
        stats.mana -= amount;
        //if(player) {
        //    player.PlayerConsumeMana(amount);
        //}
    }
    public void GiveMana(float amount) {
        stats.mana += amount;
    }
    public bool CanUseMana(float amount) {
        return stats.mana >= amount;
    }
    public void TakeDamage(float amount, AttackType attackType) {
        float damage = 0f;
        if(stats) {
            damage = CalculateDamage.Calculate(amount, attackType, stats);
        }
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
        damage = Mathf.Clamp(damage, 0f, stats.maxDamageTimesHealth * stats.maxHealth);
        stats.health -= damage;
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
        Debug.Log($"{gameObject.name} took {damage} damage of {attackType}");
        /*if(enemy && stats.health <= 0f) {
            stats.health = 0;
            enemy.EnemyDie();
        }*/
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
    // Inform the AI about stun and immobiliziation
    private void ToggleCrowdControl() {
        //enemy.enemyDetection.CheckFollowStatus();
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
    public void AddLightningStacks(int amount, float damage, float duration) {
        lightningStacks += amount;
        CheckLightningStacks(damage, duration);
    }
    // Check lightningStacks
    public void CheckLightningStacks(float damage, float duration) {
        if(lightningStacks >= stats.shockThreshold) {
            lightningStacks = 0;
            StartStun(duration);
            //statusParticles.StartShockedParticles(duration);
            TakeDamage(damage, AttackType.Lightning);
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
        yield return new WaitForSeconds(1);
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
        yield return new WaitForSeconds(1);
        if(iceStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseIceStacks());
        }
        else {
            iceStacksCounterRunning = false;
        }
    }
    // Start regeneration coroutine
    public void StartRegen(float duration, float healAmountPerTick) {
        if(gameObject.activeInHierarchy) {
            if(regenerating) {
                StopRegen();
            }
            regenCounter = StartCoroutine(Regeneration(duration, healAmountPerTick));
        }
    }
    // Stop regeneration
    public void StopRegen() {
        StopCoroutine(regenCounter);
        Debug.Log("Regeneration stopped");
    }
    // Damage over time coroutines
    public IEnumerator Regeneration(float duration, float healAmountPerTick) {
        float period = 1f;
        bool done = false;
        while(!done) {
            yield return new WaitForSeconds(period);
            Heal(healAmountPerTick);
            duration -= period;
            if(duration <= 0) {
                done = true;
            }
        }
    }
    public void AddFireStacks(int amount, float damage, float duration) {
        fireStacks += amount;
        CheckFireStacks(duration, damage);
    }
    // Check earthStacks
    public void CheckFireStacks(float duration, float damage) {
        if(fireStacks >= stats.burningThreshold) {
            fireStacks = 0;
            StartBurn(duration, damage);
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
        yield return new WaitForSeconds(1);
        if(fireStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseFireStacks());
        }
        else {
            fireStacksCounterRunning = false;
        }
    }
    private void StartBurn(float duration, float damage) {
        if(gameObject.activeInHierarchy) {
            if(burning) {
                StopBurn();
            }
            //statusParticles.StartBurningParticles(duration);
            burnCounter = StartCoroutine(Burn(duration, damage));
        }
    }
    public void StopBurn() {
        if(burning) {
            //statusParticles.StopBurningParticles();
            StopCoroutine(burnCounter);
            burning = false;
        }
    }
    public IEnumerator Burn(float duration, float damage) {
        float period = 2f;
        bool done = false;
        burning = true;
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage, AttackType.Fire);
            duration -= period;
            if(duration <= 0) {
                done = true;
            }
        }
        //statusParticles.StopBurningParticles();
        burning = false;
    }
    public void AddPoisonStacks(int amount, float damage, float duration) {
        poisonStacks += amount;
        CheckPoisonStacks(duration, damage);
    }
    // Check stacks
    public void CheckPoisonStacks(float duration, float damage) {
        if(poisonStacks >= stats.poisonThreshold) {
            poisonStacks = 0;
            float poisonDamageInterval = 1f;
            // Decrease poison damage interval by 1 second per tick
            StartPoison(duration, damage, poisonDamageInterval);
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
        yield return new WaitForSeconds(1);
        if(poisonStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreasePoisonStacks());
        }
        else {
            poisonStacksCounterRunning = false;
        }
    }
    private void StartPoison(float duration, float damage, float periodTimeDecrease) {
        if(gameObject.activeInHierarchy) {
            if(poisoned) {
                StopPoison();
            }
            poisonCounter = StartCoroutine(Poison(duration, damage, periodTimeDecrease));
        }
    }
    public void StopPoison() {
        StopCoroutine(poisonCounter);
        poisoned = false;
    }
    private IEnumerator Poison(float duration, float damage, float periodTimeDecrease) {
        float period = 10f;
        bool done = false;
        poisoned = true;
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage, AttackType.Poison);
            duration -= period;
            period = period <= 1 ? 1 : period -= periodTimeDecrease;
            if(duration <= 0) {
                done = true;
            }
        }
        poisoned = false;
    }
    public void AddBleedStacks(int amount, float damage, float duration) {
        bleedStacks += amount;
        CheckBleedStacks(duration, damage);
    }
    // Check stacks
    public void CheckBleedStacks(float duration, float damage) {
        if(bleedStacks >= stats.bleedThreshold) {
            bleedStacks = 0;
            float bleedDamageIncrease = damage / 4f;
            // Increase bleed damage by damage / 4 per tick
            StartBleed(duration, damage, bleedDamageIncrease);
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
        yield return new WaitForSeconds(1);
        if(bleedStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseBleedStacks());
        }
        else {
            bleedStacksCounterRunning = false;
        }
    }
    private void StartBleed(float duration, float damage, float damageIncrease) {
        if(gameObject.activeInHierarchy) {
            if(bleeding) {
                StopBleed();
            }
            bleedCounter = StartCoroutine(Bleed(duration, damage, damageIncrease));
        }
    }
    public void StopBleed() {
        StopCoroutine(bleedCounter);
        bleeding = false;
    }
    public IEnumerator Bleed(float duration, float damage, float damageIncrease) {
        float period = 5f;
        bool done = false;
        bleeding = true;
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage, AttackType.Bleed);
            duration -= period;
            damage += damageIncrease;
            if(duration <= 0) {
                done = true;
            }
        }
        bleeding = false;
    }
    public void AddCurseStacks(int amount, float damage, float duration) {
        curseStacks += amount;
        CheckCurseStacks(duration, damage);
    }
    // Check stacks
    public void CheckCurseStacks(float duration, float damage) {
        if(curseStacks >= stats.curseThreshold) {
            curseStacks = 0;
            // Increase bleed damage by damage / 4 per tick
            StartCurse(duration, damage);
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
        yield return new WaitForSeconds(1);
        if(curseStacks > 0 && gameObject.activeInHierarchy) {
            StartCoroutine(DecreaseCurseStacks());
        }
        else {
            curseStacksCounterRunning = false;
        }
    }
    public void StartCurse(float duration, float damage) {
        if(gameObject.activeInHierarchy) {
            if(cursed) {
                StopCurse();
            }
            curseCounter = StartCoroutine(Curse(duration, damage));
        }
    }
    public void StopCurse() {
        StopCoroutine(curseCounter);
        cursed = false;
    }
    public IEnumerator Curse(float duration, float damage) {
        float period = 2f;
        bool done = false;
        cursed = true;
        while(!done) {
            yield return new WaitForSeconds(period);
            TakeDamage(damage, AttackType.Curse);
            duration -= period;
            if(duration <= 0) {
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
            stunCounter = StartCoroutine(Stun(duration));
        }
    }
    public void StopStun() {
        stunned = false;
        StopCoroutine(stunCounter);
    }
    // Makes target unable to attack and move for a couple of seconds
    private IEnumerator Stun(float duration) {
        // Stun here
        stunned = true;
        /*
        if(enemy)
            ToggleCrowdControl();*/
        yield return new WaitForSeconds(duration);
        // Return to normal here
        stunned = false;
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
    }
    private IEnumerator Immobilize(float duration) {
        immobilized = true;
        /*if(enemy)
            ToggleCrowdControl();*/
        yield return new WaitForSeconds(duration);
        immobilized = false;
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
        frostbitten = false;
        StopCoroutine(frostbiteCounter);
    }
    // Frostbite effect stops burn
    private IEnumerator Frostbite(float duration, float amplifiedDamage) {
        frostbitten = true;
        if(burning) {
            StopBurn();
        }
        /*if(enemy) {
            ToggleCrowdControl();
        }*/
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
        if(gameObject.activeInHierarchy) {
            if(chilled) {
                StopChill();
            }
            chillCounter = StartCoroutine(Chill(duration, slowRate));
        }
    }
    public void StopChill() {
        chilled = false;
        stats.runSpeed = originalSpeed;
        StopCoroutine(chillCounter);
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
        stats.runSpeed *= 1 - slowRate;
        /*
        if(enemy)
            enemy.enemyDetection.aiPath.maxSpeed *= 1 - slowRate;
        else if(player)
            GetComponent<PlayerMovement>().walkSpeed *= 1 - slowRate;*/
        yield return new WaitForSeconds(duration);
        chilled = false;
        stats.runSpeed = originalSpeed;
        /*
        if(enemy)
            enemy.enemyDetection.aiPath.maxSpeed = originalSpeed;
        else if(player)
            GetComponent<PlayerMovement>().walkSpeed = originalSpeed;*/
    }
    public void StartDarkSigil(float duration, float damageRate) {
        if(gameObject.activeInHierarchy) {
            if(!darkSigilRunning && !cannotBeDarkSigiled) {
                Debug.LogWarning("Dark sigil started");
                StartCoroutine(DarkSigilCooldown(darkSigilCooldown));
                darkSigilCounter = StartCoroutine(DarkSigil(duration, damageRate));
            }
        }
    }
    public void EndDarkSigil() {
        darkSigilRunning = false;
        darkSigilCharge = 0;
        StopCoroutine(darkSigilCounter);
    }
    // Dark sigil deals dark damage when time ends depending on the target's 
    // Dark damage taken during DarkSigil effect
    private IEnumerator DarkSigil(float duration, float damageRate) {
        darkSigil = true;
        if(!darkSigilRunning) {
            darkSigilRunning = true;
        }
        yield return new WaitForSeconds(duration);
        TakeDamage(darkSigilCharge * damageRate, AttackType.Dark);
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
        immobilized = false;
    }
    public void StartTimeBomb(float seconds, float damage, AttackType attackType) {
        if(gameObject.activeInHierarchy) {
            if(timeBombed) {
                EndTimeBomb();
            }
            timeBombCounter = StartCoroutine(TimeBomb(seconds, damage, attackType));
        }
    }
    public void EndTimeBomb() {
        StopCoroutine(timeBombCounter);
        timeBombed = false;
    }
    public IEnumerator TimeBomb(float seconds, float damage, AttackType attackType) {
        timeBombed = true;
        yield return new WaitForSeconds(seconds);
        TakeDamage(damage, attackType);
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
        yield return new WaitForSeconds(1);
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
        StopCoroutine(earthenCounter);
        earthed = false;
    }
    private IEnumerator Earthen(float duration) {
        earthed = true;
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
        yield return new WaitForSeconds(1);
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
        StopCoroutine(lightCounter);
        lit = false;
    }
    private IEnumerator Enlighten(float duration) {
        lit = true;
        yield return new WaitForSeconds(duration);
        lit = false;
    }
    public void StartSpeedUp(float speedRate, float duration) {
        if(gameObject.activeInHierarchy) {
            if(spedUp) {
                StopSpeedUp();
            }
            speedUpCounter = StartCoroutine(SpeedUp(speedRate, duration));
        }
    }
    public void StopSpeedUp() {
        StopCoroutine(speedUpCounter);
        stats.runSpeed = originalSpeed;
        spedUp = false;
    }
    private IEnumerator SpeedUp(float speedRate, float duration) {
        if(chilled) {
            StopChill();
        }
        stats.runSpeed *= 1 * speedRate;
        spedUp = true;
        yield return new WaitForSeconds(duration);
        spedUp = false;
        stats.runSpeed = originalSpeed;
    }
}
