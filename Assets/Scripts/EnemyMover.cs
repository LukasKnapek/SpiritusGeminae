using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float speed = 2;
    private GameSettings settings;

    private Rigidbody body;
    private MeshRenderer bodyRenderer;
    private ParticleSystem ps;
    private System.Random random;


    void OnEnable() {
        settings = GameObject.Find("GameMaster").GetComponent<GameSettings>();
        settings.onModeSwitch += HandleModeChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        random = new System.Random();
        bodyRenderer = GetComponent<MeshRenderer>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = body.transform.forward * speed;
    }

    private void HandleModeChange(bool isSpiritMode) {
        if (isSpiritMode) {
            if(bodyRenderer != null)bodyRenderer.enabled = true;
            if(ps != null)ps.Stop();
        }
        else {
            if(bodyRenderer != null)bodyRenderer.enabled = false;
            if(ps != null)ps.Play();
        }
    }
}
