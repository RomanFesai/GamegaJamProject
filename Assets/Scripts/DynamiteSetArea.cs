using Assets.Scripts.SaveLoad;
using System.Collections;
using TMPro;
using UnityEngine;
using Assets.Scripts.Translation;
using Assets.Scripts.ItemTools;

namespace Assets.Scripts
{
    public class DynamiteSetArea : MonoBehaviour
    {
        [SerializeField] private bool playerInRange = false;
        [SerializeField] private Animator ui_anim;
        [SerializeField] private TextMeshProUGUI ui_anim_text_mesh;
        [SerializeField] private string ui_anim_text;
        [SerializeField] private string ui_anim_text_eng;
        // Use this for initialization
        private GameObject player;
        [SerializeField] private GameObject dynamite;
        [SerializeField] private Rigidbody apple;

        private void Start()
        {
            playerInRange = false;
        }


        private void Update()
        {
            if (playerInRange && Dynamite.itemReady)
            {
                if (Language.instance?.currentLanguage == "Русский")
                    ui_anim_text_mesh.text = ui_anim_text;
                else if (Language.instance?.currentLanguage == "English")
                    ui_anim_text_mesh.text = ui_anim_text_eng;
                ui_anim.SetBool("ShowHintEnter", true);
            }
            else { ui_anim.SetBool("ShowHintEnter", false); }

            if (playerInRange && Dynamite.itemReady && Input.GetMouseButtonDown(0))
            {
                dynamite.SetActive(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && Dynamite.itemReady)
            {
                player = other.gameObject;/*
                if (Language.instance?.currentLanguage == "Русский")
                    ui_anim_text_mesh.text = ui_anim_text;
                else if (Language.instance?.currentLanguage == "English")
                    ui_anim_text_mesh.text = ui_anim_text_eng;
                ui_anim.SetBool("ShowHintEnter", true);*/
                Dynamite.playerInArea = true;
            }
            if(other.gameObject.tag == "Player")
            {
                playerInRange = true;
                apple.isKinematic = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                ui_anim.SetBool("ShowHintEnter", false);
                playerInRange = false;
                Dynamite.playerInArea = false;
            }
        }
    }
}