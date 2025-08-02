using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : Unit
{
    [SerializeField] AudioClip damageClip;
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float visionRange = 10f;
    [SerializeField] float attackRange = 7f;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Light2D hitFX;
    [SerializeField] float deaggroThreshold = 1;
    
    private Coroutine blinkRoutine;
    private Transform player => Player.instance.transform;
    private Rigidbody2D rb;

    private enum State { Idle, Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    public int currentPatrolIndex = 0;

    
    
    protected override void Start()
    {
        initialPosition = transform.position;
        LoopManager.Instance.OnGameReset += OnReset;
        rb = GetComponent<Rigidbody2D>();
        if (hitFX) hitFX.enabled = false;
    }

    void Update()
    {
        if (isDead) return;
        HandleState();
    }

    void HandleState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        /*if (CanSee(Player.instance))
        {
            if (distanceToPlayer < attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }*/
        
        switch (currentState)
        {
            case State.Patrol:
                if (CanSee(Player.instance))
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (distanceToPlayer < attackRange && CanSee(Player.instance))
                    ChangeState(State.Attack);
                else if (!CanSee(Player.instance, deaggroThreshold))
                    ChangeState(State.Patrol);
                break;

            case State.Attack:
                if (!CanSee(Player.instance, deaggroThreshold * 2))
                    ChangeState(State.Patrol);
                else if (distanceToPlayer > attackRange + deaggroThreshold)
                    ChangeState(State.Chase);
                break;
        }
        
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                ChasePlayer();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }
    private void ChangeState(State state)
    {
        if (currentState == state)
            return;
        
        if(anim)
        anim.SetBool("Moving", false);
        currentState = state;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.4f)
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void ChasePlayer()
    {
        if (player == null) return;
        MoveTowards(player.position);
    }

    void Attack()
    {
        var dir = (Player.instance.transform.position - transform.position).normalized;
        weapon.transform.right = dir;
        FlipSprite(dir.x);
        AdjustWeaponRotation(true);
        TryAttacking();
    }

    bool CanSee(Targetable target, float threshold = 0f)
    {
        //Check range
        float distance = Vector3.Distance(target.transform.position, transform.position) - threshold;
        if(distance >= Mathf.Min(visionRange, attackRange)) return false;

        //Check line of sight (if there is something in the way)
        Vector2 direction = (target.transform.position - transform.position).normalized;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, distance);

        return hit.All(x => x.collider.isTrigger || x.collider.GetComponent<Targetable>() != null);
    }
    
    void MoveTowards(Vector2 target)
    {
        if (anim)
        anim.SetBool("Moving", true);
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite(direction.x);
        AdjustWeaponRotation(false);
    }

    void FlipSprite(float directionX)
    {
        if (directionX != 0)
        transform.localScale = new Vector3(Mathf.Sign(-directionX), 1, 1);
    }
    
    private void AdjustWeaponRotation(bool forceDirection)
    {
        Vector3 localScale = weapon.transform.localScale;

        bool weaponRight = weapon.transform.right.x > 0;
        bool facingRight = transform.localScale.x > 0;
            
        bool flipX = facingRight;
        bool flipY = !weaponRight;
        
        if(!forceDirection)
            flipX = !weaponRight;
        
        localScale.x = Mathf.Abs(localScale.x) * (flipX ? -1 : 1);
        localScale.y = Mathf.Abs(localScale.y) * (flipY ? -1 : 1);
        
        weapon.transform.localScale = localScale;
    }
    private IEnumerator Blink()
    {
        if (hitFX == null) yield break;
        hitFX.enabled = true;
        yield return new WaitForSeconds(0.1f);
        hitFX.enabled = false;
    }
    public override void Damage(float amount)
    {
        if (isDead || !isVulnerable)
        return;

        currentHealth -= amount;

        if (damageClip)
        AudioManager.Instance.PlaySound(damageClip, 0.7f, transform);

        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            if (hitFX) hitFX.enabled = false;
        }
        blinkRoutine = StartCoroutine(Blink());

        if (currentHealth <= 0)
        {
            Invoke("Die", 0.1f);
            return;
        }
    }
    public override void Die()
    {
        if (deathFX)
        Instantiate(deathFX, transform.position, Quaternion.identity);
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Enemy died!");
        gameObject.SetActive(false);
    }
    public override void OnReset()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();

        if (hitFX) hitFX.enabled = false;
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        isDead = false;
    }
}