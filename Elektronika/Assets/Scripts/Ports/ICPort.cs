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
    }

    public void Deactivate() {
        isActive = false;

        if (connectedPort is BridgeInputPort bridgeInputPort)
            bridgeInputPort.Deactivate();
    }

    public override void Connect(Port other) {
        base.Connect(other);

        if ((information == "VCC" && other is VCCPort vCCPort && vCCPort.IsActive) ||
            ((information == "Ground" || information == "GS" || information == "EO" || information == "E1") && 
            other is GroundPort groundPort && groundPort.IsActive)) {
            icPortManager.Activate(other, information);
        }
    }

    public override void Disconnect() {
        if ((information == "VCC" && connectedPort is VCCPort) ||
            ((information == "Ground" || information == "GS" || information == "EO" || information == "E1") && 
            connectedPort is GroundPort)) {
            icPortManager.Deactivate(connectedPort, information);
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
        if ((information == "VCC" && connectedPort is VCCPort) ||
            ((information == "Ground" || information == "GS" || information == "EO" || information == "E1") && 
            connectedPort is GroundPort)) {
            icPortManager.Deactivate(connectedPort, information);
        }

        information = "";
        portManager.DisconnectExistingConnection(this);
    }
}

