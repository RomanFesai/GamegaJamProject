using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using Assets.Scripts.Inventory;

public class PlayerStats : MonoBehaviour
{
    public int maxHealht;
    public float currentHealht;
    public int damage;
    public int maxStamina;
    public float currentStamina;
    public float currentFreeze;

    public float staminaRegenTimer = 0;
    public float minusStaminaTimer = 0;
    public float freezeTimer = 0;

    [SerializeField] private Image HealthBar;
    [SerializeField] private Image StaminaBar;
    [SerializeField] private Image FreezeBar;

    public CanvasGroup DamageInformation;
    public GameObject GameOverWindow;

    [SerializeField] private bool isFreezing = true;
    public static PlayerStats instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
        }

        currentHealht = maxHealht;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = currentHealht / 100f;
        StaminaBar.fillAmount = currentStamina / 100f;
        FreezeBar.fillAmount = currentFreeze / 100f;

        if(currentHealht > 100)
            currentHealht = 100;

        if (currentStamina > 100)
            currentStamina = 100f;

        if (currentFreeze >= 100)
            currentFreeze = 100;

        /*if (PlayerMovementCC.isSprinting == false && currentStamina < maxStamina)
            StaminaRefill();
        if (PlayerMovementCC.isSprinting == true)
            StaminaDefilling(0.2f);*/
        Die();

        if (currentStamina == maxStamina)
            PlayerMovementCC.canSprint = true;
    }

    private void FixedUpdate()
    {
        if (PlayerMovementCC.isSprinting == false && currentStamina < maxStamina)
            StaminaRefill();
        if (PlayerMovementCC.isSprinting == true)
            StaminaDefilling(0.2f);

        if(isFreezing)
            FreezeRefill();

        if (currentFreeze == 100)
            DecreaseHealth();
    }

    public void StaminaRefill()
    {
        staminaRegenTimer += Time.deltaTime;
        if (currentStamina != maxStamina && staminaRegenTimer > .5f)
        {
            if (currentStamina <= 0)
            {
                //PlayerMovementCC.canSprint = false;
                if (currentStamina != maxStamina)
                {
                    currentStamina += 0.1f;
                }
            }
            currentStamina += 0.1f;
        }
    }

    public void FreezeRefill()
    {
        freezeTimer += Time.deltaTime;
        if (currentFreeze != 100 && freezeTimer > .5f)
        {
            currentFreeze += 0.05f;
        }
    }

    public void DecreaseHealth()
    {
        minusStaminaTimer += Time.deltaTime;
        if (minusStaminaTimer > .5f)
        {
            currentHealht -= 0.05f;
        }
    }

    public void FreezeDefill()
    {
        freezeTimer += Time.deltaTime;
        if (currentFreeze > 0 && freezeTimer > .5f)
        {
            currentFreeze -= 1f;
        }
    }

    public void StaminaDefilling(float amount)
    {
        minusStaminaTimer += Time.deltaTime;
        if (minusStaminaTimer > .5f)
        {
            currentStamina -= amount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "mixamorig:RightHand" || other.gameObject.name == "AttackBox")
        {
            DamageInformation.alpha = 1;
            TakeDamage(15);
            //CameraShake.instance?.Play(.1f, .4f);
            //AudioManager.instance.Play("Injured");
            StartCoroutine(DamageInfoFadeOut());
            Debug.Log("Thats Hurt");
        }
    }
    IEnumerator DamageInfoFadeOut()
    {
        if (DamageInformation.alpha > 0)
        {
            while (DamageInformation.alpha > 0)
            {
                yield return new WaitForSeconds(0.01f);
                DamageInformation.alpha -= 0.002f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealht -= damage;
    }

    public void Die()
    {
        if(currentHealht <= 0)
        {
            gameObject.GetComponent<PlayerMovementCC>().enabled = false;
            gameObject.GetComponentInChildren<MouseLook>().enabled = false;
            PlayerInventory.instance.DropItem();
            GameOverWindow.SetActive(true);
        }
    }
}
