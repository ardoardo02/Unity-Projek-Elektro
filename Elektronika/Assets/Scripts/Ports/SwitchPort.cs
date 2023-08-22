using UnityEngine;

public class SwitchPort : Port
{
    [Header("Switch Settings")]
    [SerializeField, Tooltip("Related Ground Switch Port")] private GroundSwitchPort relatedGroundSwitchPort;
    [SerializeField, Tooltip("Related Toggle Object")] private Switch toggleObject;
    [SerializeField] SwitchManager switchManager;

    string receivedInformation = "";
    bool isToggleActive = false;

    public bool IsToggleActive { get => isToggleActive; }
    public string ReceivedInformation { get => receivedInformation; }

    private void Start() {
        toggleObject.TriggerClickEvent += ToggleSwitch;
        relatedGroundSwitchPort.TriggerTurnOffLightEvent += TurnOffSwitch;
        relatedGroundSwitchPort.TriggerPortActivatedEvent += TurnOnSwitch;
    }

    private void OnDestroy() {
        toggleObject.TriggerClickEvent -= ToggleSwitch;
        relatedGroundSwitchPort.TriggerTurnOffLightEvent -= TurnOffSwitch;
        relatedGroundSwitchPort.TriggerPortActivatedEvent -= TurnOnSwitch;
    }

    public override void Connect(Port other) {
        base.Connect(other);

        // Jika terhubung dengan BridgeOutputPort, terima informasi
        if (other is BridgeOutputPort bridgeOutputPort) {
            receivedInformation = bridgeOutputPort.Information;

            // Aktifkan Toggle jika menerima informasi
            if (!string.IsNullOrEmpty(receivedInformation) && IsSwitchRelatedActive(other)) {
                Debug.Log("Connect Activate Toggle");
                toggleObject.ToggleSwitch(true);
            }
        }
    }

    public override void Disconnect(){
        base.Disconnect();

        // Nonaktifkan Toggle saat terputus
        receivedInformation = "";
        TurnOffSwitch();
    }

    public void ToggleSwitch() {
        isToggleActive = !isToggleActive;
        toggleObject.ChangeColor(isToggleActive ? Color.green : Color.white);
        switchManager.CheckSwitches();
    }

    public void TurnOnSwitch(bool isGround = false) {
        if (isGround && connectedPort is BridgeOutputPort bridgeOutputPort && bridgeOutputPort.IsActive)
            toggleObject.ToggleSwitch(true);
        else if (IsSwitchRelatedActive())
            toggleObject.ToggleSwitch(true);
    }

    public void TurnOffSwitch() {
        isToggleActive = false;
        toggleObject.ToggleSwitch(false);
        toggleObject.ChangeColor(Color.white);
        switchManager.CheckSwitches();
    }

    public bool IsSwitchRelatedActive(Port otherPort = null) {
        if (otherPort is BridgeOutputPort || connectedPort is BridgeOutputPort) {
            BridgeOutputPort bridgeOutputPort = otherPort is BridgeOutputPort ? (BridgeOutputPort)otherPort : (BridgeOutputPort)connectedPort;
            
            if (bridgeOutputPort.IsActive &&
                ((relatedGroundSwitchPort.ConnectedPort is GroundPort && relatedGroundSwitchPort.IsActive) ||
                (relatedGroundSwitchPort.ConnectedPort is GroundSwitchExtraPort && ((GroundSwitchExtraPort)relatedGroundSwitchPort.ConnectedPort).IsActive))) {
                return true;
            }
            // if (bridgeOutputPort.IsActive) {
            //     if (otherPort is GroundPort || relatedGroundSwitchPort.ConnectedPort is GroundPort) {
            //         GroundPort groundPort = otherPort is GroundPort ? (GroundPort)otherPort : (GroundPort)relatedGroundSwitchPort.ConnectedPort;
            //         if (groundPort.IsActive)
            //             return true;
            //     }else if(otherPort is GroundSwitchExtraPort || relatedGroundSwitchPort.ConnectedPort is GroundSwitchExtraPort) {
            //         GroundSwitchExtraPort groundSwitchExtraPort = otherPort is GroundSwitchExtraPort ? (GroundSwitchExtraPort)otherPort : (GroundSwitchExtraPort)relatedGroundSwitchPort.ConnectedPort;
            //         if (groundSwitchExtraPort.IsActive)
            //             return true;
            //     }
            // }
        }
        return false;
    }
}
