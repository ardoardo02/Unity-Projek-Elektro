using UnityEngine;

public class BridgeInputPort : Port
{
    [Header("Bridge Settings")]
    [SerializeField] PortManager portManager; // Referensi ke PortManager
    [SerializeField, Tooltip("Related Bridge Output Port")] BridgeOutputPort relatedBridgeOutput; // Referensi ke BridgeOutputPort terkait

    private void Start() {
        relatedBridgeOutput.gameObject.SetActive(false); // Awalnya dinonaktifkan
    }

    public override void Connect(Port other) {
        base.Connect(other);

        if (relatedBridgeOutput != null) {
            relatedBridgeOutput.gameObject.SetActive(true); // Aktifkan BridgeOutputPort terkait

            // Salin informasi jika ini adalah ICPort
            if (other is ICPort icPort)
            {
                relatedBridgeOutput.Information = icPort.Information;
            }
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        if (relatedBridgeOutput != null) {
            relatedBridgeOutput.Information = ""; // Hapus informasi
            relatedBridgeOutput.gameObject.SetActive(false); // Nonaktifkan BridgeOutputPort terkait
            portManager.DisconnectExistingConnection(relatedBridgeOutput); // Hapus garis yang terhubung dengan BridgeOutputPort
        }
    }
}