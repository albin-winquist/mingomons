using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    TrailRenderer trail;
    GameObject player;
    [SerializeField] int trailPower;
    private float timer = 0;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        trail = GetComponent<TrailRenderer>();

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
        Debug.Log(timer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    
}
