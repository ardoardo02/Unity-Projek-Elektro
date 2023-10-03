using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSwitchExtraPort : Port
{
    GroundSwitchPort relatedGroundSwitchPort;
    bool isActive = false;
    
    public bool IsActive { get => isActive; set => isActive = value; }

    private void Start() {
        relatedGroundSwitchPort = GetComponentInParent<GroundSwitchPort>();
    }

    public override void Connect(Port other) 
    {
        base.Connect(other);
        
        if (other is GroundSwitchPort) return;
        else GameManager.Instance.AddMistake();
    }

    public void Deactivate() {
        isActive = false;

        if(connectedPort is GroundSwitchPort groundSwitchPort)
            groundSwitchPort.TriggerTurnOffLightEvent?.Invoke();
    }

    public override bool CanConnect(Port other) {
        // Cegah terhubung dengan Port yang sedang terhubung dengan GroundSwitchPort
        if (other.GetConnectedPortType() == typeof(GroundSwitchPort) || 
            (relatedGroundSwitchPort != null && other == relatedGroundSwitchPort)) {
            return false;
        }

        return base.CanConnect(other) && gameObject.activeSelf;
    }
}
