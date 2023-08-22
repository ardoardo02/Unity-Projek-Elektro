using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPort : Port
{
    [Header("Connection Settings")]
    [SerializeField, Tooltip("Connected Line Renderer")] string information; // Informasi untuk tipe IC
    
    bool isActive = false;

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
}

