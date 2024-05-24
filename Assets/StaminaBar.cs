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
    public GameObject hpBar;
    GameObject menu;
    int starter = 0;
    float health;
    float minusVal;
    float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        hpBar = GameObject.FindGameObjectWithTag("hpBar");
        menu = GameObject.FindGameObjectWithTag("MenuTag");

        Stamina = MaxStamina;

         
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Stamina < 26)
        {
            menu.GetComponent<ScoreChanger>().GetMana(true);
        }
        if (Input.GetKeyDown(KeyCode.F) && Stamina < 40)
        {
            menu.GetComponent<ScoreChanger>().GetMana(true);
        }
        if (starter > 0)
        {
            timer += Time.deltaTime;
        }

        health = hpBar.GetComponentInChildren<Healthbar>().Health;

        
        if (staminaSlider.value != Stamina)
        {
            staminaSlider.value = Stamina;
        }
        if (timer > 0)
        {
            if (Stamina < 100)
            {
                Stamina = Stamina + 0.01f + minusVal;
            }
            if (timer > 10)
            {
                starter = 0;
                timer = 0;
            }
        }
        if (Stamina <= 0)
        {
            Stamina = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina >= 25)
        {
            SpendStamina(25, 0);
           
        }
        if (Input.GetKeyDown(KeyCode.Q) && Stamina >= 30 && health < hpBar.GetComponentInChildren<Healthbar>().maxHealth)
        {
            SpendStamina(30, 0);
            
        }
    }

    public void SpendStamina(float Spent, float Health)
    {
        Stamina -= Spent; 
        staminaBar.fillAmount = Stamina / 100f;
        hpBar.GetComponentInChildren<Healthbar>().Heal(Health);
    }

    public void ManaCharge(int num)
    {
        starter += num;
    }
}
