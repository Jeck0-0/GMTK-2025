using System;
using UnityEngine;

public class Enemy : Unit
{
    public float moveSpeed = 2f;
    public float visionRange = 10f;
    public float attackRange = 7f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private enum State { Idle, Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
                if (distanceToPlayer < visionRange)
                currentState = State.Chase;
                break;

            case State.Chase:
                ChasePlayer();
                if (distanceToPlayer < attackRange)
                currentState = State.Attack;
                else if (distanceToPlayer > visionRange)
                currentState = State.Patrol;
                break;

            case State.Attack:
                Attack();
                if (distanceToPlayer > attackRange)
                currentState = State.Chase;
                break;
        }
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

    void MoveTowards(Vector2 target)
    {
        anim.SetBool("Moving", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        FlipSprite(direction.x);
    }

    void FlipSprite(float directionX)
    {
        if (directionX != 0)
        transform.localScale = new Vector3(Mathf.Sign(directionX), 1, 1);
    }

    public override void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
