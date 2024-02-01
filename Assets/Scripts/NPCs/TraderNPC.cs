using Assets.Scripts.Inventory;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class TraderNPC : EnemyAi
    {
        [Header("Dialogue Box Parameters")]
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private Animator textBoxAnimator;

        [TextAreaAttribute]
        [SerializeField] private string message;

        [Header("Trade")]
        [SerializeField] private GameObject toGet;
        [SerializeField] private string toGiveName;
        [SerializeField] private bool tradeAvailable;
        [SerializeField] private Animator ui_anim;
        [SerializeField] private TextMeshProUGUI ui_anim_text_mesh;
        [SerializeField] private string ui_anim_text;
        private void Update()
        {
            InitAiBehaviour();
        }

        protected override void InitAiBehaviour()
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            //Patroling();
            //GoToPoint();

            //if (!playerInSightRange && !playerInAttackRange) anim.SetBool("Chase", false);
            if (playerInSightRange)
            {
                anim.Play("Trader");
                if (tradeAvailable)
                { 
                    textBoxAnimator.SetBool("playerInRange", playerInSightRange);
                    ui_anim_text_mesh.text = ui_anim_text;
                    ui_anim.SetBool("ShowHintTrade", true);
                }
                textMeshPro.text = message;
                agent.isStopped = true;
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                if (Input.GetKeyDown(KeyCode.E) && tradeAvailable)
                {
                    Trade(toGet, toGiveName);
                }
            }
            else if(!playerInSightRange)
            {
                ui_anim.SetBool("ShowHintTrade", false);
                textBoxAnimator.SetBool("playerInRange", false);
                agent.isStopped = false;
                GoToPoint();
            }
        }

        public override void die()
        {
            GetComponent<Animator>().Play("dead");
            gameObject.layer = 9;
            TraderNPC enemy = GetComponent<TraderNPC>();
            agent.isStopped = true;
            textBoxAnimator.SetBool("playerInRange", false);
            enemy.enabled = false;
            /*if (tradeAvailable)
            {
                toGet.SetActive(true);
                toGet.transform.parent = null;
                tradeAvailable = false;
                keyHint.SetActive(false);
            }*/
            Destroy(gameObject, 10f);
        }

        private void Trade(GameObject toGet, string toGiveName)
        {
            if (toGiveName == "free")
            {
                toGet.SetActive(true);
                toGet.transform.parent = null;
                textBoxAnimator.SetBool("playerInRange", false);
                tradeAvailable = false;
                ui_anim.SetBool("ShowHintTrade", false);
                return;
            }

            if (toGet != null && toGiveName != null)
            {
                foreach (var obj in PlayerInventory.instance.inventoryList)
                {
                    if (obj.activeInHierarchy == true && obj.transform.childCount > 0 && obj.transform.GetChild(0).gameObject.name == toGiveName)
                    {
                        var item = obj.transform.GetChild(0).gameObject;
                        PlayerInventory.instance.DropItem();
                        Destroy(item);
                        toGet.SetActive(true);
                        toGet.transform.parent = null;
                        textBoxAnimator.SetBool("playerInRange", false);
                        tradeAvailable = false;
                        ui_anim.SetBool("ShowHintTrade", false);
                    }
                    else if (obj.activeInHierarchy == true && obj.transform.childCount > 0 && obj.transform.GetChild(0).gameObject.name != toGiveName || obj.activeInHierarchy == true && obj.transform.childCount < 1)
                    {
                        AudioManager.instance.Play("Blocked");
                    }
                }
            }
        }
    }
}