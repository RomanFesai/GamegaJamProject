using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bonfire : MonoBehaviour
    {
        [SerializeField] private bool playerInRange = false;
        public bool bonfireLit = false;
        [SerializeField] private float bonfireTimer = 10;
        private float timer = 0;
        private SphereCollider sphereCollider;
        private GameObject fire;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            fire = gameObject.transform.GetChild(2).gameObject;
        }

        private void FixedUpdate()
        {
            if (playerInRange)
            {
                PlayerStats.instance.FreezeDefill();
                PlayerStats.instance.currentHealht += 1;
            }

            if (sphereCollider.enabled == true && fire.activeInHierarchy == true)
            { 
                timer += Time.deltaTime; 
                if (timer > bonfireTimer)
                {
                    sphereCollider.enabled = false;
                    fire.SetActive(false);
                    playerInRange = false;
                    bonfireLit = false;
                    timer = 0;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerInRange = false;
            }
        }
    }
}