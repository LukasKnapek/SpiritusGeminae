using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public delegate void ModeSwitchedHandler(bool isSpiritMode);
    public event ModeSwitchedHandler onModeSwitch;
    private Image deadScreen;

    public bool spiritMode = false;
    private bool gameEnding = false;

    // Start is called before the first frame update
    void Start()
    {
        deadScreen = GameObject.Find("DeadScreen").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.GetKeyDown("space"))
        {
            spiritMode = !spiritMode;
            onModeSwitch(spiritMode);
        }

        if (!gameEnding) {
            var screenColor = deadScreen.color;
            if (screenColor.a > 0) {
                screenColor.a -= 0.004f;
                deadScreen.color = screenColor;
            }
        }
    }

    public void cancelSpiritMode() {
        spiritMode = false;
        onModeSwitch(spiritMode);
    }

    public void incrementDeadScreenAlpha(float n) {
        var color = deadScreen.color;
        color.a += n;
        deadScreen.color = color;
    }

    public void gameOver() {
        if (!gameEnding) {
            gameEnding = true;
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.05f * 0.02f;

            StartCoroutine(restartGame());

        }
    }

    IEnumerator restartGame() {
        var screenColor = deadScreen.color;
        
        while (screenColor.a <= 1) {
            screenColor.a += 0.004f;
            deadScreen.color = screenColor;
            yield return null;
        }

        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
        screenColor.a += 0f;
        deadScreen.color = screenColor;

        Time.timeScale = 1;
        Time.fixedDeltaTime = 1f * 0.02f;
    }

}
