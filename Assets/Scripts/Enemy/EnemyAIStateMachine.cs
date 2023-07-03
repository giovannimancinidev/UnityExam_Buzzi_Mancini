using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStateMachine : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    }

    public State CurrentState;
    public Transform Player;
    public Transform PlayerHitTarget;
    public Transform RaycastOrigin;
    public float StoppingDistance = 2f;
    public float DetectionRange = 10f;
    public float AttackDelay = 1f;

    private NavMeshAgent agent;
    private EnemyAI enemyScript;
    private Animator enemyAnim;
    private bool isAttacking = false;
    private bool playerVisible = false;

    private void Start()
    {
        enemyScript = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        agent.stoppingDistance = StoppingDistance;
        CurrentState = State.Idle;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, Player.position);
        float targetDistance = Vector3.Distance(RaycastOrigin.position, PlayerHitTarget.position);
        Vector3 dir = PlayerHitTarget.position - RaycastOrigin.position;
        RaycastHit hit;
        Physics.Raycast(RaycastOrigin.position, dir, out hit, Mathf.Infinity);
        
        if (hit.transform.gameObject.CompareTag("Surface"))
        {
            Debug.DrawRay(RaycastOrigin.position, dir * 1000, Color.red);
            playerVisible = false;
        }
        else
        {
            Debug.DrawRay(RaycastOrigin.position, dir * 1000, Color.green);
            playerVisible = true;
        }

        if (playerVisible)
        {
            if (CurrentState == State.Idle || CurrentState == State.Chasing)
            {
                agent.isStopped = true;
                CurrentState = State.Attacking;
                print("Attack");
            }
        }
        else if (CurrentState == State.Attacking)
        {
            CurrentState = State.Chasing;
        }

        switch (CurrentState)
        {
            case State.Idle:
                // Do nothing
                break;

            case State.Chasing:
                if (agent.enabled)
                {
                    agent.isStopped = false;
                    ChasePlayer();
                    enemyAnim.SetBool("IsFiring", false);
                }
                break;

            case State.Attacking:
                if (!isAttacking)
                {
                    AttackPlayer();
                    enemyAnim.SetBool("IsFiring", true);
                }
                break;
        }
    }

    private void ChasePlayer()
    {
        if (agent.enabled)
        {
            agent.SetDestination(Player.position);
            enemyAnim.SetFloat("VelocityZ", agent.speed);
        }
    }

    private void AttackPlayer()
    {
        if (!isAttacking)
        {
            enemyScript.Attack();
            enemyAnim.SetFloat("VelocityZ", 0);
            StartCoroutine(AttackDelayCount());
        }
    }

    private IEnumerator AttackDelayCount()
    {
        isAttacking = true;
        yield return new WaitForSeconds(AttackDelay);
        isAttacking = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Surface") && CurrentState == State.Chasing)
        {
            CurrentState = State.Idle;
            agent.isStopped = true;
            enemyAnim.SetFloat("VelocityZ", 0);
        }
    }
}