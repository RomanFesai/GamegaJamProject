using System.Collections;
using TMPro;
using UnityEngine;
using Assets.Scripts.NPCs;
using UnityEngine.UI;
using Assets.Scripts.SaveLoad;

namespace Assets.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float Damage = 1f;
        [SerializeField] private float Range = 100f;
        [SerializeField] private float FireRate = 15f;
        [SerializeField] private float spread = 1;
        [SerializeField] private float aimSpeed = 1f;
        [SerializeField] private int bulletsPerTap, timeBetweenShots;
        [SerializeField] private bool gunready = false;
        [SerializeField] private bool isReloading = false;
        public static bool aim = false;
        [SerializeField] private float impactForce = 30f;
        private float nextTimeToFire = 0f;
        [SerializeField] private Animator weaponAnim;
        //Animator Recoil;

        [SerializeField] private GameObject shootHole;

        [Header("Effects")]
        [SerializeField] private GameObject hitmarker;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private GameObject bulletHole;

        [Header("Ammo")]
        [SerializeField] private GameObject FollowUI;
        public TextMeshProUGUI ammoInfo;
        [SerializeField] private int maxAmmo = 8;
        public static int magazineSize = 8;
        [SerializeField] private int _magazineSize;
        public static int currentAmmo = 3;
        public float reloadTime = 1f;
        private bool NoAmmo = false;
        private bool isAmmoCheck = false;
        private int bulletsShot;


        [Header("Reference Points:")]
        public Transform recoilPosition;
        public Transform rotationPoint;
        [Space(10)]

        [Header("Speed Settings:")]
        public float positionalRecoilSpeed = 8f;
        public float rotationalRecoilSpeed = 8f;
        [Space(10)]

        public float positionalReturnSpeed = 18f;
        public float rotationalReturnSpeed = 38f;
        [Space(10)]

        [Header("Amount Settings:")]
        public Vector3 RecoilRotation = new Vector3(10, 5, 7);
        public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
        [Space(10)]
        public Vector3 RecoilRotationAim = new Vector3(10, 4, 6);
        public Vector3 RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);
        [Space(10)]

        [Header("Aim Parameters")]
        [SerializeField] private Vector3 aimHandsPos = new Vector3(-0.0780000016f, -0.398000002f, -0.377999991f);
        [SerializeField] private Vector3 initialHandsPos = new Vector3(0f, -0.610000074f, -0.113000065f);

        Vector3 rotationalRecoil;
        Vector3 positionalRecoil;
        Vector3 Rot;

        private void Awake()
        {
            currentAmmo = maxAmmo;

            if (TempData.instance != null)
            { currentAmmo = TempData.instance.rifleAmmo; }

            magazineSize = _magazineSize;
        }

        private void OnEnable()
        {
            gunready = true;
            aim = false;

            if (shootHole == null || FollowUI == null || ammoInfo == null || recoilPosition == null || rotationPoint == null)
            {
                shootHole = GameObject.Find("ShootHole");
                FollowUI = GameObject.Find("Ammo");
                ammoInfo = GameObject.Find("currentAmmo").GetComponent<TextMeshProUGUI>();
                recoilPosition = GameObject.Find("Head").transform;
                rotationPoint = GameObject.Find("Hand").transform;
            }

            FollowUI.GetComponentInChildren<Image>().enabled = true;
            FollowUI.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }

        private void OnDisable()
        {
            gunready = false;
            aim = false;

            if(FollowUI != null)
            {
                FollowUI.GetComponentInChildren<Image>().enabled = false;
                FollowUI.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }

        protected void Start()
        {
            if (hitmarker != null)
                hitmarker.SetActive(false);
        }

        protected void Update()
        {
            ammoInfo.text = currentAmmo.ToString();

            Mouse();
            Recoil();
            if (aim && !PlayerMovementCC.isSprinting && !isAmmoCheck && gunready)
            {
                Aim();
            }
            else if (!aim || PlayerMovementCC.isSprinting || isAmmoCheck)
            {
                StopAim();
            }

            if(Input.GetMouseButtonDown(0))
            {
                PerformShoot();
            }

            /*weaponAnim.SetBool("isWalking", PlayerMovementCC.isWalking);
            weaponAnim.SetBool("isSprinting", PlayerMovementCC.isSprinting);*/

            Debug.DrawRay(shootHole.transform.position, shootHole.transform.forward * Range, Color.red);

            /*if (currentAmmo == 0 && magazineSize == 0)
            {
                NoAmmo = true;
                return;
            }
            else
            {
                NoAmmo = false;
            }

            if (isReloading)
            {
                return;
            }*/
        }

        private void StartReload()
        {
            if (!isReloading && gunready && currentAmmo != maxAmmo && magazineSize != 0 || currentAmmo == 0 && !isReloading && !isAmmoCheck)
            {
                StartCoroutine(Reload());
                return;
            }
        }

        protected void Mouse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                aim = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                aim = false;
            }
        }

        protected void Recoil()
        {
            rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
            positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

            recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
            Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
            rotationPoint.localRotation = Quaternion.Euler(Rot);
        }

        protected void PerformShoot()
        {
            if (gunready && Time.time >= nextTimeToFire && !NoAmmo && !isReloading /*&& !PlayerMovementCC.isSprinting*/ && currentAmmo != 0 && !isAmmoCheck)
            {
                bulletsShot = bulletsPerTap;
                nextTimeToFire = Time.time + 1f / FireRate;
                Shoot();
                AudioManager.GetInstance()?.Play("Shoot");
                weaponAnim.SetTrigger("Shoot");
                muzzleFlash.Play();
            }
            else if (Time.time >= nextTimeToFire && !isReloading)
            {
                AudioManager.GetInstance().Play("Click");
            }
        }

        protected void Shoot()
        {
            currentAmmo--;
            bulletsShot--;

            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate Direction with Spread
            Vector3 direction = shootHole.transform.forward + new Vector3(x, y, 0);

            RaycastHit hit;
            if (Physics.Raycast(shootHole.transform.position, direction, out hit, Range))
            {
                EnemyAi enemy = hit.transform.GetComponent<EnemyAi>();

                if (enemy != null)
                {
                    //HitActive();
                    //Invoke("HitDisabled", 0.2f);
                    Debug.Log("Hit");
                    enemy.TakeDamage(Damage);
                }

                if (hit.rigidbody != null /*&& enemy == null*/)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                if (hit.transform.tag == "Shootable")
                {
                    GameObject obj = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    Destroy(obj, 10f);
                }

            }

            rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
            positionalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);

            if(bulletsShot > 0 && currentAmmo > 0)
                Invoke(nameof(Shoot), timeBetweenShots);
        }

        protected void Aim()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimHandsPos, aimSpeed * Time.deltaTime);
        }

        protected void StopAim()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialHandsPos, aimSpeed * Time.deltaTime);
        }

        private void PerformCheckAmmo()
        {
            StartCoroutine(CheckAmmo());
        }

        private IEnumerator CheckAmmo()
        {
            if (isAmmoCheck == false && !isReloading)
            {
                isAmmoCheck = true;
                weaponAnim.Play("CheckAmmo");
                FollowUI.SetActive(true);
                yield return new WaitForSeconds(1.8f);
                FollowUI.SetActive(false);
                isAmmoCheck = false;
            }
        }

        protected IEnumerator Reload()
        {
            if (isReloading == false && magazineSize > 0)
            {
                isReloading = true;
                weaponAnim.SetBool("Reload", isReloading);
                yield return new WaitForSeconds(3f);
                if (magazineSize >= maxAmmo)
                {
                    magazineSize -= maxAmmo - currentAmmo;
                    currentAmmo += maxAmmo - currentAmmo;
                }
                else
                {
                    int oldCA = currentAmmo;
                    currentAmmo += magazineSize;
                    if (currentAmmo > maxAmmo)
                    {
                        magazineSize = currentAmmo - maxAmmo;
                        currentAmmo = maxAmmo;
                    }
                    else
                    {
                        currentAmmo = oldCA + magazineSize;
                        magazineSize = 0;
                    }
                }
                if (magazineSize < 0)
                    magazineSize = 0;
                isReloading = false;
                weaponAnim.SetBool("Reload", isReloading);
            }
        }

        public void HitActive()
        {
            hitmarker.SetActive(true);
        }

        public void HitDisabled()
        {
            hitmarker.SetActive(false);
        }
    }
}