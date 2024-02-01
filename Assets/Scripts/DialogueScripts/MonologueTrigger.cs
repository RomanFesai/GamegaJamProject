using Cinemachine;
using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool playerInRange;

    private void Awake()
    {
        //playerInRange = false;
        TimelinePlayer.GetInstance().PauseTimeline();
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
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