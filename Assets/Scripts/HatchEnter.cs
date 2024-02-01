using Assets.Scripts.SaveLoad;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class HatchEnter : MonoBehaviour
    {
        [SerializeField] private bool playerInRange = false;
        [SerializeField] private Animator ui_anim;
        [SerializeField] private TextMeshProUGUI ui_anim_text_mesh;
        [SerializeField] private string ui_anim_text;
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
                ui_anim_text_mesh.text = ui_anim_text;
                ui_anim.SetBool("ShowHintEnter", true);
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                ui_anim.SetBool("ShowHintEnter", false);
                playerInRange = false;
            }
        }
    }
}