using Assets.Scripts.Inventory;
using Assets.Scripts.PauseMenuScripts;
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
            if (itemReady && Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused)
            {
                AudioManager.instance.Play("Medkit");
                PlayerStats.instance.currentHealht += 100f;
                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
            }
        }
    }
}