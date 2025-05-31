using UnityEngine;
using System.Collections.Generic;

public class PlayerEffects : MonoBehaviour
{
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    public void addEffect(StatusEffect effect)
    {
        if (!activeEffects.Contains(effect))
            activeEffects.Add(effect);
    }

    public List<StatusEffect> getActiveEffects()
    {
        return activeEffects;
    }
}
