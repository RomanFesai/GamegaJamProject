using Assets.Scripts.Inventory;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class TempData : MonoBehaviour
    {
        [SerializeField] private List<GameObject> itemsToLoad;

        [SerializeField] private ItemSlotData[] items;

        public int rifleAmmo;
        public static TempData instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
        public void Save(TempData data)
        {
            data.rifleAmmo = Rifle.currentAmmo;
            for (int i = 0; i < PlayerInventory.instance.inventoryList.Length; i++)
            {
                if (PlayerInventory.instance.inventoryList[i] != null && PlayerInventory.instance.inventoryList[i].transform.childCount > 0)
                {
                    data.items[i].SlotNumber = i;
                    data.items[i].itemObjName = PlayerInventory.instance.inventoryList[i].transform.GetChild(0).gameObject.name;
                }
            }
        }

        public void Load(TempData data)
        {
            Rifle.currentAmmo = data.rifleAmmo;
            for (int i = 0; i < items.Length; i++)
            {
                if (data.items[i].itemObjName != "")
                {
                    var itemToAdd = Instantiate(itemsToLoad.Find(p => p.gameObject.name == data.items[i].itemObjName));
                    PlayerInventory.instance.AddItemToSlot(PlayerInventory.instance.inventoryList[i], data.items[i].SlotNumber + 1, itemToAdd);
                }
            }
        }
    }
}