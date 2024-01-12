using System.Collections;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class MimicNPC : EnemyAi
    {
        // Update is called once per frame
        [SerializeField] private bool mimicAwaken = false;
        private bool musicPlayed = false;
        [SerializeField] private GameObject toGet;

        private void Start()
        {
            mimicAwaken = false;
            musicPlayed = false;
        }

        void Update()
        {
            InitAiBehaviour();
        }

        protected override void InitAiBehaviour()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            //Patroling();

            //if (!playerInSightRange && !playerInAttackRange) anim.SetBool("Chase", false);
            if (mimicAwaken == false)
            {
                GoToPoint();
            }

            if(playerInSightRange && !musicPlayed)
            {
                AudioManager.instance.Play("Violin");
                musicPlayed = true;
            }

            if (playerInSightRange && !playerInAttackRange)
            {
                mimicAwaken = true;
                agent.speed = 3;
                ChasePlayer();
            }
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
            if (!playerInSightRange && mimicAwaken == true)
            {
                Patroling();
            }
        }

        protected override void GoToPoint()
        {
            anim.Play("mimicWalk");
            agent.speed = 1;
            agent.SetDestination(walkPoint);
        }

        /*public override void ChasePlayer()
        {
            //EnemyAudioSource.Play();
            StartCoroutine(mimicAwake());
        }*/

        public override void die()
        {
            GetComponent<Animator>().Play("dead");
            gameObject.layer = 9;
            MimicNPC enemy = GetComponent<MimicNPC>();
            enemy.ResetAttack();
            enemy.agent.isStopped = true;
            enemy.enabled = false;
            if (toGet != null)
            {
                toGet.SetActive(true);
                toGet.transform.parent = null;
            }
            Destroy(gameObject, 10f);
        }

        IEnumerator mimicAwake()
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            anim.Play("Mimic");
            yield return new WaitForSeconds(2.4f);
            anim.Play("mimicChase");
            agent.SetDestination(player.position);
        }
    }
}