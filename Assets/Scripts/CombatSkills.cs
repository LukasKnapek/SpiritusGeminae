using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSkills : MonoBehaviour
{
    public int health;

    void Start()
    {

    }

    void Update()
    {
        if(health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }

    public void decreaseHealth(int amount){
        health -= amount;
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag("Player")){
            Debug.Log("Decrease Player Health");
        }
    }
}
