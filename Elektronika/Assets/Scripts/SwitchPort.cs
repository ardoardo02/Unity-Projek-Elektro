using UnityEngine;

public class SwitchPort : Port
{
    [Header("Switch Settings")]
    [SerializeField, Tooltip("Related Toggle Object")] private Switch toggleObject;
    [SerializeField, Tooltip("Related Light Object Parent")] private Transform lightObjectParent;

    SpriteRenderer lightSpriteRenderer;
    string receivedInformation = "";

    private void Start() {
        toggleObject.TriggerClickEvent += ToggleLight;
    }

    private void OnDestroy() {
        toggleObject.TriggerClickEvent -= ToggleLight;
    }

    public override void Connect(Port other)
    {
        base.Connect(other);

        // Jika terhubung dengan BridgeOutputPort, terima informasi
        if (other is BridgeOutputPort bridgeOutputPort)
        {
            receivedInformation = bridgeOutputPort.Information;
            // Aktifkan Toggle jika menerima informasi
            if (!string.IsNullOrEmpty(receivedInformation))
            {
                toggleObject.ToggleSwitch(true);
            }
        }
    }

    public override void Disconnect()
    {
        base.Disconnect();

        // Nonaktifkan Toggle saat terputus
        toggleObject.ToggleSwitch(false);
        TurnOffLights();
        receivedInformation = "";
    }

    public void ToggleLight()
    {
        if (lightObjectParent != null && toggleObject.gameObject.activeSelf)
        {
            foreach (Transform child in lightObjectParent)
            {
                if (child.name == receivedInformation)
                {
                    SpriteRenderer lightSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    lightSpriteRenderer.color = lightSpriteRenderer.color == Color.white ? Color.yellow : Color.white;
                }
            }
        }
    }

    private void TurnOffLights()
    {
        if (lightObjectParent != null)
        {
            foreach (Transform child in lightObjectParent)
            {
                if (child.name == receivedInformation)
                {
                    SpriteRenderer lightSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    lightSpriteRenderer.color = Color.white;
                }
            }
        }
    }
}
