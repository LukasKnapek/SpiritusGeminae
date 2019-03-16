using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private GameObject bodyObject;
    private System.Random random;
    private Rigidbody playerBody;
    private GameSettings settings;

    private GameObject player;
    private HealthBarController healthBar;

    // Start is called before the first frame update
    void Start()
    {
        settings = GameObject.Find("GameMaster").GetComponent<GameSettings>();
        bodyObject = transform.parent.gameObject;
        random = new System.Random();
        player = GameObject.FindWithTag("Player");
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBarController>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            healthBar.incrementHealth(-30);
            Vector3 repelDirection = other.transform.position - transform.position;
            player.GetComponent<Rigidbody>().AddForce(repelDirection * 400);
            settings.incrementDeadScreenAlpha(0.5f);
        } else {
            bodyObject.transform.Rotate(new Vector3(0, random.Next(125,225), 0));
        }
    }
}
