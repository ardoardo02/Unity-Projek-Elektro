using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCC_SegmentPort : Port {
    [SerializeField] bool isActive = false;
    public bool IsActive { get => isActive; }

    public override void Connect(Port other) {
        base.Connect(other);

        if (other is VCCPort vCCPort) {
            GameManager.Instance.CheckPort(this, true);
            if (vCCPort.IsActive) {
                isActive = true;
                SwitchManager.Instance.CheckSwitches();
            }
        }
        else GameManager.Instance.AddMistake();
    }

    public override void Disconnect() {
        base.Disconnect();

        GameManager.Instance.CheckPort(this, false);
        isActive = false;
        SwitchManager.Instance.CheckSwitches();
    }

    public void Activate(bool isOn) {
        isActive = isOn;
        SwitchManager.Instance.CheckSwitches();
    }
}
