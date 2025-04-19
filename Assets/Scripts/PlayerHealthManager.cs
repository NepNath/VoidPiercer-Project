using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Management")]
   public float maxHealth;
   public float playerHealth;
   public float DamageCoroutineSeconds;
   public float HealingCoroutineSeconds;
   private float damage;
   public bool IsTakingDamages = false;
   public bool IsBeingHealed = false;

   // -------- Coroutines --------
   private Coroutine DamageCoroutine;
   private Coroutine HealCodroutine;
    // -------- References --------


    void Start()
    {
        playerHealth = maxHealth;
    }

    private void Update()
    {
        if(playerHealth > maxHealth)
        {
            Debug.Log("PlayerHealth is greater than max health");
            playerHealth = maxHealth;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DamageDealer dmg = other.GetComponent<DamageDealer>();
        HealDealer heal = other.GetComponent<HealDealer>();

        Debug.Log("Entered Trigger");

        if(other.CompareTag("DealsDamage"))
        {
            Debug.Log("Dealing Damages");
            playerHealth -= dmg.Damages;
            DamageCoroutine = StartCoroutine(TakeDamage(dmg.Damages));
        }

        if(other.CompareTag("HealZone"))
        {
            Debug.Log("Healing");
            HealCodroutine = StartCoroutine(HealZone(heal.Health));
        }
    }


    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Damage Trigger");
        if(other.CompareTag("DealsDamage"))
        {
            StopCoroutine(DamageCoroutine);
            DamageCoroutine = null;
            IsTakingDamages = false;
        }
         if(other.CompareTag("HealZone"))
        {
            Debug.Log("Healing");
            StopCoroutine(HealCodroutine);
            IsBeingHealed = false;
        }
    }

    IEnumerator TakeDamage(float Damage)
    {
        IsTakingDamages = true;
        while(playerHealth > 0)
        {
            Debug.Log("Taking Damages");
            yield return new WaitForSeconds(DamageCoroutineSeconds);
            playerHealth -= Damage;
        }
        IsTakingDamages = false;
    }

    IEnumerator HealZone(float Heal)
    {
        IsBeingHealed = true;
        while(playerHealth < maxHealth)
        {
            yield return new WaitForSeconds(HealingCoroutineSeconds);
            playerHealth += Heal;
        }
        IsBeingHealed = false;
    }

}
