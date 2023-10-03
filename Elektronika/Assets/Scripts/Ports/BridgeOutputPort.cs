using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeOutputPort : Port
{
    [Header("Output Settings")]
    [SerializeField, Tooltip("Information received from Bridge Input")] string information; // Informasi yang diterima dari Bridge Input
    
    [SerializeField] bool isActive = false; // Status aktif atau tidaknya Port
    bool isPowerActive = false; // Status aktif atau tidaknya Power
    bool isICActive = false; // Status aktif atau tidaknya IC

    public string Information { get => information; set => information = value; }
    public bool IsActive { get => isActive; }

    public override bool CanConnect(Port other) {
        // Cegah terhubung dengan Port yang sedang terhubung dengan BridgeInputPort
        if (other.GetConnectedPortType() == typeof(BridgeInputPort)) {
            return false;
        }

        return base.CanConnect(other) && gameObject.activeSelf;
    }

    public override void Connect(Port other) 
    {
        base.Connect(other);
        
        if (other is SwitchPort) return;
        else GameManager.Instance.AddMistake();
    }

    public void Activate(Port port) {
        if (port is PowerBridgePort) {
            isPowerActive = true;
        } else if (port is BridgeInputPort) {
            isICActive = true;
        }

        if (isPowerActive && isICActive) {
            isActive = true;

            if (connectedPort is SwitchPort switchPort)
                switchPort.TurnOnSwitch();
        }
    }

    public void Deactivate(Port port) {
        if (port is PowerBridgePort) {
            isPowerActive = false;
        } else if (port is BridgeInputPort) {
            isICActive = false;
        }

        if (!isPowerActive || !isICActive) {
            isActive = false;

            if (connectedPort is SwitchPort switchPort)
                switchPort.TurnOffSwitch();
        }
    }
}