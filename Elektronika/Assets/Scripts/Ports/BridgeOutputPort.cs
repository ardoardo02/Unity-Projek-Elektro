using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeOutputPort : Port
{
    [Header("Output Settings")]
    [SerializeField, Tooltip("Information received from Bridge Input")] string information; // Informasi yang diterima dari Bridge Input

    public string Information { get => information; set => information = value; }

    public override bool CanConnect(Port other)
    {
        // Cegah terhubung dengan Port yang sedang terhubung dengan BridgeInputPort
        if (other.GetConnectedPortType() == typeof(BridgeInputPort))
        {
            return false;
        }

        return base.CanConnect(other) && gameObject.activeSelf;
    }
}