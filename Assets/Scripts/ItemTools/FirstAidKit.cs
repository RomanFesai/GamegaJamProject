using Assets.Scripts.Inventory;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ItemTools
{
    public class FirstAidKit : MonoBehaviour
    {
        [SerializeField] private bool itemReady = false;

        private void OnEnable()
        {
            itemReady = true;
        }

        private void OnDisable()
        {
            itemReady = false;
        }
        // Update is called once per frame
        void Update()
        {
            UseAid();
        }

        private void UseAid()
        {
            if (itemReady && Input.GetMouseButtonDown(0))
            {
                PlayerStats.instance.currentHealht += 30f;
                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
            }
        }
    }
}