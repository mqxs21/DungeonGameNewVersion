using UnityEngine;
using UnityEngine.AI;

public class enemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Animator skeleAnim;

    public float stoppingDistance; // Add this variable to set the stopping distance

    private void Awake()
    {
        player = GameObject.Find("GSFirstPersonController (1)").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
        else
        {
            StopChasingPlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.stoppingDistance = stoppingDistance; // Set the stopping distance
        skeleAnim.SetBool("running", true);
        skeleAnim.SetBool("attacking", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Only stop moving if within attack range and the agent has reached its destination
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(transform.position); // Stop moving
            LookAtPlayerOnYAxis();

            if (!alreadyAttacked)
            {
                Debug.Log("attack");
                skeleAnim.SetBool("running", false);
                skeleAnim.SetBool("attacking", true);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else
        {
            agent.SetDestination(player.position); // Continue moving until within stopping distance
        }
    }

    private void LookAtPlayerOnYAxis()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void StopChasingPlayer()
    {
        agent.SetDestination(transform.position); // Stop moving
        agent.velocity = Vector3.zero; // Set velocity to zero
        skeleAnim.SetBool("running", false);
        skeleAnim.SetBool("attacking", false);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        skeleAnim.SetBool("attacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
