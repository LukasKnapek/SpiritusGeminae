using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManagerUnskip : MonoBehaviour
{
    public float timer = 0f;
    void Start()
    {
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 76){
            SceneManager.LoadScene(0);
        } 
    }
}
