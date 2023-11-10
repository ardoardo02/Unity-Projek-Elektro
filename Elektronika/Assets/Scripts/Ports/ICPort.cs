using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPort : Port
{
    [Header("Connection Settings")]
    [SerializeField, Tooltip("Connected Line Renderer")] string information; // Informasi untuk tipe IC
    [SerializeField] PortManager portManager;
    [SerializeField] ICPortManager icPortManager;
    
    [SerializeField] bool isActive = false;

    public string Information { get => information; }
    public bool IsActive { get => isActive; }

    public void Activate() {
        isActive = true;

        if (connectedPort is BridgeInputPort bridgeInputPort)
            bridgeInputPort.Activate();
        else if (connectedPort is SwitchPort switchPort)
            switchPort.TurnOnSwitch();
        else if (connectedPort is ResistorInputPort resistorInputPort)
            resistorInputPort.Activate(true);
    }

    public void Deactivate() {
        isActive = false;

        if (connectedPort is BridgeInputPort bridgeInputPort)
            bridgeInputPort.Deactivate();
        else if (connectedPort is SwitchPort switchPort)
            switchPort.TurnOffSwitch();
        else if (connectedPort is ResistorInputPort resistorInputPort)
            resistorInputPort.Activate(false);
    }

    public override void Connect(Port other) {
        base.Connect(other);

        // if (((information == "VCC" || information == "EO") && other is VCCPort vCCPort && vCCPort.IsActive) ||
        //     ((information == "Ground" || information == "GS" || information == "E1") && other is GroundPort groundPort && groundPort.IsActive)) {
        if (((information == "VCC" || information == "EO") && other is VCCPort) ||
            ((information == "Ground" || information == "GS" || information == "E1") && other is GroundPort)) {
            GameManager.Instance.CheckPort(this, true);

            if((other is VCCPort vCCPort && vCCPort.IsActive) || (other is GroundPort groundPort && groundPort.IsActive))
                icPortManager.Activate(other, information);
        }
        else if(information == "VCC" || information == "EO" || information == "Ground" || information == "GS" || information == "E1" || information == "") {
            GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() {
        if (((information == "VCC" || information == "EO") && connectedPort is VCCPort) ||
            ((information == "Ground" || information == "GS" || information == "E1") && connectedPort is GroundPort)) {
            icPortManager.Deactivate(connectedPort, information);
            GameManager.Instance.CheckPort(this, false);
        }

        base.Disconnect();
    }

    public void UpdateActivateIC(bool isActivate, Port port) {
        if (isActivate)
            icPortManager.Activate(port, information);
        else
            icPortManager.Deactivate(port, information);
    }

    public void UpdateInformation(string newInformation) {
        information = newInformation;
    }

    public void DisconnectConnection() {
        if (((information == "VCC" || information == "EO") && connectedPort is VCCPort) ||
            ((information == "Ground" || information == "GS" || information == "E1") && connectedPort is GroundPort)) {
            icPortManager.Deactivate(connectedPort, information);
            GameManager.Instance.CheckPort(this, false);
        }

        information = "";
        portManager.DisconnectExistingConnection(this);
    }
}

