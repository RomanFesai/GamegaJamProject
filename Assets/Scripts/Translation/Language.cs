using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Translation
{
    [System.Serializable]
    public class Language : MonoBehaviour
    {
        public string currentLanguage = "English";

        public static Language instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Found more than one Language script in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}