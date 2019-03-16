using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPillarInteraction : MonoBehaviour
{
    public int numOfSides;
    int anglePerRotation;
    public bool interacted;
    private Quaternion targetRotation;
    public int side;

    void Start()
    {
        side = 0;
        anglePerRotation = 360 / numOfSides;
        targetRotation = transform.rotation;
    }
    
    void Update()
    {
        if (interacted)
        {
            targetRotation *= Quaternion.AngleAxis(anglePerRotation, Vector3.up);
            interacted = false;
            if(side < (numOfSides - 1)){
                side++;
            }else{
                side = 0;
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
}
