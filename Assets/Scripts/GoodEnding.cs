using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GoodEnding : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        // Use this for initialization
        private void Awake()
        {
            StartCoroutine(Ending());
        }

        IEnumerator Ending ()
        {
            anim.Play("FadeIn");
            yield return new WaitForSeconds(2f);
            AudioManager.instance.Play("RescueHelicopter");
        }
    }
}