using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCCPort : Port
{
    bool isActive = false;

    public bool IsActive { get => isActive; }

    public void Activate() {
        isActive = true;

        if(connectedPort is PowerBridgePort powerBridgePort)
            powerBridgePort.Activate();
    }

    public void Deactivate() {
        isActive = false;

        if(connectedPort is PowerBridgePort powerBridgePort)
            powerBridgePort.Deactivate();
    }
}
