using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public bool interacted;
    public bool pressed;
    public bool released;
    int countdown;

    void Start()
    {
        countdown = 60;
    }
    
    void Update()
    {
        if(interacted && !released){
            Debug.Log("Pressed");
            pressed = true;
            interacted = false;
        }
        if(pressed){
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.25f, 1f, 1f), 2f * Time.deltaTime);
            if(countdown <= 0){
                released = true;
                Debug.Log("Released");
                pressed = false;
                countdown = 60;
            }else{
                countdown -= 1;
            }
        }
        if(released){
           transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 2f * Time.deltaTime);
            if(countdown <= 0){
                released = false;
                countdown = 60;
            }else{
                countdown -= 1;
            }
        }
    }
}
