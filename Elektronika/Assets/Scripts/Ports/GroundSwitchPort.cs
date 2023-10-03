using System;
using UnityEngine;

public class GroundSwitchPort : Port
{
    [Header("Ground Switch Settings")]
    [SerializeField] PortManager portManager;
    [SerializeField, Tooltip("Related Ground Switch Extra Port")] GroundSwitchExtraPort relatedGroundSwitchExtra;

    bool isActive = false;

    public Action TriggerTurnOffLightEvent;
    public Action<bool> TriggerPortActivatedEvent;
    public bool IsActive { get => isActive; }

    private void Start() 
    {
        // Jika ada relatedGroundSwitchExtra, nonaktifkan awalnya
        if(relatedGroundSwitchExtra)
            relatedGroundSwitchExtra.gameObject.SetActive(false);
    }

    public void ActivatePort() 
    {
        if(relatedGroundSwitchExtra) {
            isActive = true;
            relatedGroundSwitchExtra.IsActive = true;
            TriggerPortActivatedEvent?.Invoke(true);
            if(relatedGroundSwitchExtra.ConnectedPort is GroundSwitchPort groundSwitchPort)
                groundSwitchPort.ActivatePort();
        }
    }

    public void DeactivatePort() 
    {
        if(relatedGroundSwitchExtra) {
            isActive = false;
            TriggerTurnOffLightEvent?.Invoke();
            relatedGroundSwitchExtra.Deactivate();
            if(relatedGroundSwitchExtra.ConnectedPort is GroundSwitchPort groundSwitchPort)
                groundSwitchPort.DeactivatePort();
        }
    }

    public override void Connect(Port other) 
    {
        base.Connect(other);
        
        if(relatedGroundSwitchExtra) {
            relatedGroundSwitchExtra.gameObject.SetActive(true);
            
            if (other is GroundPort || other is GroundSwitchExtraPort){
                GameManager.Instance.CheckPort(this, true);
                if ((other is GroundPort groundPort && groundPort.IsActive) ||
                    (other is GroundSwitchExtraPort groundSwitchExtraPort && groundSwitchExtraPort.IsActive)) {
                    ActivatePort();
                }
            }
            else GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() 
    {
        if (connectedPort is GroundPort || connectedPort is GroundSwitchExtraPort)
            GameManager.Instance.CheckPort(this, false);

        base.Disconnect();
        
        // Nonaktifkan relatedGroundSwitchExtra saat terputus
        if(relatedGroundSwitchExtra) {
            portManager.DisconnectExistingConnection(relatedGroundSwitchExtra);
            relatedGroundSwitchExtra.IsActive = false;
            relatedGroundSwitchExtra.gameObject.SetActive(false);
        }
        
        TriggerTurnOffLightEvent?.Invoke();
    }
}
