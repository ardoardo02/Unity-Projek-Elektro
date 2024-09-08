using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image progressFillBar;
    [SerializeField] GameObject finishedPanel;
    [SerializeField] TMP_Text totalMistakeText;
    [SerializeField] TMP_Text mistakeCountText;
    [SerializeField] Task task;

    [Header("Barriers")]
    [SerializeField] GameObject cablesBarrier;
    [SerializeField] GameObject powerCablesBarrier;
    [SerializeField] PowerPort[] powerPorts;
    [SerializeField] GameObject powerBarrier;
    [SerializeField] PowerManager powerManager;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip connectedSound;
    [SerializeField] AudioClip powerSwitchSound;
    [SerializeField] AudioClip ledSwitchSound;
    [SerializeField] AudioClip mistakeSound;
    [SerializeField] AudioClip gameFinishedSound;
    
    [Header("Configurable Variables")]
    [SerializeField] int totalTask = 17;
    [SerializeField] int totalCables = 39;

    [Header("Debug Variables")]
    [SerializeField] ComponentManager.ComponentType componentType;
    [SerializeField] int taskFinished = 0;
    [SerializeField] int mistakeCount = 0;
    [SerializeField] int cablesConnected = 0;
    [SerializeField] int ledGroundConnected = 0;
    [SerializeField] int switchGroundConnected = 0;
    [SerializeField] int vccSegmentConnected = 0;
    [SerializeField] int powerConnected = 0;

    float targetProgress;
    float currentProgress;
    float progressSpeed = 0.025f;

    bool addMistakeDebounce = false;
    bool isICInserted = false;
    bool isPowerOn = false;

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

        progressFillBar.fillAmount = 0f;
    }

    private void Update() {
        if (currentProgress != targetProgress) {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, progressSpeed);
            progressFillBar.fillAmount = currentProgress;
        }
    }

    private void UpdateProgressFillBar() {
        // ProgressFillBar.fillAmount = (float)taskFinished / totalTask;
        targetProgress = (float)taskFinished / totalTask;
        if (taskFinished >= totalTask) {
            task.SetOutputBarActivation(true);
            progressFillBar.transform.parent.parent.gameObject.SetActive(false);
        }
        else {
            task.SetOutputBarActivation(false);
            progressFillBar.transform.parent.parent.gameObject.SetActive(true);
        }
    }

    public void AddMistake() {
        StartCoroutine(AddMistakeCoroutine());
    }

    IEnumerator AddMistakeCoroutine() {
        if (addMistakeDebounce) yield break;

        addMistakeDebounce = true;

        audioSource.PlayOneShot(mistakeSound);

        mistakeCount++;
        mistakeCountText.text = "Mistake: " + mistakeCount.ToString();
        yield return new WaitForSeconds(.5f);

        addMistakeDebounce = false;
    }

    public void CheckInput(int inputIndex) {
        if (taskFinished == totalTask){
            audioSource.PlayOneShot(ledSwitchSound);
            task.SetCompletedInput(inputIndex, true);
        }
    }

    public void CheckPort(Port port, bool isPortConnected) {
        if (isPortConnected) {
            audioSource.PlayOneShot(connectedSound);
            
            if (port is GroundLEDPort ){
                ledGroundConnected++;
                cablesConnected++;
                if (ledGroundConnected > 3){
                    cablesConnected--;
                    AddMistake();
                    return;
                }
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, true);
                    powerCablesBarrier.SetActive(false);
                }
            }
            else if (port is VCC_SegmentPort) {
                vccSegmentConnected++;
                cablesConnected++;
                if (vccSegmentConnected > 1) {
                    cablesConnected--;
                    AddMistake();
                    return;
                }
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, true);
                    powerCablesBarrier.SetActive(false);
                }
            }
            else if (componentType == ComponentManager.ComponentType._7447 && port is GroundSwitchPort){
                switchGroundConnected++;
                cablesConnected++;
                if (switchGroundConnected > 4) {
                    cablesConnected--;
                    AddMistake();
                    return;
                }
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, true);
                    powerCablesBarrier.SetActive(false);
                }
            }
            else if (port is PowerVCCPort || port is PowerGroundPort) {
                powerConnected++;
                if (powerConnected >= 2) {
                    task.SetCompletedTask(2, true);
                    powerBarrier.SetActive(false);
                }
            } 
            else {
                cablesConnected++;
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, true);
                    powerCablesBarrier.SetActive(false);
                }
            }

            taskFinished++;
            if (isPowerOn) AddMistake();
            else if (!isICInserted) AddMistake();
            else if ((port is PowerVCCPort || port is PowerGroundPort) && (taskFinished < totalTask-2 || taskFinished > totalTask-1 || (taskFinished == totalTask-2 && powerConnected > 1))) AddMistake();
        } 
        else {
            if (port is GroundLEDPort) {
                ledGroundConnected--;
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, false);
                    powerCablesBarrier.SetActive(true);
                    foreach (var powerPort in powerPorts){
                        powerPort.DisconnectConnection();
                    }
                }
                cablesConnected--;
                if (ledGroundConnected >= 3) {
                    cablesConnected++;
                    return;
                }
            }
            else if (port is VCC_SegmentPort) {
                vccSegmentConnected--;
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, false);
                    powerCablesBarrier.SetActive(true);
                    foreach (var powerPort in powerPorts){
                        powerPort.DisconnectConnection();
                    }
                }
                cablesConnected--;
                if (vccSegmentConnected >= 1) {
                    cablesConnected++;
                    return;
                }
            }
            else if (componentType == ComponentManager.ComponentType._7447 && port is GroundSwitchPort){
                switchGroundConnected--;
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, false);
                    powerCablesBarrier.SetActive(true);
                    foreach (var powerPort in powerPorts){
                        powerPort.DisconnectConnection();
                    }
                }
                cablesConnected--;
                if (switchGroundConnected >= 4) {
                    cablesConnected++;
                    return;
                }
            }
            else if (port is PowerVCCPort || port is PowerGroundPort) {
                powerConnected--;
                if (powerConnected < 2) {
                    task.SetCompletedTask(2, false);
                    powerBarrier.SetActive(true);
                    powerManager.TogglePower(false);
                }
            } 
            else {
                if (cablesConnected == totalCables) {
                    task.SetCompletedTask(1, false);
                    powerCablesBarrier.SetActive(true);
                    foreach (var powerPort in powerPorts){
                        powerPort.DisconnectConnection();
                    }
                }
                cablesConnected--;
            }
            taskFinished--;
        }

        UpdateProgressFillBar();
    }

    public void CheckIC(bool isICInserted, ComponentManager.ComponentType comType = ComponentManager.ComponentType.Others) {
        if (this.isICInserted == isICInserted) return;

        this.isICInserted = isICInserted;
        componentType = comType;
        task.SetCompletedTask(0, isICInserted);
        cablesBarrier.SetActive(!isICInserted);
        
        if (isICInserted) taskFinished++;
        else {
            taskFinished--;
            componentType = ComponentManager.ComponentType.Others;
        }

        UpdateProgressFillBar();
    }

    public ComponentManager.ComponentType GetInsertedICType() {
        return componentType;
    }

    public void CheckPowerSwitch(bool isPowerOn) {
        this.isPowerOn = isPowerOn;
        audioSource.PlayOneShot(powerSwitchSound);

        task.SetCompletedTask(3, isPowerOn);
        if (isPowerOn) {
            taskFinished++;
            if (taskFinished != totalTask) AddMistake();
        }
        else taskFinished--;

        UpdateProgressFillBar();
    }

    public void GameFinished() {
        audioSource.PlayOneShot(gameFinishedSound);
        totalMistakeText.text = "Total Mistake: " + mistakeCount.ToString();
        finishedPanel.SetActive(true);
    }
}
