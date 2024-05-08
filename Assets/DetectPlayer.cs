using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectPlayer : MonoBehaviour
{
    Transform player;
   [SerializeField] public bool PlayerInArea;
    string LocateTag = "player";
    float speed = 2f;
    float raycastDistance = 1.5f;
    float avoidanceForce = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(LocateTag))
        {
            PlayerInArea = true;
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(LocateTag))
        {
            PlayerInArea = false;
            player = null;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (PlayerInArea && player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance);

            if (hit.collider != null && !hit.collider.CompareTag("player"))
            {
                Vector3 avoidDir = Vector3.Cross(Vector3.forward, direction);
                direction += avoidDir * avoidanceForce;
                direction.Normalize();
            }

            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
