using System;
using UnityEngine;

public class GroundICPort : Port
{
    public Action<Port> OnConnect;
    public Action<Port> OnDisconnect;

    public override void Connect(Port other)
    {
        if (other is ICPort iCPort && iCPort.Information == "Ground")
        {
            base.Connect(other);
            OnConnect?.Invoke(this);
        }
    }

    public override void Disconnect()
    {
        base.Disconnect();
        OnDisconnect?.Invoke(this);
    }
}
