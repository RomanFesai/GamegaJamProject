using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;
    public GameObject controlPanel;
    public static TimelinePlayer instance;
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
        controlPanel.SetActive(false);
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        if(controlPanel != null)
            controlPanel.SetActive(true);
    }

    public void StartTimeline()
    {
        director.Play();
    }

    public void PauseTimeline()
    {
        director.Pause();
    }
}
