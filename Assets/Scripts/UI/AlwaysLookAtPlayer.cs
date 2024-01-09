using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AlwaysLookAtPlayer : MonoBehaviour
    {
        private void Update()
        {
            LookAtPlayer();
        }
        private void LookAtPlayer()
        {
            gameObject.transform.LookAt(PlayerStats.instance.transform.position);
        }
    }
}