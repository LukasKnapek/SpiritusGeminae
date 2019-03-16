using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float look_speed;
    public bool spiritMode;
    Vector3 bodyPos;
    Quaternion bodyRot;
    public GameObject body;
    LineRenderer laser;
    public float maxGaunt;
    public float currGaunt;
    int countdown;
    public GameSettings settings;
	public Transform embers;
	private HealthBarController healthBar;

    void OnEnable() {
        settings.onModeSwitch += HandleModeChange;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spiritMode = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
        currGaunt = maxGaunt;
        countdown = 30;
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBarController>();
    }

    void FixedUpdate()
    {
            float hor = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

            Vector3 hMove = hor * transform.right;
            Vector3 vMove = vert * transform.forward;
            Vector3 move = (hMove + vMove) * speed;

            // rb.MovePosition(transform.position + move);
            rb.AddForce(move * Time.deltaTime, ForceMode.VelocityChange);

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 look = new Vector3(0, mouseX, 0) * (look_speed * 0.75f) * Time.deltaTime;
            Vector3 look_hor = new Vector3(-mouseY, 0, 0) * (look_speed * 0.75f) * Time.deltaTime;

            rb.MoveRotation(rb.rotation * Quaternion.Euler(look));

            Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
            cam.eulerAngles += look_hor;
            if (cam.localEulerAngles.x > 80 && cam.localEulerAngles.x < 180)
                cam.localEulerAngles = new Vector3(80, 0, 0);
            else if (cam.localEulerAngles.x < 280 && cam.localEulerAngles.x > 180)
                cam.localEulerAngles = new Vector3(280, 0, 0);
    }

    void Update()
    {
        embers.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
        if (Input.GetButtonDown("Fire1"))
        {
            if (settings.spiritMode == true)
            {
                StopCoroutine("FireLaser");
                StartCoroutine("FireLaser");
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(GameObject.FindGameObjectWithTag("BlastPoint").transform.position.x, GameObject.FindGameObjectWithTag("BlastPoint").transform.position.y-0.25f, GameObject.FindGameObjectWithTag("BlastPoint").transform.position.z), GameObject.FindGameObjectWithTag("MainCamera").transform.forward, out hit, 100))
                {
                    if (hit.rigidbody.gameObject.CompareTag("Enemy"))
                    {
                        hit.rigidbody.gameObject.GetComponent<CombatSkills>().decreaseHealth(35);
                    }
                }
            }
            else
            {
                StopCoroutine("FireLaser");
                Debug.DrawRay(GameObject.FindGameObjectWithTag("BlastPoint").transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.forward, Color.red, 250f, true);
                RaycastHit hit;
                if (Physics.Raycast(GameObject.FindGameObjectWithTag("BlastPoint").transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.forward, out hit, 250))
                {
                    if (hit.rigidbody.gameObject.GetComponent<RotatingPillarInteraction>() != null)
                    {
                        hit.rigidbody.gameObject.GetComponent<RotatingPillarInteraction>().interacted = true;
                    }

                    if (hit.rigidbody.gameObject.GetComponent<ButtonInteraction>() != null)
                    {
                        hit.rigidbody.gameObject.GetComponent<ButtonInteraction>().interacted = true;
                    }
                }
            }
        }
    }

    IEnumerator FireLaser()
    {
        laser.enabled = true;

        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(new Vector3(GameObject.FindGameObjectWithTag("BlastPoint").transform.position.x, GameObject.FindGameObjectWithTag("BlastPoint").transform.position.y-0.25f, GameObject.FindGameObjectWithTag("BlastPoint").transform.position.z), GameObject.FindGameObjectWithTag("MainCamera").transform.forward);
            RaycastHit hit;

            laser.SetPosition(0, ray.origin);

            if (Physics.Raycast(ray, out hit, 10))
                 laser.SetPosition(1, hit.point);
            else
                laser.SetPosition(1, ray.GetPoint(10));

            yield return null;
        }

        laser.enabled = false;
    }

    private void HandleModeChange(bool isSpiritMode) {
        if (isSpiritMode) {
            Instantiate(body, new Vector3(transform.position.x, 0.05f, transform.position.z), transform.rotation);
            bodyPos = transform.position;
            bodyRot = transform.rotation;
            spiritMode = true;
        } else {
            Destroy(GameObject.Find("Body(Clone)"));
            transform.position = bodyPos;
            transform.rotation = bodyRot;
            spiritMode = false;
        }
    }

    private void OnParticleCollision(GameObject particle) {
        healthBar.incrementHealth(-1);
        settings.incrementDeadScreenAlpha(0.01f);
    }
}
