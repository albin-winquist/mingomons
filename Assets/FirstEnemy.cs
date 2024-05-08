using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstEnemy : MonoBehaviour
{

    //[SerializeField]float speed = 3f;
    private Rigidbody2D rb;
    private Transform player;
    bool PlayerDetected;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

       player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       

        Vector2 dir = player.position - transform.position;


        //transform.Translate(dir * speed * Time.deltaTime);

    }
}
