using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 PointerPosition { get; set; }
    private void Update()
    {
        transform.right = (PointerPosition - (Vector2)transform.position).normalized;
    }
}

