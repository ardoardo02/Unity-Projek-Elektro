using UnityEngine;
using static ComponentManager;

public class SwitchPort : Port
{
    [Header("Switch Settings")]
    [SerializeField, Tooltip("Related Ground Switch Port")] private GroundSwitchPort relatedGroundSwitchPort;
    [SerializeField, Tooltip("Related Toggle Object")] private Switch toggleObject;
    [SerializeField] SwitchManager switchManager;
    [SerializeField] string[] validCharacters = { "A", "B", "C", "D" };


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
        Debug.Log(GameManager.Instance.GetInsertedICType());

        // Jika terhubung dengan BridgeOutputPort, terima informasi
        if (GameManager.Instance.GetInsertedICType() == ComponentType._74LS148 && 
            other is BridgeOutputPort bridgeOutputPort) {
            
            receivedInformation = bridgeOutputPort.Information;

            // Aktifkan Toggle jika menerima informasi
            if (!string.IsNullOrEmpty(receivedInformation)) {
                GameManager.Instance.CheckPort(this, true);
                if (IsSwitchRelatedActive(other)){
                    Debug.Log("Connect Activate Toggle");
                    toggleObject.EnableSwitch(true);
                }
            }
            else GameManager.Instance.AddMistake();
        }
        else if (GameManager.Instance.GetInsertedICType() == ComponentType._7447 &&
            other is ICPort icPort && System.Array.Exists(validCharacters, element => element == icPort.Information)) {

            // Debug.Log("IC: 7447 | Port: " + icPort.Information);
            receivedInformation = icPort.Information;
            GameManager.Instance.CheckPort(this, true);

            if (IsSwitchRelatedActive(other)){
                Debug.Log("Connect Activate Toggle");
                toggleObject.EnableSwitch(true);
            }
        
        }
        else GameManager.Instance.AddMistake();
    }

    public override void Disconnect(){
        base.Disconnect();

        // Nonaktifkan Toggle saat terputus
        if(!string.IsNullOrEmpty(receivedInformation)){
            GameManager.Instance.CheckPort(this, false);
            receivedInformation = "";
        }
        TurnOffSwitch();
    }

    public void ToggleSwitch() {
        isToggleActive = !isToggleActive;
        toggleObject.ToggleSwitch(isToggleActive);
        switchManager.CheckSwitches();
    }

    public void TurnOnSwitch(bool isGround = false) {
        if (isGround && connectedPort is BridgeOutputPort bridgeOutputPort && bridgeOutputPort.IsActive)
            toggleObject.EnableSwitch(true);
        else if (isGround && connectedPort is ICPort iCPort && iCPort.IsActive)
            toggleObject.EnableSwitch(true);
        else if (IsSwitchRelatedActive())
            toggleObject.EnableSwitch(true);
    }

    public void TurnOffSwitch() {
        isToggleActive = false;
        toggleObject.EnableSwitch(false);
        toggleObject.ToggleSwitch(false);
        switchManager.CheckSwitches();
    }

    public bool IsSwitchRelatedActive(Port otherPort = null) {
        Debug.Log("IsSwitchRelatedActive");
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
        else if (otherPort is ICPort || connectedPort is ICPort) {
            ICPort icPort = otherPort is ICPort ? (ICPort)otherPort : (ICPort)connectedPort;
            if (icPort.IsActive && ((relatedGroundSwitchPort.ConnectedPort is GroundPort && relatedGroundSwitchPort.IsActive) ||
                (relatedGroundSwitchPort.ConnectedPort is GroundSwitchExtraPort && ((GroundSwitchExtraPort)relatedGroundSwitchPort.ConnectedPort).IsActive))){
                    Debug.Log("ICPort: " + icPort.Information + " | GroundPort: " + relatedGroundSwitchPort.ConnectedPort.name + " | IsActive: " + relatedGroundSwitchPort.IsActive);
                    return true;
                }
        }
        return false;
    }
}
