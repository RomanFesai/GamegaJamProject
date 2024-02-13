using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;
    public GameObject controlPanel;
    public static TimelinePlayer instance;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static TimelinePlayer GetInstance()
    {
        return instance;
    }

    private void Director_Played(PlayableDirector obj)
    {
        if (controlPanel != null)
            controlPanel.SetActive(false);
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        if(controlPanel != null)
            controlPanel.SetActive(true);
    }

    public void StartTimeline()
    {
        if (animator != null)
            animator.speed = 1;

        director.Play();
    }

    public void PauseTimeline()
    {
        if(animator != null)
            animator.speed = 0;

        director.Pause();
    }
}
