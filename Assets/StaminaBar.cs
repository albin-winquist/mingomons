using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HealthBar;

public class StaminaBar : MonoBehaviour
{

    public float Stamina;
    public float MaxStamina;
    public Image staminaBar;
    public Slider staminaSlider;
    GameObject hpBar;

    float health;
    
    // Start is called before the first frame update
    void Start()
    {
        hpBar = GameObject.FindGameObjectWithTag("hpBar");
        Stamina = MaxStamina;

         
    }

    // Update is called once per frame
    void Update()
    {
        health = hpBar.GetComponentInChildren<Healthbar>().Health;
        if (staminaSlider.value != Stamina)
        {
            staminaSlider.value = Stamina;
        }
        if (Stamina < 100)
        {
            Stamina = Stamina+0.13f;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina >= 25)
        {
            SpendStamina(25);
           
        }
        if (Input.GetKeyDown(KeyCode.Q) && Stamina >= 30 && health < hpBar.GetComponentInChildren<Healthbar>().maxHealth)
        {
            SpendStamina(30);
        }
    }

    public void SpendStamina(float Spent)
    {
        Stamina -= Spent;
        staminaBar.fillAmount = Stamina / 100f;
    }
}
