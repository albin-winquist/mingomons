using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectPlayer : MonoBehaviour
{
    Transform player;
   [SerializeField] public bool PlayerInArea;
    string LocateTag = "player";
    
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("player").transform;
        
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
        if (PlayerInArea)
        {

        }
    }
}
