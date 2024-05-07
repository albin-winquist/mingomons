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
    GameObject particleAccelerator;
    [SerializeField] int trailPower;
    Transform currPos;
    Transform playerPos;
    Transform ExpPos;
    Vector3 dir;
    private float timer = 0;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        explosion = GameObject.FindGameObjectWithTag("ExplosionTag");
        particleAccelerator = GameObject.FindGameObjectWithTag("EnemyHitPTag");
        ExpPos = particleAccelerator.GetComponent<Transform>();
        playerPos = player.GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
         spriteRenderer = GetComponent<SpriteRenderer>();
        currPos = GetComponent<Transform>();

    }
    public void Update()
    {
        
        Debug.Log(dir);
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
    public void dozing()
    {

       
        particleAccelerator.GetComponent<ParticleSystem>().Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        explosion.GetComponent<Transform>().localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
       // explosion.GetComponent<ParticleSystem>().Play();
        dozing();   
        
        Debug.Log(spriteRenderer.enabled);
        spriteRenderer.enabled = false;
        Debug.Log(spriteRenderer.enabled);
        Destroy(GetComponent<Rigidbody2D>());
        GetComponent<CapsuleCollider2D>().enabled = false;
       // explosion.GetComponent<Transform>().localPosition = new Vector3(0,0,0);   
        //  Destroy(gameObject);
    }

    
}
