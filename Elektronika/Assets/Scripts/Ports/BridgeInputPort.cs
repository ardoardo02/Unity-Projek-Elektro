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
            if (other is ICPort icPort && int.TryParse(icPort.Information, out _)) {
                relatedBridgeOutput.Information = icPort.Information;
                GameManager.Instance.CheckPort(this, true);

                if (icPort.IsActive) {
                    relatedBridgeOutput.Activate(this);
                }
            }
            else GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        if (relatedBridgeOutput != null) {
            if (relatedBridgeOutput.Information != "") {
                GameManager.Instance.CheckPort(this, false);
                relatedBridgeOutput.Information = ""; // Hapus informasi
            }

            relatedBridgeOutput.Deactivate(this); // Nonaktifkan BridgeOutputPort terkait
            relatedBridgeOutput.gameObject.SetActive(false); // Nonaktifkan BridgeOutputPort terkait
            portManager.DisconnectExistingConnection(relatedBridgeOutput); // Hapus garis yang terhubung dengan BridgeOutputPort
        }
    }

    public void Activate() {
        if (relatedBridgeOutput != null) {
            relatedBridgeOutput.Activate(this);
        }
    }

    public void Deactivate() {
        if (relatedBridgeOutput != null) {
            relatedBridgeOutput.Deactivate(this);
        }
    }
}