using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPortManager : MonoBehaviour
{
    [Header("IC Port Manager")]
    [SerializeField, Tooltip("VCC IC Port")] VCC_ICPort vcc_ICPort;
    [SerializeField, Tooltip("Ground IC Port")] GroundICPort groundICPort;
    [SerializeField, Tooltip("List of IC Ports")] List<ICPort> iCPorts;

    bool isVCCActive = false;
    bool isGroundActive = false;

    private void Start() {
        vcc_ICPort.OnConnect += Activate;
        vcc_ICPort.OnDisconnect += Deactivate;

        groundICPort.OnConnect += Activate;
        groundICPort.OnDisconnect += Deactivate;
    }

    private void OnDestroy() {
        vcc_ICPort.OnConnect -= Activate;
        vcc_ICPort.OnDisconnect -= Deactivate;

        groundICPort.OnConnect -= Activate;
        groundICPort.OnDisconnect -= Deactivate;
    }

    public void Activate(Port port) {
        if(port is VCC_ICPort) {
            isVCCActive = true;
        } else if(port is GroundICPort) {
            isGroundActive = true;
        }

        if(isVCCActive && isGroundActive) {
            foreach(ICPort iCPort in iCPorts) {
                iCPort.Activate();
            }
        }
    }

    public void Deactivate(Port port) {
        if(port is VCC_ICPort) {
            isVCCActive = false;
        } else if(port is GroundICPort) {
            isGroundActive = false;
        }

        if(!isVCCActive || !isGroundActive) {
            foreach(ICPort iCPort in iCPorts) {
                iCPort.Deactivate();
            }
        }
    }
}
