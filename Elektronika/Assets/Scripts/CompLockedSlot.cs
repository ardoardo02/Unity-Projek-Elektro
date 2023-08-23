using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompLockedSlot : ComponentSlot
{
    [SerializeField] ICPortManager icPortManager;
    [SerializeField] GameObject toggleButton;
    [SerializeField] bool isLocked;

    SpriteRenderer spriteRenderer;
    SpriteRenderer toggleSpriteRenderer;
    Color originalColor;
    AudioSource audioSource;

    public bool IsLocked => isLocked; //public bool IsLocked { get => isLocked; }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();

        toggleSpriteRenderer = toggleButton.GetComponent<SpriteRenderer>();
    }
    
    private void Start() {
        spriteRenderer.color = new Color32(94, 94, 94, 116);
        isLocked = true;
        UpdateICPortInformation(); // Update ICPorts based on the initial locked state
    }

    private void OnMouseEnter() {
        toggleSpriteRenderer.color = Color.gray;
    }

    private void OnMouseExit() {
        toggleSpriteRenderer.color = Color.white;
    }

    private void OnMouseDown() {
        audioSource.Play();
        spriteRenderer.color = isLocked ? originalColor : new Color32(94, 94, 94, 116);
        isLocked = !isLocked;

        UpdateICPortInformation(); // Update ICPorts based on the new locked state
    }

    private void UpdateICPortInformation()
    {
        if (isLocked && CurrentComponent != null && CurrentComponent.Type == Type)
        {
            icPortManager.UpdateICPort(Type); // Change ICPorts information based on component type
        }
        else
        {
            icPortManager.RemoveAllICPortInfo(); // Remove all information on ICPorts
        }
    }
}
