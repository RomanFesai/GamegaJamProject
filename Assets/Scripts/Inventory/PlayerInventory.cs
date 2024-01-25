using Assets.Scripts.Interfaces;
using Assets.Scripts.Items;
using Assets.Scripts.ItemTools;
using Assets.Scripts.Weapons;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerCam;

        [Header("General")]
        public GameObject[] inventoryList = new GameObject[4];
        public int selectedItem;
        public float playerReach;
        public float ThrowForce;

        [Header("Keys")]
        [SerializeField] private KeyCode throwItemKey;
        [SerializeField] private KeyCode pickItemKey;

        [Header("UI")]
        [SerializeField] private Image[] InventorySlotImage = new Image[4];
        [SerializeField] private Image[] InventorySlotHighlite = new Image[4];
        [SerializeField] private GameObject E_Key;

        public static PlayerInventory instance { get; private set; }

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
        }

        // Use this for initialization
        void Start()
        {
            /*itemSetActive.Add(ItemType.Rifle, rifle);
            itemSetActive.Add(ItemType.Matches, matches);*/

            NewItemSelected(0);
        }

        // Update is called once per frame
        void Update()
        {
            EnableItem();
            PickUp();
            if (Input.GetKeyDown(throwItemKey))
                DropItem();
            ShowHint();
        }

        private void PickUp()
        {
            RaycastHit hitInfo;
            Ray Vision = new Ray(playerCam.transform.position, playerCam.transform.forward);

            if(Physics.Raycast(Vision, out hitInfo, playerReach) && Input.GetKeyDown(pickItemKey))
            {
                var item = hitInfo.transform.gameObject;
                if(item != null && item.tag == "Item")
                {
                    EKeyHintDisabled();
                    int i = 0;
                    foreach (var obj in inventoryList)
                    {
                        i++;
                        if(obj.activeInHierarchy == true && obj.transform.childCount < 1)
                        {
                            AddItemToSlot(obj, i, item);
                            break;
                        }
                        else if (obj.activeInHierarchy == true && obj.transform.childCount > 0)
                        {
                            AudioManager.instance.Play("Blocked");
                            /*i = 0;
                            foreach (var obj2 in inventoryList)
                            {
                                i++;
                                if (obj2.activeInHierarchy == false && obj2.transform.childCount < 1)
                                {
                                    AddItemToSlot(obj2, i, item);
                                    break;
                                }
                            }*/
                        }
                    }
                }
                else if(item != null && item.tag == "Ammo")
                {
                    EKeyHintDisabled();
                    AudioManager.instance.Play("PickUp");
                    Rifle.currentAmmo += 1;
                    Destroy(item);
                }
            }
        }

        public void AddItemToSlot(GameObject slotGameObject, int slotNumber, GameObject itemToAdd)
        {
            itemToAdd.transform.SetParent(slotGameObject.transform);
            InventorySlotImage[slotNumber - 1].enabled = true;
            InventorySlotImage[slotNumber - 1].sprite = itemToAdd.GetComponent<Pickupable>().itemScriptableObject.item_sprite;
            itemToAdd.transform.localPosition = Vector3.zero;
            itemToAdd.transform.localRotation = Quaternion.Euler(Vector3.zero);
            itemToAdd.GetComponent<Rigidbody>().useGravity = false;
            itemToAdd.GetComponent<Rigidbody>().isKinematic = true; //"disabling" the rigidbody (it's still active but gravity won't apply to it.

            if (itemToAdd.GetComponent<Weapon>() != null)
                itemToAdd.GetComponent<Weapon>().enabled = true;
            if (itemToAdd.GetComponent<MatchBox>() != null)
                itemToAdd.GetComponent<MatchBox>().enabled = true;
            if (itemToAdd.GetComponent<AlcoholFlask>() != null)
                itemToAdd.GetComponent<AlcoholFlask>().enabled = true;
            if (itemToAdd.GetComponent<FirstAidKit>() != null)
                itemToAdd.GetComponent<FirstAidKit>().enabled = true;

            itemToAdd.GetComponent<BoxCollider>().enabled = false;
            AudioManager.instance.Play("PickUp");
            foreach (var transform in itemToAdd.gameObject.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = 8;
            }
        }

        public void DropItem()
        {
            int i = 0;
            foreach (var obj in inventoryList)
            {
                i++;
                if (obj.activeInHierarchy == true && obj.transform.childCount > 0)
                {
                    var item = obj.transform.GetChild(0).gameObject;
                    InventorySlotImage[i - 1].sprite = null;
                    InventorySlotImage[i - 1].enabled = false;
                    obj.transform.DetachChildren();
                    item.GetComponent<Rigidbody>().isKinematic = false;
                    item.GetComponent<Rigidbody>().useGravity = true;

                    if (item.GetComponent<Weapon>() != null)
                        item.GetComponent<Weapon>().enabled = false;
                    if (item.GetComponent<MatchBox>() != null)
                        item.GetComponent<MatchBox>().enabled = false;
                    if (item.GetComponent<AlcoholFlask>() != null)
                        item.GetComponent<AlcoholFlask>().enabled = false;
                    if (item.GetComponent<FirstAidKit>() != null)
                        item.GetComponent<FirstAidKit>().enabled = false;

                    item.GetComponent<BoxCollider>().enabled = true;
                    item.GetComponent<Rigidbody>().AddForce(item.transform.forward * ThrowForce);
                    foreach (Transform transform in item.gameObject.GetComponentsInChildren<Transform>())
                    { if (transform.gameObject.name != "HintCircle") transform.gameObject.layer = 12; }
                }
            }
        }
        private void EnableItem()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) && inventoryList[0] != null) 
            {
                selectedItem = 0;
                NewItemSelected(selectedItem);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList[1] != null)
            {
                selectedItem = 1;
                NewItemSelected(selectedItem);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList[2] != null)
            {
                selectedItem = 2;
                NewItemSelected(selectedItem);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList[3] != null)
            {   
                selectedItem = 3;
                NewItemSelected(selectedItem);
            }
        }

        private void NewItemSelected(int selected)
        {
            foreach(var item in inventoryList)
            {
                item.SetActive(false);
            }

            foreach(var item2 in InventorySlotHighlite)
            {
                item2.enabled = false;
            }

            GameObject selectedItemGameObject = inventoryList[selected];
            Image selectedSlot = InventorySlotHighlite[selectedItem];
            selectedItemGameObject.SetActive(true);
            selectedSlot.enabled = true;
        }

        private void ShowHint()
        {
            RaycastHit hitInfo;
            Ray Vision = new Ray(playerCam.transform.position, playerCam.transform.forward);

            if (Physics.Raycast(Vision, out hitInfo, playerReach))
            {
                string objectTag = hitInfo.collider.tag;
                switch (objectTag)
                {
                    case "Item":
                        EKeyHintActive();
                        break;
                    case "Ammo":
                        EKeyHintActive();
                        break;
                    default:
                        EKeyHintDisabled();
                        break;
                }
            }
            else
            {
                EKeyHintDisabled();
            }
        }

        public void EKeyHintActive()
        {
            E_Key.SetActive(true);
        }

        public void EKeyHintDisabled()
        {
            E_Key.SetActive(false);
        }
    }
}