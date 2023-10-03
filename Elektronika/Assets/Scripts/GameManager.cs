using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image ProgressFillBar;
    [SerializeField] TMP_Text MistakeCountText;
    
    [Header("Variables")]
    [SerializeField] int totalTask = 17;
    [SerializeField] int taskFinished = 0;
    [SerializeField] int mistakeCount = 0;
    [SerializeField] int ledGroundConnected = 0;

    float targetProgress;
    float currentProgress;
    float progressSpeed = 0.025f;

    bool addMistakeDebounce = false;
    bool isICInserted = false;

    public static GameManager Instance;

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

        ProgressFillBar.fillAmount = 0f;
    }

    private void Update() {
        if (currentProgress != targetProgress) {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, progressSpeed);
            ProgressFillBar.fillAmount = currentProgress;
        }
    }

    private void UpdateProgressFillBar() {
        // ProgressFillBar.fillAmount = (float)taskFinished / totalTask;
        targetProgress = (float)taskFinished / totalTask;
    }

    public void AddMistake() {
        StartCoroutine(AddMistakeCoroutine());
    }

    IEnumerator AddMistakeCoroutine() {
        if (addMistakeDebounce) yield break;

        addMistakeDebounce = true;

        mistakeCount++;
        MistakeCountText.text = "Mistake: " + mistakeCount.ToString();
        yield return new WaitForSeconds(1f);

        addMistakeDebounce = false;
    }

    public void CheckPort(Port port, bool isPortConnected) {
        if (isPortConnected) {
            
            if (port is GroundLEDPort ){
                ledGroundConnected++;
                if (ledGroundConnected > 3){
                    AddMistake();
                    return;
                }
            } 

            taskFinished++;
            if (!isICInserted) AddMistake();
        } 
        else {
            if (port is GroundLEDPort) {
                ledGroundConnected--;
                if (ledGroundConnected >= 3) return;
            }
            taskFinished--;
        }

        UpdateProgressFillBar();
    }

    public void CheckIC74LS148(bool isICInserted) {
        if (this.isICInserted == isICInserted) return;

        this.isICInserted = isICInserted;
        
        if (isICInserted) taskFinished++;
        else taskFinished--;

        UpdateProgressFillBar();
    }

    public void CheckPowerSwitch(bool isPowerOn) {
        if (isPowerOn) {
            taskFinished++;
            if (taskFinished != totalTask) AddMistake();
        }
        else taskFinished--;

        UpdateProgressFillBar();
    }
}
