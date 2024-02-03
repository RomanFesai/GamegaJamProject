using Assets.Scripts.Inventory;
using Assets.Scripts.PauseMenuScripts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ItemTools
{
    public class Apple : MonoBehaviour
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
            UseApple();
        }

        private void UseApple()
        {
            if (itemReady && Input.GetMouseButtonDown(0) && !PauseMenu.GameIsPaused)
            {
                AudioManager.instance?.Play("Eating");
                PlayerInventory.instance.DropItem();
                Destroy(gameObject);
                LevelLoader.GetInstance().LoadLevelByName("BadEnding");
            }
        }
    }
}