using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPortManager : MonoBehaviour
{
    [Header("IC Port Manager")]
    // [SerializeField, Tooltip("VCC IC Port")] VCC_ICPort vcc_ICPort;
    // [SerializeField, Tooltip("Ground IC Port")] GroundICPort groundICPort;
    // [SerializeField, Tooltip("List of IC Ports")] List<ICPort> iCPorts;

    // [SerializeField] private List<string> keys = new List<string>();
    [SerializeField] private List<ICPort> ports = new List<ICPort>();

    private Dictionary<string, ICPort> iCPorts = new Dictionary<string, ICPort>();

    bool isVCCActive = false;
    bool isGroundActive = false;
    bool isGSActive = false;
    bool isEOActive = false;
    bool isE1Active = false;

    private void Start() {
        // vcc_ICPort.OnConnect += Activate;
        // vcc_ICPort.OnDisconnect += Deactivate;

        // groundICPort.OnConnect += Activate;
        // groundICPort.OnDisconnect += Deactivate;

        // ICPort[] ports = FindObjectsOfType<ICPort>();
        foreach (ICPort port in ports)
        {
            iCPorts[port.name] = port;
        }
    }

    private void OnDestroy() {
        // vcc_ICPort.OnConnect -= Activate;
        // vcc_ICPort.OnDisconnect -= Deactivate;

        // groundICPort.OnConnect -= Activate;
        // groundICPort.OnDisconnect -= Deactivate;
    }

    public void Activate(Port port, string icInfo) {
        if((port is VCC_ICPort || port is VCCPort) && icInfo == "VCC") {
            isVCCActive = true;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "Ground") {
            isGroundActive = true;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "GS") {
            isGSActive = true;
        } else if((port is VCC_ICPort || port is VCCPort) && icInfo == "EO") {
            isEOActive = true;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "E1") {
            isE1Active = true;
        }

        // Debug.Log("VCC: " + isVCCActive + ", Ground: " + isGroundActive + ", GS: " + isGSActive + ", EO: " + isEOActive);

        if(isVCCActive && isGroundActive && isGSActive && isEOActive && isE1Active) {
            foreach(ICPort iCPort in iCPorts.Values) {
                iCPort.Activate();
            }
        }
    }

    public void Deactivate(Port port, string icInfo) {
        if((port is VCC_ICPort || port is VCCPort) && icInfo == "VCC") {
            isVCCActive = false;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "Ground") {
            isGroundActive = false;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "GS") {
            isGSActive = false;
        } else if((port is VCC_ICPort || port is VCCPort) && icInfo == "EO") {
            isEOActive = false;
        } else if((port is GroundICPort || port is GroundPort) && icInfo == "E1") {
            isE1Active = false;
        }

        if(!isVCCActive || !isGroundActive || !isGSActive || !isEOActive || !isE1Active) {
            foreach(ICPort iCPort in iCPorts.Values) {
                iCPort.Deactivate();
            }
        }
    }

    public void UpdateICPort(ComponentManager.ComponentType componentType, string icPos) {
        Dictionary<string, string> informationsToAdd = null;
        int icPosInt = icPos == "left" ? 0 : icPos == "mid" ? 1 : 2;

        switch (componentType) {
            case ComponentManager.ComponentType._74LS148:
                informationsToAdd = new Dictionary<string, string>
                {
                    { "Port IC Bot" + (4 + icPosInt), "7" },
                    { "Port IC Bot" + (3 + icPosInt), "6" },
                    { "Port IC Bot" + (2 + icPosInt), "5" },
                    { "Port IC Bot" + (1 + icPosInt), "4" },
                    { "Port IC Up" + (4 + icPosInt), "3" },
                    { "Port IC Up" + (5 + icPosInt), "2" },
                    { "Port IC Up" + (6 + icPosInt), "1" },
                    { "Port IC Up" + (7 + icPosInt), "0" },
                    { "Port IC Up" + (8 + icPosInt), "A0" },
                    { "Port IC Bot" + (7 + icPosInt), "A1" },
                    { "Port IC Bot" + (6 + icPosInt), "A2" },
                    { "Port IC Bot" + (5 + icPosInt), "E1" },
                    { "Port IC Up" + (2 + icPosInt), "EO" },
                    { "Port IC Up" + (3 + icPosInt), "GS" },
                    { "Port IC Bot" + (8 + icPosInt), "Ground" },
                    { "Port IC Up" + (1 + icPosInt) , "VCC" },
                };
                break;
                
            case ComponentManager.ComponentType.Type2:
                informationsToAdd = new Dictionary<string, string>
                {
                    { "ICPortName10", "3" },
                    { "ICPortName24", "5" },
                    { "ICPortName35", "2" },
                    { "ICPortName1", "7" },
                    { "ICPortName23", "4" },
                };
                break;
        }

        if (informationsToAdd != null) {
            UpdateInformations(informationsToAdd);
        }
    }

    private void UpdateInformations(Dictionary<string, string> informationsToAdd) {
        foreach (var pair in informationsToAdd)
        {
            if (iCPorts.TryGetValue(pair.Key, out ICPort port))
            {
                port.UpdateInformation(pair.Value);
            }
        }
    }

    public void RemoveAllICPortInfo () {
        foreach (ICPort iCPort in iCPorts.Values) {
            iCPort.DisconnectConnection();
        }
    }
}
