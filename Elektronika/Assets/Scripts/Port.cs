using System;
using UnityEngine;

public class Port : MonoBehaviour
{
    [Header("Connection Settings")]
    [SerializeField, Tooltip("Connected Line Renderer")] protected LineRenderer connectedLine;
    protected Port connectedPort;

    public LineRenderer ConnectedLine { get => connectedLine; }

    public virtual bool CanConnect(Port other)
    {
        // Tambahkan pengecekan ini untuk mencegah BridgeOutputPort terhubung dengan BridgeInputPort
        if ((this is BridgeOutputPort && other is BridgeInputPort) || (this is BridgeInputPort && other is BridgeOutputPort))
        {
            return false;
        }

        return connectedLine == null && other != this;
    }

    public virtual void Connect(Port other)
    {
        if (CanConnect(other))
        {
            // Logika koneksi dasar
        }
    }

    public void SetConnectedPort(Port port)
    {
        connectedPort = port;
    }

    public void SetConnectedLine(LineRenderer line)
    {
        connectedLine = line;
    }

    public virtual void Disconnect()
    {
        if (connectedLine != null)
        {
            Destroy(connectedLine.gameObject);
            connectedLine = null;
        }
    }

    public Type GetConnectedPortType()
    {
        return connectedPort?.GetType();
    }
}


/*
public class Port : MonoBehaviour
{
    public enum PortType { Normal, Bridge, Switch, Ground }
    public PortType type;
    public int maxConnections;

    public List<Port> connectedPorts = new List<Port>();
    public LineRenderer connectedLine; // Menyimpan garis yang terhubung

    private void Start()
    {
        SetMaxConnections();
    }

    private void SetMaxConnections()
    {
        switch (type)
        {
            case PortType.Normal:
                maxConnections = 1;
                break;
            case PortType.Bridge:
                maxConnections = 2;
                break;
            // Anda bisa menambahkan kasus lain sesuai kebutuhan
        }
    }

    public bool CanConnect(Port other)
    {
        return connectedPorts.Count < maxConnections && !connectedPorts.Contains(other);
    }

    public void Connect(Port other)
    {
        if (CanConnect(other))
        {
            connectedPorts.Add(other);
        }
    }
}*/
