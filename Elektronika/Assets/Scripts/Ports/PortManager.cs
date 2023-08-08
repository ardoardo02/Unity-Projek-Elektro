using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour
{
    [Header("Line Settings")]
    [SerializeField] LineRenderer linePrefab;
    
    Port selectedPort;
    LineRenderer currentLine;
    List<Connection> connections = new List<Connection>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SelectPortUnderMouse();
            if (selectedPort != null) {
                StartDrawingLine();
            }
        }

        if (Input.GetMouseButton(0) && selectedPort != null) {
            UpdateLineEndPosition();
        }

        if (Input.GetMouseButtonUp(0) && selectedPort != null) {
            ConnectPortUnderMouse();
        }
    }

    private void SelectPortUnderMouse() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            Port potentialPort = hit.collider.GetComponent<Port>();

            selectedPort = potentialPort;
            if (selectedPort != null)
            {
                DisconnectExistingConnection(selectedPort); // Hapus koneksi yang ada jika port sudah terhubung
            }
        }
    }

    private void StartDrawingLine() {
        currentLine = Instantiate(linePrefab, transform);
        currentLine.positionCount = 2;
        currentLine.SetPosition(0, selectedPort.transform.position);
    }

    private void UpdateLineEndPosition() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.SetPosition(1, mousePos);
    }

    private void ConnectPortUnderMouse() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null) {
            Port targetPort = hit.collider.GetComponent<Port>();

            if (targetPort != null && selectedPort.CanConnect(targetPort)) {
                DisconnectExistingConnection(targetPort); // Hapus koneksi yang ada jika target port sudah terhubung

                // Atur endPosition garis ke posisi tengah target port
                currentLine.SetPosition(1, targetPort.transform.position);

                selectedPort.Connect(targetPort);
                targetPort.Connect(selectedPort);

                selectedPort.SetConnectedLine(currentLine);
                targetPort.SetConnectedLine(currentLine);

                selectedPort.SetConnectedPort(targetPort);
                targetPort.SetConnectedPort(selectedPort);

                connections.Add(new Connection(selectedPort, targetPort, currentLine));
                Debug.Log(connections.Count);
            }
            else {
                Destroy(currentLine.gameObject);
            }
        }
        else {
            Destroy(currentLine.gameObject);
        }

        selectedPort = null;
        currentLine = null;
    }

    public void DisconnectExistingConnection(Port port) {
        Connection connectionToRemove = connections.Find(conn => conn.ContainsPort(port));
        
        if (connectionToRemove != null) {
            connectionToRemove.PortA.Disconnect();
            connectionToRemove.PortB.Disconnect();
            connections.Remove(connectionToRemove);
        }
    }
}