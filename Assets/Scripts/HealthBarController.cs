using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public float maxHealth;
    private float health;
    private Slider slider;
    public GameSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementHealth(int increment) {
        health += increment;
        slider.value = health / maxHealth;

        if (health <= 0) {
            settings.gameOver();
        }
    }
}
