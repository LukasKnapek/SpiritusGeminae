using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGaugeController : MonoBehaviour
{
    private Slider slider;
    public float maxPower;
    private float power;
    public GameSettings settings;

    private Coroutine fillRoutine;
    private Coroutine drainRoutine;

    private GameObject physicalBody;
    private GameObject cameraBody;

    void OnEnable() {
        settings.onModeSwitch += HandleModeChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        power = maxPower; 
        slider = GetComponent<Slider>();
        cameraBody = GameObject.FindWithTag("Player");
    }

    private void HandleModeChange(bool isSpiritMode) {
        if (isSpiritMode) {
            if (fillRoutine != null) StopCoroutine(fillRoutine);
            drainRoutine = StartCoroutine(decreaseGauge());
        } else {
            if (drainRoutine != null) StopCoroutine(drainRoutine);
            fillRoutine = StartCoroutine(fillGauge());
        }
    }

    IEnumerator decreaseGauge() {
        yield return new WaitForSeconds(0.1f);
        physicalBody = GameObject.Find("Body(Clone)");
        print(physicalBody);
        LineRenderer lr = physicalBody.gameObject.AddComponent<LineRenderer>();
        lr.SetPosition(0, physicalBody.transform.position + new Vector3(0,0.5f,0));
        lr.startColor = new Color(0,0,255);
        lr.endColor = new Color(0,0,255);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

        while (power > 0) {
            float distance = Vector3.Distance(physicalBody.transform.position, cameraBody.transform.position);
            lr.SetPosition(1, cameraBody.transform.position - new Vector3(0,0.5f,0));

            yield return new WaitForEndOfFrame();
            power -= 1 * (distance / 50);
            slider.value = power / maxPower;
        }
        settings.cancelSpiritMode();
        yield return null;
    }

    IEnumerator fillGauge() {
        while (power < maxPower) {
            yield return new WaitForEndOfFrame();
            power += 0.5f;
            slider.value = power / maxPower;
        }
        yield return null;
    }
}
