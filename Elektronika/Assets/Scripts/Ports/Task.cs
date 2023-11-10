using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip taskCompleteSound;
    [SerializeField] AudioClip taskFailSound;
    [SerializeField] AudioClip openTaskBarSound;
    [SerializeField] AudioClip openOutputBarSound;

    [Header("Task Bar")]
    [SerializeField] GameObject taskBar_closed;
    [SerializeField] GameObject taskBar_opened;
    [SerializeField] Transform completedTaskParent;
    [SerializeField] Sprite checkedTaskSprite;
    [SerializeField] Sprite uncheckedTaskSprite;
    List<Image> completedTasks = new List<Image>();

    [Header("Input Bar")]
    [SerializeField] GameObject inputBar;
    List<GameObject> completedInputs = new List<GameObject>();

    [Header("Output Bar")]
    [SerializeField] RectTransform outputBar;
    [SerializeField] TMP_Text outputText;
    List<GameObject> completedOutputs = new List<GameObject>();

    Vector2 outputBar_originPos;

    private void Start() {
        outputBar_originPos = outputBar.anchoredPosition;

        foreach (Transform child in completedTaskParent) {
            completedTasks.Add(child.GetChild(0).GetComponent<Image>());
        }
        foreach (Transform child in inputBar.transform) {
            completedInputs.Add(child.gameObject);
            // child.gameObject.SetActive(false);
        }
        foreach (Transform child in outputBar.transform) {
            completedOutputs.Add(child.gameObject);
            // child.gameObject.SetActive(false);
        }
    }
    
    public void SwitchTaskBar(bool isOpened) {
        audioSource.PlayOneShot(openTaskBarSound);
        taskBar_closed.SetActive(!isOpened);
        taskBar_opened.SetActive(isOpened);
    }

    public void SwitchOutputBar() {
        // outputBar_closed.SetActive(!isOn);
        // outputBar_opened.SetActive(isOn);
        audioSource.PlayOneShot(openOutputBarSound);
        
        if(outputBar.anchoredPosition == outputBar_originPos) {
            outputBar.anchoredPosition = new Vector2(0, outputBar_originPos.y);
            // inputBar.SetActive(true);
        } else {
            outputBar.anchoredPosition = outputBar_originPos;
            // inputBar.SetActive(false);
        }
    }

    public void SetOutputBarActivation(bool isOn) {
        if(outputBar.gameObject.activeSelf == isOn) return;

        inputBar.SetActive(isOn);
        outputBar.gameObject.SetActive(isOn);

        if(!isOn) {
            outputBar.anchoredPosition = outputBar_originPos;
        }
    }

    public void SetCompletedTask(int taskIndex, bool isCompleted) {
        // Debug.Log("SetCompletedTask: " + taskIndex + " " + isCompleted);
        // Debug.Log("completedTasks.name: " + completedTasks[taskIndex].name);
        completedTasks[taskIndex].sprite = isCompleted ? checkedTaskSprite : uncheckedTaskSprite;
        audioSource.PlayOneShot(isCompleted ? taskCompleteSound : taskFailSound);
    }

    public void SetCompletedInput(int inputIndex, bool isCompleted) {
        if(inputIndex >= 0) {
            completedInputs[inputIndex].SetActive(isCompleted);
            completedOutputs[inputIndex].SetActive(isCompleted);
        }

        int completedInputCount = 0;
        for (int i = 0; i <= 7; i++) {
            if(completedInputs[i].activeSelf) completedInputCount++;
        }

        outputText.text = completedInputCount.ToString() + "/8";

        if(completedInputCount == 8){
            completedOutputs[8].SetActive(true);
            GameManager.Instance.GameFinished();
        }
    }
}
