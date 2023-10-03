using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistorInputPort : Port
{
    [Header("Resistor Input Settings")]
    [SerializeField] PortManager portManager; // Referensi ke PortManager
    [SerializeField, Tooltip("Related Resistor Output Port")] ResistorOutputPort relatedResistorOutput; // Referensi ke ResistorOutputPort terkait

    public override void Connect(Port other) {
        base.Connect(other);

        if (relatedResistorOutput != null) {
            // Salin informasi jika ini adalah ICPort
            if (other is ICPort portIC && !string.IsNullOrEmpty(portIC.Information) && portIC.Information[0] == 'A'){
                relatedResistorOutput.Information = portIC.Information;
                relatedResistorOutput.CheckSwitch();
                GameManager.Instance.CheckPort(this, true);
                // Debug.Log("ResistorInputPort: Check Switch");
            }
            else GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        if (relatedResistorOutput != null) {
            if (relatedResistorOutput.Information != "") {
                GameManager.Instance.CheckPort(this, false);
                relatedResistorOutput.Information = ""; // Hapus informasi
            }
            portManager.DisconnectExistingConnection(relatedResistorOutput); // Hapus garis yang terhubung dengan ResistorOutputPort
        }
    }

    public override bool CanConnect(Port other) {
        if (other.GetConnectedPortType() == typeof(ResistorOutputPort) || 
            // (relatedResistorOutput != null && other == relatedResistorOutput)) {
            other is ResistorOutputPort) {
            return false;
        }

        return base.CanConnect(other);
    }
}
