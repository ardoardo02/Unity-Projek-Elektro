using System.Collections;
using System.Collections.Generic;
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

    public void DragLine(PortCable port)
    {
        if(port == portCable){
            RemoveLine();
            return;
        }

        if(portCable != null){
            points.Add(port.transform);
            portPoints.Add(portCable, port);
            SetUpLine();
            return;
        }

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
                Debug.Log("Port is already connected");
                return;
            }
        }
    }

    void GetIndexPortPoints()
    {
        foreach (KeyValuePair<PortCable, PortCable> portPoint in portPoints)
        {
            Debug.Log(portPoint.Key.name + " " + portPoint.Value.name);
        }
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
