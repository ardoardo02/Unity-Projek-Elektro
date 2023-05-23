using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] LineRenderer lr_prefab;
    LineRenderer lr;
    // Transform[] points;

    List<Transform> points = new List<Transform>();
    // List<PortCable> ports = new List<PortCable>();
    Dictionary<PortCable, PortCable> portPoints = new Dictionary<PortCable, PortCable>();

    PortCable portCable;
    Vector3 mousePos;
    
    void PrintList()
    {
        if(points.Count == 0)
        {
            Debug.Log("List is empty");
            return;
        }

        foreach (Transform point in points)
        {
            Debug.Log(point.name);
        }
    }

    void PrintPortPoints()
    {
        if(portPoints.Count == 0)
        {
            Debug.Log("List is empty");
            return;
        }

        foreach (KeyValuePair<PortCable, PortCable> portPoint in portPoints)
        {
            Debug.Log(portPoint.Key.name + " " + portPoint.Value.name);
        }
    }

    public void CheckDrag(PortCable port)
    {
        if(port == portCable){
            RemoveLine();
            return;
        }

        if(IsConnected(port)){
            CheckPortPoints(port);
            return;
        }
    }

    public void DragLine(PortCable port)
    {
        if(port == portCable){
            RemoveLine();
            return;
        }

        if(portCable != null){
            if(IsConnected(port)){
                RemoveLine();
                return;
            }
            points.Add(port.transform);
            portPoints.Add(portCable, port);
            SetUpLine();
            return;
        }

        // if(CheckPortPoints(port)){
        //     return;
        // }
        CheckPortPoints(port);

        // PrintList();

        points.Add(port.transform);

        lr = Instantiate(lr_prefab, transform);
        lr.positionCount = 2;
        // lr.SetPosition(0, port.transform.position);
        lr.SetPosition(0, new Vector3(
            port.transform.position.x,
            port.transform.position.y,
            85
        ));

        // PrintList();
        portCable = port;
    }

    public void SetUpLine()
    {
        Destroy(lr.gameObject);
        portCable = null;

        lr = Instantiate(lr_prefab, transform);
        lr.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, new Vector3(
                points[i].position.x,
                points[i].position.y,
                85
            ));
        }

        points.Clear();

        // this.points = points;
    }

    void RemoveLine()
    {
        Destroy(lr.gameObject);
        lr = null;
        points.Clear();
        portCable = null;

        Debug.Log("Line removed");
    }

    void CheckPortPoints(PortCable checkPort)
    {
        foreach (KeyValuePair<PortCable, PortCable> portPoint in portPoints)
        {
            if(portPoint.Key == checkPort || portPoint.Value == checkPort)
            {
                // Debug.Log("Port is already connected");

                PrintPortPoints();
                int i = portPoints.Keys.ToList().IndexOf(portPoint.Key);
                Debug.Log(i);
                Destroy(transform.GetChild(i).gameObject);
                portPoints.Remove(portPoint.Key);
                points.Clear();
                RearrangePortPoints();
                PrintPortPoints();
                return;
            }
        }
        // return false;
    }

    void RearrangePortPoints()
    {
        Dictionary<PortCable, PortCable> tempPortPoints = new Dictionary<PortCable, PortCable>();
        foreach (KeyValuePair<PortCable, PortCable> portPoint in portPoints)
        {
            if(portPoint.Key == null || portPoint.Value == null)
            {
                continue;
            }
            tempPortPoints.Add(portPoint.Key, portPoint.Value);
        }
        portPoints = tempPortPoints;
    }

    bool IsConnected(PortCable checkPort)
    {
        foreach (KeyValuePair<PortCable, PortCable> portPoint in portPoints)
        {
            if(portPoint.Key == checkPort || portPoint.Value == checkPort)
            {
                Debug.Log("Port is already connected");
                return true;
            }
        }
        return false;
    }

    private void Update() {
        if(portCable == null)
            return;
        
        mousePos = Input.mousePosition;
        lr.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            95
        )));
        // for (int i = 0; i < points.Count; i++)
        // {
        //     lr.SetPosition(i, points[i].position);
        // }
    }
}
