using Assets.Scripts.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class HatchEnter : MonoBehaviour
    {
        [SerializeField] private bool playerInRange = false;
        [SerializeField] private GameObject HintKey;
        private GameObject player;

        private void Start()
        {
            playerInRange = false;
        }


        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TempData.instance.Save(TempData.instance);
                LevelLoader.GetInstance().LoadLevelByName("Bunker"); 
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                player = other.gameObject;
                HintKey.SetActive(true);
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                HintKey.SetActive(false);
                playerInRange = false;
            }
        }
    }
}