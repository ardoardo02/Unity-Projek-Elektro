using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPort : Port
{
    [SerializeField] PortManager portManager;

    bool isPowerActive = false;

    public bool IsPowerActive { get => isPowerActive; }

    public override void Connect(Port other)
    {
        base.Connect(other);

        if(other is PowerVCCPort || other is PowerGroundPort)
            return;
        else GameManager.Instance.AddMistake();
    }

    public void TogglePower(bool isOn) {
        isPowerActive = isOn;

        if (connectedPort is PowerVCCPort powerVCCPort)
            powerVCCPort.TogglePower(isOn);
        else if (connectedPort is PowerGroundPort powerGroundPort)
            powerGroundPort.TogglePower(isOn);
    }

    public void DisconnectConnection() {
        portManager.DisconnectExistingConnection(this);
    }
}
