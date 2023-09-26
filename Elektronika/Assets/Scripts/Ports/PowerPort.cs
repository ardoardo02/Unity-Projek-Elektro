using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPort : Port
{
    bool isPowerActive = false;

    public bool IsPowerActive { get => isPowerActive; }

    public void TogglePower(bool isOn) {
        isPowerActive = isOn;

        if (connectedPort is PowerVCCPort powerVCCPort)
            powerVCCPort.TogglePower(isOn);
        else if (connectedPort is PowerGroundPort powerGroundPort)
            powerGroundPort.TogglePower(isOn);
    }
}
