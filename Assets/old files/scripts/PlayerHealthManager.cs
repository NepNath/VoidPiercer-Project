using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Management")]
   public float maxHealth;
   public float playerHealth;
   public float CoroutineSeconds;
   private float damage;
   public bool IsTakingDamages = false;
   public bool IsBeingHealed = false;

   // -------- Coroutines --------
   private Coroutine DamageCoroutine;
   private Coroutine HealCodroutine;
    // -------- References --------
   private DamageDealer dmg;
   private HealDealer Heal;


    private void Update()
    {
        // j'ai faim
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
        }
         if(other.CompareTag("HealZone"))
        {
            Debug.Log("Healing");
            StopCoroutine(HealCodroutine);
        }
    }

    IEnumerator TakeDamage(float Damage)
    {
        IsTakingDamages = true;
        while(playerHealth > 0)
        {
            Debug.Log("Taking Damages");
            yield return new WaitForSeconds(CoroutineSeconds);
            playerHealth -= Damage;
        }
        IsTakingDamages = false;
    }

    IEnumerator HealZone(float Heal)
    {
        IsBeingHealed = true;
        while(playerHealth < maxHealth)
        {
            yield return new WaitForSeconds(CoroutineSeconds);
            playerHealth += Heal;
        }
        IsBeingHealed = false;
    }

}
