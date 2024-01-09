using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class RedHeatEND : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(Ending());
            }
        }

        IEnumerator Ending()
        {
            anim.Play("FadeIn");
            yield return new WaitForSeconds(2f);
            AudioManager.instance.Play("ManScream");
            yield return new WaitForSeconds(6f);
            Application.Quit();
        }
    }
}