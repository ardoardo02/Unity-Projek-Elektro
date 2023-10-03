using System;
using UnityEngine;

public class GroundLEDPort : Port
{
    [Header("Ground LED Settings")]
    [SerializeField] PortManager portManager;
    [SerializeField, Tooltip("Related Ground LED Extra Port")] GroundLEDExtraPort relatedGroundLEDExtra;

    bool isActive = false;

    public Action<bool> TriggerTurnOffLightEvent;
    public Action TriggerPortActivatedEvent;

    public bool IsActive { get => isActive; }

    private void Start() 
    {
        // Jika ada relatedGroundLEDExtra, nonaktifkan awalnya
        if(relatedGroundLEDExtra)
            relatedGroundLEDExtra.gameObject.SetActive(false);
    }

    public void ActivatePort() 
    {
        if(relatedGroundLEDExtra) {
            isActive = true;
            relatedGroundLEDExtra.IsActive = true;
            TriggerPortActivatedEvent?.Invoke();
            if(relatedGroundLEDExtra.ConnectedPort is GroundLEDPort groundLEDPort)
                groundLEDPort.ActivatePort();
        }
    }

    public void DeactivatePort() 
    {
        if(relatedGroundLEDExtra) {
            isActive = false;
            TriggerTurnOffLightEvent?.Invoke(false);
            relatedGroundLEDExtra.Deactivate();
        }
    }

    public override void Connect(Port other) 
    {
        base.Connect(other);
        
        if(relatedGroundLEDExtra) {
            relatedGroundLEDExtra.gameObject.SetActive(true);

            if(other is GroundPort || other is GroundLEDExtraPort) {
                GameManager.Instance.CheckPort(this, true);
                if ((other is GroundPort groundPort && groundPort.IsActive) ||
                    (other is GroundLEDExtraPort groundLEDExtraPort && groundLEDExtraPort.IsActive)) {
                    ActivatePort();
                }
            }
            else GameManager.Instance.AddMistake();
        }
    }

    public override void Disconnect() 
    {
        if (connectedPort is GroundPort || connectedPort is GroundLEDExtraPort)
            GameManager.Instance.CheckPort(this, false);

        base.Disconnect();
        
        if(relatedGroundLEDExtra) {
            portManager.DisconnectExistingConnection(relatedGroundLEDExtra);
            relatedGroundLEDExtra.gameObject.SetActive(false);
            DeactivatePort();
        }
    }
}
