using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistorOutputPort : Port
{
    [Header("Resistor Output Settings")]
    [SerializeField, Tooltip("Information received from Resistor Input")] string information; // Informasi yang diterima dari Resistor Input
    [SerializeField] bool isActive = false;

    public string Information { get => information; set => information = value; }
    public bool IsActive { get => isActive; }

    public bool CheckSwitch() {
        if(connectedPort != null) {
            if(connectedPort is LEDPort) {
                LEDPort ledPort = (LEDPort)connectedPort;
                ledPort.CheckSwitch();
                return true;
                // Debug.Log("Resistor Output: Check Switch");
            }
            else if(connectedPort is SegmentPort) {
                SegmentPort segmentPort = (SegmentPort)connectedPort;
                if (segmentPort.CheckSwitch()) return true;
                // Debug.Log("Resistor Output: Check Switch");
            }
        }
        return false;
    }

    public override bool CanConnect(Port other) {
        // Cegah terhubung dengan Port yang sedang terhubung dengan ResistorInputPort
        if (other.GetConnectedPortType() == typeof(ResistorInputPort) ||
            other is ResistorInputPort) {
            return false;
        }

        return base.CanConnect(other);
    }

    public void Activate(bool isOn) {
        isActive = isOn;
        if (connectedPort is SegmentPort segmentPort) segmentPort.CheckSwitch();
    }
}
