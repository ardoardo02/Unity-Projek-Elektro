using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField] private SwitchPort[] switchPorts; // Referensi ke semua SwitchPort
    [SerializeField] private LEDPort[] ledPorts; // Referensi ke semua LEDPort

    public void CheckSwitches()
    {
        int step = 7;

        for (; step >= 0; step--)
        {
            bool found = false;

            foreach (var switchPort in switchPorts)
            {
                if (switchPort.ConnectedPort != null) {
                    Debug.Log("Connected");
                        if(switchPort.IsGroundSwitchActive()) {
                        Debug.Log("GroundPort Active");
                        if (switchPort.IsToggleActive && switchPort.ReceivedInformation == step.ToString())
                        {
                            Debug.Log("Found");
                            found = true;
                            break;
                        }
                    }
                        
                    
                }
            }

            if (!found)
            {
                break; // Jika tidak menemukan SwitchPort dengan informasi yang cocok, keluar dari loop
            }
        }

        switch (step)
        {
            case 7:
                TurnLamps();
                break;
            case 6:
                TurnLamps("A0");
                break;
            case 5:
                TurnLamps("A1");
                break;
            case 4:
                TurnLamps("A0", "A1");
                break;
            case 3:
                TurnLamps("A2");
                break;
            case 2:
                TurnLamps("A0", "A2");
                break;
            case 1:
                TurnLamps("A1", "A2");
                break;
            case 0:
                TurnLamps("A0", "A1", "A2");
                break;
        }
    }

    public void TurnLamps(params string[] lampCodes)
    {
        foreach (var ledPort in ledPorts)
        {
            ledPort.TurnLight(false);
        }

        if(lampCodes.Length == 0)
        {
            return;
        }

        foreach (var code in lampCodes)
        {
            foreach (var ledPort in ledPorts)
            {
                if (ledPort.ReceivedInformation == code)
                {
                    ledPort.TurnLight(true);
                }
            }
        }
    }
}