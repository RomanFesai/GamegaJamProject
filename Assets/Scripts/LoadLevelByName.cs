using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LoadLevelByName : MonoBehaviour
    {
        [SerializeField] private string levelName;
        private void Awake()
        {
            LevelLoader.GetInstance().LoadLevelByName(levelName);
        }
    }
}