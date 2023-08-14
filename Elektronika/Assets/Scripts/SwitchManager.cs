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
            Debug.Log("Step: " + step);

            foreach (var switchPort in switchPorts)
            {
                if (switchPort.ConnectedPort != null) {
                    if(switchPort.IsGroundSwitchActive()) {
                        if (switchPort.IsToggleActive && switchPort.ReceivedInformation == step.ToString())
                        {
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
        // Debug.Log("Turning Off Lamps");
        foreach (var ledPort in ledPorts)
        {
            ledPort.TurnLight(false);
        }

        if(lampCodes.Length == 0)
        {
            return;
        }

        // Debug.Log("Turning On Lamps: " + string.Join(", ", lampCodes) + "");
        foreach (var code in lampCodes)
        {
            foreach (var ledPort in ledPorts)
            {
                // Debug.Log("Checking Lamp: " + ledPort.name + " with code: " + code + " and received information: " + ledPort.ReceivedInformation);
                if (ledPort.ReceivedInformation == code)
                {
                    ledPort.TurnLight(true);
                }
            }
        }
    }
}