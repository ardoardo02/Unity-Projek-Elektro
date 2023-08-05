using UnityEngine;

public class Connection
{
    public Port PortA { get; private set; }
    public Port PortB { get; private set; }
    public LineRenderer Line { get; private set; }

    public Connection(Port portA, Port portB, LineRenderer line) {
        PortA = portA;
        PortB = portB;
        Line = line;
    }

    public void Disconnect() {
        PortA.SetConnectedLine(null);
        PortB.SetConnectedLine(null);
        Object.Destroy(Line.gameObject);
    }

    public bool ContainsPort(Port port) {
        return port == PortA || port == PortB;
    }
}
