using System.Collections;
using System.Collections.Generic;
using HealthBar;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float health;
    // Start is called before the first frame update
    float timer;
    private bool isDead = false;
    private ParticleSystem[] particleSystems;


    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        health = gameObject.GetComponentInChildren<EnemyHealth>().currHealth;
        if (health <= 0 && !isDead)
        {
            isDead = true;

            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }


            spriteRenderer.enabled = false;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
            

        }
        if(health <= 0)
        {
            timer += Time.deltaTime;
        }
        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }
}
