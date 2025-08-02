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

    private Coroutine blinkRoutine;
    private Transform player => Player.instance.transform;
    private Rigidbody2D rb;

    private enum State { Idle, Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    private int currentPatrolIndex = 0;

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

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (CanSee(Player.instance))
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                ChasePlayer();
                if (distanceToPlayer < attackRange)
                    ChangeState(State.Attack);
                else if (distanceToPlayer > visionRange)
                    ChangeState(State.Patrol);
                break;

            case State.Attack:
                Attack();
                if (distanceToPlayer > attackRange)
                    ChangeState(State.Chase);
                break;
        }
    }
    private void ChangeState(State state)
    {
        if(anim)
        anim.SetBool("Moving", false);
        currentState = state;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void ChasePlayer()
    {
        if (player == null) return;
        MoveTowards(player.position);
    }

    void Attack()
    {
        weapon.transform.right = (Player.instance.transform.position - transform.position).normalized;
        TryAttacking();
    }

    bool CanSee(Targetable target)
    {
        //Check range
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance >= visionRange) return false;

        //Check line of sight (if there is something in the way)
        Vector2 direction = (target.transform.position - transform.position).normalized;
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, distance);
        
        return hit.All(x=>x.collider.GetComponent<Targetable>() != null);
    }
    
    void MoveTowards(Vector2 target)
    {
        if (anim)
            anim.SetBool("Moving", true);
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite(direction.x);
    }

    void FlipSprite(float directionX)
    {
        if (directionX != 0)
        transform.localScale = new Vector3(Mathf.Sign(-directionX), 1, 1);
        FlipWeaponVisuals(directionX);
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
