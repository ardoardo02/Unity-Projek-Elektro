using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] Sprite sprite_toggleOn;
    [SerializeField] Sprite sprite_toggleOff;
    [SerializeField] PowerPort[] powerPorts;

    bool isPowerActive = false;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = transform.Find("Toggle").GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        TogglePower(!isPowerActive);
    }

    private void TogglePower(bool isOn) {
        isPowerActive = isOn;
        spriteRenderer.sprite = isOn ? sprite_toggleOn : sprite_toggleOff;
        foreach (var powerPort in powerPorts)
        {
            powerPort.TogglePower(isOn);
        }
    }
}
