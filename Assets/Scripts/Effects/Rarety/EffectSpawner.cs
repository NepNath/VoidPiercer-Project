using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effectItemPrefab;
    public List<RaritySpawnChance> rarityChances;
    public List<EffectByRarity> effectsByRarity;

    public void SpawnRandomEffect(Vector3 position)
    {
        Rarity selectedRarity = GetRandomRarity();
        StatusEffect selectedEffect = GetRandomEffectOfRarity(selectedRarity);

        if (selectedEffect == null) return;

        GameObject pickup = Instantiate(effectItemPrefab, position, Quaternion.identity);
        pickup.GetComponent<EffectPickup>().effect = selectedEffect;
    }

    private Rarity GetRandomRarity()
    {
        int totalWeight = 0;
        foreach (var rc in rarityChances)
            totalWeight += rc.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var rc in rarityChances)
        {
            cumulative += rc.weight;
            if (roll < cumulative)
                return rc.rarity;
        }

        return rarityChances[0].rarity;
    }

    private StatusEffect GetRandomEffectOfRarity(Rarity rarity)
    {
        var entry = effectsByRarity.Find(e => e.rarity == rarity);
        if (entry == null || entry.effects.Count == 0) return null;

        int randomIndex = Random.Range(0, entry.effects.Count);
        return entry.effects[randomIndex];
    }
}