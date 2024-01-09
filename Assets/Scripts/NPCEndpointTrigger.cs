using Assets.Scripts.NPCs;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class NPCEndpointTrigger : MonoBehaviour
    {
        [SerializeField] private bool isArrived = false;
        private GameObject NPC;

        private void Update()
        {
            if (isArrived)
            { 
                Destroy(NPC);
                isArrived = false;
            }

        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<TraderNPC>() != null)
            {
                isArrived = true;
                NPC = other.gameObject;
            }
        }
    }
}