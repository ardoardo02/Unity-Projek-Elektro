using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField] private SwitchPort[] switchPorts; // Referensi ke semua SwitchPort
    [Header("LED Settings")]
    [SerializeField] private LEDPort[] ledPorts; // Referensi ke semua LEDPort
    [Header("Segment Settings")]
    [SerializeField] private SegmentPort[] segmentPorts; // Referensi ke semua SegmentPort
    [SerializeField] private VCC_SegmentPort[] vcc_SegmentPorts; // Referensi ke semua VCC_SegmentPort
    [SerializeField] private Sprite[] segnmentSprites; // Referensi ke semua Sprite Segment
    [SerializeField] private SpriteRenderer segmentSpriteRenderer; // Referensi ke semua SpriteRenderer Segment

    public static SwitchManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckSwitches() {
        if (GameManager.Instance.GetInsertedICType() == ComponentManager.ComponentType._74LS148) {
            CheckEncoder();
        }
        else if (GameManager.Instance.GetInsertedICType() == ComponentManager.ComponentType._7447) {
            CheckDecoder();
        }
    }

    void CheckEncoder() {
        int step = 7;

        for (; step >= 0; step--)
        {
            bool found = false;
            Debug.Log("Step: " + step);

            foreach (var switchPort in switchPorts)
            {
                if (switchPort.ConnectedPort != null) {
                    if(switchPort.IsSwitchRelatedActive()) {
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

        GameManager.Instance.CheckInput(step);

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

    void CheckDecoder() {
        // Reset segment
        segmentSpriteRenderer.sprite = null;

        // // Check if all vcc segment ports are active
        // foreach (var vcc_SegmentPort in vcc_SegmentPorts) {
        //     if (!vcc_SegmentPort.IsActive) return;
        // }

        // Check if is there any active vcc segment port
        bool isAnyActiveSegment = false;
        foreach (var vcc_SegmentPort in vcc_SegmentPorts) {
            if (vcc_SegmentPort.IsActive) {
                isAnyActiveSegment = true;
                break;
            }
        }
        if (!isAnyActiveSegment) return;

        // Check if all segment ports are active
        foreach (var segmentPort in segmentPorts) {
            if (!segmentPort.IsActive) return;
        }

        List<string> activeSwitches = new List<string>();

        // find which switch is active
        foreach (var switchPort in switchPorts)
        {
            if (switchPort.ConnectedPort != null) {
                if(switchPort.IsSwitchRelatedActive()) {
                    if (switchPort.IsToggleActive)
                    {
                        activeSwitches.Add(switchPort.ReceivedInformation);
                    }
                }
            }
        }
        
        // turn on segment based on active switches
        if (activeSwitches.Count == 0) {
            segmentSpriteRenderer.sprite = segnmentSprites[0]; // = 0
            GameManager.Instance.CheckInput(0);
        }
        else if (activeSwitches.Count == 1) {
            switch (activeSwitches[0])
            {
                case "A":
                    segmentSpriteRenderer.sprite = segnmentSprites[1]; // A = 1
                    GameManager.Instance.CheckInput(1);
                    break;
                case "B":
                    segmentSpriteRenderer.sprite = segnmentSprites[2]; // B = 2
                    GameManager.Instance.CheckInput(2);
                    break;
                case "C":
                    segmentSpriteRenderer.sprite = segnmentSprites[4]; // C = 4
                    GameManager.Instance.CheckInput(4);
                    break;
                case "D":
                    segmentSpriteRenderer.sprite = segnmentSprites[8]; // D = 8
                    GameManager.Instance.CheckInput(8);
                    break;
            }
        }
        else if (activeSwitches.Count == 2) {
            if (activeSwitches.Contains("A")) {
                if (activeSwitches.Contains("B")) {
                    segmentSpriteRenderer.sprite = segnmentSprites[3]; // A + B = 3
                    GameManager.Instance.CheckInput(3);
                }
                else if (activeSwitches.Contains("C")) {
                    segmentSpriteRenderer.sprite = segnmentSprites[5]; // A + C = 5
                    GameManager.Instance.CheckInput(5);
                }
                else if (activeSwitches.Contains("D")) {
                    segmentSpriteRenderer.sprite = segnmentSprites[9]; // A + D = 9
                    GameManager.Instance.CheckInput(9);
                }
            }
            else if (activeSwitches.Contains("B")) {
                if (activeSwitches.Contains("C")) {
                    segmentSpriteRenderer.sprite = segnmentSprites[6]; // B + C = 6
                    GameManager.Instance.CheckInput(6);
                }
                else if (activeSwitches.Contains("D")) {
                    segmentSpriteRenderer.sprite = segnmentSprites[10]; // B + D = 10
                    GameManager.Instance.CheckInput(10);
                }
            }
            else if (activeSwitches.Contains("C") && activeSwitches.Contains("D")) {
                segmentSpriteRenderer.sprite = segnmentSprites[10]; // C + D = 12
                GameManager.Instance.CheckInput(10);
            }
        }
        else if (activeSwitches.Count == 3) {
            if (!activeSwitches.Contains("D")) {
                segmentSpriteRenderer.sprite = segnmentSprites[7]; // A + B + C = 7
                GameManager.Instance.CheckInput(7);
            }
            else if (!activeSwitches.Contains("C")) {
                segmentSpriteRenderer.sprite = segnmentSprites[10]; // A + B + D = 11
                GameManager.Instance.CheckInput(10);
            }
            else if (!activeSwitches.Contains("B")) {
                segmentSpriteRenderer.sprite = segnmentSprites[10]; // A + C + D = 13
                GameManager.Instance.CheckInput(10);
            }
            else if (!activeSwitches.Contains("A")) {
                segmentSpriteRenderer.sprite = segnmentSprites[10]; // B + C + D = 14
                GameManager.Instance.CheckInput(10);
            }
        }
        else if (activeSwitches.Count == 4) {
            segmentSpriteRenderer.sprite = segnmentSprites[10]; // A + B + C + D = 15
            GameManager.Instance.CheckInput(10);
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