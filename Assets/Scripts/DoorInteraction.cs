using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool locked;
    public bool open;
    public GameObject unlocker;
    public bool combo_open;
    public int num_of_pillars;
    public GameObject[] pillars;
    public int[] sides;

    void Start()
    {
    }

    void Update()
    {
        if(!combo_open && unlocker != null){
            if(unlocker.GetComponent<PlateInteraction>()){
                if(unlocker.GetComponent<PlateInteraction>().pressed){
                    locked = false;
                    if(!open)openDoor();
                }
            }
            if(unlocker.GetComponent<ButtonInteraction>()){
                if(unlocker.GetComponent<ButtonInteraction>().pressed){
                    locked = false;
                    if(!open)openDoor();
                }
            }
        }
        if(combo_open){
            int num_correct = 0;
           for(int i = 0; i < num_of_pillars; i++){
               if(pillars[i].GetComponent<RotatingPillarInteraction>().side == sides[i]){
                   num_correct++;
               }
           } 
           if(num_correct == num_of_pillars){
               locked = false;
                if(!open)openDoor();
           }else{
               locked = true;
                if(open)closeDoor();
           }
        }
    }

    void openDoor(){
        transform.rotation *= Quaternion.AngleAxis(90f, new Vector3(0f, 1f, 0f));
        open = true;
    }

    void closeDoor(){
        transform.rotation *= Quaternion.AngleAxis(-90f, new Vector3(0f, 1f, 0f));
        open = false;
    }
}
