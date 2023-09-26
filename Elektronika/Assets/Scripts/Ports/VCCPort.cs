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
        else if(connectedPort is ICPort iCPort && (iCPort.Information == "VCC" || iCPort.Information == "EO"))
            iCPort.UpdateActivateIC(true, this);
    }

    public void Deactivate() {
        isActive = false;

        if(connectedPort is PowerBridgePort powerBridgePort)
            powerBridgePort.Deactivate();
        else if(connectedPort is ICPort iCPort)
            iCPort.UpdateActivateIC(false, this);
    }
}
