using Assets.Scripts.Interfaces;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Pickupable : MonoBehaviour, IPickable
    {
        public ItemSO itemScriptableObject;
        /*[SerializeField] private GameObject CircleHint;
        [SerializeField] private SphereCollider pickUpRadius;
        private bool playerInRange = false;*/

       /* private void OnEnable()
        {
            pickUpRadius.enabled = true;
        }

        private void OnDisable()
        {
            pickUpRadius.enabled = false;
        }

        private void Update()
        {
            if (playerInRange)
                CircleHint?.SetActive(true);
            else
                CircleHint?.SetActive(false);
        }*/

        public void PickItem()
        {
            Destroy(gameObject);
        }

        /*private void OnTriggerEnter(Collider other)
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
        }*/
    }
}