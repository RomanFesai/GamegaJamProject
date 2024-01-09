using Assets.Scripts.Inventory;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ItemTools
{
    public class AlcoholFlask : MonoBehaviour
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
            UseFlask();
        }

        private void UseFlask()
        {
            if (itemReady && Input.GetMouseButtonDown(0))
            {
                PlayerStats.instance.currentHealht -= 30f;
                PlayerStats.instance.currentFreeze -= 50f;

                if(PlayerStats.instance.currentFreeze < 0)
                    PlayerStats.instance.currentFreeze = 0;

                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
            }
        }
    }
}