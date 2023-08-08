using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPort : Port
{
    bool isActive = false;

    public bool IsActive { get => isActive; }
    
    public void Activate() {
        isActive = true;
        
        if(ConnectedPort is GroundSwitchPort groundSwitchPort)
            groundSwitchPort.ActivatePort();
    }

    public void Deactivate() {
        isActive = false;

        if(ConnectedPort is GroundSwitchPort groundSwitchPort)
            groundSwitchPort.DeactivatePort();
    }
}