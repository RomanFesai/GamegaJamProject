using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnableNPCTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject NPC;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                NPC.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}