using Assets.Scripts.Inventory;
using Assets.Scripts.NPCs;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ItemTools
{
    public class MatchBox : MonoBehaviour
    {
        [SerializeField] private bool itemReady = false;
        [SerializeField] private GameObject ray;

        private void OnEnable()
        {
            itemReady = true;

            if(ray == null)
            {
                ray = GameObject.Find("ShootHole");
            }
        }

        private void OnDisable()
        {
            itemReady = false;
        }

        private void Update()
        {
            LitFire();
        }

        private void LitFire()
        {
            RaycastHit hit;
            if (Physics.Raycast(ray.transform.position, ray.transform.forward, out hit, 3))
            {
                var obj = hit.collider.gameObject;
                
                if (obj != null && obj.tag == "Bonfire")
                {
                    if (Input.GetMouseButtonDown(0) && itemReady && obj.GetComponent<Bonfire>().bonfireLit == false)
                    {
                        AudioManager.instance.Play("Matches");
                        obj.GetComponent<Bonfire>().bonfireLit = true;
                        obj.GetComponent<SphereCollider>().enabled = true;
                        obj.transform.GetChild(2).gameObject.SetActive(true);
                        PlayerInventory.instance.DropItem();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}