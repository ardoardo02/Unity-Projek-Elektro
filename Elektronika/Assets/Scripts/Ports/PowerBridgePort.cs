using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBridgePort : Port
{
    [SerializeField] List<BridgeOutputPort> outputPorts = new List<BridgeOutputPort>();
    bool isActive = false;

    public bool IsActive { get => isActive; }

    public override void Connect(Port other) {
        base.Connect(other);

        if (other is VCCPort vccPort && GameManager.Instance.GetInsertedICType() == ComponentManager.ComponentType._74LS148) {
            GameManager.Instance.CheckPort(this, true);
            if (vccPort.IsActive)
                Activate();
        }
        else GameManager.Instance.AddMistake();
    }

    public override void Disconnect() {
        if (connectedPort is VCCPort) {
            GameManager.Instance.CheckPort(this, false);
        }

        base.Disconnect();
        Deactivate();
    }

    public void Activate() {
        isActive = true;

        foreach (BridgeOutputPort outputPort in outputPorts) {
            outputPort.Activate(this);
        }
    }

    public void Deactivate() {
        isActive = false;

        foreach (BridgeOutputPort outputPort in outputPorts) {
            outputPort.Deactivate(this);
        }
    }
}
