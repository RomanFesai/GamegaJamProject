using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class PlaySoundOnStart : MonoBehaviour
    {
        [SerializeField] private string songName;
        // Use this for initialization
        void Start()
        {
            AudioManager.instance.StopAll();
            AudioManager.instance.Play(songName);
        }
    }
}