using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPortManager : MonoBehaviour
{
    [Header("IC Port Manager")]
    [SerializeField, Tooltip("VCC IC Port")] VCC_ICPort vcc_ICPort;
    [SerializeField, Tooltip("Ground IC Port")] GroundICPort groundICPort;
    // [SerializeField, Tooltip("List of IC Ports")] List<ICPort> iCPorts;

    // [SerializeField] private List<string> keys = new List<string>();
    [SerializeField] private List<ICPort> ports = new List<ICPort>();

    private Dictionary<string, ICPort> iCPorts = new Dictionary<string, ICPort>();

    bool isVCCActive = false;
    bool isGroundActive = false;

    private void Start() {
        vcc_ICPort.OnConnect += Activate;
        vcc_ICPort.OnDisconnect += Deactivate;

        groundICPort.OnConnect += Activate;
        groundICPort.OnDisconnect += Deactivate;

        // ICPort[] ports = FindObjectsOfType<ICPort>();
        foreach (ICPort port in ports)
        {
            iCPorts[port.name] = port;
        }
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
            foreach(ICPort iCPort in iCPorts.Values) {
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
            foreach(ICPort iCPort in iCPorts.Values) {
                iCPort.Deactivate();
            }
        }
    }

    public void UpdateICPort(ComponentManager.ComponentType componentType) {
        Dictionary<string, string> informationsToAdd = null;

        switch (componentType) {
            case ComponentManager.ComponentType._74LS148:
                informationsToAdd = new Dictionary<string, string>
                {
                    { "Port IC 7", "7" },
                    { "Port IC 6", "6" },
                    { "Port IC 5", "5" },
                    { "Port IC 4", "4" },
                    { "Port IC 3", "3" },
                    { "Port IC 2", "2" },
                    { "Port IC 1", "1" },
                    { "Port IC 0", "0" },
                    { "Port IC A0", "A0" },
                    { "Port IC A1", "A1" },
                    { "Port IC A2", "A2" },
                    { "Port IC Ground", "Ground" },
                    { "Port IC VCC", "VCC" },
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
            iCPort.UpdateInformation("");
            iCPort.DisconnectConnection();
        }
    }
}
