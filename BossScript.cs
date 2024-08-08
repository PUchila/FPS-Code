
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float force = 50f;
    public float padding = 5f;
    public bool dead;
    public Transform turret;
    public Transform empty;
    public AudioSource source;
    public AudioClip gunshoot;

    private void Awake()
    {
      anim.SetBool("isRunning", false);
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
      if(dead) return;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
      anim.SetBool("isRunning", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        anim.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        anim.SetBool("isRunning", false);
        Vector3 des = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(des);

        if (!alreadyAttacked)
        {
            ///Attack code here

            empty.transform.LookAt(player);
            empty.transform.Rotate(90.0f,0f,0f,Space.Self);
            Vector3 despos = new Vector3(turret.transform.position.x, turret.transform.position.y, turret.transform.position.z);
            GameObject bullet = Instantiate(projectile, despos, empty.rotation);
            source.PlayOneShot(gunshoot);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.up * force, ForceMode.Impulse);
            ///End of attack code
            Destroy(bullet, 2f);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
