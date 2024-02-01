using Assets.Scripts;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    private void Awake()
    {
        LevelLoader.GetInstance().LoadNextLevel();
    }
}
