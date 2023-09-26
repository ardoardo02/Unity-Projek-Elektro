using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGroundPort : Port
{
    [SerializeField] List<GroundPort> groundPorts = new List<GroundPort>();

    public override void Connect(Port other)
    {
        base.Connect(other);

        if(other is PowerPort powerPort && powerPort.IsPowerActive) {
            foreach (var groundPort in groundPorts) {
                groundPort.Activate();
            }
        }
    }

    public override void Disconnect()
    {
        base.Disconnect();

        foreach (var groundPort in groundPorts) {
            groundPort.Deactivate();
        }
    }

    public void TogglePower(bool isOn) {
        if (isOn) {
            foreach (var groundPort in groundPorts) {
                groundPort.Activate();
            }
        } else {
            foreach (var groundPort in groundPorts) {
                groundPort.Deactivate();
            }
        }
    }
}
