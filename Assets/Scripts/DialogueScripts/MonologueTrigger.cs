using Assets.Scripts.Translation;
using Cinemachine;
using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON_russian;
    [SerializeField] private TextAsset inkJSON_english;
    private bool playerInRange;

    private void Awake()
    {
        //playerInRange = false;
        TimelinePlayer.GetInstance().PauseTimeline();

        if(Language.instance.currentLanguage == "English")
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON_english);
        else if (Language.instance.currentLanguage == "Русский" && inkJSON_russian != null)
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON_russian);
    }

  /*  private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            TimelinePlayer.GetInstance().PauseTimeline();
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);   
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }*/
}