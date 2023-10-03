using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompLockedSlot : ComponentSlot
{
    [Header("References")]
    [SerializeField] Sprite sprite_lockOpen;
    [SerializeField] Sprite sprite_lockClose;
    [SerializeField] ICPortManager icPortManager;
    [SerializeField] GameObject toggleButton;
    [Header("Collider")]
    [SerializeField] BoxCollider2D toggleCollider;
    [SerializeField] BoxCollider2D leftCollider;
    [SerializeField] BoxCollider2D midCollider;
    [SerializeField] BoxCollider2D rightCollider;
    
    bool isLocked;
    string icPos;
    // SpriteRenderer spriteRenderer;
    SpriteRenderer toggleSpriteRenderer;
    // Color originalColor;
    AudioSource audioSource;

    public bool IsLocked => isLocked; //public bool IsLocked { get => isLocked; }
    public BoxCollider2D LeftCollider { get => leftCollider; }
    public BoxCollider2D MidCollider { get => midCollider; }
    public BoxCollider2D RightCollider { get => rightCollider; }

    private void Awake() {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();

        toggleSpriteRenderer = toggleButton.GetComponent<SpriteRenderer>();
    }
    
    private void Start() {
        // spriteRenderer.color = new Color32(94, 94, 94, 116);
        toggleSpriteRenderer.sprite = sprite_lockClose;
        isLocked = true;
        UpdateICPortInformation(); // Update ICPorts based on the initial locked state
    }

    /*
    private void OnMouseEnter() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);

        if (hit.collider != null && hit.collider == toggleCollider)
            toggleSpriteRenderer.color = Color.gray;
    }

    private void OnMouseExit() {
        toggleSpriteRenderer.color = Color.white;
    }
    */

    private void OnMouseDown() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);

        if (hit.collider != null && hit.collider == toggleCollider){
            audioSource.Play();
            // spriteRenderer.color = isLocked ? originalColor : new Color32(94, 94, 94, 116);
            isLocked = !isLocked;
            toggleSpriteRenderer.sprite = isLocked ? sprite_lockClose : sprite_lockOpen;

            if (!isLocked) GameManager.Instance.CheckIC74LS148(false); // Check if the inserted IC is correct

            UpdateICPortInformation(); // Update ICPorts based on the new locked state
        }
    }

    private void UpdateICPortInformation()
    {
        if (isLocked && CurrentComponent != null && CurrentComponent.Type == Type)
        {
            icPortManager.UpdateICPort(Type, icPos); // Change ICPorts information based on component type
            GameManager.Instance.CheckIC74LS148(true); // Check if the inserted IC is correct
        }
        else
        {
            icPortManager.RemoveAllICPortInfo(); // Remove all information on 
        }
    }

    public void SetColliderActive(bool active) {
        leftCollider.enabled = active;
        midCollider.enabled = active;
        rightCollider.enabled = active;
    }

    public void SetICPos(string icPos) {
        this.icPos = icPos;
    }
}
