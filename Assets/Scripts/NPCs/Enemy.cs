using System.Collections;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class Enemy : MonoBehaviour
    {

        public float health = 5f;

        public void TakeDamage(float amount)
        {
            if(health > 0) 
            {
                health -= amount;
                AudioManager.GetInstance().Play("EnemyInjured");
                GetComponent<EnemyAi>().ChasePlayer();
                if (health <= 0f)
                {
                    die();
                    AudioManager.GetInstance().Play("EnemyDead");
                }
            }
        }
        public void die()
        {
            GetComponent<Animator>().Play("dead");
            gameObject.layer = 9;
            EnemyAi enemy = GetComponent<EnemyAi>();
            enemy.agent.isStopped = true;
            enemy.ResetAttack();
            enemy.enabled = false;
            Destroy(gameObject, 10f);
        }

       /* void setColliderState(bool state)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                collider.enabled = state;
            }
            GetComponent<Collider>().enabled = !state;
        }*/
    }
}