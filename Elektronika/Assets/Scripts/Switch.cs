using System;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // [SerializeField] private SwitchManager switchManager;
    [SerializeField] Sprite sprite_switchOn;
    [SerializeField] Sprite sprite_switchOff;

    bool isEnabled;
    SpriteRenderer spriteRenderer;

    public Action TriggerClickEvent;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EnableSwitch(false);
    }

    private void OnMouseEnter() {
        if (!isEnabled) return;
        spriteRenderer.color = Color.gray;
    }

    private void OnMouseExit() {
        if (!isEnabled) return;
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown() {
        // switchManager.OnSwitchPressed(this);
        if (!isEnabled) return;
        TriggerClickEvent?.Invoke();
    }

    public void EnableSwitch(bool isOn)
    {
        isEnabled = isOn;
        spriteRenderer.color = isEnabled ? Color.white : new Color32(60, 60, 60, 255);
    }

    public void ToggleSwitch(bool isOn) {
        spriteRenderer.sprite = isOn ? sprite_switchOn : sprite_switchOff;
        // spriteRenderer.material.color = color;
    }
}