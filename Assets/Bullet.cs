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
    CapsuleCollider2D yupsers;
    GameObject explosion;
    [SerializeField] int trailPower;
    private float timer = 0;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        explosion = GameObject.FindGameObjectWithTag("ExplosionTag");
        trail = GetComponent<TrailRenderer>();
         spriteRenderer = spriteRendererTe.GetComponent<SpriteRenderer>();

    }
    public void Update()
    {
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        
        spriteRenderer.enabled = false;
        CapsuleCollider2D yup = yupsers.GetComponent<CapsuleCollider2D>();
        yupsers.enabled = false;
      //  Destroy(gameObject);
    }

    
}
