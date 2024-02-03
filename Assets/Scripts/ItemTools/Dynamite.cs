using Assets.Scripts.Inventory;
using Assets.Scripts.PauseMenuScripts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ItemTools
{
    public class Dynamite : MonoBehaviour
    {
        public static bool itemReady = false;
        public static bool playerInArea = false;

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
            UseDynamite();
        }

        private void UseDynamite()
        {
            if (itemReady && Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused && playerInArea)
            {
                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
                LevelLoader.GetInstance().LoadLevelByName("GoodEnding");
            }
        }
    }
}