using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDPort : Port
{
    [Header("LED Settings")]
    // [SerializeField] SwitchManager switchManager;
    [SerializeField, Tooltip("Related Ground LED Port")] GroundLEDPort relatedGroundLEDPort;
    [SerializeField, Tooltip("Related LED/Light Object")] SpriteRenderer lightSpriteRenderer;

    string receivedInformation = "";

    public string ReceivedInformation { get => receivedInformation; }

    private void Start() {
        relatedGroundLEDPort.TriggerTurnOffLightEvent += TurnLight;
        relatedGroundLEDPort.TriggerPortActivatedEvent += CheckSwitch;
    }

    private void OnDestroy() {
        relatedGroundLEDPort.TriggerTurnOffLightEvent -= TurnLight;
        relatedGroundLEDPort.TriggerPortActivatedEvent -= CheckSwitch;
    }

    public override void Connect(Port other) {
        base.Connect(other);

        if (GameManager.Instance.GetInsertedICType() == ComponentManager.ComponentType._74LS148 && 
            other is ResistorOutputPort resistorOutputPort) {
            if (!string.IsNullOrEmpty(resistorOutputPort.Information)){
                receivedInformation = resistorOutputPort.Information;
                SwitchManager.Instance.CheckSwitches();
                GameManager.Instance.CheckPort(this, true);
            }
            else GameManager.Instance.AddMistake();
        }
        else GameManager.Instance.AddMistake();
    }

    public override void Disconnect() {
        base.Disconnect();

        if (!string.IsNullOrEmpty(receivedInformation)){
            GameManager.Instance.CheckPort(this, false);
            receivedInformation = "";
        }
        TurnLight(false);
    }

    public void TurnLight(bool isOn) {
        lightSpriteRenderer.color = isOn && IsGroundLEDActive() ? Color.yellow : Color.white;
    }

    public void CheckSwitch() {
        if(connectedPort != null && connectedPort is ResistorOutputPort) {
            ResistorOutputPort resistorOutputPort = (ResistorOutputPort)connectedPort;
            if (!string.IsNullOrEmpty(resistorOutputPort.Information)){
                receivedInformation = resistorOutputPort.Information;
                SwitchManager.Instance.CheckSwitches();
                // Debug.Log("LEDPort: Check Switch");
            }
        }
    }

    public bool IsGroundLEDActive() {
        return relatedGroundLEDPort.IsActive;
    }   
}