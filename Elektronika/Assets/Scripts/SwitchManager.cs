using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [System.Serializable]
    public struct SwitchPair
    {
        public Switch toggleBtn;
        public GameObject lightbulb;
        public DrawLine.Pair pair;
    }

    [SerializeField] private DrawLine drawLine;
    [SerializeField] SwitchPair[] switchPairs;

    private Dictionary<GameObject, bool> lightbulbStatus = new Dictionary<GameObject, bool>();

    void Start()
    {
        foreach (var pair in switchPairs)
        {
            lightbulbStatus[pair.lightbulb] = false;
        }
    }

    private void ToggleLight(GameObject lightbulb, bool isOn)
    {
        SpriteRenderer bulbSprite = lightbulb.GetComponent<SpriteRenderer>();
        bulbSprite.color = isOn ? Color.yellow : Color.white;
    }

    /*
    public void OnSwitchPressed(Switch pressedSwitch)
    {
        foreach (var switchPair in switchPairs)
        {
            if (switchPair.toggleBtn == pressedSwitch)
            {
                // Checking whether the points are correctly connected in pointToLine
                if (drawLine.PointToLine.ContainsKey(switchPair.pair.point1) &&
                    drawLine.PointToLine.ContainsKey(switchPair.pair.point2) &&
                    drawLine.PointToLine[switchPair.pair.point1] == drawLine.PointToLine[switchPair.pair.point2])
                {
                    lightbulbStatus[switchPair.lightbulb] = !lightbulbStatus[switchPair.lightbulb];
                    ToggleLight(switchPair.lightbulb, lightbulbStatus[switchPair.lightbulb]);
                }
                break;
            }
        }
    }
    */

    /*
    public void OnSwitchPressed(Switch pressedSwitch)
    {
        foreach (var switchPair in switchPairs)
        {
            if (switchPair.toggleBtn == pressedSwitch)
            {
                // Check whether the points are correctly connected in pointToLines
                if (drawLine.PointToLines.ContainsKey(switchPair.pair.point1) &&
                    drawLine.PointToLines.ContainsKey(switchPair.pair.point2))
                {
                    List<LineRenderer> lines1 = drawLine.PointToLines[switchPair.pair.point1];
                    List<LineRenderer> lines2 = drawLine.PointToLines[switchPair.pair.point2];

                    // Check if there is a common line connecting the points
                    foreach (LineRenderer line1 in lines1)
                    {
                        foreach (LineRenderer line2 in lines2)
                        {
                            if (line1 == line2)
                            {
                                // The points are correctly connected
                                lightbulbStatus[switchPair.lightbulb] = !lightbulbStatus[switchPair.lightbulb];
                                ToggleLight(switchPair.lightbulb, lightbulbStatus[switchPair.lightbulb]);
                                return;
                            }
                        }
                    }
                }

                // The points are not correctly connected
                Debug.Log("Salah");
                return;
            }
        }
    }
    */

    public void OnSwitchPressed(Switch pressedSwitch)
    {
        foreach (var switchPair in switchPairs)
        {
            if (switchPair.toggleBtn == pressedSwitch)
            {
                // Accessing DrawLine's private pointToLine dictionary by reflection
                var pointToLineField = typeof(DrawLine).GetField("pointToLine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pointToLine = pointToLineField.GetValue(drawLine) as Dictionary<GameObject, LineRenderer>;

                if (pointToLine.ContainsKey(switchPair.pair.point1) &&
                    pointToLine.ContainsKey(switchPair.pair.point2) &&
                    pointToLine[switchPair.pair.point1] == pointToLine[switchPair.pair.point2])
                {
                    lightbulbStatus[switchPair.lightbulb] = !lightbulbStatus[switchPair.lightbulb];
                    ToggleLight(switchPair.lightbulb, lightbulbStatus[switchPair.lightbulb]);
                }
                else
                {
                    // The points are not correctly connected
                    Debug.Log("Salah");
                }
                return;
            }
        }
    }
}
