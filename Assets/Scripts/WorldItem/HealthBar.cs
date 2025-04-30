using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  
    public Image health;

    public void UpdateHealthBar(float fraction)
    {
        health.fillAmount = fraction;
    }

    void Update()
    {
        
    }
}
