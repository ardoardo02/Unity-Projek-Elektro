using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSlot : MonoBehaviour
{
    [SerializeField] ComponentManager.ComponentType type;
    [SerializeField] ComponentManager.SlotType slotType;

    public ComponentManager.ComponentType Type { get => type; }
    public Component CurrentComponent { get; private set; }

    public void SetCurrentComponent(Component component) {
        CurrentComponent = component;
    }
}
