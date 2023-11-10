using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ResistorInputPort : Port
{
    [Header("Resistor Input Settings")]
    [SerializeField] PortManager portManager; // Referensi ke PortManager
    [SerializeField, Tooltip("Related Resistor Output Port")] ResistorOutputPort relatedResistorOutput; // Referensi ke ResistorOutputPort terkait

    [SerializeField] string[] validCharacters = { "a", "b", "c", "d", "e", "f", "g", "A0", "A1", "A2" };

    public override void Connect(Port other) {
        base.Connect(other);

        if (relatedResistorOutput != null) {
            // Salin informasi jika ini adalah ICPort
            if (other is ICPort portIC && System.Array.Exists(validCharacters, element => element == portIC.Information)){
                // if (!string.IsNullOrEmpty(portIC.Information) && portIC.Information[0] == 'A' || Regex.IsMatch(portIC.Information, "[a-g]")){
                // if(System.Array.Exists(validCharacters, element => element == portIC.Information)) {
                    
                relatedResistorOutput.Information = portIC.Information;
                if (relatedResistorOutput.CheckSwitch()) GameManager.Instance.CheckPort(this, true);
                GameManager.Instance.CheckPort(this, true);
                // Debug.Log("ResistorInputPort: Check Switch");
                
                // }
                if (portIC.IsActive) Activate(true);
            }
            else GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        if (relatedResistorOutput != null) {
            if (relatedResistorOutput.Information != "") {
                GameManager.Instance.CheckPort(this, false);
                Activate(false);
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

    public void Activate(bool isOn) {
        if (relatedResistorOutput != null) relatedResistorOutput.Activate(isOn);
    }
}
