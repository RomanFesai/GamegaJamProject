using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.NPCs
{
    public class EnemyAi : MonoBehaviour
    {
        public float health = 5f;

        public NavMeshAgent agent;

        public Transform player;

        public LayerMask whatIsGround, whatIsPlayer;

        public Animator anim;

        public GameObject AttackHand;

        //[SerializeField] private AudioSource EnemyAudioSource = default;
        //[SerializeField] private AudioClip EnemyDetected = default;
        //public float health;

        //Patroling
        public Vector3 walkPoint;
        protected bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool alreadyAttacked;
        //public GameObject projectile;
        //public GameObject EnemyShootHole;

        //States
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;
        protected bool isDead;

        protected void Awake()
        {
            player = GameObject.Find("ChaseNavPoint").transform;
            agent = GetComponent<NavMeshAgent>();
            isDead = false;
        }

        protected virtual void InitAiBehaviour()
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            //Patroling();
            GoToPoint();

            //if (!playerInSightRange && !playerInAttackRange) anim.SetBool("Chase", false);
            if (playerInSightRange && !playerInAttackRange)
            {
                agent.speed = 3;
                ChasePlayer();
            }
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
            if (!playerInSightRange)
            {
                Patroling();
            }
        }

        protected virtual void GoToPoint()
        {
            anim.Play("walk");
            agent.speed = 1;
            agent.SetDestination(walkPoint);
        }

        protected void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }
        protected void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                walkPointSet = true;
        }

        public virtual void ChasePlayer()
        {
            //anim.SetBool("Chase", true);
            //EnemyAudioSource.Play();
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            agent.SetDestination(player.position);
        }

        public void AttackPlayer()
        {
            //Make sure enemy doesn't move
            //agent.SetDestination(transform.position);
            AttackHand.GetComponent<BoxCollider>().enabled = true;

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            if (!alreadyAttacked)
            {
                agent.isStopped = true;
                ///Attack code here
                anim.SetBool("Attack", true);
                ///End of attack code

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        public void ResetAttack()
        {
            AttackHand.GetComponent<BoxCollider>().enabled = false;
            anim.SetBool("Attack", false);
            alreadyAttacked = false;
            agent.isStopped = false;
        }
        public void TakeDamage(float amount)
        {
            if (health > 0)
            {
                health -= amount;
                AudioManager.GetInstance().Play("EnemyInjured");
                StartCoroutine(Stun(4f));
                //ChasePlayer();
                if (health <= 0f)
                {
                    die();
                    AudioManager.GetInstance().Play("EnemyDead");
                }
            }
        }

        private IEnumerator Stun(float stunTime)
        {
            agent.isStopped = true;
            anim.Play("injured");
            yield return new WaitForSeconds(stunTime);
            if(!isDead)
                agent.isStopped = false;
        }

        public virtual void die()
        {
            isDead = true;
            anim.Play("dead");
            gameObject.layer = 9;
            EnemyAi enemy = GetComponent<EnemyAi>();
            enemy.agent.isStopped = true;
            enemy.ResetAttack();
            enemy.enabled = false;
            Destroy(gameObject, 10f);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}