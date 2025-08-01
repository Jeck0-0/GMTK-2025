using System;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float visionRange = 10f;
    [SerializeField] float attackRange = 7f;
    [SerializeField] Transform[] patrolPoints;


    private Transform player;
    private Rigidbody2D rb;

    private enum State { Idle, Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    private int currentPatrolIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = Player.instance.transform;
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
    }

    public override void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
