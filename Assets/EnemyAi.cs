using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;  
public class enemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBetweenAttacks;

    public bool canAttack;
    bool alreadyAttacked;
    public GameObject swordSkeleton;
    public bool swingUp;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Animator skeleAnim;

    public float stoppingDistance; // Add this variable to set the stopping distance

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            player = GameObject.Find(("GSFirstPersonController (1)")).transform;
        }else if(SceneManager.GetActiveScene().name == "World1"){
            player = GameObject.Find(("GSFirstPersonController (1) Variant")).transform;
        }
        
        
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
       
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(transform.position); // Stop moving
            LookAtPlayerOnYAxis();

            if (!alreadyAttacked)
            {
                canAttack = true;
                swordSkeleton.GetComponent<PlayerHitter>().isSwingingUpSkeleton = true;
                swingUp = true;
                skeleAnim.SetBool("running", false);
                skeleAnim.SetBool("attacking", true);
                alreadyAttacked = true;
                
                swingUp = false;
                
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else
        {
            agent.SetDestination(player.position); // Continue moving until within stopping distance
            canAttack = false;
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
    private IEnumerator WaitAttack(float delay){
            yield return new WaitForSeconds(delay);
            Debug.Log("can take damage");
            swordSkeleton.GetComponent<PlayerHitter>().isSwingingUpSkeleton = false;
    }
}
