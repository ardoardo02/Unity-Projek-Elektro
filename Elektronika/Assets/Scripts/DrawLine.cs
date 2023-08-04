using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [System.Serializable]
    public struct Pair
    {
        public GameObject point1;
        public GameObject point2;
    }

    [SerializeField] GameObject linePrefab;
    [SerializeField] List<GameObject> normalPoints = new List<GameObject>();
    [SerializeField] List<GameObject> doublePoints = new List<GameObject>();
    [SerializeField] Pair[] pairs;


    private Dictionary<GameObject, GameObject> correctPairs = new Dictionary<GameObject, GameObject>();
    // private Dictionary<GameObject, List<LineRenderer>> pointToLines = new Dictionary<GameObject, List<LineRenderer>>();
    private Dictionary<GameObject, int> pointConnectionCount = new Dictionary<GameObject, int>();
    // public Dictionary<GameObject, List<LineRenderer>> PointToLines { get => pointToLines; }
    // public Dictionary<GameObject, GameObject> CorrectPairs { get; private set; } = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, LineRenderer> pointToLine = new Dictionary<GameObject, LineRenderer>();
    public Dictionary<GameObject, LineRenderer> PointToLine { get; private set; } = new Dictionary<GameObject, LineRenderer>();


    private bool isMousePressed;
    private GameObject startPoint;
    private LineRenderer currentLine;



    void Start()
    {
        isMousePressed = false;
        startPoint = null;
        currentLine = null;

        // Add correct pairs from the defined pairs
        foreach (Pair pair in pairs)
        {
            correctPairs[pair.point1] = pair.point2;
            correctPairs[pair.point2] = pair.point1;
        }
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
        else if (isMousePressed && startPoint != null)
        {
            HandleMouseHold();
        }
    }

    /*
    private void HandleMouseDown()
    {
        startPoint = GetNearPoint();

        if (startPoint != null)
        {
            isMousePressed = true;
            GameObject lineObject = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            currentLine = lineObject.GetComponent<LineRenderer>();
            currentLine.positionCount = 1;
            currentLine.SetPosition(0, startPoint.transform.position);

            RemoveExistingLineFromPoint(startPoint);
        }
    }
    */

    private void HandleMouseDown()
    {
        startPoint = GetNearPoint();

        if (startPoint != null)
        {
            // Jika startPoint adalah bagian dari doublePoints dan sudah memiliki dua garis, batalkan operasi
            if (doublePoints.Contains(startPoint) && pointConnectionCount.ContainsKey(startPoint) && pointConnectionCount[startPoint] >= 2)
            {
                startPoint = null;
                return;
            }

            // Jika startPoint adalah bagian dari normalPoints dan sudah memiliki satu garis, batalkan operasi
            if (normalPoints.Contains(startPoint) && pointConnectionCount.ContainsKey(startPoint) && pointConnectionCount[startPoint] >= 1)
            {
                startPoint = null;
                return;
            }

            isMousePressed = true;
            GameObject lineObject = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            currentLine = lineObject.GetComponent<LineRenderer>();
            currentLine.positionCount = 1;
            currentLine.SetPosition(0, startPoint.transform.position);
        }
    }

    /*
    private void HandleMouseUp()
    {
        if (startPoint != null && isMousePressed)
        {
            GameObject endPoint = GetNearPoint();

            if (endPoint != null && startPoint != endPoint)
            {
                if (doublePoints.Contains(startPoint) && GetLineCountForPoint(startPoint) >= 2)
                {
                    // This is the third line for doublePoints, it cannot draw more lines
                    Destroy(currentLine.gameObject);
                }
                else
                {
                    // This is the first line or normalPoints, or doublePoints with no existing lines
                    currentLine.positionCount = 2;
                    currentLine.SetPosition(1, endPoint.transform.position);
                    AddLineToPoints(startPoint, endPoint, currentLine);
                }
            }
            else
            {
                Destroy(currentLine.gameObject);
            }
        }

        isMousePressed = false;
        startPoint = null;
        currentLine = null;
    }
    */

    private void HandleMouseUp()
    {
        if (startPoint != null && isMousePressed)
        {
            GameObject endPoint = GetNearPoint();

            if (endPoint != null && normalPoints.Contains(endPoint) && pointConnectionCount.ContainsKey(endPoint) && pointConnectionCount[endPoint] >= 1 && doublePoints.Contains(startPoint) == false)
            {
                Destroy(currentLine.gameObject);
            }
            else if (endPoint != null && startPoint != endPoint)
            {
                currentLine.positionCount = 2;
                currentLine.SetPosition(1, endPoint.transform.position);
                pointToLine[startPoint] = currentLine;

                RemoveExistingLineFromPoint(endPoint);

                pointToLine[endPoint] = currentLine;

                // Check if the pair is correct
                if (correctPairs[startPoint] == endPoint)
                {
                    Debug.Log("Benar");
                }
                else
                {
                    Debug.Log("Salah");
                }

                // Tambahkan informasi koneksi ke dictionary
                if (!pointConnectionCount.ContainsKey(startPoint))
                    pointConnectionCount[startPoint] = 0;
                pointConnectionCount[startPoint]++;

                if (!pointConnectionCount.ContainsKey(endPoint))
                    pointConnectionCount[endPoint] = 0;
                pointConnectionCount[endPoint]++;
            }
            else
            {
                Destroy(currentLine.gameObject);
            }
        }

        isMousePressed = false;
        startPoint = null;
        currentLine = null;
    }

    private void HandleMouseHold()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLine.positionCount = 2;
        currentLine.SetPosition(1, mousePosition);
    }

    /*
    private void RemoveExistingLineFromPoint(GameObject point)
    {
        if (PointToLines.ContainsKey(point))
        {
            List<LineRenderer> lines = PointToLines[point];
            foreach (LineRenderer line in lines)
            {
                if (line != null)
                {
                    PointToLines.Remove(point);
                    foreach (var pair in PointToLines)
                    {
                        if (pair.Value.Contains(line))
                        {
                            PointToLines[pair.Key].Remove(line);
                        }
                    }
                    Destroy(line.gameObject);
                }
            }
        }
    }
    */

    private void RemoveExistingLineFromPoint(GameObject point)
    {
        if (pointToLine.ContainsKey(point))
        {
            LineRenderer oldLine = pointToLine[point];
            pointToLine.Remove(point);

            if (oldLine != null)
            {
                foreach (var pair in pointToLine)
                {
                    if (pair.Value == oldLine && pair.Key != point)
                    {
                        pointToLine.Remove(pair.Key);
                        break;
                    }
                }

                Destroy(oldLine.gameObject);
            }
        }
    }

    /*
    private void AddLineToPoints(GameObject startPoint, GameObject endPoint, LineRenderer line)
    {
        if (!pointToLines.ContainsKey(startPoint))
            pointToLines[startPoint] = new List<LineRenderer>();
        if (!pointToLines.ContainsKey(endPoint))
            pointToLines[endPoint] = new List<LineRenderer>();

        pointToLines[startPoint].Add(line);
        pointToLines[endPoint].Add(line);

        // Check if the pair is correct
        if (correctPairs[startPoint] == endPoint)
        {
            Debug.Log("Benar");
        }
        else
        {
            Debug.Log("Salah");
        }
    }
    */

    private GameObject GetNearPoint()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (GameObject point in normalPoints)
        {
            if ((mousePosition - (Vector2)point.transform.position).magnitude < 0.2f)
            {
                return point;
            }
        }

        foreach (GameObject point in doublePoints)
        {
            if ((mousePosition - (Vector2)point.transform.position).magnitude < 0.2f)
            {
                return point;
            }
        }

        return null;
    }

    /*
    private int GetLineCountForPoint(GameObject point)
    {
        if (pointToLines.ContainsKey(point))
        {
            return pointToLines[point].Count;
        }

        return 0;
    }
    */
}