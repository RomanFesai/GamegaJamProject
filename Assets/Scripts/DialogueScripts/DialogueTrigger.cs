using Cinemachine;
using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    [SerializeField] private GameObject ActionKeyHint = default;
    [SerializeField] private CanvasGroup CanvasActionKeyHint = default;

    [SerializeField] private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        //targetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            ActionKeyHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.X))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                StartCoroutine(FadeOutActionKeyHint());
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            targetGroup.AddMember(gameObject.transform, 1, 0);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            StartCoroutine(FadeOutActionKeyHint());
            targetGroup.RemoveMember(gameObject.transform);
        }
    }

    IEnumerator FadeOutActionKeyHint()
    {
        if (CanvasActionKeyHint.alpha > 0)
        {
            while (CanvasActionKeyHint.alpha > 0)
            {
                yield return new WaitForSeconds(0.01f);
                CanvasActionKeyHint.alpha -= 0.1f;
            }
        }
        ActionKeyHint.SetActive(false);
        CanvasActionKeyHint.alpha = 1f;
    }
}