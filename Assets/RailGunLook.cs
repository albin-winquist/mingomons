using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunLook : MonoBehaviour
{

    // Start is called before the first frame update
    Vector2 mousePosition;
    public float aimAngle;
    public Rigidbody2D rb;
    Rigidbody2D parentRB;
    void Start()
    {
        

    }
    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 aimDir = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
       


    }
}
