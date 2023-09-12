using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    [SerializeField] ComponentManager.ComponentType type;
    // [SerializeField] Transform slotParent;
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip_pickUp, clip_drop;

    bool isDragging;
    Vector2 offset, originalPos;
    // SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    ComponentSlot currentSlot;

    public ComponentManager.ComponentType Type { get => type; }

    void Awake() {
        originalPos = transform.position;
        // spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        HandleRaycastDrag();
    }

    void HandleRaycastDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Component>() == this)
            {
                if (currentSlot != null && IsSlotLocked(currentSlot)) return;
                if (currentSlot is CompLockedSlot lockedSlot) {
                    lockedSlot.SetColliderActive(true);
                    lockedSlot.SetICPos("");
                }
                currentSlot?.SetCurrentComponent(null);
                currentSlot = null;

                isDragging = true;
                offset = GetMousePos() - (Vector2)transform.position;
                audioSource.PlayOneShot(clip_pickUp);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            if (IsSlotLocked(currentSlot)) return;

            isDragging = false;
            audioSource.PlayOneShot(clip_drop);

            boxCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);
            boxCollider.enabled = true;

            if (hit.collider != null)
            {
                ComponentSlot slotComponent = hit.collider.GetComponent<ComponentSlot>();
                if (slotComponent != null && slotComponent.CurrentComponent == null)
                {
                    if (IsSlotLocked(slotComponent)) {
                        transform.position = originalPos;
                        return;
                    }

                    if (slotComponent is CompLockedSlot lockedSlot) {
                        if (hit.collider == lockedSlot.LeftCollider) {
                            transform.position = new Vector2(slotComponent.transform.position.x - .11f, slotComponent.transform.position.y);
                            lockedSlot.SetICPos("left");
                        } else if (hit.collider == lockedSlot.MidCollider) {
                            transform.position = new Vector2(slotComponent.transform.position.x, slotComponent.transform.position.y);
                            lockedSlot.SetICPos("mid");
                        } else if (hit.collider == lockedSlot.RightCollider) {
                            transform.position = new Vector2(slotComponent.transform.position.x + .11f, slotComponent.transform.position.y);
                            lockedSlot.SetICPos("right");
                        }

                        lockedSlot.SetColliderActive(false);
                    }

                    slotComponent.SetCurrentComponent(this);
                    currentSlot = slotComponent;
                    return;
                }
            }

            transform.position = originalPos;
        }

        if (isDragging)
        {
            transform.position = GetMousePos() - offset;
        }
    }

    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsSlotLocked(ComponentSlot slot)
    {
        return slot is CompLockedSlot lockedSlot && lockedSlot.IsLocked;
    }
}

/* ----------- KODE METHOD LAMA ------------
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
                if (Vector2.Distance(transform.position, slot.position) < 1) {
                    if (IsSlotLocked(slotComponent)) {
                        transform.position = originalPos;
                        return;
                    }

                    transform.position = new Vector2(slot.position.x + .11f, slot.position.y);
                    slotComponent.SetCurrentComponent(this);
                    currentSlot = slotComponent;

                    // spriteRenderer.color = slotComponent.Type == type ? Color.green : Color.red;

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
*/