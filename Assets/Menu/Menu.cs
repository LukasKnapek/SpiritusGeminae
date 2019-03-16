using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene(3); // load skippable cutscene
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Lore ()
    {
        Debug.Log("Lore");
        SceneManager.LoadScene(2); // load unskippable cutscene
    }
}
