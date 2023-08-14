using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDPort : Port
{
    [Header("LED Settings")]
    [SerializeField] SwitchManager switchManager;
    [SerializeField, Tooltip("Related LED/Light Object")] SpriteRenderer lightSpriteRenderer;

    string receivedInformation = "";

    public string ReceivedInformation { get => receivedInformation; }

    public override void Connect(Port other) {
        base.Connect(other);

        if (other is ResistorOutputPort resistorOutputPort) {
            if (!string.IsNullOrEmpty(resistorOutputPort.Information)){
                receivedInformation = resistorOutputPort.Information;
                switchManager.CheckSwitches();
            }
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        receivedInformation = "";
        TurnLight(false);
    }

    public void TurnLight(bool isOn) {
        lightSpriteRenderer.color = isOn ? Color.yellow : Color.white;
    }

    public void CheckSwitch() {
        if(connectedPort != null && connectedPort is ResistorOutputPort) {
            ResistorOutputPort resistorOutputPort = (ResistorOutputPort)connectedPort;
            if (!string.IsNullOrEmpty(resistorOutputPort.Information)){
                receivedInformation = resistorOutputPort.Information;
                switchManager.CheckSwitches();
                // Debug.Log("LEDPort: Check Switch");
            }
        }
    }
}