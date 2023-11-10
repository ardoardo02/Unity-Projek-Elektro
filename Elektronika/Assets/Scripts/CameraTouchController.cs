using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraTouchController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField, Range(0, 20)] float filterFactor = 10;
    [SerializeField, Range(0, 3)] float dragFactor = 1;
    [SerializeField, Range(0, 2)] float zoomFactor = 1;
    [SerializeField] float minCamSize = 5;
    [SerializeField] float maxCamSize = 10;
    [SerializeField] float maxDragX = 4;
    [SerializeField] float maxDragY = 6;
    [SerializeField] Collider2D topCollider;

    [Header("UI References")]
    [SerializeField] Image camSpriteRenderer;
    [SerializeField] Sprite moveCamSprite;
    [SerializeField] Sprite interactSprite;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] TMP_Text modeText;

    [Header("Debug")]
    [SerializeField] bool isMoveCam = false;

    Vector3 touchBeganWorldPos;
    Vector3 cameraBeganWorldPos;
    Camera mainCamera;
    [SerializeField] float firstHalfCamSize;
    [SerializeField] float secondHalfCamSize;

    private void Start() {
        mainCamera = Camera.main;
        topCollider.enabled = isMoveCam;
        firstHalfCamSize = ((maxCamSize - minCamSize) / 4) + minCamSize;
        secondHalfCamSize = maxCamSize - ((maxCamSize - minCamSize) / 4);
    }

    private void Update() {
        if(Input.touchCount == 0 || isMoveCam == false)
            return;
        
        var touch0 = Input.GetTouch(0);

        if(Input.touchCount == 1){// simpan posisi awal tapi posisi real world
            if(touch0.phase == TouchPhase.Began){
                touchBeganWorldPos = new Vector3(touch0.position.x, touch0.position.y, transform.position.z);
                cameraBeganWorldPos = this.transform.position;
            }

            if(touch0.phase == TouchPhase.Moved){
                var touchMovedWorldPos = new Vector3(touch0.position.x, touch0.position.y, transform.position.z);
                var delta = touchMovedWorldPos - touchBeganWorldPos;

                var targetPos = cameraBeganWorldPos - delta * dragFactor;

                targetPos = mainCamera.orthographicSize > secondHalfCamSize? 
                    new Vector3(
                        Mathf.Clamp(targetPos.x, topCollider.bounds.min.x + maxDragX, topCollider.bounds.max.x - maxDragX),
                        Mathf.Clamp(targetPos.y, topCollider.bounds.min.y + maxDragY, topCollider.bounds.max.y - maxDragY),
                        targetPos.z
                    ): mainCamera.orthographicSize < firstHalfCamSize?
                    new Vector3(
                        Mathf.Clamp(targetPos.x, topCollider.bounds.min.x + 1, topCollider.bounds.max.x - 1),
                        Mathf.Clamp(targetPos.y, topCollider.bounds.min.y + 1, topCollider.bounds.max.y - 1),
                        targetPos.z
                    ): new Vector3(
                        Mathf.Clamp(targetPos.x, topCollider.bounds.min.x + (maxDragX/2) + 1, topCollider.bounds.max.x - (maxDragX/2) - 1),
                        Mathf.Clamp(targetPos.y, topCollider.bounds.min.y + (maxDragY/2) + 1, topCollider.bounds.max.y - (maxDragY/2) - 1),
                        targetPos.z
                    );

                this.transform.position = Vector3.Lerp(
                    this.transform.position,
                    targetPos,
                    Time.deltaTime * filterFactor
                );
            }
        }

        if (Input.touchCount < 2)
            return;

        var touch1 = Input.GetTouch(1);

        if(touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved){
            var touch0PrevPos = touch0.position - touch0.deltaPosition;
            var touch1PrevPos = touch1.position - touch1.deltaPosition;
            
            var prevDistance = Vector3.Distance(touch0PrevPos, touch1PrevPos);
            var currDistance = Vector3.Distance(touch0.position, touch1.position);

            var delta = currDistance - prevDistance;

            // Adjust the orthographic size based on zoomFactor and clamp it
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - delta * zoomFactor,
                                                      minCamSize, maxCamSize);

            // this.transform.position -= new Vector3(0, 0, delta * zoomFactor);
            // this.transform.position = new Vector3(
            //     this.transform.position.x,
            //     this.transform.position.y,
            //     Mathf.Clamp(this.transform.position.z, minCamPos, maxCamPos)
            // );
            // distance = this.transform.position.z;
        }
    }

    public void SetIsMoveCam(){
        isMoveCam = !isMoveCam;
        topCollider.enabled = isMoveCam;
        buttonText.text = isMoveCam ? "Interact Objects" : "Move Camera";
        modeText.text = isMoveCam ? "Mode: Moving Camera" : "Mode: Interacting Objects";
        camSpriteRenderer.sprite = isMoveCam ? moveCamSprite : interactSprite;
    }
}
