using Assets.Scripts.Inventory;
using Assets.Scripts.PauseMenuScripts;
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
            if (itemReady && Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused)
            {
                PlayerStats.instance.currentHealht -= 30f;
                PlayerStats.instance.currentFreeze -= 100f;

                if(PlayerStats.instance.currentFreeze < 0)
                    PlayerStats.instance.currentFreeze = 0;

                AudioManager.instance.Play("Drinking");
                PlayerStats.instance.EnableDrunkEffect(20f);
                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
            }
        }
    }
}