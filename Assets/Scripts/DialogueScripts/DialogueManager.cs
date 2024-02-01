using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System;
using Cinemachine;
using Assets.Scripts;
using Assets.Scripts.SaveLoad;
using UnityEngine.SceneManagement;
using Assets.Scripts.DialogueScripts;

public class DialogueManager : MonoBehaviour/*, IDataPersistence*/
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset LoadGlobalsJSON;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private GameObject continueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private Coroutine displayLineCoroutine;

    private bool canContinueToNextLine = false;

    private DialogueVariables dialogueVariables;

    private const string SPEAKER_TAG = "speaker";

    private const string LAYOUT_TAG = "layout";

    private const string AUDIO_TAG = "audio";

    [SerializeField] private GameObject mainPlayerCam;

    [SerializeField] private GameObject dialoguePlayerCam;

    [SerializeField] private GameObject player;

    [SerializeField] private Animator animator;

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;

    [SerializeField] private bool makePredictable;

    private DialogueAudioInfoSO currentAudioInfo;

    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;

    private AudioSource audioSource;

    //Utilities utilities;

    private InkExternalFunctions inkExternalFunctions;

    //public string jsonState;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(LoadGlobalsJSON);
        inkExternalFunctions = new InkExternalFunctions();

        audioSource = this.gameObject.AddComponent<AudioSource>();

        currentAudioInfo = defaultAudioInfo;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //mainPlayerCam = GameObject.Find("PlayerCM");
        //dialoguePlayerCam = GameObject.Find("DialogueCM");
        //player = GameObject.Find("Player");

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;

        if (choices != null)
        {
            foreach (GameObject choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }
        }

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach(DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if (audioInfo != null) 
        {
            this.currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }  
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 && (Input.anyKey || Input.GetKeyDown(KeyCode.Space)))
        {
            ContinueStory();
        }

        //Prevents from deselect of answer choices, which leaded to soft lock 
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            StartCoroutine(SelectFirstChoice());
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        if (player != null)
        {
            player.GetComponent<PlayerMovementCC>().enabled = false;
            animator.SetBool("PlayerMoving", false);
        }
        dialogueVariables.StartListening(currentStory);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        if (mainPlayerCam != null)
            mainPlayerCam.SetActive(true);
        if (dialoguePlayerCam != null)
            dialoguePlayerCam.SetActive(false);
        if (player != null) 
            player.GetComponent<PlayerMovementCC>().enabled = true;

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        /*if (dialogueVariables != null)
            dialogueVariables.SaveVariables();*/
        SetCurrentAudioInfo(defaultAudioInfo.id);

        //DataPersistenceManager.instance.SaveGame();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            if (mainPlayerCam != null) 
                mainPlayerCam.SetActive(false);
            if (dialoguePlayerCam != null) 
                dialoguePlayerCam.SetActive(true);

            string nextLine = currentStory.Continue();
            //handle case where the last line is an external function
            if(nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else
            {
                //handle tags
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
            if(TimelinePlayer.instance!= null)
                TimelinePlayer.GetInstance().StartTimeline();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2 ) 
            {
                Debug.LogError("Tag couldn't be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case LAYOUT_TAG:
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);

        if(choices.Length > 0)
            DisplayChoices();

        canContinueToNextLine = true;
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        if(currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            AudioClip soundClip = null;

            if(makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];

                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt != 0) 
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

            }
            audioSource.PlayOneShot(soundClip);
        }
    } 

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than UI can supprot. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.Log("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    /*public void LoadData(GameData data)
    {
        DialogueVariables.jsonState = data.answers;
    }

    public void SaveData(ref GameData data)
    {
        if (this != null)
        {
            dialogueVariables.VariablesToStory(dialogueVariables.globalVariablesStory);
            data.answers = this.dialogueVariables.globalVariablesStory.state.ToJson();
        }
    }*/
}