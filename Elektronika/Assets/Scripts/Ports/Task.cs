using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static ComponentManager;

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

    [Header("Notification")]
    [SerializeField] GameObject notification;
    [SerializeField] TMP_Text notificationText;

    Vector2 outputBar_originPos;

    private void Start() {
        outputBar_originPos = outputBar.anchoredPosition;

        foreach (Transform child in completedTaskParent) {
            completedTasks.Add(child.GetChild(0).GetComponent<Image>());
        }

        if (inputBar.transform.childCount > 5) {
            foreach (Transform child in inputBar.transform) {
                Debug.Log("child: " + child.name);
                completedInputs.Add(child.gameObject);
            }
        }
        else if (inputBar.transform.GetChild(1).childCount > 5) {
            foreach (Transform child in inputBar.transform.GetChild(1)) {
                Debug.Log("child: " + child.name);
                completedInputs.Add(child.gameObject);
            }
        }
        // foreach (Transform child in GameManager.Instance.GetInsertedICType() == ComponentType._74LS148 ? inputBar.transform : inputBar.transform.GetChild(1)) {
        //     completedInputs.Add(child.gameObject);
        //     // child.gameObject.SetActive(false);
        // }
        
        foreach (Transform child in outputBar.transform) {
            completedOutputs.Add(child.gameObject);
            // child.gameObject.SetActive(false);
        }

        StartCoroutine(ShowNotification());
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
        inputBar.SetActive(isOn);

        if (GameManager.Instance.GetInsertedICType() != ComponentType._74LS148) return;
        if(outputBar.gameObject.activeSelf == isOn) return;

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
        StartCoroutine(ShowNotification());
    }

    public void SetCompletedInput(int inputIndex, bool isCompleted) {
        // if (inputIndex > completedInputs.Count-1) return;
        Debug.Log("input index: " + inputIndex + " " + isCompleted);

        if(inputIndex >= 0) {
            completedInputs[inputIndex].SetActive(isCompleted);
            Debug.Log("Activating input " + inputIndex);

            if (GameManager.Instance.GetInsertedICType() == ComponentType._74LS148) {
                completedOutputs[inputIndex].SetActive(isCompleted);
                Debug.Log("Activating output " + inputIndex);
            }
        }

        int completedInputCount = 0;
        if (GameManager.Instance.GetInsertedICType() == ComponentType._74LS148) {
            for (int i = 0; i <= 7; i++) {
                if(completedInputs[i].activeSelf) completedInputCount++;
            }

            outputText.text = completedInputCount.ToString() + "/8";

            if(completedInputCount == 8){
                completedOutputs[8].SetActive(true);
                GameManager.Instance.GameFinished();
            }
        }
        else if (GameManager.Instance.GetInsertedICType() == ComponentType._7447) {
            for (int i = 0; i <= 9; i++) {
                if(completedInputs[i].activeSelf) completedInputCount++;
            }

            if(completedInputCount == 10){
                GameManager.Instance.GameFinished();
            }
        }
    }
    
    IEnumerator ShowNotification() {
        int completedTaskCount = 0;

        foreach (Image child in completedTasks){
            if(child.sprite == checkedTaskSprite) completedTaskCount++;
        }

        notification.SetActive(true);

        notificationText.text = 
            completedTaskCount == 0 ? "Let's install the IC!" :
            completedTaskCount == 1 ? "Let's connect the cables!" :
            completedTaskCount == 2 ? "Let's connect the power cables!" :
            completedTaskCount == 3 ? "Let's turn the power on!" :
            "Let's test the inputs!";
        
        // modulate to 1
        CanvasGroup canvasGroup = notification.GetComponent<CanvasGroup>();
        for (float i = 0; i <= 1; i += 0.02f) {
            canvasGroup.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(2f);

        // modulate to 0
        for (float i = 1; i >= 0; i -= 0.02f) {
            canvasGroup.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        canvasGroup.alpha = 0;

        notification.SetActive(false);
    }
}
