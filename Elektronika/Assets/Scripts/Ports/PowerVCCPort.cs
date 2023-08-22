using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerVCCPort : Port
{
    [SerializeField] List<VCCPort> vccPorts = new List<VCCPort>();

    public override void Connect(Port port)
    {
        base.Connect(port);

        if (port is PowerPort){
            foreach (VCCPort vccPort in vccPorts){
                vccPort.Activate();
            }
        }
    }

    public override void Disconnect()
    {
        base.Disconnect();
        
        foreach (VCCPort vccPort in vccPorts){
            vccPort.Deactivate();
        }
    }
}
