using Assets.Scripts.NPCs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfNPC : EnemyAi
{
    [SerializeField] private bool musicPlayed;
    [SerializeField] private bool isPatroling = true;
    [SerializeField] private AudioSource wolf_bark;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        

        if (playerInSightRange && !musicPlayed)
        {
            wolf_bark.PlayOneShot(wolf_bark.clip);
            musicPlayed = true;
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            agent.speed = 6;
            anim.SetBool("wolf_run", true);
            anim.SetBool("wolf_walk", false);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        if (!playerInSightRange && isPatroling)
        {
            agent.speed = 3;
            anim.SetBool("wolf_walk", true);
            anim.SetBool("wolf_run", false);
            anim.SetBool("isPatroling", isPatroling);
            Patroling();
        }
    }

    public override void die()
    {
        isDead = true;
        GetComponent<Animator>().Play("Wolf_die");
        gameObject.layer = 9;
        WolfNPC enemy = GetComponent<WolfNPC>();
        base.ResetAttack();
        agent.isStopped = true;
        enemy.enabled = false;
        Destroy(gameObject, 10f);
    }
}
