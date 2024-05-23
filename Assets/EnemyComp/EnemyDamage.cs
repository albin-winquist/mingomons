using System.Collections;
using System.Collections.Generic;
using HealthBar;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    GameObject playerFinder;
    string damageTag = "player";
    // Start is called before the first frame update
    void Start()
    {
        playerFinder = GameObject.FindGameObjectWithTag("player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
     
    }


    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag(damageTag) && playerFinder != null)
        { 
            playerFinder.GetComponentInChildren<Healthbar>().TakeDamg(2);
        }
    }
}
