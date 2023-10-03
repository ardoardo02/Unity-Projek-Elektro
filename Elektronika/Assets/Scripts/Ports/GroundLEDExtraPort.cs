using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLEDExtraPort : Port
{
    GroundLEDPort relatedGroundLEDPort;
    bool isActive = false;
    
    public bool IsActive { get => isActive; set => isActive = value; }

    private void Start() {
        relatedGroundLEDPort = GetComponentInParent<GroundLEDPort>();
    }

    public override void Connect(Port other) 
    {
        base.Connect(other);
        
        if (other is GroundLEDPort) return;
        else GameManager.Instance.AddMistake();
    }

    public void Deactivate() {
        isActive = false;
    }

    public override bool CanConnect(Port other) {
        // Cegah terhubung dengan Port yang sedang terhubung dengan GroundLEDPort
        if (other.GetConnectedPortType() == typeof(GroundLEDPort) || 
            (relatedGroundLEDPort != null && other == relatedGroundLEDPort)) {
            return false;
        }

        return base.CanConnect(other) && gameObject.activeSelf;
    }
}
