using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrapController : MonoBehaviour
{
    private GameObject barrel;
    private GameObject tripwire;
    private GameSettings settings;
    private ParticleSystem flameThrow;

    // Start is called before the first frame update
    void Start()
    {
        settings = GameObject.Find("GameMaster").GetComponent<GameSettings>();
        barrel = transform.parent.Find("Flamethrower").Find("Barrel").gameObject;
        flameThrow = barrel.GetComponent<ParticleSystem>();
        flameThrow.Stop();
    }

    void OnCollisionEnter(Collision collision) {
        if (!settings.spiritMode) flameThrow.Play();
    }

}
