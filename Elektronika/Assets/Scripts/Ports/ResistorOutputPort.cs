using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistorOutputPort : Port
{
    [Header("Resistor Output Settings")]
    [SerializeField, Tooltip("Information received from Resistor Input")] string information; // Informasi yang diterima dari Resistor Input

    public string Information { get => information; set => information = value; }

    public void CheckSwitch() {
        if(connectedPort != null && connectedPort is LEDPort) {
            LEDPort ledPort = (LEDPort)connectedPort;
            ledPort.CheckSwitch();
            // Debug.Log("Resistor Output: Check Switch");
        }
    }

    public override bool CanConnect(Port other) {
        // Cegah terhubung dengan Port yang sedang terhubung dengan ResistorInputPort
        if (other.GetConnectedPortType() == typeof(ResistorInputPort) ||
            other is ResistorInputPort) {
            return false;
        }

        return base.CanConnect(other);
    }
}
