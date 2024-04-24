using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HealthBar
{
    public class Healthbar : MonoBehaviour
    {
     public Image healthBar;
     public Slider healthSlider;
     public float maxHealth = 100f;
     public float Health;
    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
            Debug.Log(Health);
        if (healthSlider.value != Health)
        {
            healthSlider.value = Health;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamg(20);

        }

        if (Input.GetKeyDown(KeyCode.Q) && Health != maxHealth)
            {
                Heal(20);
            }

        if (Health <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void TakeDamg(float damage)
    {
        Health -= damage;
        healthBar.fillAmount = Health / 100;
    }

    public void Heal(float Healing)
    {
        Health += Healing;
        Health = Mathf.Clamp(Health, 0, 100);
        healthBar.fillAmount = Health / 100;
    }


}


}
