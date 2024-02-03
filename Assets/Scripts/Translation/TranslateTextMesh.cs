using Assets.Scripts.Inventory;
using Assets.Scripts.NPCs;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Translation
{
    public class TranslateTextMesh : MonoBehaviour
    {
        [TextArea][SerializeField] private string ruText;
        [TextArea][SerializeField] private string enText;

        private TextMeshProUGUI objTextMesh;
        private Text objText;
        private void Awake()
        {
            objTextMesh = gameObject.GetComponent<TextMeshProUGUI>();
            objText = gameObject.GetComponent<Text>();
        }

        private void Update()
        {
            if (objTextMesh != null)
            {
                if (Language.instance?.currentLanguage == "English")
                    objTextMesh.text = enText;
                else if (Language.instance?.currentLanguage == "Русский")
                    objTextMesh.text = ruText;
            }
            else if(objText != null)
            {
                if (Language.instance?.currentLanguage == "English")
                    objText.text = enText;
                else if (Language.instance?.currentLanguage == "Русский")
                    objText.text = ruText;
            }
        }
    }
}