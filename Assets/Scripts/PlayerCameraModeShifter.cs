using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerCameraModeShifter : MonoBehaviour
{
    private PostProcessVolume processVolume;
    public GameSettings settings;

    void OnEnable() {
        settings.onModeSwitch += HandleModeChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        processVolume = GetComponent<PostProcessVolume>();
    }

    void HandleModeChange(bool isSpiritMode) {
        processVolume.enabled = isSpiritMode;
    }

}
