using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            Debug.Log("No collider found on Gun object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
