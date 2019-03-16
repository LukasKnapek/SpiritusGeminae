using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    bool prompted;
    public GameObject skipMessage;
    public float timer = 0f;
    void Start()
    {
        prompted = false;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetButtonDown("Jump")){
            if(prompted){
                SceneManager.LoadScene(1);
            }else{
                skipMessage.SetActive(true);
                prompted = true;
            }
        }
        if(timer >= 76){
            SceneManager.LoadScene(1);
        } 
    }
}
