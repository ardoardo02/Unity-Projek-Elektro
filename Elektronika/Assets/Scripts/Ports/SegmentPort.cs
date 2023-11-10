using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentPort : Port
{
    [Header("Segment Settings")]
    [SerializeField, Tooltip("Segment ID")] char segmentID; // Nomor segment
    [SerializeField] bool isActive = false;
    string receivedInformation = ""; // Informasi untuk tipe IC

    public bool IsActive { get => isActive; }

    public override void Connect(Port other) {
        base.Connect(other);

        if (GameManager.Instance.GetInsertedICType() == ComponentManager.ComponentType._7447 && 
            other is ResistorOutputPort resistorOutputPort) {
            if (resistorOutputPort.Information == segmentID.ToString()){
                receivedInformation = resistorOutputPort.Information;

                if(resistorOutputPort.IsActive) {
                    isActive = true;
                    SwitchManager.Instance.CheckSwitches();
                }
                
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
        isActive = false;
        SwitchManager.Instance.CheckSwitches();
    }

    public bool CheckSwitch() {
        if(connectedPort != null && connectedPort is ResistorOutputPort) {
            ResistorOutputPort resistorOutputPort = (ResistorOutputPort)connectedPort;
            if (resistorOutputPort.Information == segmentID.ToString()){
                receivedInformation = resistorOutputPort.Information;

                isActive = resistorOutputPort.IsActive;
                SwitchManager.Instance.CheckSwitches();

                return true;
            }
        }
        return false;
    }
}
