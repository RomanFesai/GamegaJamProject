using Assets.Scripts.SaveLoad;
using Assets.Scripts.Translation;
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
        [SerializeField] private string ui_anim_text_eng;

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
                if(Language.instance?.currentLanguage == "Русский")
                    ui_anim_text_mesh.text = ui_anim_text;
                else if (Language.instance?.currentLanguage == "English")
                    ui_anim_text_mesh.text = ui_anim_text_eng;
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