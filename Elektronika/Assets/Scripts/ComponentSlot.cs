using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSlot : MonoBehaviour
{
    [Header("Component Types")]
    [SerializeField] ComponentManager.ComponentType type;
    [SerializeField] ComponentManager.SlotType slotType;

    Component currentComponent = null;

    public ComponentManager.ComponentType Type { get => type; }
    public Component CurrentComponent { get => currentComponent; }

    public void SetCurrentComponent(Component component) {
        currentComponent = component;

        // Update IC Port information based on the component type
        // if (component != null && component.Type == type)
        // {
        //     icPortManager.UpdateICPort(type);
        // }
    }
}
