using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "StatusEffects/PoisonEffect")]
public class PoisonEffect : StatusEffect
{
    public int totalDamage = 20;
    public float duration = 4f;
    public string rarety = "Common";

    public override void apply(GameObject target)
    {
        target.GetComponent<EnemyHealth>()?.StartCoroutine(applyPoison(target));
    }

    private IEnumerator applyPoison(GameObject target)
    {
        if (target == null) yield break;
        EnemyHealth ennemy = target.GetComponent<EnemyHealth>();

        int ticks = 4;
        float tickInterval = duration / ticks;
        int damagePerTick = totalDamage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            ennemy.takeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
    public string getRarety()
    {
        return rarety;
    }
}
