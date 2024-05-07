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
    
    GameObject explosion;
    ParticleSystem particleAccelerator;

    Transform test;
    [SerializeField] int trailPower;
    
    Vector3 dir;
    private float timer = 0;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        explosion = GameObject.FindGameObjectWithTag("ExplosionTag");
        particleAccelerator = GetComponentInChildren<ParticleSystem>();
        test = GetComponentInChildren<Transform>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        

    }
    public void Update()
    {
        
       // Debug.Log(dir);
        if (trail != null)
        {
            trail.widthMultiplier = player.GetComponent<Movement>().chargePower / trailPower;
        } // player.GetComponent<Movement>().chargePower;
        timer += Time.deltaTime;
        if (timer > 5)
        {
            Destroy(gameObject);
        }
       // Debug.Log(timer);
    }

    private void OnTriggerEnter2D(Collider2D trigger)   
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        if (particleAccelerator != null)
        {
            particleAccelerator.Play();
        }
        spriteRenderer.enabled = false;

        numOfHit++;

    }
    

}
