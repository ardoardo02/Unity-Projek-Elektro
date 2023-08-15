using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class GroundPort : Port
{
    bool isActive = false;

    public bool IsActive { get => isActive; }
    
    public void Activate() {
        isActive = true;
        
        if(ConnectedPort is GroundSwitchPort groundSwitchPort)
            groundSwitchPort.ActivatePort();
        else if(ConnectedPort is GroundLEDPort groundLEDPort)
            groundLEDPort.ActivatePort();
    }

    public void Deactivate() {
        isActive = false;

        if(ConnectedPort is GroundSwitchPort groundSwitchPort)
            groundSwitchPort.DeactivatePort();
        else if(ConnectedPort is GroundLEDPort groundLEDPort)
            groundLEDPort.DeactivatePort();
    }
}