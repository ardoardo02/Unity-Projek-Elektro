using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [SerializeField] ComponentManager.ComponentType type;
    [SerializeField] Transform slotParent;
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip_pickUp, clip_drop;

    bool isDragging;
    Vector2 offset, originalPos;
    SpriteRenderer spriteRenderer;
    ComponentSlot currentSlot;

    public ComponentManager.ComponentType Type { get => type; }

    void Awake() {
        originalPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() { // Dragging
        if (!isDragging) return;
        
        var mousePos = GetMousePos();
        transform.position = mousePos - offset;
    }

    void OnMouseDown() { // Pick up
        if (currentSlot != null && IsSlotLocked(currentSlot)) return;
        
        currentSlot?.SetCurrentComponent(null);
        currentSlot = null;

        isDragging = true;
        audioSource.PlayOneShot(clip_pickUp);
        spriteRenderer.color = Color.white;

        offset = GetMousePos() - (Vector2)transform.position;
    }

    private void OnMouseUp() { // Drop
        if (IsSlotLocked(currentSlot)) return;

        isDragging = false;
        audioSource.PlayOneShot(clip_drop);

        foreach (Transform slot in slotParent) { // Check if the component is near a slot
            if (slot.TryGetComponent<ComponentSlot>(out var slotComponent) && slotComponent.CurrentComponent == null) {
                if (Vector2.Distance(transform.position, slot.position) < 2) {
                    if (IsSlotLocked(slotComponent)) {
                        transform.position = originalPos;
                        return;
                    }

                    transform.position = slot.position;
                    slotComponent.SetCurrentComponent(this);
                    currentSlot = slotComponent;

                    spriteRenderer.color = slotComponent.Type == type ? Color.green : Color.red;

                    return;
                }
            }
        }

        transform.position = originalPos;
    }

    Vector2 GetMousePos() { // Get mouse position in world space
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsSlotLocked(ComponentSlot slot) { // Check if the slot is locked
        return slot is CompLockedSlot lockedSlot && lockedSlot.IsLocked;
    }
}


// ---------------------- KODE LAMA ARDO ---------------------- //

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [SerializeField] ComponentManager.ComponentType type;
    [SerializeField] Transform slotParent;
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip_pickUp, clip_drop;

    bool isDragging;
    Vector2 offset, originalPos;
    SpriteRenderer spriteRenderer;
    ComponentSlot currentSlot;
    // CompLockedSlot lockedSlot;

    void Awake() {
        originalPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() { // Dragging
        if (!isDragging) return;
        
        var mousePos = GetMousePos();

        transform.position = mousePos - offset;
    }

    void OnMouseDown() { // Pick up
        // if (lockedSlot != null){ // If the component is in a locked slot
        //     if (lockedSlot.IsLocked) return;
        //     lockedSlot.CurrentComponent = null; // Remove this component from its current slot.
        //     lockedSlot = null;
        // }
        if (currentSlot != null) { 
            if (currentSlot is CompLockedSlot lockedSlot && lockedSlot.IsLocked) return;
            currentSlot.CurrentComponent = null; 
            currentSlot = null;
        }

        isDragging = true;
        audioSource.PlayOneShot(clip_pickUp);
        spriteRenderer.color = Color.white;

        offset = GetMousePos() - (Vector2)transform.position;
    }

    ------------------
    private void OnMouseUp() { // Drop
        // if (lockedSlot != null && lockedSlot.IsLocked) return; // If the component is in a locked slot
        if (currentSlot is CompLockedSlot lockedSlot && lockedSlot.IsLocked) return;

        isDragging = false;
        audioSource.PlayOneShot(clip_drop);

        foreach (Transform slot in slotParent) { // Check if the component is in a slot
            if ((Vector2.Distance(transform.position, slot.position) < 2)) {
                // ComponentSlot componentSlot = slot.GetComponent<ComponentSlot>();
                var slotComponent = slot.GetComponent<ComponentSlot>();
                if (slotComponent.CurrentComponent != null) {
                    transform.position = originalPos;
                    return;
                }

                // if(slot.GetComponent<CompLockedSlot>() != null) { // If the slot is a locked slot
                //     if (slot.GetComponent<CompLockedSlot>().IsLocked) { // If the slot is 
                //         transform.position = originalPos;
                //         return;
                //     }
                //     lockedSlot = slot.GetComponent<CompLockedSlot>();
                // }

                // var slotComponent = slot.GetComponent<ComponentSlot>();
                // if (slotComponent.CurrentComponent != null) {
                    // If the slot already has a component, return this component to its original position.
                    // transform.position = originalPos;
                    // return;
                // }

                transform.position = slot.position;
                slotComponent.CurrentComponent = this;
                currentSlot = slotComponent;
                // componentSlot.IsOccupied = true;
                
                if(slotComponent.Type == type) {
                    spriteRenderer.color = Color.green;
                } else {
                    spriteRenderer.color = Color.red;
                }

                return;
            }
        }

        transform.position = originalPos;
    }
    ------------------

    private void OnMouseUp() {
        if (currentSlot is CompLockedSlot lockedSlot && lockedSlot.IsLocked) return;
        
        isDragging = false;
        audioSource.PlayOneShot(clip_drop);

        foreach (Transform slot in slotParent) {
            if ((Vector2.Distance(transform.position, slot.position) < 2)) {
                var slotComponent = slot.GetComponent<ComponentSlot>();
                if (slotComponent.CurrentComponent != null) {
                    transform.position = originalPos;
                    return;
                }

                if (slotComponent is CompLockedSlot lockedSlot2 && lockedSlot2.IsLocked) {
                    transform.position = originalPos;
                    return;
                }

                transform.position = slot.position;
                slotComponent.CurrentComponent = this;
                currentSlot = slotComponent;

                if(slotComponent.Type == type) {
                    spriteRenderer.color = Color.green;
                } else {
                    spriteRenderer.color = Color.red;
                }

                return;
            }
        }

        transform.position = originalPos;
    }

    Vector2 GetMousePos() { // Get mouse position in world space
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
*/