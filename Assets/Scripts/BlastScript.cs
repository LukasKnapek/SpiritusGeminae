using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastScript : MonoBehaviour
{
    LineRenderer laser;
    
    void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().spiritMode == true)
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }

    IEnumerator FireLaser()
    {
        laser.enabled = true;

        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);

            laser.SetPosition(0, ray.origin);
            laser.SetPosition(1, ray.GetPoint(100));

            yield return null;
        }

        laser.enabled = false;
    }
}
