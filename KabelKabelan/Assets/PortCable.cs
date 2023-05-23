using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortCable : MonoBehaviour
{
    [SerializeField] LineController line;
    // [SerializeField] Transform[] points;
    Button portBtn;

    private void Awake() {
		portBtn = GetComponent<Button>();
		portBtn.onClick.AddListener(TaskOnClick);
    }

	void Start () {
        // line.SetUpLine(points);
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
        line.DragLine(this);
	}

  public void CheckLine()
  {
    line.CheckDrag(this);
  }
}
