using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateInteraction : MonoBehaviour
{
    public bool pressed;
    void Start()
    {
        pressed = false;
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().spiritMode == false)
            {
                transform.localScale = new Vector3(1f, 0.025f, 1f);
                pressed = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().spiritMode == false)
            {
                transform.localScale = new Vector3(1f, 0.1f, 1f);
                pressed = false;
            }
        }
    }
}
