using System;
using UnityEngine;

public class Port : MonoBehaviour
{
    [Header("Connection Settings")]
    [SerializeField, Tooltip("Connected Line Renderer")] protected LineRenderer connectedLine;
    protected Port connectedPort;

    public LineRenderer ConnectedLine { get => connectedLine; }
    public Port ConnectedPort { get => connectedPort; }

    public virtual bool CanConnect(Port other) {
        // Pengecekan ini untuk mencegah BridgeOutputPort terhubung dengan BridgeInputPort
        if ((this is BridgeOutputPort && other is BridgeInputPort) || 
            (this is BridgeInputPort && other is BridgeOutputPort)) {
            return false;
        }

        return connectedLine == null && other != this;
    }

    public virtual void Connect(Port other) {
        if (CanConnect(other)) {
            // Logika koneksi dasar
        }
        // Debug.Log("Port is port: " + other.GetType() == "Port");
        if (other.GetType().ToString() == "Port") GameManager.Instance.AddMistake();
    }

    public void SetConnectedPort(Port port) {
        connectedPort = port;
    }

    public void SetConnectedLine(LineRenderer line) {
        connectedLine = line;
    }

    public virtual void Disconnect() {
        if (connectedLine != null) {
            Destroy(connectedLine.gameObject);
            connectedLine = null;
        }

        connectedPort = null;
    }

    public Type GetConnectedPortType() {
        return connectedPort?.GetType();
    }
}
