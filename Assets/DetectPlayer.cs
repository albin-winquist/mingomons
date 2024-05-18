using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private Transform player;
    [SerializeField] private bool playerInArea = false;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float raycastDistance = 1.5f;
    [SerializeField] private float avoidanceForce = 3f;
    private string playerTag = "player";
    public float maxSpeed;

    private void Start()
    {
        GameObject Detecotronamachine = GameObject.FindWithTag("Enemy");
        // Attempt to find the player at the start
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            playerInArea = true;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            playerInArea = false;
            player = null;
        }
    }
    
    private void Update()
    {
        if (playerInArea == true)
        {
            speed = 3f;
        }
    }
}
