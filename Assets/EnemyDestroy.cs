using System.Collections;
using System.Collections.Generic;
using HealthBar;
using Pathfinding;
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
    GameObject player;
    GameObject menu;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        menu = GameObject.FindGameObjectWithTag("MenuTag");
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
            player.GetComponent<Movement>().ScreenShake(4, 2);
            player.GetComponentInChildren<StaminaBar>().ManaCharge(1);
            menu.GetComponent<ScoreChanger>().GetScore(1);
            spriteRenderer.enabled = false;
            GetComponentInChildren<BoxCollider2D>().enabled = false;
            GetComponentInChildren<PolygonCollider2D>().enabled = false;
            GetComponentInChildren<EnemyFlipGFX>().enabled = false;
            GetComponentInChildren<Rigidbody2D>().simulated = false;
            GetComponent<Seeker>().enabled = false;
            Destroy(GetComponent<AIPath>());



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
