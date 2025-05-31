using UnityEngine;
public abstract class StatusEffect : ScriptableObject
{
    public abstract void apply(GameObject target);
}