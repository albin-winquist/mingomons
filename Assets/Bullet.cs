using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    TrailRenderer trail;
    GameObject player;
    GameObject spriteRendererTe;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    int numOfHit = 0;
    float damage;
    GameObject enemy;

    GameObject explosion;
    ParticleSystem particleAccelerator;
    float enemyHealth;
    Transform test;
    [SerializeField] int trailPower;
    private float startSpeed;
    private float startSize;
    Vector3 dir;
    float piercingPower = 11;
    private float timer = 0;

    public void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("EnemyTag");
        player = GameObject.FindGameObjectWithTag("player");
        explosion = GameObject.FindGameObjectWithTag("ExplosionTag");
        particleAccelerator = GetComponentInChildren<ParticleSystem>();

        test = GetComponentInChildren<Transform>();
        startSpeed = particleAccelerator.startSize;
        startSize = particleAccelerator.startSpeed;
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (trail != null)
        {
            trail.widthMultiplier = player.GetComponent<Movement>().chargePower / trailPower;
        }


    }
    public void Update()
    {
        
       // Debug.Log(dir);
        if (trail != null)
        {
            //trail.widthMultiplier = player.GetComponent<Movement>().chargePower / trailPower;
        } // player.GetComponent<Movement>().chargePower;
        timer += Time.deltaTime;
        if (timer > 5)
        {
            Destroy(gameObject);
        }
       // Debug.Log(timer);
    }
    private void particleAccelGun()
    {
        
        particleAccelerator.Play();
    }

    private void OnTriggerEnter2D(Collider2D trigger)   
    {
        
        if(trigger.GetComponent<EnemyHealth>() != null)
        {
           enemyHealth = trigger.GetComponent<EnemyHealth>().currHealth;
        }
        float damageS = player.GetComponent<Movement>().chargePower / 10;

        if (trigger.CompareTag("EnemyTag"))
        {
            if(gameObject.name == "bullet(Clone)")
            {
                damage = 1;
            }
            else
            {
                damage = Mathf.Ceil(player.GetComponent<Movement>().chargePower / 10);
                piercingPower -= (1/damage) * 10;
            }


            trigger.GetComponent<EnemyHealth>().takeDamage(damage);
            
        }
        if(trigger.GetComponent<EnemyHealth>() != null)
        {
            if (trigger.GetComponent<EnemyHealth>().currHealth < piercingPower && gameObject.name == "railBullet1(Clone)" && enemyHealth < damageS)
            {

            }
            else
            {
                piercingPower = 11;
                GetComponent<Rigidbody2D>().simulated = false;
                GetComponent<CapsuleCollider2D>().enabled = false;
                spriteRenderer.enabled = false;
            }

            if (particleAccelerator != null)
            {
                particleAccelGun();
            }
        }
        


        numOfHit++;

    }
    

}
