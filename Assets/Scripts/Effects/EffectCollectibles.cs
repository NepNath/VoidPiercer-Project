using UnityEngine;
public class EffectCollectibles : MonoBehaviour
{
    public StatusEffect effect;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerEffects>()?.addEffect(effect);
            Destroy(gameObject);
        }
    }
}