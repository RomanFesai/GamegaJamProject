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
                //StartCoroutine(Ending());

                AudioManager.instance.Play("JSFXn2");
                LevelLoader.GetInstance().LoadLevelByName("OtherRealm");
            }
        }

        IEnumerator Ending()
        {
            AudioManager.instance.Play("JSFXn2");
            yield return new WaitForSeconds(1f);
            anim.Play("FadeIn");
            yield return new WaitForSeconds(2f);
            AudioManager.instance.Play("ManScream");
            yield return new WaitForSeconds(6f);
            Application.Quit();
        }
    }
}