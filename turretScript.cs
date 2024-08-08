
using UnityEngine;

public class turretScript : MonoBehaviour
{

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
    public Transform turret;
    public Transform turret2;
    public float force = 50f;
    public float padding = 5f;
    public bool dead;
    private Vector3 range;
    public float moveRange = 12;
    public float speed;
    public bool firstshot = true;
    private Vector3 despos;
    public AudioSource source;
    public AudioClip gunshoot;

    private void Start()
    {
        range = transform.position;
        player = GameObject.Find("Player").transform;

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
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet){
          if (Vector3.Distance(range, walkPoint)<moveRange)
          transform.LookAt(walkPoint);
            transform.position = Vector3.MoveTowards(transform.position, walkPoint, speed*Time.deltaTime);
          }else{
            walkPointSet = false;
          }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        transform.LookAt(player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        Vector3 des = new Vector3(player.position.x, player.position.y, player.position.z);
        transform.LookAt(des);

        if (!alreadyAttacked)
        {
            ///Attack code here
            if(firstshot){
               despos = new Vector3(turret.transform.position.x, turret.transform.position.y, turret.transform.position.z);

            }else{
               despos = new Vector3(turret2.transform.position.x, turret2.transform.position.y, turret2.transform.position.z);

            }
            source.PlayOneShot(gunshoot);
            GameObject bullet = Instantiate(projectile, despos, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.transform.LookAt(player);
            bullet.transform.Rotate(90.0f,0f,0f,Space.Self);
            rb.AddForce(bullet.transform.up * force, ForceMode.Impulse);
            firstshot = !firstshot;
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
